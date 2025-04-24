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

        [SerializeField] bool canDie;

        public int Score1 = 0;

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
            }

            if (collider.gameObject.CompareTag("Collectable"))
            {
                GameObject collect = collider.gameObject;
                int Collecter = collect.GetComponent<Collects>().type2;
                switch(Collecter)
                {
                    case 1:
                    Bread1(collect);
                    break;

                    case 2:
                    Rice1();
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
    
        void Bread1(GameObject spawn)
        {
            if (GetComponent<BirdScript>().player == 1)
                MinigameController.Instance.AddScore(1, 10);
            else if (GetComponent<BirdScript>().player == 2)
                MinigameController.Instance.AddScore(2, 10);
            Instantiate(spawn, new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-4.8f, 4.8f), 0f), Quaternion.identity);
            Score1+=10;
        }

        void Rice1()
        {
            if (GetComponent<BirdScript>().player == 1)
                MinigameController.Instance.AddScore(1, 1);
            else if (GetComponent<BirdScript>().player == 2)
                MinigameController.Instance.AddScore(2, 1);
            Score1++;
        }

        public PowerupType getCurrent1(){
            return currentPowerup;
        }

    }
}