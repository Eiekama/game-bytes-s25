using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionT
{
    Left,
    Right,
    Neutral
}

public class BSTrail : MonoBehaviour
{

    private SpriteRenderer sprite;
    [SerializeField] private KeyCode flap;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpStrength;

    [SerializeField] private float gravityMax;
    
    public DirectionT direction;
    // How quickly the bird changes direction (left/right) (higher -> faster)
    public const float TRACTION = 0.25f;    
    Rigidbody2D rb;

    float gravStorage;



    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gravStorage = rb.gravityScale;
        direction = DirectionT.Neutral;
        //When the actual game is made this will cause the bird to start moving automatically.
        //I'm not currently running it as it's annoying for testing collisions.
        // direction = Direction.Left;
        // sprite.flipX = true;
        // Shouldn't need?:
        //rb.velocity = new Vector2(moveSpeed*2.0f, rb.velocity.y);
    }

    // Update is called once per frame
    private void Update()
    {
            if(Input.GetKeyDown(flap)){
                flappy();
            }

            if(Input.GetKeyDown(left)
            || Input.GetKeyUp(right) && Input.GetKey(left)){
                sprite.flipX = true;
                direction = DirectionT.Left;
            } else if(Input.GetKeyDown(right) 
                   || Input.GetKeyUp(left) && Input.GetKey(right)) {
                // Go right:
                sprite.flipX = false;
                direction = DirectionT.Right;
            }
            
            if(direction == DirectionT.Left) {
                //transform.Translate(Vector2.left * (Time.fixedDeltaTime * moveSpeed));
                float xVelocity = rb.velocity.x - TRACTION;
                xVelocity = Math.Max(xVelocity, -2*moveSpeed);
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
            } else if (direction == DirectionT.Right) {
                //transform.Translate(Vector2.right * (Time.fixedDeltaTime * moveSpeed));
                float xVelocity = rb.velocity.x + TRACTION;
                xVelocity = Math.Min(xVelocity, 2*moveSpeed);
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
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
            
            const float WIDTH = Spawning.WIDTH;
            const float HEIGHT = Spawning.HEIGHT;
        
            if(rb.position.x > WIDTH * 1.025f || rb.position.x < -WIDTH * 1.025f)
                transform.Translate((Vector2.right*-1)*(float)rb.position.x*2f);

            if(rb.position.y > HEIGHT * 1.05f || rb.position.y < -HEIGHT * 1.05f)
                transform.Translate((Vector2.up*-1)*(float)rb.position.y*1.95f);         
    }

    private void flappy(){
        if (rb.velocity.y < jumpStrength+jumpStrength*1.33){
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(Vector2.up*jumpStrength,ForceMode2D.Impulse);
        }
        // rb.velocity.Set(rb.velocity.x, jumpStrength); // <Same?
    }
}

