using UnityEngine;
using UnityEngine.UI;

public class TeleportUI : MonoBehaviour
{
    private bool isInZone = false; // Flag to check if player is in the zone
    private GameObject currentUIPrompt; // Reference to the current zone's UI prompt

    private void Start()
    {
        if (MainManager.Instance.isReturning == true)
            // Sets Player position
            transform.position = MainManager.Instance.PlayerPos;
    }

    // This will detect when the player enters a zone (2D trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the zone is tagged as "TeleportZone"
        if (other.CompareTag("TeleportZone"))
        {
            isInZone = true;

            // Get the zone's specific UI prompt and show it
            currentUIPrompt = other.gameObject.GetComponent<TeleportZoneInfo>().uiPrompt;
            if (currentUIPrompt != null)
            {
                currentUIPrompt.SetActive(true); // Show the UI prompt
            }
        }
    }

    // Detect when the player leaves the zone (2D trigger)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TeleportZone"))
        {
            isInZone = false;
            HideUIPrompt();
            Debug.Log("Player left the zone");
        }
    }

    // Hides the UI prompt
    public void HideUIPrompt()
    {
        if (currentUIPrompt != null)
        {
            currentUIPrompt.SetActive(false);
            currentUIPrompt = null; // Clear the reference to the current UI prompt
        }
    }

    private void Update()
    {
        // Check if the player is in the zone and presses the 'P' key
        if (isInZone && Input.GetKeyDown(KeyCode.P))
        {
            // Records Player position
            MainManager.Instance.PlayerPos = transform.position;

            // Player is not returning from painting
            MainManager.Instance.isReturning = false;
        }
    }
}
