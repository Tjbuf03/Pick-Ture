using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using System.Collections;

public class CannonTimer : MonoBehaviour
{
    public float timeLimit = 300f; // 5 minutes in seconds
    private float timeRemaining;
    public Text timerText; // UI Text element to display the timer

    void Start()
    {
        timeRemaining = timeLimit; // Initialize remaining time
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for one second
            timeRemaining--; // Decrease the timer
            UpdateTimerDisplay(); // Update the displayed time
        }

        // Timer finished logic (optional)
        TimerFinished();
    }

    void UpdateTimerDisplay()
    {
        // Convert time remaining to minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Update the UI text
    }

    void TimerFinished()
    {
        timerText.text = "Time's up!"; // Display a message when the timer finishes
        // Additional logic can be added here (e.g., stopping the game, etc.)
    }
}

