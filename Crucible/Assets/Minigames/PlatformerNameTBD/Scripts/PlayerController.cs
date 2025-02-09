using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;

    /// <summary>
    /// This function is called once.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// This function is called once per frame.
    /// </summary>
    void Update()
    {
        // Handle input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground check currently beams a ray from the center of the capsule towards the ground and checks if it hit the ground layer.
        // TODO: Find a better way to mark this.
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // && isGrounded
        {
            Debug.Log("Try Jumping");
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// Draws a line from the center of the capsule to the ground to show where it marks ground status.
    /// TODO: Find a better way to mark this.
    /// </summary>
    void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.grey;

        // Draw a line representing the ground check ray
        Vector2 start = transform.position;
        Vector2 end = start + Vector2.down * groundCheckDistance;
        Gizmos.DrawLine(start, end);
    }
}
