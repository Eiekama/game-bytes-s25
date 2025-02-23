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
        [SerializeField] private PowerupType currentPowerup = PowerupType.None;
        
        // Can't use final + SerializeField AFAIK, but this s/b effectively final:
        // Measured in seconds
        [SerializeField] private int powerupDuration = 10;

        private Camera _mainCamera;
        public float screenRotationTime = 0.25f;

        BirdScript bs;

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
                switch (powerupType)
                {
                    case PowerupType.EnergyShield:
                        EnergyShield();
                        break;
                    case PowerupType.RiceMagnet:
                        RiceMagnet();
                        break;
                    case PowerupType.LimeBoost:
                        LimeBoost();
                        break;
                    case PowerupType.Stopwatch:
                        Stopwatch();
                        break;
                    case PowerupType.MushroomFlip:
                        StartCoroutine(MushroomFlip());
                        break;
                    case PowerupType.SwapWarp:
                        SwapWarp();
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
                    Bread(collect);
                    break;
                }

                Destroy(collect);
            }

            if (collider.gameObject.CompareTag("Danger"))
            {
                if (canDie)
                    bs.dead = true;
            }
        }
        
        void EnergyShield()
        {
    
        }

        void RiceMagnet()
        {

        }

        void LimeBoost()
        {

        }

        void Stopwatch()
        {

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
            }
            else
            {
                // Directly set it to <upside down>, in case the anim overshot it.
                _mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1);
            }
        }
        void SwapWarp()
        {

        }

        void OneUp()
        {

        }

        private void OnDestroy()
        {
            Debug.Log("Destoryed");
            _mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
    
        void Bread(GameObject spawn)
        {
            if (GetComponent<BirdScript>().player == 1)
                MinigameController.Instance.AddScore(1, 1);
            else if (GetComponent<BirdScript>().player == 2)
                MinigameController.Instance.AddScore(2, 1);
            Instantiate(spawn, new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-4.8f, 4.8f), 0f), Quaternion.identity);
            Score++;
        }
    }
}