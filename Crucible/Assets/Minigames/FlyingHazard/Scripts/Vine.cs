using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public GameObject topVine;
    public GameObject bodyVine;

    Animator vineAnim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(vineActivate());
        vineAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator vineActivate(){
        GameObject[] clone = new GameObject[20];
        GetComponent<ProjectileScript>().changeDirection(new Vector2(0,1));
        for (int i = 1; i < 21; i++){
            clone[i-1] = Instantiate(bodyVine, (Vector2)transform.position - new Vector2(0, 0.5f*i), Quaternion.identity);
            clone[i-1].transform.rotation *= Quaternion.AngleAxis(90, Vector3.forward);
        }
        yield return new WaitForSeconds(4.7f);
        for (int i = 1; i < 21; i++){
            clone[i-1].GetComponent<ProjectileScript>().changeDirection(new Vector2(0,0));
        }
        GetComponent<ProjectileScript>().changeDirection(new Vector2(0,0));
        vineAnim.SetBool("Halt", true);
        yield return new WaitForSeconds(10f);
        vineAnim.SetBool("Halt", false);
        GetComponent<ProjectileScript>().changeDirection(new Vector2(0,-1));
        for (int i = 1; i < 21; i++){
            clone[i-1].GetComponent<ProjectileScript>().changeDirection(new Vector2(-1,0));
            clone[i-1].AddComponent<Destroyer>();
        }
    }
}
