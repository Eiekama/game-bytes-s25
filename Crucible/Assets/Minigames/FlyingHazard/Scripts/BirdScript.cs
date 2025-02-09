using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private KeyCode flap;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    [SerializeField] private float moveSpeed;

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(flap)){
            flappy();
        }

        if(Input.GetKey(left)){
            transform.Translate(Vector2.left *Time.deltaTime *moveSpeed);
        }

        if(Input.GetKey(right)){
            transform.Translate(Vector2.right *Time.deltaTime *moveSpeed);
        }

        if (GetComponent<Rigidbody2D>().velocity.y > -6.0)
            GetComponent<Rigidbody2D>().AddForce(Vector2.down*0.02f,ForceMode2D.Impulse);

    }

    void flappy(){
        Rigidbody2D something = GetComponent<Rigidbody2D>();
        if (something.velocity.y < 4.0){
            something.velocity = new Vector2(something.velocity.x, 0.0f);
            something.AddForce(Vector2.up*3.0f,ForceMode2D.Impulse);
        }
    }
}
