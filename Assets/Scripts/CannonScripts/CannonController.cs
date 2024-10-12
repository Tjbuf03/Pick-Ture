using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab;   // Prefab for the projectile
    public Transform firePoint;           // Where the projectile spawns
    public float rotationSpeed = 50f;     // Speed at which the cannon rotates automatically
    public float projectileForce = 20f;   // Force applied to the projectile
    public PlatformManager platformManager;  // Reference to the platform manager

    private bool projectileInAir = false; // Flag to check if the projectile is in the air
    private GameObject currentProjectile; // Reference to the current projectile

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
            if (platformManager.platforms.Count < platformManager.maxPlatforms)
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
            platformManager.RemoveFirstPlatform();  // Use the method from PlatformManager
            UpdatePlatformCounterUI();
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

            // Tell the PlatformManager to create a platform
            platformManager.SpawnPlatform(projectilePosition);

            // Reset projectileInAir flag
            projectileInAir = false;

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
        platformCounterText.text = platformManager.platforms.Count + "/" + platformManager.maxPlatforms;
    }
}













