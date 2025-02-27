using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import SceneManager

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Text scoreText;
    public Text comboMessageText; // Reference for combo message UI
    public Text comboMultiplierText; // Reference for combo multiplier UI

    private int score = 0;

    private int consecutiveHits = 0;
    private float comboTime = 6f;  // Combo resets after 6 seconds of no hits
    private float comboResetTimer = 0f;

    private string[] comboMessages = new string[] { "OK!", "Nice!", "Great!", "Amazing!", "Fantastic!", "Picturesque!" };
    private int currentComboLevel = 0;
    private float comboMultiplier = 1f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // Handle combo reset after 6 seconds of no targets hit
        if (consecutiveHits > 0)
        {
            comboResetTimer += Time.deltaTime;
            if (comboResetTimer >= comboTime)
            {
                ResetCombo();
            }
        }

        // Check if score reached 7500 to change the scene
        if (score >= 7500)
        {
            LoadNextScene(); // Call method to load the next scene
        }
    }

    public void AddScore(int points)
    {
        int finalPoints = Mathf.RoundToInt(points * comboMultiplier);
        score += finalPoints;
        UpdateScoreText();
    }

    public void SubtractScore(int points)
    {
        score = Mathf.Max(0, score - points);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Method to update combo when a target is hit
    public void UpdateCombo()
    {
        consecutiveHits++;
        comboResetTimer = 0f;  // Reset timer

        // Increase combo level every 5 consecutive hits
        if (consecutiveHits % 5 == 0 && currentComboLevel < 6) // Only update if combo level < 6
        {
            currentComboLevel++;
            ShowComboMessage(comboMessages[currentComboLevel - 1]); // Show message after the 5th hit

            // Update combo multiplier based on consecutive hits, cap at x4
            comboMultiplier = 1f + (currentComboLevel * 0.5f);
            if (comboMultiplier > 4f) comboMultiplier = 4f;  // Cap multiplier at x4
            UpdateComboMultiplierText();
        }
    }

    // Show the combo message
    private void ShowComboMessage(string message)
    {
        comboMessageText.text = message;

        // Make sure the message is active and visible
        comboMessageText.gameObject.SetActive(true);
    }

    // Update combo multiplier UI text
    private void UpdateComboMultiplierText()
    {
        comboMultiplierText.text = "Combo x" + comboMultiplier.ToString("F1");
    }

    // Reset the combo counter
    public void ResetCombo()
    {
        consecutiveHits = 0;
        currentComboLevel = 0;
        comboMultiplier = 1f;
        comboResetTimer = 0f;

        // Hide the combo message if the combo is reset
        comboMessageText.gameObject.SetActive(false);

        // Reset the combo multiplier text to default
        UpdateComboMultiplierText();
    }

    // Load the next scene
    private void LoadNextScene()
    {
        // This will load the next scene in the build settings by index
        SceneManager.LoadScene("WinCowboy"); // Replace "NextScene" with the actual name of your scene
    }
}
