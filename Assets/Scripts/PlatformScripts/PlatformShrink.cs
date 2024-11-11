using UnityEngine;

public class PlatformShrink : MonoBehaviour
{
    public float shrinkSpeed = 2f;  // Speed at which platform shrinks
    public float restoreSpeed = 1f; // Speed at which platform restores
    public float minWidth = 0.5f;   // Minimum width when fully shrunk

    private Vector3 originalScale;
    private bool playerOnPlatform = false;

    private void Start()
    {
        // Store the platform's original scale
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Shrink platform if player is on it
        if (playerOnPlatform)
        {
            // Reduce width gradually to minimum limit
            float newWidth = Mathf.Max(minWidth, transform.localScale.x - shrinkSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newWidth, originalScale.y, originalScale.z);
        }
        else
        {
            // Restore width gradually to original scale
            float newWidth = Mathf.Min(originalScale.x, transform.localScale.x + restoreSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newWidth, originalScale.y, originalScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect if player lands on platform
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detect if player leaves the platform
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }
}
