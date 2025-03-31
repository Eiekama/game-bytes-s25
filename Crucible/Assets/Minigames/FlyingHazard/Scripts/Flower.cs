using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject projectile;
    public GameObject bulletFlower;
    void Start()
    {
        StartCoroutine(activateSpit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private IEnumerator activateSpit(){
        //spitAnim.SetTrigger("Spawn");
        yield return new WaitForSeconds(0.5f);
        //spitAnim.SetBool("Spawned", true);
        for (int i = 0; i < 3; i++){
            //spitAnim.SetTrigger("Spit");
            yield return new WaitForSeconds(0.3f);
            GameObject clone1 = Instantiate(projectile, (Vector2)transform.position + new Vector2(0, 0.4f), Quaternion.identity);
            GameObject clone2 = Instantiate(projectile, (Vector2)transform.position + new Vector2(0.1f, 0.3f), Quaternion.identity);
            GameObject clone3 = Instantiate(projectile, (Vector2)transform.position + new Vector2(-0.1f, 0.3f), Quaternion.identity);
            clone1.GetComponent<ProjectileScript>().changeDirection(new Vector2(0,1));
            clone1.AddComponent<Destroyer>();
            clone2.GetComponent<ProjectileScript>().changeDirection(new Vector2(1,1));
            clone2.AddComponent<Destroyer>();
            clone3.GetComponent<ProjectileScript>().changeDirection(new Vector2(-1,1));
            clone3.AddComponent<Destroyer>();
            yield return new WaitForSeconds(2.0f);
        }
        yield return new WaitForSeconds(0.5f);
        //spitAnim.SetTrigger("Die");   
    }
}