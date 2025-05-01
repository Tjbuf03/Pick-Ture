using UnityEngine;

public class MovingBomb : TargetBase
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public bool moveRight = true;
    public float rotationSpeed = 200f;
    public GameObject explosionPrefab; // Assign in Inspector
    public AudioClip destroySound;     // Assign sound clip in Inspector

    private Vector3 movementDirection;
    private int penalty = 100;

    void Start()
    {
        movementDirection = moveRight ? Vector3.right : Vector3.left;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        float rotationDirection = moveRight ? -1f : 1f;
        transform.Rotate(0, 0, rotationSpeed * rotationDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector3.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                SubtractPoints();
                ResetCombo();
                Explode();
                PlaySound2D();
                DestroySelf();
            }
        }
    }

    void SubtractPoints()
    {
        ScoreManager.Instance.SubtractScore(penalty);
    }

    void ResetCombo()
    {
        ScoreManager.Instance.ResetCombo();
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.15f, 0.1f);
        }
    }

    void PlaySound2D()
    {
        if (destroySound != null)
        {
            GameObject audioObj = new GameObject("Temp2DSound_MovingBomb");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = destroySound;
            audioSource.volume = 1.0f;
            audioSource.spatialBlend = 0f; // 2D sound
            audioSource.Play();
            Destroy(audioObj, destroySound.length);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned on MovingBomb script.");
        }
    }
}
