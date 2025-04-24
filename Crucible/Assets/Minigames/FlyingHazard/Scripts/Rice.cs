using System;
using Minigames.FlyingHazard.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Rice : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    
    public const float MAX_MAGNET_DISTANCE = 5f;
    public const float FRICTION = 0.9f;
    public const float MAGNET_STRENGTH = 10f;
    public const float MAX_SPEED = 7f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Player magnetPlayer;
        if (player1.getPowerup() == PowerupType.RiceMagnet) {
            magnetPlayer = player1;
        } else if (player2.getPowerup() == PowerupType.RiceMagnet) {
            magnetPlayer = player2;
        } else
        {
            rb.velocity = rb.velocity * FRICTION;
            return;
        }

        // distance to the player
        float dist = Vector3.Distance(gameObject.transform.position, magnetPlayer.gameObject.transform.position);
        if (dist > MAX_MAGNET_DISTANCE) {
            rb.velocity = rb.velocity * FRICTION;
            return;
        }
        
        // magnetPlayer is a player with the RiceMagnet, within MAXMAGNETDISTANCE from this rice:
        
        float speed = Math.Min(MAX_SPEED, MAGNET_STRENGTH * 1 / (dist * dist));
        Vector2 direction = (magnetPlayer.gameObject.transform.position - gameObject.transform.position).normalized;
        rb.velocity = direction * speed;
    }
}