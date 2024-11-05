using UnityEngine;

public class SemisolidPlatform : MonoBehaviour
{
    private Collider2D platformCollider;

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>(); // Get the platform's collider
    }

    public void EnableCollider()
    {
        platformCollider.enabled = true; // Enable the collider
    }

    public void DisableCollider()
    {
        platformCollider.enabled = false; // Disable the collider
    }
}
