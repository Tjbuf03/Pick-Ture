using UnityEngine;
using UnityEngine.UI;  // Required for UI elements
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float scrollSpeed = 2f;  // Initial speed at which the camera moves sideways
    public float speedIncreaseInterval = 10f;  // Time interval to increase camera speed
    public float speedIncreaseAmount = 0.5f;  // Amount to increase the camera speed by each interval
    private float timeSinceLastIncrease;  // Tracks time since the last speed increase

    public Text speedIncreasePopup;  // Reference to the UI Text for the popup
    public float popupDuration = 2f;  // Duration to show the popup


    void Start()
    {
        timeSinceLastIncrease = 0f;  // Initialize the timer for speed increase
        speedIncreasePopup.gameObject.SetActive(false);  // Hide popup at start
    }

    void Update()
    {
        // Update the timer for camera speed increase
        timeSinceLastIncrease += Time.deltaTime;

        // Check if enough time has passed to increase camera speed
        if (timeSinceLastIncrease >= speedIncreaseInterval)
        {
            scrollSpeed += speedIncreaseAmount;  // Increase the camera speed
            timeSinceLastIncrease = 0f;  // Reset the timer

            StartCoroutine(ShowSpeedIncreasePopup());
        }

        // Move the camera to the right at the current scroll speed
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
    }

    // Method to get the current camera scroll speed
    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }

    IEnumerator ShowSpeedIncreasePopup()
    {
        // Enable the popup text and display a message
        speedIncreasePopup.text = "Speed Up!";
        speedIncreasePopup.gameObject.SetActive(true);

        // Wait for the popup duration
        yield return new WaitForSeconds(popupDuration);

        // Hide the popup after the duration
        speedIncreasePopup.gameObject.SetActive(false);
    }
}

