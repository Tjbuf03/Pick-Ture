using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StarryNightController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float horizontalSpeed = 2f;

    public int maxHealth = 3;
    private int currentHealth;

    public Slider healthBar;

    private float minX, maxX, minY, maxY;

    private bool isInvincible = false;
    public float invincibilityDuration = 2f;
    public float flashInterval = 0.1f;

    private SpriteRenderer spriteRenderer;
    private CameraFollow cameraFollow;

    public AudioClip hitSound;              // Hit sound clip
    private AudioSource audioSource;        // AudioSource for playing sound

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        audioSource = GetComponent<AudioSource>(); // Get AudioSource component
    }

    void Update()
    {
        CalculateCameraBounds();
        horizontalSpeed = cameraFollow.GetScrollSpeed();

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movement += Vector3.down;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement += Vector3.right;
        }

        transform.Translate(movement * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);

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
            SceneManager.LoadScene("WinStarryNight");
            MainManager.Instance.GlideUnlocked = true;
        }
    }

    void TakeDamage()
    {
        currentHealth--;
        healthBar.value = currentHealth;

        // Play hit sound if assigned
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(ActivateInvincibility());
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("FailStarryNight");
    }

    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += flashInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
