using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // Public variables to specify the scene names in the Inspector
    [Header("Scene Names")]
    public string enterScene;
    public string backspaceScene;

    void Update()
    {
        // Check if Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadScene(enterScene);
        }

        // Check if Backspace key is pressed
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            LoadScene(backspaceScene);

            //Set returning bool to true since player is returning from painting
            MainManager.Instance.isReturning = true;
        }
    }

    // Method to load a specified scene
    void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name not set in the inspector.");
        }
    }
}
