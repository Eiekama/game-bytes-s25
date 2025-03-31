using System.Collections;
using System.Collections.Generic;
using Minigames.FlyingHazard.Scripts;
using UnityEngine;



public class Spawning : MonoBehaviour
{

    public bool enabled;
    public GameObject bird1;

    private BirdScript bs1;
    Minigames.FlyingHazard.Scripts.Player t1;
    public GameObject bird2;

    private BirdScript bs2;
    Minigames.FlyingHazard.Scripts.Player t2;
    public GameObject bread;
    public GameObject rice;
    public GameObject spikeBalloonEnemy;
    public GameObject dropEnemy;
    public GameObject muncher;
    public GameObject spitSeed;
    public GameObject bulletSeed;
    public GameObject vineSeed;


    public float inter1;
    public float inter2;

    public float muncherint;

    public float breadint;

    public float powerint;
    public float riceint;

    public float Scaling;
    private int powerupCount = 0;

    [SerializeField] private int powerupMax;
    public GameObject[] powerups;
    // Start is called before the first frame update
    void Start()
    {
        t1 = bird1.GetComponent<Minigames.FlyingHazard.Scripts.Player>();
        t2 = bird2.GetComponent<Minigames.FlyingHazard.Scripts.Player>();
        bs1 = bird1.GetComponent<BirdScript>();
        bs2 = bird2.GetComponent<BirdScript>();
        if (enabled){
            Instantiate(bread,
                new Vector3(UnityEngine.Random.Range(-9f, 9f),
                    UnityEngine.Random.Range(-4.8f, 4.8f), 0f),
                Quaternion.identity);
            StartCoroutine(spawnBalloon(inter1, spikeBalloonEnemy));
            StartCoroutine(spawnDrop(inter2, dropEnemy));
            StartCoroutine(spawnRice(riceint));
            StartCoroutine(munchSpawnTest(muncherint, muncher));
            StartCoroutine(seedSpawn(7, spitSeed, 10));
            StartCoroutine(seedSpawn(20, bulletSeed, 30));
            StartCoroutine(seedSpawn(25, vineSeed, 40));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupCount < powerupMax
            && enabled
            && t1.getPowerup() == Minigames.FlyingHazard.Scripts.PowerupType.None
            && t2.getPowerup() == Minigames.FlyingHazard.Scripts.PowerupType.None)
        {
            StartCoroutine(spawnPowerup(powerint + Random.Range(-2f,2f)));
        }
    }

    // Note: If these spawn functions are only ever called on this specific enemy,
    // can remove the second parameter & reference it directly. 
    private IEnumerator spawnBalloon(float interval, GameObject enemy){
        GameObject clone;
        float timeScaling = (((MinigameController.Instance.GetElapsedTime())/180f)*Scaling)+1;
        yield return new WaitForSeconds(interval/timeScaling);
        clone = Instantiate(enemy, new Vector3(Random.Range(-9.8f, 9.8f), -5f, 0f), Quaternion.identity);
        clone.GetComponent<ProjectileScript>().enabled = false;
        clone.GetComponent<CircleCollider2D>().enabled = false;
        clone.AddComponent<Destroyer>();
        StartCoroutine(flickerSpawn(clone));
        if (interval < 0.5f)
            interval += 0.5f;
        StartCoroutine(spawnBalloon(interval + Random.Range(-0.5f, 0.5f), enemy));
        yield return new WaitForSeconds(2);
        clone.GetComponent<ProjectileScript>().enabled = true;
        clone.GetComponent<CircleCollider2D>().enabled = true;
    }

