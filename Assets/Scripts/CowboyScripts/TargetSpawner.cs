using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject bombPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public float bombChance = 0.3f;
    public float minSpawnInterval = 0.5f;
    public float spawnAcceleration = 0.05f;
    public float respawnCooldown = 1f; // Cooldown before new target/bomb spawns in same spot

    private Dictionary<string, bool> occupiedSpots = new Dictionary<string, bool>();
    private Dictionary<string, bool> cooldownSpots = new Dictionary<string, bool>();

    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            occupiedSpots[spawnPoint.name] = false;
            cooldownSpots[spawnPoint.name] = false;
        }

        // Start checking when to begin spawning
        StartCoroutine(WaitForCountdown());
    }

    private IEnumerator WaitForCountdown()
    {
        while (!ScoreManager.Instance.gameStarted) // Wait for countdown to finish
        {
            yield return null;
        }

        // Start spawning targets after countdown
        InvokeRepeating(nameof(SpawnRandomTarget), 0f, spawnInterval);
    }

    void SpawnRandomTarget()
    {
        if (!ScoreManager.Instance.gameStarted) return; // Ensure no spawning before game starts

        List<string> availableSpots = new List<string>();

        foreach (var entry in occupiedSpots)
        {
            if (!entry.Value && !cooldownSpots[entry.Key]) // Check if the spot is not occupied or on cooldown
            {
                availableSpots.Add(entry.Key);
            }
        }

        if (availableSpots.Count == 0) return; // No available spawn spots

        // Get a random available spawn point
        string selectedSpotName = availableSpots[Random.Range(0, availableSpots.Count)];
        Transform spawnPoint = System.Array.Find(spawnPoints, sp => sp.name == selectedSpotName);

        // Decide whether to spawn a target or a bomb
        GameObject spawnedObject;
        if (Random.value < bombChance)
        {
            spawnedObject = Instantiate(bombPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            spawnedObject = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);
        }

        // Mark the spawn point as occupied
        occupiedSpots[selectedSpotName] = true;

        // Move to hidden position before rising up
        spawnedObject.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 1.5f, spawnPoint.position.z);

        // Assign spawn info to target
        spawnedObject.GetComponent<TargetBase>().SetSpawnPoint(spawnPoint, this);
    }


    public void FreeSpawnPoint(Transform spawnPoint)
    {
        if (occupiedSpots.ContainsKey(spawnPoint.name))
        {
            occupiedSpots[spawnPoint.name] = false;
            cooldownSpots[spawnPoint.name] = true; // Start cooldown for the spawn point
            StartCoroutine(CooldownTimer(spawnPoint.name));
        }
    }

    private IEnumerator CooldownTimer(string spawnPointName)
    {
        // Wait for the respawn cooldown before allowing the spawn point to be used again
        yield return new WaitForSeconds(respawnCooldown);
        cooldownSpots[spawnPointName] = false; // Make the spawn point available again
    }

    public void StartSpawning()
    {
        // Start spawning targets and bombs at the specified interval
        InvokeRepeating(nameof(SpawnRandomTarget), 1f, spawnInterval);
    }

}
