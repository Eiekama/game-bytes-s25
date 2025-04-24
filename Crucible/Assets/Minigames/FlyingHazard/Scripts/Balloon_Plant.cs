using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon_Plant : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectile;

    Animator BPanim;
    void Start()
    {
        BPanim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator activatePlant(){
        for (int i = 0; i < 4; i++){
            if (i >= 2)
            GetComponent<SpriteRenderer>().flipX = true;
            yield return new WaitForSeconds(0.35f);
            BPanim.SetTrigger("Fire");
            yield return new WaitForSeconds(0.1f);
            if (i < 2){
                GameObject clone = Instantiate(projectile, (Vector2)transform.position + new Vector2(0.2f, 0), Quaternion.identity);
                clone.GetComponent<ProjectileScript>().changeDirection(new Vector2(1,0));
                clone.AddComponent<Destroyer>();
            } else {
                GameObject clone = Instantiate(projectile, (Vector2)transform.position + new Vector2(-0.2f, 0), Quaternion.identity);
                clone.GetComponent<ProjectileScript>().changeDirection(new Vector2(1,0));
                clone.transform.Rotate(0,0,180f);
                clone.AddComponent<Destroyer>();
            }
            yield return new WaitForSeconds(0.7f);
        }
        yield return new WaitForSeconds(0.5f);  
    }
}
