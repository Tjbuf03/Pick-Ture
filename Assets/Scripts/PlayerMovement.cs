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
    [SerializeField] private bool canMove;

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

    [Header("Cannon")]
    [SerializeField] private bool canShoot;
    [SerializeField] private float cannonCooldown;
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private GameObject cannonBallPoint;
    [SerializeField] private GameObject leftcannonBall;
    [SerializeField] private GameObject leftcannonBallPoint;
    [SerializeField] private bool cannonFire;


    [Header("Jump Cooldown")]
    [SerializeField] private float jumpCooldown = 0.2f;
    private float jumpCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        onGround = true;
        canMove = true;
        canGlide = false;
        canShoot = false;
        cannonFire = false;
        MainManager.Instance.isRestarting = false;
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

        if (canMove)
        {
            //2 directional movement inside bool to be called whenever movement is locked
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
        }
        

        //Glide mechanic unlocks when bool in Main Manager is set to true
        if (Input.GetKeyUp(KeyCode.Space) && !onGround && MainManager.Instance.GlideUnlocked)
        {
            canGlide = true;
        }

        //When player presses space, lets go, then presses space again in air, the player gravity switches off replaced by a smaller downward force
        if (canGlide)
        {
            if (Input.GetKey(KeyCode.Space) && rb.velocity.y <= -1.5f)
            {
                rb.gravityScale = 0f;
                //Adds force to the y axis that you can change to change glide falling rate
                rb.AddForce(new Vector2(0f, -0.05f));

                playerAnimator.SetBool("IsGliding", true);
            }
            else
            {
                rb.gravityScale = 1f;

                playerAnimator.SetBool("IsGliding", false);
            }
        }

        //Cannon mechanic unlocks when bool in MainManager is set to true, and when player is on the ground
        if(Input.GetKeyDown(KeyCode.C) && MainManager.Instance.CannonUnlocked && onGround)
        {
            canShoot = true;
            playerAnimator.SetBool("IsShooting", true);
            //Enables cannonballs to spawn
            cannonFire = true;
        }

        if(canShoot)
        {
            //Cooldown begins, set the length in inspector
            cannonCooldown -= Time.deltaTime;

            //Stops player movement
            canMove = false;

            //Shooting projectile when character pulls out cannon
            if(cannonCooldown <= 0.4f && cannonFire)
            {
                    //Shooting facing right
                    if(spr.flipX == false)
                    {
                        Instantiate(cannonBall, cannonBallPoint.transform.position, cannonBallPoint.transform.rotation);
                    }
                    //Shooting facing left
                    if(spr.flipX)
                    {
                        Instantiate(leftcannonBall, leftcannonBallPoint.transform.position, leftcannonBallPoint.transform.rotation);
                    }

                    //Disables more than one cannonball from spawning
                    cannonFire = false;
            }

            //Shooting state ends when cooldown runs out
            if(cannonCooldown <= 0f)
            { 
                //reset cooldown and bools
                cannonCooldown = 1f;
                canShoot = false;
                canMove = true;

                playerAnimator.SetBool("IsShooting", false);
            } 
        }

    }


    //When player hits the ground jump and glide settings turn off
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                onGround = true;
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsGliding", false);
                canGlide = false;
                rb.gravityScale = 1f;
            }
        }
    }

    //When player does not jump but leaves the ground play jump anim
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
