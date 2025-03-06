using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public Text scoreText;
    public Text comboMessageText;
    public Text comboMultiplierText;

    public int winScore = 7500;
    public GameObject levelCompletePanel;
    public Text levelCompleteText;
    public string nextSceneName;

    private int score = 0;
    private bool levelCompleted = false;

    private int consecutiveHits = 0;
    private float comboTime = 6f;
    private float comboResetTimer = 0f;

    private string[] comboMessages = new string[] { "OK!", "Nice!", "Great!", "Amazing!", "Fantastic!", "Picturesque!" };
    private int currentComboLevel = 0;
    private float comboMultiplier = 1f;

    [Header("Countdown Settings")]
    public Text countdownText;
    public GameObject countdownPanel;
    public bool gameStarted = false;

    [Header("Level Timer")]
    public Text levelTimerText;
    public float levelTimeLimit = 60f; // Editable in Inspector
    private float levelTimer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (countdownPanel != null)
            countdownPanel.SetActive(true);

        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        if (levelCompleted && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter key pressed, loading next scene...");
            LoadNextScene();
        }

        if (!gameStarted || levelCompleted) return;

        if (score >= winScore)
        {
            ShowLevelCompletePopup();
        }

        if (consecutiveHits > 0)
        {
            comboResetTimer += Time.deltaTime;
            if (comboResetTimer >= comboTime)
            {
                ResetCombo();
            }
        }

        // Level timer logic
        if (levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
            if (levelTimer < 0) levelTimer = 0; // Ensure it does not go negative
            UpdateLevelTimerText();
        }

        // Check if time ran out and trigger scene switch
        if (levelTimer == 0 && gameStarted)
        {
            gameStarted = false; // Prevent multiple calls
            GoToLoseScene();
        }
    }


    private void UpdateLevelTimerText()
    {
        int minutes = Mathf.FloorToInt(levelTimer / 60);
        int seconds = Mathf.FloorToInt(levelTimer % 60);
        levelTimerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void AddScore(int points)
    {
        if (!gameStarted) return;

        int finalPoints = Mathf.RoundToInt(points * comboMultiplier);
        score += finalPoints;
        UpdateScoreText();
    }

    public void SubtractScore(int points)
    {
        if (!gameStarted) return;

        score = Mathf.Max(0, score - points);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateCombo()
    {
        if (!gameStarted) return;

        consecutiveHits++;
        comboResetTimer = 0f;

        if (consecutiveHits % 5 == 0 && currentComboLevel < 6)
        {
            currentComboLevel++;
            ShowComboMessage(comboMessages[currentComboLevel - 1]);

            comboMultiplier = 1f + (currentComboLevel * 0.5f);
            if (comboMultiplier > 4f) comboMultiplier = 4f;
            UpdateComboMultiplierText();
        }
    }

    private void ShowComboMessage(string message)
    {
        comboMessageText.text = message;
        comboMessageText.gameObject.SetActive(true);
    }

    private void UpdateComboMultiplierText()
    {
        comboMultiplierText.text = "Combo x" + comboMultiplier.ToString("F1");
    }

    public void ResetCombo()
    {
        consecutiveHits = 0;
        currentComboLevel = 0;
        comboMultiplier = 1f;
        comboResetTimer = 0f;

        comboMessageText.gameObject.SetActive(false);
        UpdateComboMultiplierText();
    }

    private void ShowLevelCompletePopup()
    {
        levelCompleted = true;

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            if (levelCompleteText != null)
                levelCompleteText.text = "Level Complete!\nPress ENTER to continue";
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set in the Inspector!");
        }
    }

    private void GoToLoseScene()
    {
        SceneManager.LoadScene("LoseCowboy");
    }

    private IEnumerator StartCountdown()
    {
        gameStarted = false;

        if (countdownText != null)
        {
            countdownText.text = "3";
            yield return new WaitForSeconds(1f);
            countdownText.text = "2";
            yield return new WaitForSeconds(1f);
            countdownText.text = "1";
            yield return new WaitForSeconds(1f);
            countdownText.text = "GO!";
            yield return new WaitForSeconds(1f);
            countdownPanel.SetActive(false);
        }

        levelTimer = levelTimeLimit;
        gameStarted = true;
    }
}
