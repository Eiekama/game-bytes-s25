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
            Debug.Log(name + " collided with " + collider.name);
        }
    }
}