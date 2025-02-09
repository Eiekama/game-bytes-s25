using System;
using Unity.UNetWeaver;
using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public class Player : MonoBehaviour
    {
        private void Update()
        {
            // Debug.Log("New frame");
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Controls notes:
            // Input.getAxis("") or Input.getButton("") or bool Input.getKey(key)
            // Or getKeyDown(key) / getKeyUp(key) for the exact frames
            // Edit->Project Settings->Input Manager to see the axis names
            
            // To add a force (e.g. when jumping:)
            // Rigidbody rb = ...;
            // rb.AddForce(Vector2.up * 10f, ForceMode.Impulse);
            Debug.Log("Player Collided with " + collider.name);
        }
    }
}