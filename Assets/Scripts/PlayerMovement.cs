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
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float groundCheckWidth = 0.5f;
    [SerializeField] private Vector2 groundCheckOffset = Vector2.zero;

    [Header("Wall Check Settings")]
    [SerializeField] private float wallCheckDistance = 0.1f; // Distance to check for walls
    private bool isTouchingWall;

    [Header("Glide")]
    [SerializeField] private bool canGlide;

    [Header("Jump Cooldown")]
    [SerializeField] private float jumpCooldown = 0.2f;
    private float jumpCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        onGround = true;
        canGlide = false;
    }

    void Update()
    {
        // Update the jump cooldown timer
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;

        float x_value = Input.GetAxisRaw("Horizontal") * speed;

        // Wall Check
        Vector2 wallCheckDirection = x_value < 0 ? Vector2.left : Vector2.right;
        isTouchingWall = Physics2D.Raycast(transform.position, wallCheckDirection, wallCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, wallCheckDirection * wallCheckDistance, Color.blue);

        // Stop horizontal movement if touching a wall
        if (isTouchingWall)
        {
            x_value = 0;
        }

        transform.position += new Vector3(x_value * Time.deltaTime, 0f, 0f);

        playerAnimator.SetFloat("Speed", Mathf.Abs(x_value));

        if (x_value < 0) spr.flipX = true;
        if (x_value > 0) spr.flipX = false;

        // Ground Check using OverlapBox
        Vector2 boxCenter = (Vector2)transform.position + groundCheckOffset + Vector2.down * (groundCheckDistance / 2);
        Vector2 boxSize = new Vector2(groundCheckWidth, groundCheckDistance);
        onGround = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
        Debug.DrawLine(boxCenter - Vector2.right * groundCheckWidth / 2, boxCenter + Vector2.right * groundCheckWidth / 2, Color.red);

        // Jump only if grounded and cooldown has passed
        if (Input.GetKeyDown(KeyCode.Space) && onGround && jumpCooldownTimer <= 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
            onGround = false;
            jumpCooldownTimer = jumpCooldown;

            playerAnimator.SetBool("IsJumping", true);
        }

        if (Input.GetKeyUp(KeyCode.Space) && !onGround)
        {
            canGlide = true;
        }

        if (canGlide)
        {
            if (Input.GetKey(KeyCode.Space) && rb.velocity.y <= -1.5f)
            {
                rb.gravityScale = 0f;
                rb.AddForce(new Vector2(0f, -0.1f));
            }
            else
            {
                rb.gravityScale = 1f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                onGround = true;
                playerAnimator.SetBool("IsJumping", false);
                canGlide = false;
                rb.gravityScale = 1f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
            playerAnimator.SetBool("IsJumping", true);
        }
    }

    // Draw the box in the scene view for visual debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 boxCenter = (Vector2)transform.position + groundCheckOffset + Vector2.down * (groundCheckDistance / 2);
        Vector2 boxSize = new Vector2(groundCheckWidth, groundCheckDistance);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
