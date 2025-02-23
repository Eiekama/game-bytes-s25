using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public enum PowerupType
    {
        EnergyShield,
        RiceMagnet,
        LimeBoost,
        Stopwatch,
        MushroomFlip,
        SwapWarp,
        OneUp,
        None
    }
    public class Powerup : MonoBehaviour
    {
        [SerializeField] public PowerupType type;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;
            GameObject player = collider.gameObject;
            Debug.Log(type + " triggered by " + player.name);
            
        }
    }
}