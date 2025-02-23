using System;
using Unity.UNetWeaver;
using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private PowerupType currentPowerup = PowerupType.None;

        private Camera _mainCamera;

        BirdScript bs;

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
                        EnergyShield1();
                        break;
                    case PowerupType.RiceMagnet:
                        RiceMagnet1();
                        break;
                    case PowerupType.LimeBoost:
                        LimeBoost1();
                        break;
                    case PowerupType.Stopwatch:
                        Debug.Log("test2");
                        Stopwatch1();
                        break;
                    case PowerupType.MushroomFlip:
                        MushroomFlip1();
                        break;
                    case PowerupType.SwapWarp:
                        SwapWarp1();
                        break;
                    case PowerupType.OneUp:
                        OneUp1();
                        break;
                    case PowerupType.None:
                        Debug.Log("Powerup " + powerup.name + " is of None type (shouldn't be possible.)");
                        break;
                }
                
                Destroy(powerup);
                Debug.Log("Powerup " + powerup + " destroyed");
            } else {
                bs.dead = true;
            }
        }
        
        void EnergyShield1()
        {

        }

        void RiceMagnet1()
        {

        }

        void LimeBoost1()
        {

        }

        void Stopwatch1()
        {

        }

        void MushroomFlip1()
        {
            _mainCamera.transform.Rotate(new Vector3(0, 0, 180));
        }

        void SwapWarp1()
        {

        }

        void OneUp1()
        {

        }
    }
}