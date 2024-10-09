using UnityEngine;
using UnityEngine.SceneManagement;

public class CannonInfoButton : MonoBehaviour
{
    // Function to be called when the button is pressed
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

