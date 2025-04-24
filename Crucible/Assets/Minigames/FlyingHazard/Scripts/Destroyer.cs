using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        const float WIDTH = Spawning.WIDTH;
        const float HEIGHT = Spawning.HEIGHT;
        
        if (transform.position.x > (WIDTH + 5f) || transform.position.x < -(WIDTH + 5f)
            || transform.position.y > (HEIGHT + 1f) || transform.position.y < -(HEIGHT + 1f))
            Destroy(gameObject);
    }
}
