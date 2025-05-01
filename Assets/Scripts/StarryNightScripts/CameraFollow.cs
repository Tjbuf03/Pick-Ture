using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float speedIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;
    private float timeSinceLastIncrease;

    public Text speedIncreasePopup;
    public float popupDuration = 2f;

    public AudioClip speedUpSound;           // New: Sound for speed increase
    private AudioSource audioSource;         // New: AudioSource for playing the sound

    void Start()
    {
        timeSinceLastIncrease = 0f;
        speedIncreasePopup.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component
    }

    void Update()
    {
        timeSinceLastIncrease += Time.deltaTime;

        if (timeSinceLastIncrease >= speedIncreaseInterval)
        {
            scrollSpeed += speedIncreaseAmount;
            timeSinceLastIncrease = 0f;

            StartCoroutine(ShowSpeedIncreasePopup());
        }

        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
    }

    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }

    IEnumerator ShowSpeedIncreasePopup()
    {
        speedIncreasePopup.text = "Speed Up!";
        speedIncreasePopup.gameObject.SetActive(true);

        // Play sound if assigned
        if (speedUpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(speedUpSound);
        }

        yield return new WaitForSeconds(popupDuration);
        speedIncreasePopup.gameObject.SetActive(false);
    }
}
