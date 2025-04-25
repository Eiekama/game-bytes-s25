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
        private CircleCollider2D _collider;
        
        [SerializeField] private PowerupType currentPowerup = PowerupType.None;
        // Time before they lose the powerup.
        // NOTE: Only accurate for some powerups (e.g. not necessarily accurate for Stopwatch/SwapWarp.)
        [SerializeField] private float powerupSecondsLeft = 0.0f;
        
        // Can't use const + SerializeField AFAIK, but this s/b effectively constant:
        // Measured in seconds
        [SerializeField] private int powerupDuration = 10;
        
        public float screenRotationTime = 0.25f;
        private static bool _screenUpsideDown = false;

        [SerializeField] private TMP_Text livesDisplay;
        [SerializeField] private int lives = 3;
        // Seconds the bird is invincible after using a OneUp. 
        private const float invincibilityTime = 2f;
        [SerializeField] bool canDie;

        private static bool screenFlipping;
        
        public GameObject musicbox;

        AudioSource music;
        public int Score = 0;

        private void Start()
        {
            _mainCamera = Camera.main;
            bs = GetComponent<BirdScript>();
            _collider = GetComponent<CircleCollider2D>();
            livesDisplay.text = "" + lives;
            StartCoroutine(StopSpawnCamping());
            music = musicbox.GetComponent<AudioSource>();
            screenFlipping = false;
        }

        private void Update()
        {
            if (powerupSecondsLeft > 0.0f)
            {
                powerupSecondsLeft -= Time.deltaTime;
            } else if (powerupSecondsLeft < 0.0f)
            {
                powerupSecondsLeft = 0.0f;
                currentPowerup = PowerupType.None;
            }
            else
            {
                powerupSecondsLeft = 0.0f;
            }
        }

        // If the player hasn't moved (or died) after 10s, spawns a balloon under them.
        private IEnumerator StopSpawnCamping()
        {
            int startingLives = lives;
            yield return new WaitForSeconds(10);
            if (lives == startingLives && bs.direction == Direction.Neutral)
            {
                StartCoroutine(spawning.spawnSingleBalloon(transform.position.x));
                StartCoroutine(spawning.spawnSingleDrop(transform.position.x));
            }
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
                bs.jumps_Powerups[UnityEngine.Random.Range(3,7)].Play();
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

            if (canDie && collider.gameObject.CompareTag("Danger")
             && currentPowerup != PowerupType.EnergyShield
             && !screenFlipping)
            {
                Die();
            }
        }

        private void Die()
        {
            // Debug.Log("Dying");
            _collider.enabled = false;
            if (lives > 0){
                lives--;
                livesDisplay.text = "" + lives;
            }
            bs.getAnim().SetBool("Death", true);
            bs.dead = true;
            bs.DeathEffects();
            bs.jumps_Powerups[10].Play();
            StartCoroutine(fallSound());
            if (music.pitch != 1 && currentPowerup == PowerupType.Stopwatch){
                music.pitch = 1;
            }
            
            //This first canDie check is to make sure lives get updated before the other functions
            if (lives > 0) {
                StartCoroutine(respawnBird());
            }

            if (_screenUpsideDown && currentPowerup == PowerupType.MushroomFlip)
            {
                Debug.Log("Flipping Screen Back");
                StartCoroutine(FlipScreen(-1));
            }
            currentPowerup = PowerupType.None;
        }
        
        IEnumerator EnergyShield()
        {
            // Waits until Invincible is finished.
            powerupSecondsLeft = powerupDuration;
            yield return new WaitForSeconds(powerupDuration);
        }

        IEnumerator Invincible(float time)
        {
            bool couldDie = canDie;
            canDie = false;
            
            yield return new WaitForSeconds(time);

            canDie = couldDie;
        }

        void RiceMagnet()
        {
            powerupSecondsLeft = powerupDuration;
        }

        void LimeBoost()
        {
            powerupSecondsLeft = powerupDuration;
        }

        IEnumerator Stopwatch()
        {
            // TODO: Make this not have a visible reduction in framerate? (i.e. just slow things down manually pbly.)
            float slowAmt = 0.5f;
            
            music.pitch = 0.5f;

            Time.timeScale = slowAmt;
            
            // E.g. only do it for 5 seconds (instead of 10) with slowAmt = 0.5f.
            powerupSecondsLeft = powerupDuration * slowAmt;
            
            yield return new WaitForSecondsRealtime(powerupDuration);
            
            music.pitch = 1;

            Time.timeScale = 1;
        }

        // Doesn't work:
        // private static Vector3 V3SmoothStep(Vector3 start, Vector3 end, float t)
        // {
        //     float smoothX = Mathf.SmoothStep(start.x, end.x, t);
        //     float smoothY = Mathf.SmoothStep(start.y, end.y, t);
        //     float smoothZ = Mathf.SmoothStep(start.z, end.z, t);
        //     return new Vector3(smoothX, smoothY, smoothZ);
        // }

        IEnumerator FlipScreen(int direction = 1)
        {
            Debug.Log("Flipping Screen, dir = " + direction + ", _sUD: " + _screenUpsideDown);
            if ((_screenUpsideDown && direction == 1)
                || (!_screenUpsideDown && direction == -1))
            {
                yield break;
            }
            Vector3 start = new Vector3(0, 0, 0);
            Vector3 end = new Vector3(0, 0, 180);
            
            screenFlipping = true;

            for (float time = 0; time < screenRotationTime; time += Time.deltaTime)
            {
                _mainCamera.transform.Rotate(direction * Vector3.Lerp(start, end, Time.deltaTime / screenRotationTime));
                yield return null;
            }
            
            _screenUpsideDown = !_screenUpsideDown;

            // Direction == 1 means rotating clockwise; -1 is counterclockwise.
            // If direction is 1, we rotate it back after powerupDuration seconds. 
            if (direction == 1)
            {
                // Directly set it to <upside down>, in case the anim overshot it.
                _mainCamera.transform.rotation = new Quaternion(0, 0, 1, 0);
            } else if (direction == -1)
            {
                // Directly set it to <upside down>, in case the anim overshot it.
                _mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1);
            }
            
            yield return new WaitForSeconds(0.05f);

            screenFlipping = false;
        }

        IEnumerator MushroomFlip()
        {
            Debug.Log("Mushroom Flip");
            yield return FlipScreen(1);
            Debug.Log("Mushroom Flip 2");
            
            powerupSecondsLeft = powerupDuration + screenRotationTime;
            int prevLives = lives;
        
            yield return new WaitForSeconds(powerupDuration);

            if (lives < prevLives)
            {
                yield break;
            }
            
            // Rotates it back the right way
            StartCoroutine(FlipScreen(-1));
        }
        IEnumerator SwapWarp()
        {
            currentPowerup = PowerupType.SwapWarp;
            powerupSecondsLeft = powerupDuration;
            
            if (otherBird.bs.dead)
            {
                // TODO: Hold onto the powerup until another player spawns, then swap?
                Debug.Log("No other bird to swap with :(");
                // Early return if not 2 players in play:
                powerupSecondsLeft = 0.0001f; // Immediately lose the powerup.
                yield break;
            }

            Time.timeScale = 0f;
            
            Vector3 pos = transform.position;
            transform.position = otherBird.transform.position;
            otherBird.transform.position = pos;
            
            // TODO: Do an animation of them swapping positions?
            
            yield return new WaitForSecondsRealtime(0.75f);

            Time.timeScale = 1;
            
            // Subtract off the time spent doing the animation
            yield return new WaitForSeconds(powerupDuration - 0.0f);
            // ResetPowerup();
        }

        void OneUp()
        {
            lives++;
            livesDisplay.text = "" + lives;
            
            powerupSecondsLeft = powerupDuration;
            // Put animations/etc here ig.
            // All the functionality rn is in other places in this script.
        }

        // TODO: Check if this is ever called.
        // It isn't in our code^. (Is OnDestroy a built-in unity fn though?)
        private void OnDestroy()
        {
            Debug.Log(name + " Destroyed");
            _mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
    
        IEnumerator Bread(GameObject spawn)
        {
            AddScore(10);
            bs.jumps_Powerups[7].Play();
            yield return new WaitForSeconds(spawning.breadint);
            // StartCoroutine(Bread(spawn));
            // StartCoroutine(Bread(spawn));
            Instantiate(spawn, new Vector3(UnityEngine.Random.Range(-Spawning.WIDTH+0.8f, Spawning.WIDTH-0.8f), UnityEngine.Random.Range(-Spawning.HEIGHT+1f, Spawning.HEIGHT-1f), 0f), Quaternion.identity);
        }
        void Rice()
        {
            bs.jumps_Powerups[8].Play();
            AddScore(1);
        }

        void AddScore(int score)
        {
            int playerNum = bs.player;
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
            if (bs.player == 1){
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, .2f, 0f, 1f);
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            } else {
                for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            }
            canDie = true;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0.0f, 0.0f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 4.0f;
            bs.dead = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;

            bs.ResetDirection();
            
            // I-Frames
            StartCoroutine(Invincible(invincibilityTime));
            if (bs.player == 1){
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, .2f, 0f, 1f);
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            } else {
                for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                yield return new WaitForSeconds(0.2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            }
            
            // After i-frames end:
            StartCoroutine(spawning.spawnSingleBalloon(transform.position.x));
            StartCoroutine(spawning.spawnSingleDrop(transform.position.x));
        }

        public int getLives(){
            return lives;
        }

        IEnumerator fallSound(){
            yield return new WaitForSeconds(1);
            bs.jumps_Powerups[9].Play();
        }
    }
}