using UnityEngine;

public class TNT : MonoBehaviour
{
    public float offScreenY = -6f; // Adjust based on your game's bottom boundary
    public GameObject explosionPrefab; // Assign explosion prefab in the Inspector
    public AudioClip destroySound;     // Assign sound in Inspector

    private int penalty = 100;
    private bool hasExploded = false;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = .25f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector3.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                Explode();
            }
        }

        if (transform.position.y < offScreenY)
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        ScoreManager.Instance.SubtractScore(penalty);
        ScoreManager.Instance.ResetCombo();

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.15f, 0.1f);
        }

        PlaySound2D();
        Destroy(gameObject);
    }

    private void PlaySound2D()
    {
        if (destroySound != null)
        {
            GameObject audioObj = new GameObject("Temp2DSound_TNT");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = destroySound;
            audioSource.volume = 1.0f;
            audioSource.spatialBlend = 0f; // 2D sound
            audioSource.Play();
            Destroy(audioObj, destroySound.length);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned on TNT script.");
        }
    }
}
