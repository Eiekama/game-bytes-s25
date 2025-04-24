using System;
using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    
    // Largely copied from Rice.cs
    public class PlayerMagnet : MonoBehaviour
    {
        private Rigidbody2D rb;
        [SerializeField] private Player player1;
        [SerializeField] private Player player2;
        private BirdScript p1BS;
        private BirdScript p2BS;

        // If there are any PlayerMagnets yet. (Incl. the one off to the side.)
        private static bool OneExists = false;
        public bool enabled;
    
        public const float MAX_MAGNET_DISTANCE = 9999f;
        public const float FRICTION = 0.75f;
        public const float MAGNET_STRENGTH = 100f;
        public const float MAX_SPEED = 3f;

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            p1BS = player1.GetComponent<BirdScript>();
            p2BS = player2.GetComponent<BirdScript>();
            // if (OneExists)
            // {
            //     enabled = true;
            // }
            // else
            // {
            //     enabled = false;
            //     OneExists = true;
            // }
            enabled = false;
        }
        void FixedUpdate()
        {
            Player magnetPlayer;
            Vector3 pos = transform.position;
            
            float p1Dist = Vector3.Distance(pos, player1.transform.position);
            float p2Dist = Vector3.Distance(pos, player2.transform.position);
            float dist;

            // Get closest living player.
            if ((p1BS.dead && p2BS.dead) || !enabled)
            {
                rb.velocity *= FRICTION;
                return;
            } else if (p1BS.dead)
            {
                magnetPlayer = player2;
                dist = p2Dist;
            } else if (p2BS.dead)
            {
                magnetPlayer = player1;
                dist = p1Dist;
            } else
            {
                bool p1 = p1Dist < p2Dist;
                magnetPlayer = p1 ? player1 : player2;
                dist = p1 ? p1Dist : p2Dist;
            }
            
            

            // float dist = Vector3.Distance(gameObject.transform.position, magnetPlayer.gameObject.transform.position);
            if (dist > MAX_MAGNET_DISTANCE) {
                rb.velocity *= FRICTION;
                return;
            }
        
            float speed = Math.Min(MAX_SPEED, MAGNET_STRENGTH * 1 / (dist * dist));
            Vector2 direction = (magnetPlayer.gameObject.transform.position - gameObject.transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }
}