using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{   
    
    // Function to be called when the button is pressed
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Function to be called when button to return to museum is pressed
    public void LoadMuseumScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        //Set returning bool to true since player is returning from painting
        MainManager.Instance.isReturning = true;
    }
}

