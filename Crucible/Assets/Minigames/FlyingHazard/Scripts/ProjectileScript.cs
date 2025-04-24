using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocity;
    [SerializeField] private Vector2 direction;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity*Time.deltaTime*direction);
    }

    public void changeDirection(Vector2 input){
        direction = input;
    }
}
