using UnityEngine;

public class CannonCollectable : MonoBehaviour
{
    public int pointsValue = 1; // Value for each collectable
    public GameObject newCollectablePrefab; // Prefab for the new collectable that spawns
    private static int collectableCount = 0; // Static count to keep track of collected items

    // Define the spawn position for the new collectable
    public Vector3 spawnPosition = new Vector3(0, 0, 0); // Change this to your desired position

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the collectable
        if (other.CompareTag("Player"))
        {
            // Increment the collectable count
            collectableCount++;

            // Destroy the collectable after the player touches it
            Destroy(gameObject);

            // Check if 3 collectables have been collected
            if (collectableCount >= 3)
            {
                SpawnNewCollectable();
                collectableCount = 0; // Reset the count after spawning a new collectable
            }
        }
    }

    void SpawnNewCollectable()
    {
        // Instantiate the new collectable at the specified position
        Instantiate(newCollectablePrefab, spawnPosition, Quaternion.identity);
    }
}


