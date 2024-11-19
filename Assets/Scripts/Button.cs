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

    //Function to be called when player wins Starry Night painting and returns to museum 
    public void LoadMuseumSceneStarry(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        //Set returning bool to true since player is returning from painting
        MainManager.Instance.isReturning = true;

        //Glide upgrade unlocks for Starry night finish
        MainManager.Instance.GlideUnlocked = true;
    }

    //Function to be called when player wins Cannon painting and returns to museum 
    public void LoadMuseumSceneCannon(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        //Set returning bool to true since player is returning from painting
        MainManager.Instance.isReturning = true;

        //Cannon upgrade unlocks for Cannon painting finish
        MainManager.Instance.CannonUnlocked = true;
    }
}

