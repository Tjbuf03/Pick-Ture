using UnityEngine;

public class TNT : MonoBehaviour
{
    public float offScreenY = -6f; // Adjust based on your game's bottom boundary
    public GameObject explosionPrefab; // Assign explosion prefab in the Inspector
    private int penalty = 100;
    private bool hasExploded = false;

    private void Start()
    {
        // Ensure the TNT has gravity (requires Rigidbody2D)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody if not present
        }
        rb.gravityScale = .25f; // Adjust gravity strength if needed
    }

    private void Update()
    {
        // Check if the player presses P while the crosshair is near the TNT
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector3.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                Explode();
            }
        }

        // If TNT falls past the bottom of the screen, destroy it without explosion
        if (transform.position.y < offScreenY)
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (hasExploded) return; // Prevents multiple explosions
        hasExploded = true;

        // Subtract points and reset combo
        ScoreManager.Instance.SubtractScore(penalty);
        ScoreManager.Instance.ResetCombo();

        // Spawn explosion effect
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f); // Destroy explosion after half a second
        }

        // Destroy the TNT object
        Destroy(gameObject);
    }
}
