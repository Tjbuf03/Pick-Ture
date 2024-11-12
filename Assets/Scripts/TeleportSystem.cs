using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    [System.Serializable]
    public class TeleportPair
    {
        public Transform teleportPointA; // First teleport location
        public Transform teleportPointB; // Second teleport location
    }

    public TeleportPair[] teleportPairs; // Array of teleport pairs
    public TeleportUI teleportUI; // Reference to the TeleportUI script
    private Transform currentTeleportTarget; // Stores the target location

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (TeleportPair pair in teleportPairs)
        {
            if (other.transform == pair.teleportPointA)
            {
                currentTeleportTarget = pair.teleportPointB;
                return;
            }
            else if (other.transform == pair.teleportPointB)
            {
                currentTeleportTarget = pair.teleportPointA;
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (TeleportPair pair in teleportPairs)
        {
            if (other.transform == pair.teleportPointA || other.transform == pair.teleportPointB)
            {
                currentTeleportTarget = null;
                return;
            }
        }
    }

    private void Update()
    {
        if (currentTeleportTarget != null && Input.GetKeyDown(KeyCode.P))
        {
            // Hide the UI prompt before teleporting
            teleportUI.HideUIPrompt();

            // Teleport to the target position
            transform.position = currentTeleportTarget.position;
            currentTeleportTarget = null; // Reset after teleporting
        }
    }
}
