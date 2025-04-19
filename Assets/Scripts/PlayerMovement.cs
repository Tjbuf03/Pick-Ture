using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    [SerializeField] private Image GlideLoad;
    [SerializeField] private float downwardPull;

    [Header("Cannon")]
    [SerializeField] private bool canShoot;
    [SerializeField] private float cannonCooldown;
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private GameObject cannonBallPoint;
    [SerializeField] private GameObject leftcannonBall;
    [SerializeField] private GameObject leftcannonBallPoint;
    [SerializeField] private bool cannonFire;
    [SerializeField] private Image CannonLoad;
    [SerializeField] private float recoil;

    [Header("TNT")]
    [SerializeField] private bool canIgnite;
    [SerializeField] private bool canspawnTNT;
    [SerializeField] private GameObject TNT;
    [SerializeField] private float TNTCooldown;
    [SerializeField] private Image TNTLoad;
    [SerializeField] private float blastForce;

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
        canspawnTNT = false;
        MainManager.Instance.isRestarting = false;
        //Cannon UI shows it can be fired by default
        CannonLoad.fillAmount = 0f;
        //TNT UI shows it can be fired by default
        TNTLoad.fillAmount = 0f;
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
            //Loading bar empties
            GlideLoad.fillAmount = 0f;
        }

        //When player presses space, lets go, then presses space again in air, the player gravity switches off replaced by a smaller downward force
        if (canGlide)
        {   
            //if player reaches top of their jump and presses space
            if (Input.GetKey(KeyCode.Space) && rb.velocity.y <= -1.5f)
            {
                rb.gravityScale = 0f;
                //Adds force to the y axis that you can change to change glide falling rate
                rb.AddForce(new Vector2(0f, -downwardPull));

                playerAnimator.SetBool("IsGliding", true);
            }
            else
            {
                rb.gravityScale = 1f;
                playerAnimator.SetBool("IsGliding", false);
            }
        }

        //Cannon mechanic unlocks when bool in MainManager is set to true, and when player is on the ground, and when player is not using TNT
        if(Input.GetKeyDown(KeyCode.C) && MainManager.Instance.CannonUnlocked && onGround && canShoot == false && canIgnite == false)
        {
            canShoot = true;
            playerAnimator.SetBool("IsShooting", true);
            //Enables cannonballs to spawn
            cannonFire = true;
            //Cannon Load bar fills
            CannonLoad.fillAmount = 1f;
        }
        //If Player is firing cannon
        if(canShoot)
        {
            //Cooldown begins, set the length in inspector
            cannonCooldown -= Time.deltaTime;

            //Cannon Load bar goes down
            CannonLoad.fillAmount -= Time.deltaTime;

            //Stops player movement
            canMove = false;

            //Shooting projectile when character pulls out cannon
            if (cannonCooldown <= 0.4f && cannonFire)
            {
                //Shooting facing right
                if(spr.flipX == false)
                {
                    Instantiate(cannonBall, cannonBallPoint.transform.position, cannonBallPoint.transform.rotation);
                    rb.AddForce(new Vector2(-recoil, 0f), ForceMode2D.Impulse);
                }
                //Shooting facing left
                if(spr.flipX)
                {
                    Instantiate(leftcannonBall, leftcannonBallPoint.transform.position, leftcannonBallPoint.transform.rotation);
                    rb.AddForce(new Vector2(recoil, 0f), ForceMode2D.Impulse);
                }

                //Disables more than one cannonball from spawning
                cannonFire = false;
            }

            //Shooting state ends when cooldown runs out
            if(cannonCooldown <= 0f)
            { 
                //reset cooldown to same as set in inspector, reset bools
                cannonCooldown = 1f;
                canShoot = false;
                canMove = true;
                playerAnimator.SetBool("IsShooting", false);

                //Shooting facing right
                if (spr.flipX == false)
                {
                    //Adds equal force in opposite direction to cancel out
                    rb.AddForce(new Vector2(recoil, 0f), ForceMode2D.Impulse);
                }
                //Shooting facing left
                if (spr.flipX)
                {
                    //Adds equal force in the opposite direction to cancel out
                    rb.AddForce(new Vector2(-recoil, 0f), ForceMode2D.Impulse);
                }
            } 
        }

        //TNT mechanic unlocks when bool in Main Manager is set to true, and player is on ground, and player is not using Cannon
        if (Input.GetKeyDown(KeyCode.X) && MainManager.Instance.TNTUnlocked && onGround && canIgnite == false && canShoot == false)
        {
            canIgnite = true;
            playerAnimator.SetBool("IsIgniting", true);
            canspawnTNT = true;
            //Cannon Load bar fills
            TNTLoad.fillAmount = 1f;
        }

        //If Player is igniting TNT
        if (canIgnite)
        {
            //Stops Player movement
            canMove = false;
            //Cooldown begins, set the length in inspector
            TNTCooldown -= Time.deltaTime;
            //TNT Load bar goes down
            TNTLoad.fillAmount -= 0.5f * Time.deltaTime;
            //Zeroes forces on player so that jump is consistent
            rb.velocity = Vector3.zero;

            //Spawns TNT at correct point in animation
            if (TNTCooldown <= 1.5f && canspawnTNT)
            {
                //Placement facing right
                if (spr.flipX == false)
                {
                    //Instantiates TNT to the left of player
                    Instantiate(TNT, new Vector3(transform.position.x - 1f, transform.position.y, 0f), transform.rotation);

                }
                //Placement facing left
                if (spr.flipX == true)
                {
                    //Instantiates TNT to the right of player
                    Instantiate(TNT, new Vector3(transform.position.x + 1f, transform.position.y, 0f), transform.rotation);

                }

                //Turns off TNT instantiation
                canspawnTNT = false;
            } 

            //TNT Blasts player upward at correct point in animation
            if (TNTCooldown <= 0.2f)
            {
                rb.gravityScale = 0f;
                //Adds external force, forceMode2D.Impulse greatly amplifies the amount to resemble more of an explosion
                rb.AddForce(new Vector2(0f, blastForce), ForceMode2D.Impulse);
                canMove = true;
            }

            //Igniting state ends when cooldown runs out
            if (TNTCooldown <= 0f)
            {
                //Reset cooldown to same as set in inspector, reset bools
                TNTCooldown = 2f;
                canIgnite = false;
                playerAnimator.SetBool("IsIgniting", false);
                rb.gravityScale = 1f;

                //Gives player ability to glide after TNT jump if they have unlocked it 
                if (MainManager.Instance.GlideUnlocked)
                {
                    canGlide = true;
                    //Loading bar empties
                    GlideLoad.fillAmount = 0f;
                }
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
                //Glide loading bar fills
                GlideLoad.fillAmount = 1f;
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
