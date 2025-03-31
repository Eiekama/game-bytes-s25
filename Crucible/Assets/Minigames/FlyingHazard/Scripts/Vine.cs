using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public GameObject topVine;
    public GameObject bodyVine;
    // Start is called before the first frame update
    void Start()
    {
        GameObject clone;
        for (int i = 1; i < 28; i++){
            clone = Instantiate(bodyVine, (Vector2)transform.position - new Vector2(0, 0.5f*i), Quaternion.identity);
            clone.transform.rotation *= Quaternion.AngleAxis(90, Vector3.forward);
            clone.AddComponent<VineDestroy>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
