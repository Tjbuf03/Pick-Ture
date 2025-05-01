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

    [Header("Wintext Settings")]
    public Text winScoreText;
    public GameObject winScorePanel;

    [Header("Level Timer")]
    public Text levelTimerText;
    public float levelTimeLimit = 60f;
    private float levelTimer;

    [Header("Combo Sounds")]
    public AudioClip[] comboSounds; // Assign one sound per combo level
    private AudioSource audioSource;
    private int lastComboLevel = 0;

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

        if (winScorePanel != null)
            winScorePanel.SetActive(true);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
            if (levelTimer < 0) levelTimer = 0;
            UpdateLevelTimerText();
        }

        if (levelTimer == 0 && gameStarted)
        {
            gameStarted = false;
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

            // Play sound once for new combo level
            if (currentComboLevel > lastComboLevel && currentComboLevel <= comboSounds.Length)
            {
                AudioClip clip = comboSounds[currentComboLevel - 1];
                if (clip != null)
                {
                    audioSource.PlayOneShot(clip);
                }
                lastComboLevel = currentComboLevel;
            }
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
        lastComboLevel = 0;

        comboMessageText.gameObject.SetActive(false);
        UpdateComboMultiplierText();
    }

    private void ShowLevelCompletePopup()
    {
        levelCompleted = true;
        Time.timeScale = 0f;

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            if (levelCompleteText != null)
                levelCompleteText.text = "Level Complete!\nPress ENTER to continue";
        }
    }

    private void LoadNextScene()
    {
        Time.timeScale = 1f;

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

        if (winScoreText != null)
        {
            winScoreText.text = $"You need {winScore} points to advance!";
        }

        if (countdownText != null)
        {
            countdownText.text = "5"; yield return new WaitForSeconds(1f);
            countdownText.text = "4"; yield return new WaitForSeconds(1f);
            countdownText.text = "3"; yield return new WaitForSeconds(1f);
            countdownText.text = "2"; yield return new WaitForSeconds(1f);
            countdownText.text = "1"; yield return new WaitForSeconds(1f);
            countdownText.text = "GO!"; yield return new WaitForSeconds(1f);
        }

        if (countdownPanel != null)
            countdownPanel.SetActive(false);

        if (winScorePanel != null)
            winScorePanel.SetActive(false);

        levelTimer = levelTimeLimit;
        gameStarted = true;
    }
}
