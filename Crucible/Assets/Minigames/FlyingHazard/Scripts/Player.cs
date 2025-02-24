using System;
using System.Collections;
using System.Numerics;
using Unity.UNetWeaver;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Minigames.FlyingHazard.Scripts
{
    public class Player : MonoBehaviour
    {
        public Player otherBird;
        BirdScript bs;

        [SerializeField] private Spawning spawning;
        private Camera _mainCamera;
        
        [SerializeField] private PowerupType currentPowerup = PowerupType.None;
        
        // Can't use const + SerializeField AFAIK, but this s/b effectively constant:
        // Measured in seconds
        [SerializeField] private int powerupDuration = 10;
        public float screenRotationTime = 0.25f;
        // Seconds the bird is invincible after using a OneUp. 
        private const float invincibilityTime = 1f;
        [SerializeField] bool canDie;
        
        public int Score = 0;

        private void Start()
        {
            _mainCamera = Camera.main;
            bs = GetComponent<BirdScript>();
        }

        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log(name + " collided with " + collider.name);
            
            if (collider.gameObject.CompareTag("Powerup"))
            {
                if (currentPowerup != PowerupType.None)
                {
                    Debug.Log("> " + name + " already has a powerup in effect (" + currentPowerup + "); skipping.");
                    return;
                }
                GameObject powerup = collider.gameObject;
                PowerupType powerupType = powerup.GetComponent<Powerup>().type;
                currentPowerup = powerupType;
                spawning.subtractPowerupCount(); //This helps track powerups on field.
                switch (powerupType)
                {
                    case PowerupType.EnergyShield:
                        StartCoroutine(EnergyShield());
                        break;
                    case PowerupType.RiceMagnet:
                        RiceMagnet();
                        break;
                    case PowerupType.LimeBoost:
                        LimeBoost();
                        break;
                    case PowerupType.Stopwatch:
                        StartCoroutine(Stopwatch());
                        break;
                    case PowerupType.MushroomFlip:
                        StartCoroutine(MushroomFlip());
                        break;
                    case PowerupType.SwapWarp:
                        StartCoroutine(SwapWarp());
                        break;
                    case PowerupType.OneUp:
                        OneUp();
                        break;
                    case PowerupType.None:
                        Debug.Log("Powerup " + powerup.name + " is of None type (shouldn't be possible.)");
                        break;
                }
                
                Destroy(powerup);
                Debug.Log("Powerup " + powerup + " destroyed");
            }

            if (collider.gameObject.CompareTag("Collectable"))
            {
                GameObject collect = collider.gameObject;
                int Collecter = collect.GetComponent<Collects>().type2;
                switch(Collecter)
                {
                    case 1:
                    StartCoroutine(Bread(spawning.bread));
                    break;

                    case 2:
                    Rice();
                    break;
                }

                Destroy(collect);
            }

            if (collider.gameObject.CompareTag("Danger"))
            {
                if (canDie && currentPowerup != PowerupType.OneUp)
                {
                    bs.dead = true;
                }

                if (canDie && currentPowerup == PowerupType.OneUp)
                {
                    StartCoroutine(Invincible(invincibilityTime, true));
                }
            }
        }
        
        IEnumerator EnergyShield()
        {
            // Waits until Invincible is finished.
            yield return StartCoroutine(Invincible(powerupDuration, true));
        }

        IEnumerator Invincible(float time, bool resetPowerupAfter = false)
        {
            bool couldDie = canDie;
            canDie = false;
            
            yield return new WaitForSeconds(time);

            canDie = couldDie;
            if(resetPowerupAfter)
                ResetPowerup();
        }

        void RiceMagnet()
        {
            
            ResetPowerup();
        }

        void LimeBoost()
        {

            ResetPowerup();
        }

        IEnumerator Stopwatch()
        {
            // TODO: Make this not have a visible reduction in framerate (i.e. just slow things down manually pbly.)
            float slowAmt = 0.5f;
            
            Time.timeScale = slowAmt;
            
            yield return new WaitForSecondsRealtime(powerupDuration);
            
            ResetPowerup();
            Time.timeScale = 1;
        }

        IEnumerator MushroomFlip(int direction = 1)
        {
            Vector3 start = new Vector3(0, 0, 0);
            Vector3 end = new Vector3(0, 0, 180);

            for (float time = 0; time < screenRotationTime; time += Time.deltaTime)
            {
                _mainCamera.transform.Rotate(direction * Vector3.Lerp(start, end, Time.deltaTime / screenRotationTime));
                yield return null;
            }
            
            // Direction == 1 means rotating clockwise; -1 is counterclockwise.
            // If direction is 1, we rotate it back after powerupDuration seconds. 
            if (direction == 1)
            {
                // Directly set it to <upside down>, in case the anim overshot it.
                _mainCamera.transform.rotation = new Quaternion(0, 0, 1, 0);
            
                yield return new WaitForSeconds(powerupDuration);
                
                // Rotates it back the right way
                StartCoroutine(MushroomFlip(-1));

                ResetPowerup();
            }
            else
            {
                // Directly set it to <upside down>, in case the anim overshot it.
                _mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1);
            }
        }
        IEnumerator SwapWarp()
        {
            currentPowerup = PowerupType.SwapWarp;
            
            if (otherBird.bs.dead)
            {
                // TODO: Hold onto the powerup until another player spawns, then swap?
                Debug.Log("No other bird to swap with :(");
                // Early return if not 2 players in play:
                yield break;
            }

            Time.timeScale = 0f;
            
            Vector3 pos = transform.position;
            transform.position = otherBird.transform.position;
            otherBird.transform.position = pos;
            
            // TODO: Do an animation of them swapping positions?
            
            yield return new WaitForSecondsRealtime(0.5f);

            Time.timeScale = 1;
            
            // Subtract off the time spent doing the animation
            yield return new WaitForSeconds(powerupDuration - 0.0f);
            ResetPowerup();
        }

        void OneUp()
        {
            // Put animations/etc here ig.
            // All the functionality rn is in other places in this script.
        }

        private void ResetPowerup()
        {
            currentPowerup = PowerupType.None;
        }

        // TODO: Check if this is ever called.
        private void OnDestroy()
        {
            Debug.Log(name + " Destoryed");
            _mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
    
        IEnumerator Bread(GameObject spawn)
        {
            if (GetComponent<BirdScript>().player == 1)
                MinigameController.Instance.AddScore(1, 10);
            else if (GetComponent<BirdScript>().player == 2)
                MinigameController.Instance.AddScore(2, 10);
            Score+=10;
            yield return new WaitForSeconds(spawning.breadint);
            Instantiate(spawn, new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-4.8f, 4.8f), 0f), Quaternion.identity);
        }
        void Rice()
        {
            if (GetComponent<BirdScript>().player == 1)
                MinigameController.Instance.AddScore(1, 1);
            else if (GetComponent<BirdScript>().player == 2)
                MinigameController.Instance.AddScore(2, 1);
            Score++;
        }

        public PowerupType getCurrent(){ // Using this for powerup spawning detection
            return currentPowerup;
        }
    }
}