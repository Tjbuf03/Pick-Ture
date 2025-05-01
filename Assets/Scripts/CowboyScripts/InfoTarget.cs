using UnityEngine;

public class InfoTarget : MonoBehaviour
{
    private InfoTargetBase baseScript;
    public AudioClip destroySound; // Assign this in the Inspector

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
                PlayDestroySound();
                baseScript.DestroySelf();
            }
        }
    }

    private void PlayDestroySound()
    {
        if (destroySound != null)
        {
            GameObject audioObj = new GameObject("Temp2DSound_InfoTarget");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = destroySound;
            audioSource.volume = 1.0f;
            audioSource.spatialBlend = 0f; // 2D sound
            audioSource.Play();
            Destroy(audioObj, destroySound.length);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned on InfoTarget script.");
        }
    }
}
