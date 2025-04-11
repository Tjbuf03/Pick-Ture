using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject bombPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public float bombChance = 0.3f;
    public float respawnCooldown = 1f;

    private Dictionary<string, bool> occupiedSpots = new Dictionary<string, bool>();
    private Dictionary<string, bool> cooldownSpots = new Dictionary<string, bool>();

    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            occupiedSpots[spawnPoint.name] = false;
            cooldownSpots[spawnPoint.name] = false;
        }

        InvokeRepeating(nameof(SpawnRandomTarget), 0f, spawnInterval);
    }

    void SpawnRandomTarget()
    {
        List<string> availableSpots = new List<string>();

        foreach (var entry in occupiedSpots)
        {
            if (!entry.Value && !cooldownSpots[entry.Key])
            {
                availableSpots.Add(entry.Key);
            }
        }

        if (availableSpots.Count == 0) return;

        string selectedSpotName = availableSpots[Random.Range(0, availableSpots.Count)];
        Transform spawnPoint = System.Array.Find(spawnPoints, sp => sp.name == selectedSpotName);

        GameObject spawnedObject;
        if (Random.value < bombChance)
        {
            spawnedObject = Instantiate(bombPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            spawnedObject = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);
        }

        occupiedSpots[selectedSpotName] = true;

        spawnedObject.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 1.5f, 0f); // z = 0

        spawnedObject.GetComponent<InfoTargetBase>().SetSpawnPoint(spawnPoint, this);
    }

    public void FreeSpawnPoint(Transform spawnPoint)
    {
        if (occupiedSpots.ContainsKey(spawnPoint.name))
        {
            occupiedSpots[spawnPoint.name] = false;
            cooldownSpots[spawnPoint.name] = true;
            StartCoroutine(CooldownTimer(spawnPoint.name));
        }
    }

    private IEnumerator CooldownTimer(string spawnPointName)
    {
        yield return new WaitForSeconds(respawnCooldown);
        cooldownSpots[spawnPointName] = false;
    }
}
