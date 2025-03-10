using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawning : MonoBehaviour
{

    public bool on;
    public GameObject bird1;
    Minigames.FlyingHazard.Scripts.Player t1;
    public GameObject bird2;
    Minigames.FlyingHazard.Scripts.Player t2;
    public GameObject bread;
    public GameObject rice;
    public GameObject weapon1;
    public GameObject weapon2;
    public float inter1;
    public float inter2;

    public float breadint;

    public float powerint;
    public float riceint;

    private int powerupCount = 0;

    [SerializeField] private int powerupMax;
    public GameObject[] power;
    // Start is called before the first frame update
    void Start()
    {
        t1 = bird1.GetComponent<Minigames.FlyingHazard.Scripts.Player>();
        t2 = bird2.GetComponent<Minigames.FlyingHazard.Scripts.Player>();
        if (on == true){
            Instantiate(bread, new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-4.8f, 4.8f), 0f), Quaternion.identity);
            StartCoroutine(spawn1(inter1, weapon1));
            StartCoroutine(spawn2(inter2, weapon2));
            StartCoroutine(spawn3(riceint, rice));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupCount < powerupMax && on == true && t1.getCurrent() == Minigames.FlyingHazard.Scripts.PowerupType.None && t2.getCurrent() == Minigames.FlyingHazard.Scripts.PowerupType.None)
            StartCoroutine(spawn4(powerint + Random.Range(-2f,2f), power));
    }

    private IEnumerator spawn1(float interval, GameObject enemy){
        GameObject clone;
        yield return new WaitForSeconds(interval);
        clone = Instantiate(enemy, new Vector3(Random.Range(-9f, 9f), -5f, 0f), Quaternion.identity);
        clone.AddComponent<Destroyer>();
        if (interval < 0.25f)
            interval += 0.5f;
        StartCoroutine(spawn1(interval + Random.Range(-0.5f, 0.5f), enemy));
    }

    private IEnumerator spawn2(float interval, GameObject enemy){
        yield return new WaitForSeconds(interval);
        GameObject clone = Instantiate(enemy, new Vector3(Random.Range(-9f, 9f), 5f, 0f), Quaternion.identity);
        clone.AddComponent<Destroyer>();
        if (interval < 0.25f)
            interval += 0.5f;
        StartCoroutine(spawn2(interval + Random.Range(-0.5f, 0.5f), enemy));

    }

    private IEnumerator spawn3(float interval, GameObject enemy){
        yield return new WaitForSeconds(interval);
        GameObject clone = Instantiate(enemy, new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0f), Quaternion.identity);
        StartCoroutine(spawn3(interval, enemy));
    }

    private IEnumerator spawn4(float interval, GameObject[] enemy){
        powerupCount++;
        yield return new WaitForSeconds(interval);
        int a = Random.Range(0, enemy.Length);
        GameObject clone = Instantiate(enemy[a], new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0f), Quaternion.identity);
    }


    public void subtractPowerupCount(){
        powerupCount--;
    }

    public IEnumerator flickerSpawn(GameObject thing)
    //Idk why this doesn't work
    {
        SpriteRenderer a = thing.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 5; i++){
            yield return new WaitForSeconds(0.2f);
            a.color = Color.black;
            yield return new WaitForSeconds(0.2f);
            a.color = Color.white;
        }   
    }
}
