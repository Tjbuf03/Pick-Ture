using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public LayerMask groundLayer; // Layer for the ground
    public float groundCheckDistance = 0.1f; // Distance for the ground check

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle left and right movement with A and D keys only
        float move = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            move = -1f;  // Move left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move = 1f;   // Move right
        }

        // Apply movement velocity to the Rigidbody
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Raycast downwards to check if the player is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // Debug the raycast
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);
        Debug.Log("Is Grounded: " + isGrounded); // Log grounded state

        // Handle jump with spacebar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}



