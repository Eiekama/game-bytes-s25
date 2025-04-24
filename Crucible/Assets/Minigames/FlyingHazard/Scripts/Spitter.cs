using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject spitter;

    public GameObject projectile;
    Animator spitAnim;


    void Start()
    {
        spitAnim = GetComponent<Animator>();
        StartCoroutine(activateSpit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator activateSpit(){
        spitAnim.SetTrigger("Spawn");
        yield return new WaitForSeconds(0.5f);
        spitAnim.SetBool("Spawned", true);
        for (int i = 0; i < 3; i++){
            spitAnim.SetTrigger("Spit");
            yield return new WaitForSeconds(0.3f);
            GameObject clone = Instantiate(projectile, (Vector2)transform.position + new Vector2(0, 0.4f), Quaternion.identity);
            clone.GetComponent<ProjectileScript>().changeDirection(new Vector2(1,0));
            clone.transform.rotation *= Quaternion.AngleAxis(90, Vector3.forward);
            clone.AddComponent<Destroyer>();
            yield return new WaitForSeconds(0.7f);
        }
        yield return new WaitForSeconds(0.5f);
        spitAnim.SetTrigger("Die");   
    }
}
