using UnityEngine;

public class Target : TargetBase
{
    private int maxPoints = 150;
    private int minPoints = 75;
    private float lifetime = 3f;
    private float timeAlive = 0f;

    public AudioClip destroySound; // Assign in Inspector

    void Update()
    {
        timeAlive += Time.deltaTime;
        int currentScore = Mathf.RoundToInt(Mathf.Lerp(maxPoints, minPoints, timeAlive / lifetime));

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                ScoreManager.Instance.AddScore(currentScore);
                ScoreManager.Instance.UpdateCombo();
                crosshair.GetComponent<CrosshairController>()?.TryShowShotEffect();
                PlaySound2D();
                DestroySelf();
            }
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
            audioSource.spatialBlend = 0f; // 2D sound
            audioSource.Play();
            Destroy(audioObj, destroySound.length);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned on Target script.");
        }
    }
}
