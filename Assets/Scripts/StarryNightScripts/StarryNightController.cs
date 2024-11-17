using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;  // Required for using Coroutines

public class StarryNightController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float horizontalSpeed = 2f;  // This will now be synced with the camera speed

    public int maxHealth = 3;
    private int currentHealth;

    public Slider healthBar;

    private float minX, maxX, minY, maxY;

    private bool isInvincible = false;  // Track if the player is invincible
    public float invincibilityDuration = 2f;  // Duration of invincibility in seconds
    public float flashInterval = 0.1f;  // Time interval between player flashes (visual feedback)

    private SpriteRenderer spriteRenderer;  // To control player sprite visibility for flashing effect
    private CameraFollow cameraFollow;  // Reference to the CameraFollow script

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the player's sprite renderer

        // Get reference to the CameraFollow script
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        CalculateCameraBounds();

        // Update the player's horizontal speed to match the camera's scroll speed
        horizontalSpeed = cameraFollow.GetScrollSpeed();

        Vector3 movement = Vector3.zero;

        // Vertical movement (W/S or Up/Down Arrow keys)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movement += Vector3.down;
        }

        // Horizontal movement (A/D or Left/Right Arrow keys)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement += Vector3.right;
        }

        // Apply movement to player, multiplying horizontal movement by the synced speed
        transform.Translate(movement * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);

        // Clamp player within camera bounds
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }


    void CalculateCameraBounds()
    {
        Camera mainCamera = Camera.main;
        float verticalExtent = mainCamera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;
        Vector3 cameraPosition = mainCamera.transform.position;
        minX = cameraPosition.x - horizontalExtent;
        maxX = cameraPosition.x + horizontalExtent;
        minY = cameraPosition.y - verticalExtent;
        maxY = cameraPosition.y + verticalExtent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("StarryNightObstacle") && !isInvincible)
        {
            TakeDamage();
        }
        if (collision.gameObject.CompareTag("WinSquare"))
        {
            SceneManager.LoadScene("WinStarryNight");  // Load the win scene

            //Glide upgrade unlocks for Starry night finish
            MainManager.Instance.GlideUnlocked = true;
        }
    }

    void TakeDamage()
    {
        currentHealth--;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(ActivateInvincibility());  // Start the invincibility period
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("FailStarryNight");
    }

    // Coroutine to handle invincibility frames
    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // Visual feedback: Flash the player sprite on and off
        for (float i = 0; i < invincibilityDuration; i += flashInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;  // Toggle sprite visibility
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.enabled = true;  // Ensure the sprite is visible at the end
        isInvincible = false;  // End the invincibility period
    }
}
