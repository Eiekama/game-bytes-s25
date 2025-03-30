using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeedSpawn : MonoBehaviour
{
    public GameObject spawn;
    public float time;

    public float spawnOffset;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -5.0f){
            GameObject clone = Instantiate(spawn, (Vector2)transform.position + new Vector2(0, spawnOffset), Quaternion.identity);
            Destroy(gameObject);
            if (time != 0)
                Destroy(clone, time);
        }
    }

}
