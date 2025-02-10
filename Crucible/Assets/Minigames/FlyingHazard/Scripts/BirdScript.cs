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

    [SerializeField] private float jumpStrength;

    [SerializeField] private float gravityStrength;

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D something = GetComponent<Rigidbody2D>();

        if(Input.GetKeyDown(flap)){
            flappy();
        }

        if(Input.GetKey(left)){
            transform.Translate(Vector2.left *Time.deltaTime *moveSpeed);
        }

        if(Input.GetKey(right)){
            transform.Translate(Vector2.right *Time.deltaTime *moveSpeed);
        }

        if(something.velocity.y > gravityStrength*-6.0f)
            something.AddForce(Vector2.down*gravityStrength/50.0f,ForceMode2D.Impulse);

        if(something.position.x > 9 || something.position.x < -9)
            transform.Translate((Vector2.right*-1)*(float)something.position.x*1.95f);

        if(something.position.y > 5 || something.position.y < -5)
            transform.Translate((Vector2.up*-1)*(float)something.position.y*1.95f);            
    }

    void flappy(){
        Rigidbody2D something = GetComponent<Rigidbody2D>();
        if (something.velocity.y < jumpStrength+jumpStrength*1.33){
            something.velocity = new Vector2(something.velocity.x, 0.0f);
            something.AddForce(Vector2.up*jumpStrength,ForceMode2D.Impulse);
        }
    }
}
