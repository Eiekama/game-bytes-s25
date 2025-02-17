using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawning : MonoBehaviour
{

    public bool on;
    public GameObject weapon1;
    
    public GameObject weapon2;
    public float inter1;
    
    public float inter2;
    // Start is called before the first frame update
    void Start()
    {
        if (on == true){
            StartCoroutine(spawn1(inter1, weapon1));
            StartCoroutine(spawn2(inter2, weapon2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawn1(float interval, GameObject enemy){
        GameObject clone;
        yield return new WaitForSeconds(interval);
        clone = Instantiate(enemy, new Vector3(Random.Range(-13f, 13f), -5f, 0f), Quaternion.identity);
        clone.AddComponent<Destroyer>();
        StartCoroutine(spawn1(interval + Random.Range(-0.5f, 0.5f), enemy));
    }

    private IEnumerator spawn2(float interval, GameObject enemy){
        yield return new WaitForSeconds(interval);
        GameObject clone = Instantiate(enemy, new Vector3(Random.Range(-13f, 13f), 5f, 0f), Quaternion.identity);
        clone.AddComponent<Destroyer>();
        StartCoroutine(spawn2(interval + Random.Range(-0.5f, 0.5f), enemy));

    }
}
