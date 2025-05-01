using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject platformPrefab;
    public Transform firePoint;
    public float rotationSpeed = 50f;
    public float projectileForce = 20f;
    public int maxPlatforms = 5;
    public Color firstPlatformColor = Color.red;
    public Color normalPlatformColor = Color.white;

    private bool projectileInAir = false;
    private GameObject currentProjectile;
    private List<GameObject> platforms = new List<GameObject>();

    public Text platformCounterText;
    public Text maxPlatformPopup;
    public float popupDuration = 2f;
    private float popupTimer = 0f;

    // Audio fields
    public AudioClip shootSound;
    public AudioClip platformAppearSound;
    public AudioClip maxPlatformPopupSound;
    private AudioSource audioSource;

    void Start()
    {
        UpdatePlatformCounterUI();
        HideMaxPlatformPopup();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RotateCannonAutomatically();

        if (popupTimer > 0f)
        {
            popupTimer -= Time.deltaTime;
            if (popupTimer <= 0f)
            {
                HideMaxPlatformPopup();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (projectileInAir)
            {
                ConvertProjectileToPlatform();
            }
            else
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
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveFirstPlatform();
        }
    }

    void RotateCannonAutomatically()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        currentProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * projectileForce, ForceMode2D.Impulse);

        projectileInAir = true;

        currentProjectile.AddComponent<ProjectileCollision>().cannonController = this;

        // Play shoot sound
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    void ConvertProjectileToPlatform()
    {
        if (currentProjectile != null)
        {
            Vector3 projectilePosition = currentProjectile.transform.position;

            Destroy(currentProjectile);

            GameObject platform = Instantiate(platformPrefab, projectilePosition, Quaternion.identity);
            platform.transform.position = new Vector3(platform.transform.position.x, platform.transform.position.y, 0);
            platforms.Add(platform);
            projectileInAir = false;

            UpdatePlatformColors();
            UpdatePlatformCounterUI();

            // Play platform appear sound
            if (platformAppearSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(platformAppearSound);
            }
        }
    }

    void RemoveFirstPlatform()
    {
        if (platforms.Count > 0)
        {
            GameObject firstPlatform = platforms[0];
            Destroy(firstPlatform);
            platforms.RemoveAt(0);
            UpdatePlatformColors();
            UpdatePlatformCounterUI();
        }
    }

    void UpdatePlatformColors()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            SpriteRenderer sr = platforms[i].GetComponent<SpriteRenderer>();
            sr.color = (i == 0) ? firstPlatformColor : normalPlatformColor;
        }
    }

    void ShowMaxPlatformPopup()
    {
        if (maxPlatformPopup != null)
        {
            maxPlatformPopup.enabled = true;
            popupTimer = popupDuration;

            // Play max platform popup sound
            if (maxPlatformPopupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(maxPlatformPopupSound);
            }
        }
    }

    void HideMaxPlatformPopup()
    {
        if (maxPlatformPopup != null)
        {
            maxPlatformPopup.enabled = false;
        }
    }

    void UpdatePlatformCounterUI()
    {
        platformCounterText.text = platforms.Count + "/" + maxPlatforms;
    }

    public void OnProjectileCollision(GameObject projectile)
    {
        if (projectile != null)
        {
            Destroy(projectile);
            projectileInAir = false;
        }
    }
}

public class ProjectileCollision : MonoBehaviour
{
    public CannonController cannonController;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
        {
            cannonController.OnProjectileCollision(gameObject);
        }
    }
}
