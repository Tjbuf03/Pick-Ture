using UnityEngine;

public class InfoBomb : MonoBehaviour
{
    private InfoTargetBase baseScript;
    public AudioClip destroySound;     // Assign sound in Inspector

    void Start()
    {
        baseScript = GetComponent<InfoTargetBase>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                DestroySelf();
            }
        }
    }

    void DestroySelf()
    {
        PlaySound2D();
        baseScript.DestroySelf();
    }

    private void PlaySound2D()
    {
        if (destroySound != null)
        {
            GameObject audioObj = new GameObject("Temp2DSound_InfoBomb");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = destroySound;
            audioSource.volume = 1.0f;
            audioSource.spatialBlend = 0f; // 2D sound
            audioSource.Play();
            Destroy(audioObj, destroySound.length);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned on InfoBomb script.");
        }
    }
}
