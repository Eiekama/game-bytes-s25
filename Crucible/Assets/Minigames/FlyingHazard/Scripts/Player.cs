using System;
using Unity.UNetWeaver;
using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PowerupType currentPowerup = PowerupType.None;

        private Camera _mainCamera;

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
                        Debug.Log("test2");
                        Stopwatch();
                        break;
                    case PowerupType.MushroomFlip:
                        MushroomFlip();
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

        void MushroomFlip()
        {
            _mainCamera.transform.Rotate(new Vector3(0, 0, 180));
        }

        void SwapWarp()
        {

        }

        void OneUp()
        {

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