using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject shotEffectPrefab; // Assign your prefab here in the Inspector
    public AudioClip shootSound; // Assign the shoot sound in Inspector

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
        transform.position += move;

        if (Input.GetKeyDown(KeyCode.P))
        {
            TryShowShotEffect(); // Show visual
            PlayShootSound();    // Play sound
        }
    }

    public void TryShowShotEffect()
    {
        if (shotEffectPrefab != null)
        {
            GameObject effect = Instantiate(shotEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.1f); // Auto destroy after brief time
        }
    }

    public void SuppressShotEffect()
    {
        // Optional override method
    }

    private void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
