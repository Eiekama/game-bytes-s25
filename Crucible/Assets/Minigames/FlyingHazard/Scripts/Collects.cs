using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Collects : MonoBehaviour
    {
        [SerializeField] public int type2;

        public Collectable thing;
        public enum Collectable
        {
            Bread,
            Rice,
        }

        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;
            GameObject player = collider.gameObject;
            Debug.Log(type2 + " triggered by " + player.name);
            
        }
}
