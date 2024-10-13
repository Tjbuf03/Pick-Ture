using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab;   // Prefab for the projectile
    public GameObject platformPrefab;     // Prefab for the platform
    public Transform firePoint;           // Where the projectile spawns
    public float rotationSpeed = 50f;     // Speed at which the cannon rotates automatically
    public float projectileForce = 20f;   // Force applied to the projectile
    public int maxPlatforms = 5;          // Maximum number of platforms allowed
    public Color firstPlatformColor = Color.red; // Color for the first platform
    public Color normalPlatformColor = Color.white; // Default platform color

    private bool projectileInAir = false; // Flag to check if the projectile is in the air
    private GameObject currentProjectile; // Reference to the current projectile
    private List<GameObject> platforms = new List<GameObject>(); // List to keep track of platforms

    // Reference to the UI Text that will display the platform count
    public Text platformCounterText;

    // Reference to the UI Text for the popup message
    public Text maxPlatformPopup;

    // How long the popup message should be shown
    public float popupDuration = 2f;
    private float popupTimer = 0f;

    void Start()
    {
        // Initialize the UI text with the initial platform count
        UpdatePlatformCounterUI();
        HideMaxPlatformPopup();
    }

    void Update()
    {
        // Automatically rotate the cannon
        RotateCannonAutomatically();

        // Check if the max platform popup timer is active and hide it after the duration
        if (popupTimer > 0f)
        {
            popupTimer -= Time.deltaTime;
            if (popupTimer <= 0f)
            {
                HideMaxPlatformPopup();
            }
        }

        // Handle input for the up arrow
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (projectileInAir)
            {
                ConvertProjectileToPlatform(); // Convert the projectile to a platform
            }
            else
            {
                if (platforms.Count < maxPlatforms)
                {
                    Shoot(); // Shoot a new projectile if under max platform limit
                }
                else
                {
                    ShowMaxPlatformPopup(); // Show popup if max platforms reached
                }
            }
        }

        // Remove the first platform when R is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveFirstPlatform();
        }
    }

    // Method to rotate the cannon automatically (without player input)
    void RotateCannonAutomatically()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    // Method to shoot a projectile
    void Shoot()
    {
        // Create the projectile at the fire point
        currentProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Apply force to the projectile
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * projectileForce, ForceMode2D.Impulse);

        // Set projectile in air flag to true
        projectileInAir = true;

        // Add the collision detection for the projectile
        currentProjectile.AddComponent<ProjectileCollision>().cannonController = this; // Set the reference
    }

    // Convert the current projectile into a platform
    void ConvertProjectileToPlatform()
    {
        if (currentProjectile != null)
        {
            // Get the position of the current projectile
            Vector3 projectilePosition = currentProjectile.transform.position;

            // Destroy the current projectile
            Destroy(currentProjectile);

            // Instantiate the platform prefab at the position where the projectile was
            GameObject platform = Instantiate(platformPrefab, projectilePosition, Quaternion.identity);

            // Ensure the platform stays flat by setting Z-axis to 0
            platform.transform.position = new Vector3(platform.transform.position.x, platform.transform.position.y, 0);

            // Add the platform to the list of platforms
            platforms.Add(platform);

            // Reset projectileInAir flag
            projectileInAir = false;

            // Update platform colors (highlight first platform)
            UpdatePlatformColors();

            // Update the platform counter in the UI
            UpdatePlatformCounterUI();
        }
    }

    // Remove the first platform placed
    void RemoveFirstPlatform()
    {
        if (platforms.Count > 0)
        {
            // Get the first platform in the list
            GameObject firstPlatform = platforms[0];

            // Destroy the first platform
            Destroy(firstPlatform);

            // Remove the first platform from the list
            platforms.RemoveAt(0);

            // Update platform colors (highlight new first platform)
            UpdatePlatformColors();

            // Update the platform counter in the UI
            UpdatePlatformCounterUI();
        }
    }

    // Update the color of the platforms (first platform is red)
    void UpdatePlatformColors()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            SpriteRenderer sr = platforms[i].GetComponent<SpriteRenderer>();
            if (i == 0)
            {
                sr.color = firstPlatformColor; // First platform is red
            }
            else
            {
                sr.color = normalPlatformColor; // Other platforms are default color
            }
        }
    }

    // Show the popup message that the maximum number of platforms is reached
    void ShowMaxPlatformPopup()
    {
        if (maxPlatformPopup != null)
        {
            maxPlatformPopup.enabled = true;  // Show the popup
            popupTimer = popupDuration;       // Start the timer to hide it after some time
        }
    }

    // Hide the popup message
    void HideMaxPlatformPopup()
    {
        if (maxPlatformPopup != null)
        {
            maxPlatformPopup.enabled = false; // Hide the popup
        }
    }

    // Update the platform counter in the UI
    void UpdatePlatformCounterUI()
    {
        platformCounterText.text = platforms.Count + "/" + maxPlatforms;
    }

    // Method to handle projectile collision with walls
    public void OnProjectileCollision(GameObject projectile)
    {
        if (projectile != null)
        {
            Destroy(projectile); // Destroy the projectile on collision
            projectileInAir = false; // Reset the projectile in air flag
        }
    }
}

// Create a new class for projectile collision
public class ProjectileCollision : MonoBehaviour
{
    public CannonController cannonController; // Reference to the CannonController

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the projectile collides with an object tagged as "Wall"
        if (collider.CompareTag("Wall"))
        {
            cannonController.OnProjectileCollision(gameObject); // Call the method in CannonController
        }
    }
}
















