using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    [SerializeField] private KeyCode flap;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpStrength;

    [SerializeField] private float gravityMax;

    Animator birdAnim;
    
    Rigidbody2D rb;

    float gravStorage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravStorage = rb.gravityScale;
        birdAnim = GetComponent<Animator>();
        //When the actual game is made this will cause the bird to start moving automatically.
        //I'm not currently running it as it's annoying for testing collisions.
        //rb.velocity = new Vector2(moveSpeed*2.0f, rb.velocity.y);
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(flap)){
            flappy();
        }

        if(Input.GetKeyDown(left)){
            GetComponent<SpriteRenderer>().flipX = true;
            //transform.Translate(Vector2.left * (Time.fixedDeltaTime * moveSpeed));
            rb.velocity = new Vector2(moveSpeed*-2.0f, rb.velocity.y);
        }

        if(Input.GetKeyDown(right)){
            GetComponent<SpriteRenderer>().flipX = false;
            //transform.Translate(Vector2.right * (Time.fixedDeltaTime * moveSpeed));
            rb.velocity = new Vector2(moveSpeed*2.0f, rb.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        // Might just be easier to use the rigidbody for gravity instead of manually adding forces for it
        //if(rb.velocity.y > gravityStrength*-6.0f)
           // rb.AddForce(Vector2.down*gravityStrength,ForceMode2D.Impulse);

        if(rb.velocity.y < gravityMax*-6.0f)
            rb.gravityScale = 0.0f;

        if (rb.velocity.y >= 6.0f)
            rb.gravityScale = gravStorage;

        if(rb.position.x > 9 || rb.position.x < -9)
            transform.Translate((Vector2.right*-1)*(float)rb.position.x*1.95f);

        if(rb.position.y > 5 || rb.position.y < -5)
            transform.Translate((Vector2.up*-1)*(float)rb.position.y*1.95f);            
    }

    private void flappy(){
        birdAnim.SetTrigger("Flap");
        if (rb.velocity.y < jumpStrength+jumpStrength*1.33){
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(Vector2.up*jumpStrength,ForceMode2D.Impulse);
        }
        // rb.velocity.Set(rb.velocity.x, jumpStrength); // <Same?
    }
}
