using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintingSceneSwitch : MonoBehaviour
{
    private string sceneToLoad; // Scene to load
    private bool isInZone = false; // Flag to check if player is in the zone

    // This will detect when the player enters a zone (2D trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the zone is tagged as "Zone"
        if (other.CompareTag("Zone"))
        {
            isInZone = true;
            // Get the scene name from the Zone's assigned scene
            sceneToLoad = other.gameObject.GetComponent<ZoneInfo>().sceneToLoad;
            Debug.Log("Player entered zone: " + sceneToLoad);
        }
    }

    // Detect when the player leaves the zone (2D trigger)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            isInZone = false;
            sceneToLoad = "";
            Debug.Log("Player left the zone");
        }
    }

    private void Update()
    {
        // Check if the player is in the zone and presses the 'P' key
        if (isInZone && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key pressed, loading scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

