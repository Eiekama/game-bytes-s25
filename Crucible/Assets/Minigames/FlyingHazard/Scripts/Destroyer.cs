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
        if (transform.position.x > 15 || transform.position.x < -15 
            || transform.position.y > 6 || transform.position.y < -6)
            Destroy(gameObject);
    }
}
