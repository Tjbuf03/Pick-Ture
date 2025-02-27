using UnityEngine;

public class TargetBase : MonoBehaviour
{
    private Transform spawnPoint;
    private TargetSpawner spawner;
    private float despawnTime = 5f; // Time before target despawns if not hit

    public void SetSpawnPoint(Transform point, TargetSpawner targetSpawner)
    {
        spawnPoint = point;
        spawner = targetSpawner;

        // Start the despawn timer for the target
        Invoke(nameof(Despawn), despawnTime);
    }

    private void Despawn()
    {
        // Inform the spawner to free the spawn point when the target or bomb despawns
        if (spawnPoint != null)
        {
            spawner.FreeSpawnPoint(spawnPoint);
        }
        Destroy(gameObject); // Destroy the target or bomb
    }

    // Optional: You can have a method that gets called when the target or bomb is shot
    public void DestroySelf()
    {
        if (spawnPoint != null)
        {
            spawner.FreeSpawnPoint(spawnPoint);
        }
        Destroy(gameObject);
    }
}
