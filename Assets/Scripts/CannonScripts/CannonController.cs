using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab;   // Prefab for the projectile
    public GameObject platformPrefab;     // Prefab for the platform
    public Transform firePoint;           // Where the projectile spawns
    public float rotationSpeed = 50f;     // Speed at which the cannon rotates automatically
    public float projectileForce = 20f;   // Force applied to the projectile
    public int maxPlatforms = 5;          // Maximum number of platforms allowed

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

        // Shoot projectile when the up arrow is pressed, only if less than max platforms exist
        if (Input.GetKeyDown(KeyCode.UpArrow) && !projectileInAir)
        {
            if (platforms.Count < maxPlatforms)
            {
                Shoot();
            }
            else
            {
                ShowMaxPlatformPopup();
            }
        }

        // Turn the projectile into a platform when the down arrow is pressed
        if (Input.GetKeyDown(KeyCode.DownArrow) && projectileInAir)
        {
            ConvertProjectileToPlatform();
        }

        // Remove the last platform when R is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveLastPlatform();
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

            // Update the platform counter in the UI
            UpdatePlatformCounterUI();
        }
    }

    // Remove the last platform placed
    void RemoveLastPlatform()
    {
        if (platforms.Count > 0)
        {
            // Get the last platform in the list
            GameObject lastPlatform = platforms[platforms.Count - 1];

            // Destroy the last platform
            Destroy(lastPlatform);

            // Remove the last platform from the list
            platforms.RemoveAt(platforms.Count - 1);

            // Update the platform counter in the UI
            UpdatePlatformCounterUI();
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
}












