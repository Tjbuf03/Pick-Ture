using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool onGround;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer spr;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckDistance = 0.2f; // Distance for the ground check

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        onGround = true;

        //If Player is returning from a painting, their position will be set to their last spot in the museum
        if(MainManager.Instance.isReturning == true)
        {
            transform.position = MainManager.Instance.PlayerPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 2-directional movement for player
        float x_value = Input.GetAxisRaw("Horizontal") * speed;
        transform.position += new Vector3(x_value * Time.deltaTime, 0f, 0f);

        // To change Idle to Run Animations
        playerAnimator.SetFloat("Speed", Mathf.Abs(x_value));

        //Sprites flip when player changes direction
        if (x_value < 0)
        {
            spr.flipX = true;
        }
        if (x_value > 0)
        {
            spr.flipX = false;
        }

        // Ground check using raycast
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);

        // Jump with ground check
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
            onGround = false;

            // Jump Animation set to true
            playerAnimator.SetBool("IsJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // When player collides with ground, check if grounded from the bottom
        if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                onGround = true;
                // Jump Animation set to false
                playerAnimator.SetBool("IsJumping", false);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // When player exits ground collision, set onGround to false
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
            // Jump Animation set to true
            playerAnimator.SetBool("IsJumping", true);
        }
    }
}
