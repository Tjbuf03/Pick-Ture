using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab; // Assign your target prefab
    public Transform[] spawnPoints; // Assign empty GameObjects here
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnTarget), 1f, spawnInterval);
    }

    void SpawnTarget()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(targetPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
