using UnityEngine;

public class Bomb : TargetBase
{
    private int penalty = 100;
    public GameObject explosionPrefab; // Assign in Inspector
    public AudioClip destroySound;     // Assign in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                SubtractPoints();
                ResetCombo(); // Reset combo when bomb is hit
                Explode();    // Show explosion effect and play sound
                DestroySelf(); // Destroy the bomb
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
        GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        if (crosshair != null)
        {
            crosshair.GetComponent<CrosshairController>()?.SuppressShotEffect();
        }

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f); // Adjust timing to match animation
        }

        PlaySound2D();

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.15f, 0.1f);
        }
    }

    void PlaySound2D()
    {
        if (destroySound != null)
        {
            GameObject audioObj = new GameObject("Temp2DSound");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = destroySound;
            audioSource.volume = 1.0f;
            audioSource.spatialBlend = 0f; // 0 = 2D sound
            audioSource.Play();
            Destroy(audioObj, destroySound.length);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned on Bomb script.");
        }
    }
}
