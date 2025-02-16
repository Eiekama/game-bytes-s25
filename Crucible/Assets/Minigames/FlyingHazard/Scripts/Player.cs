using System;
using Unity.UNetWeaver;
using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PowerupType currentPowerup = PowerupType.None;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
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
    }
}