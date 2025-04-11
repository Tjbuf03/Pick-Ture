using UnityEngine;
using System.Collections;

public class TNTSpawner : MonoBehaviour
{
    public GameObject tntPrefab; // Assign the TNT prefab in the Inspector
    public Transform[] spawnPoints; // Assign multiple spawn points in the Inspector
    public float spawnInterval = 3f; // Time between each spawn
    public float disableTime = 2f; // Time to disable the spawn point after spawning
    public float initialDelay = 5f; // Delay before spawning starts

    private Transform lastSpawnPoint = null;

    private void Start()
    {
        StartCoroutine(SpawnTNTRoutine());
    }

    private IEnumerator SpawnTNTRoutine()
    {
        yield return new WaitForSeconds(initialDelay); // Wait before spawning starts

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // Wait before each spawn

            Transform chosenSpawn = GetAvailableSpawnPoint();
            if (chosenSpawn != null)
            {
                SpawnTNT(chosenSpawn);
                lastSpawnPoint = chosenSpawn; // Save last used spawn point
            }
        }
    }

    private Transform GetAvailableSpawnPoint()
    {
        // Get spawn points that are enabled and NOT the last used one
        Transform[] availableSpawns = System.Array.FindAll(spawnPoints, sp =>
            sp.gameObject.activeSelf && sp != lastSpawnPoint
        );

        if (availableSpawns.Length == 0)
        {
            // If no other option, allow reuse of the last one (avoids soft-lock)
            availableSpawns = System.Array.FindAll(spawnPoints, sp => sp.gameObject.activeSelf);
        }

        if (availableSpawns.Length == 0) return null;

        return availableSpawns[Random.Range(0, availableSpawns.Length)];
    }

    private void SpawnTNT(Transform spawnPoint)
    {
        Instantiate(tntPrefab, new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0), Quaternion.identity);
        StartCoroutine(DisableSpawnPointTemporarily(spawnPoint));
    }

    private IEnumerator DisableSpawnPointTemporarily(Transform spawnPoint)
    {
        spawnPoint.gameObject.SetActive(false);
        yield return new WaitForSeconds(disableTime);
        spawnPoint.gameObject.SetActive(true);
    }
}
