using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CannonTimer : MonoBehaviour
{
    public float timeLimit = 300f; // 5 minutes in seconds
    private float timeRemaining;
    public Text timerText; // UI Text element to display the timer
    public string[] scenesThatNeedTimer; // Array of scene names where the timer is needed
    public string sceneToLoadOnTimerEnd; // Name of the scene to load when the timer ends

    private static CannonTimer instance;

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this; // Set the instance to this if it doesn't exist
            DontDestroyOnLoad(gameObject); // Prevent the timer GameObject from being destroyed
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy the new one
        }
    }

    void Start()
    {
        timeRemaining = timeLimit; // Initialize remaining time
        UpdateTimerDisplay(); // Ensure the timer displays the correct time at the start
        StartCoroutine(StartTimer());
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event to update the timerText reference when switching scenes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Coroutine to start the countdown timer
    IEnumerator StartTimer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for one second
            timeRemaining--; // Decrease the timer
            UpdateTimerDisplay(); // Update the displayed time
        }

        // Timer finished logic
        TimerFinished();
    }

    // Method to update the timer text
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Update the UI text
        }
    }

    // Method to handle when the timer finishes
    void TimerFinished()
    {
        if (timerText != null)
        {
            timerText.text = "Time's up!"; // Display message when timer finishes
        }

        // Load the specified scene when the timer finishes
        if (!string.IsNullOrEmpty(sceneToLoadOnTimerEnd))
        {
            SceneManager.LoadScene(sceneToLoadOnTimerEnd);
        }
        else
        {
            Debug.LogError("Scene to load on timer end is not set.");
        }
    }

    // This method is called every time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is one of the scenes that needs the timer
        if (System.Array.Exists(scenesThatNeedTimer, sceneName => sceneName == scene.name))
        {
            // Reassign the timerText in the new scene
            timerText = GameObject.FindWithTag("TimerText").GetComponent<Text>();
            UpdateTimerDisplay(); // Update the display in the new scene
        }
        else
        {
            // Destroy the timer if it enters a scene where it's no longer needed
            Destroy(gameObject);
        }
    }
}
