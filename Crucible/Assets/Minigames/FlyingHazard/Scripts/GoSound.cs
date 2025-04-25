using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSound : MonoBehaviour
{
    // Start is called before the first frame update

    AudioSource[] a;
    void Start()
    {
        a = GetComponents<AudioSource>();
        StartCoroutine(b());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator b(){
        yield return new WaitForSeconds(3);
        a[1].Play();
        yield return new WaitForSeconds(1.5f);
        a[0].Play();
    }
}