    private IEnumerator spawnDrop(float interval, GameObject enemy){
        float timeScaling = ((((MinigameController.Instance.GetElapsedTime()+180f)/180f)-1)*Scaling)+1;
        yield return new WaitForSeconds(interval/timeScaling);
        GameObject clone = Instantiate(enemy, new Vector3(Random.Range(-9.8f, 9.8f), 5f, 0f), Quaternion.identity);
        clone.AddComponent<Destroyer>();
        clone.GetComponent<ProjectileScript>().enabled = false;
        clone.GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(flickerSpawn(clone));   
        if (interval < 0.5f)
            interval += 0.5f;
        StartCoroutine(spawnDrop(interval + Random.Range(-0.5f, 0.5f), enemy));
        yield return new WaitForSeconds(2);
        clone.GetComponent<ProjectileScript>().enabled = true;
        clone.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    private IEnumerator spawnRice(float interval){
        float timeScaling = ((((MinigameController.Instance.GetElapsedTime()+180f)/180f)-1)*Scaling)+1;
        Debug.Log("Scale:" + timeScaling);
        yield return new WaitForSeconds(interval/timeScaling);
        GameObject clone = Instantiate(rice, new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-4.7f, 4.7f), 0f), Quaternion.identity);
        //StartCoroutine(flickerSpawn(clone));
        //clone.GetComponent<CapsuleCollider2D>.enabled = false;
        StartCoroutine(spawnRice(interval));
        //clone.GetComponent<CapsuleCollider2D>.enabled = true;
    }

    private IEnumerator spawnPowerup(float interval){
        powerupCount++;
        yield return new WaitForSeconds(interval);
        int a = Random.Range(0, powerups.Length);
        GameObject clone = Instantiate(powerups[a], new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-5f, 5f), 0f), Quaternion.identity);
        StartCoroutine(flickerSpawn(clone));
        clone.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        clone.GetComponent<CircleCollider2D>().enabled = true;
        if (clone.GetComponent<PlayerMagnet>() != null)
        {
            clone.GetComponent<PlayerMagnet>().enabled = true;
        }
    }

    private IEnumerator munchSpawnTest(float interval, GameObject enemy){
        Vector2 xdir = new Vector2(1,0); // Entire top bit is just declaring variables to use
        Vector2 aim;
        float a;
        GameObject clone;
        float timeScaling = ((((MinigameController.Instance.GetElapsedTime()+180f)/180f)-1)*Scaling)+1;
        yield return new WaitForSeconds(interval/timeScaling); // This is regulating how long it takes for a muncher to spawn after running coroutine
        if (interval < 3f)
            interval += 1f; // Hardcapping interval so not too many spawn
        StartCoroutine(munchSpawnTest(muncherint + Random.Range(-2f, 2f), muncher)); //Starts another muncher spawn
        if (Random.Range(0.0f, 1.0f) < 0.5){ //The random chooses which side spawns
            clone = Instantiate(enemy, new Vector3(-10f, Random.Range(-5f, 5f), 0f), Quaternion.identity);
            clone.AddComponent<Destroyer>();
            StartCoroutine(flickerSpawn(clone)); 
            clone.GetComponent<ProjectileScript>().enabled = false;            
            clone.GetComponent<CapsuleCollider2D>().enabled = false;
            yield return new WaitForSeconds(2); // 4 lines above is flicker spawning
            clone.GetComponent<CapsuleCollider2D>().enabled = true;
            clone.GetComponent<ProjectileScript>().enabled = true;              
            if (Vector3.Distance(bird1.transform.position, clone.transform.position) 
                < Vector3.Distance(bird2.transform.position, clone.transform.position) &&
                !bs1.dead){ //This if finds which bird is closer and makes sure target is alive
                aim = (Vector2)(bird1.transform.position - clone.transform.position);
                a = Vector2.SignedAngle(xdir, aim);
                clone.transform.rotation = Quaternion.Euler(0.0f, 0.0f, a); //Targetting bird1 with angle a found from bird1
            } else {
                aim = (Vector2)(bird2.transform.position - clone.transform.position);
                a = Vector2.SignedAngle(xdir, aim);
                clone.transform.rotation = Quaternion.Euler(0.0f, 0.0f, a); //Targetting bird2 with angle a found from bird2
            }
        } else if (!bs2.dead){
            clone = Instantiate(enemy, new Vector3(10f, Random.Range(-5f, 5f), 0f), Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().flipX = true; //Flipping the flickering sprite to face left
            clone.AddComponent<Destroyer>();
            StartCoroutine(flickerSpawn(clone));
            clone.GetComponent<CapsuleCollider2D>().enabled = false;
            clone.GetComponent<ProjectileScript>().enabled = false;
            yield return new WaitForSeconds(2);
            clone.GetComponent<SpriteRenderer>().flipX = false; //Flipping back so that the muncher is facing the right way
            clone.GetComponent<CapsuleCollider2D>().enabled = true;
            clone.GetComponent<ProjectileScript>().enabled = true;            
            if (Vector3.Distance(bird1.transform.position, clone.transform.position) 
            < Vector3.Distance(bird2.transform.position, clone.transform.position) &&
            !bs1.dead){
                aim = (Vector2)(bird1.transform.position - clone.transform.position);
                a = Vector2.SignedAngle(xdir, aim);
                clone.transform.rotation = Quaternion.Euler(0.0f, 0.0f, a); //Angle is negative cause facing opposite direction
            } else {
                aim = (Vector2)(bird2.transform.position - clone.transform.position);
                a = Vector2.SignedAngle(xdir, aim);
                clone.transform.rotation = Quaternion.Euler(0.0f, 0.0f, a);
            }   
        }
    }

    public IEnumerator seedSpawn(float interval, GameObject enemy, float spawnStart){
        if (MinigameController.Instance.GetElapsedTime() < spawnStart)
            yield return new WaitForSeconds(spawnStart);
        yield return new WaitForSeconds(interval);
            GameObject clone = Instantiate(enemy, new Vector3(Random.Range(-9f, 9f), 4.8f, 0f), Quaternion.identity);
            clone.GetComponent<ProjectileScript>().changeDirection(new Vector2(0,-1));
            clone.GetComponent<ProjectileScript>().enabled = false;
            clone.GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(flickerSpawn(clone));   
        StartCoroutine(seedSpawn(interval, enemy, spawnStart));
        yield return new WaitForSeconds(2);
            clone.GetComponent<ProjectileScript>().changeDirection(new Vector2(0,-1));
            clone.GetComponent<ProjectileScript>().enabled = true;
            clone.GetComponent<CapsuleCollider2D>().enabled = true;
    }
    public void subtractPowerupCount(){
        powerupCount--;
    }

    public IEnumerator flickerSpawn(GameObject thing)
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
