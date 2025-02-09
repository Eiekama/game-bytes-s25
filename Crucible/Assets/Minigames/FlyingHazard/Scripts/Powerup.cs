using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public class Powerup : MonoBehaviour
    {
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;
            
            // TODO: do stuff
            Debug.Log("Powerup triggered");
            Destroy(gameObject);
        }
    }
}