using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrop : MonoBehaviour
{
    private Collider2D currentPlatformCollider; // The collider of the current semisolid platform
    private bool onSemiSolidPlatform = false; // Flag to check if on a semisolid platform

    private void Update()
    {
        // Press the S key to drop through only when on a semisolid platform
        if (Input.GetKeyDown(KeyCode.S) && onSemiSolidPlatform && currentPlatformCollider != null)
        {
            StartCoroutine(DropThroughPlatform()); // Start the drop coroutine
        }
    }

    private IEnumerator DropThroughPlatform()
    {
        if (currentPlatformCollider != null)
        {
            SemisolidPlatform platform = currentPlatformCollider.GetComponent<SemisolidPlatform>();
            if (platform != null)
            {
                platform.DisableCollider(); // Disable the platform's collider
            }

            // Wait a short time to allow the player to fall
            yield return new WaitForSeconds(0.4f);

            if (platform != null)
            {
                platform.EnableCollider(); // Enable the platform's collider after the player has dropped
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collided object has a PlatformEffector2D
        PlatformEffector2D platformEffector = other.gameObject.GetComponent<PlatformEffector2D>();
        if (platformEffector != null)
        {
            onSemiSolidPlatform = true; // Set the flag if on a semisolid platform
            currentPlatformCollider = other.collider; // Store the current platform's collider
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Reset the flag when leaving the semisolid platform
        if (other.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            onSemiSolidPlatform = false; // Clear the flag
            currentPlatformCollider = null; // Clear the reference to the platform collider
        }
    }
}
