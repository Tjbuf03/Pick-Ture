using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class NewCollectable : MonoBehaviour
{
    public string sceneToLoad = "NewScene"; // The name of the scene to load

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the new collectable
        if (other.CompareTag("Player"))
        {
            // Load the specified scene when the collectable is collected
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

