using UnityEngine;
using System.Collections;

public class TNTSpawner : MonoBehaviour
{
    public GameObject tntPrefab; // Assign the TNT prefab in the Inspector
    public Transform[] spawnPoints; // Assign multiple spawn points in the Inspector
    public float spawnInterval = 3f; // Time between each spawn
    public float disableTime = 2f; // Time to disable the spawn point after spawning
    public float initialDelay = 5f; // Delay before spawning starts

    private void Start()
    {
        StartCoroutine(SpawnTNTRoutine());
    }

    private IEnumerator SpawnTNTRoutine()
    {
        yield return new WaitForSeconds(initialDelay); // Wait 5 seconds before spawning starts

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // Wait before spawning each TNT

            Transform chosenSpawn = GetAvailableSpawnPoint();
            if (chosenSpawn != null)
            {
                SpawnTNT(chosenSpawn);
            }
        }
    }

    private Transform GetAvailableSpawnPoint()
    {
        // Get a random spawn point that is currently enabled
        Transform[] availableSpawns = System.Array.FindAll(spawnPoints, sp => sp.gameObject.activeSelf);
        if (availableSpawns.Length == 0) return null; // If all are disabled, return null

        return availableSpawns[Random.Range(0, availableSpawns.Length)];
    }

    private void SpawnTNT(Transform spawnPoint)
    {
        GameObject tnt = Instantiate(tntPrefab, new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0), Quaternion.identity);
        StartCoroutine(DisableSpawnPointTemporarily(spawnPoint));
    }

    private IEnumerator DisableSpawnPointTemporarily(Transform spawnPoint)
    {
        spawnPoint.gameObject.SetActive(false);
        yield return new WaitForSeconds(disableTime);
        spawnPoint.gameObject.SetActive(true);
    }
}
