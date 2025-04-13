using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Right,
    Neutral
}

public class BirdScript : MonoBehaviour
{
    public bool dead;

    public int player;
    [SerializeField] private KeyCode flap;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpStrength;

    [SerializeField] private float gravityMax;
    
    public Direction direction;
    // How quickly the bird changes direction (left/right) (higher -> faster)
    public const float TRACTION = 0.25f;
    private SpriteRenderer sprite;

    Animator birdAnim;
    
    Rigidbody2D rb;

    float gravStorage;

    public AudioSource[] jumps_Powerups;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravStorage = rb.gravityScale;
        birdAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        jumps_Powerups = GetComponents<AudioSource>();
        direction = Direction.Neutral;
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
        if(!dead){
            if(Input.GetKeyDown(flap)){
                flappy();
                jumps_Powerups[UnityEngine.Random.Range(0,3)].Play();
            }

            if(Input.GetKeyDown(left)){
                sprite.flipX = true;
                direction = Direction.Left;
            } else if(Input.GetKeyDown(right)) {
                sprite.flipX = false;
                direction = Direction.Right;
            }
            if(direction == Direction.Left) {
                //transform.Translate(Vector2.left * (Time.fixedDeltaTime * moveSpeed));
                float xVelocity = rb.velocity.x - TRACTION;
                xVelocity = Math.Max(xVelocity, -2*moveSpeed);
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
            } else if (direction == Direction.Right) {
                //transform.Translate(Vector2.right * (Time.fixedDeltaTime * moveSpeed));
                float xVelocity = rb.velocity.x + TRACTION;
                xVelocity = Math.Min(xVelocity, 2*moveSpeed);
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
            }

            
        } else {
            // DeathEffects();
        }
    }

    public void DeathEffects()
    {
        rb.gravityScale = 0.0f;
        // if (rb.velocity.x != 0.0f){
        // TODO: Next line s/b unnecessary:
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        rb.velocity = new Vector2(0.0f, 0.0f);
        StartCoroutine(deathPause());
        // }
        Debug.Log("Bird is dead");
    }

    private void FixedUpdate()
    {
        // Might just be easier to use the rigidbody for gravity instead of manually adding forces for it
        //if(rb.velocity.y > gravityStrength*-6.0f)
           // rb.AddForce(Vector2.down*gravityStrength,ForceMode2D.Impulse);
        if (!dead){
            if(rb.velocity.y < gravityMax*-6.0f)
                rb.gravityScale = 0.0f;

            if (rb.velocity.y >= 6.0f)
                rb.gravityScale = gravStorage;
            
        
            if(rb.position.x > 10.5f || rb.position.x < -10.5f)
                transform.Translate((Vector2.right*-1)*(float)rb.position.x*1.95f);

            if(rb.position.y > 5.25f || rb.position.y < -5.25f)
                transform.Translate((Vector2.up*-1)*(float)rb.position.y*1.95f);      
        }      
    }

    private void flappy(){
        birdAnim.SetTrigger("Flap");
        if (rb.velocity.y < jumpStrength+jumpStrength*1.33){
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(Vector2.up*jumpStrength,ForceMode2D.Impulse);
        }
        // rb.velocity.Set(rb.velocity.x, jumpStrength); // <Same?
    }

    private IEnumerator deathPause(){
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(0.0f, -10.0f);
    }

    public Animator getAnim(){
        return birdAnim;
    }
}
