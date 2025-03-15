using UnityEngine;

public class MovingBombSpawner : MonoBehaviour
{
    public GameObject movingBombPrefab; // Assign in Inspector
    public float spawnInterval = 3f; // Change in Inspector
    public float initialDelay = 5f; // Delay before first spawn

    private float timer = 0f;
    private bool canSpawn = false;

    void Start()
    {
        Invoke("EnableSpawning", initialDelay);
    }

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnMovingBomb();
            timer = 0f; // Reset timer
        }
    }

    void EnableSpawning()
    {
        canSpawn = true;
    }

    void SpawnMovingBomb()
    {
        Instantiate(movingBombPrefab, transform.position, Quaternion.identity);
    }
}
