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

    [Header("Glide")]
    [SerializeField] private bool canGlide;

    [Header("Jump Cooldown")]
    [SerializeField] private float jumpCooldown = 0.2f;  // Set a cooldown time in seconds
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
        transform.position += new Vector3(x_value * Time.deltaTime, 0f, 0f);

        playerAnimator.SetFloat("Speed", Mathf.Abs(x_value));

        if (x_value < 0) spr.flipX = true;
        if (x_value > 0) spr.flipX = false;

        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);

        // Jump only if grounded and cooldown has passed
        if (Input.GetKeyDown(KeyCode.Space) && onGround && jumpCooldownTimer <= 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
            onGround = false;
            jumpCooldownTimer = jumpCooldown;  // Reset the cooldown timer

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
}