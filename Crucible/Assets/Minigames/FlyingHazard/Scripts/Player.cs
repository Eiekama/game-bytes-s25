using System;
using System.Collections;
using System.Numerics;
using Unity.UNetWeaver;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using TMPro;

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

        [SerializeField] private TMP_Text livesDisplay;
        private int lives = 3;
        private const float invincibilityTime = 2f;
        [SerializeField] bool canDie;
        
        public int Score = 0;

        private void Start()
        {
            _mainCamera = Camera.main;
            bs = GetComponent<BirdScript>();
            livesDisplay.text = "" + lives;
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
                        StartCoroutine(RiceMagnet());
                        break;
                    case PowerupType.LimeBoost:
                        StartCoroutine(LimeBoost());
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
                        StartCoroutine(OneUp());
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
                if (canDie){
                    lives--;
                    livesDisplay.text = "" + lives;
                }    
                //This first canDie check is to make sure lives get updated before the other functions
                if (canDie && lives == 0)
                {
                    bs.getAnim().SetBool("Death", true);
                    bs.dead = true;
                } else if (canDie && lives > 0) {
                    bs.getAnim().SetBool("Death", true);
                    bs.dead = true;
                    StartCoroutine(respawnBird());
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

        IEnumerator RiceMagnet()
        {
            yield return new WaitForSeconds(powerupDuration);
            ResetPowerup();
        }

        IEnumerator LimeBoost()
        {
            yield return new WaitForSeconds(powerupDuration);
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

        IEnumerator OneUp()
        {
            lives++;
            livesDisplay.text = "" + lives;
            yield return new WaitForSecondsRealtime(powerupDuration);
            
            ResetPowerup();
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
            AddScore(10);
            yield return new WaitForSeconds(spawning.breadint);
            Instantiate(spawn, new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-4.8f, 4.8f), 0f), Quaternion.identity);
        }
        void Rice()
        {
            AddScore(1);
        }

        void AddScore(int score)
        {
            int playerNum = GetComponent<BirdScript>().player;
            if (currentPowerup == PowerupType.LimeBoost)
            {
                score *= Powerup.LIMEBOOSTMULT;
            }
            MinigameController.Instance.AddScore(playerNum, score);
            this.Score += score;
        }

        public PowerupType getPowerup(){ // Using this for powerup spawning detection
            return currentPowerup;
        }

        IEnumerator respawnBird(){
            //A lot of the numbers right now are really arbitrary, gonna add an interval field or something so that it's adjustable
            canDie = false;
            yield return new WaitForSeconds(4);
            //Time to wait for the bird to fall off the screen
            bs.getAnim().SetBool("Death", false);
            bs.getAnim().SetTrigger("Respawn");
            bs.direction = Direction.Neutral;
            yield return new WaitForSeconds(1);
            //Time to wait for the bird's animations to finish (This will have to be set or else the bird will respawn face down)
            if(bs.player == 1){
                transform.position = new Vector3(-5, 0, 0);
            } else if (bs.player == 2){
                transform.position = new Vector3(5, 0, 0);
            }    
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0.0f, 0.0f);
            for (int i = 0; i < 5; i++)
            //I tried to make this a separate method called flickering in spawning, but it didn't work so i
            //just put the whole for loop in here
            {
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }    
            canDie = true;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0.0f, 0.0f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 4.0f;
            bs.dead = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            StartCoroutine(Invincible(invincibilityTime, true));
        }

        public int getLives(){
            return lives;
        }
    }
}