using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;      // Platform prefab
    public Transform platformParent;       // Parent for the platforms (optional, for organization)
    public int maxPlatforms = 5;           // Max platforms allowed
    public List<GameObject> platforms = new List<GameObject>();  // List to track platforms

    // Spawn a platform at a specified position
    public void SpawnPlatform(Vector3 position)
    {
        if (platforms.Count < maxPlatforms)
        {
            GameObject newPlatform = Instantiate(platformPrefab, position, Quaternion.identity, platformParent);
            newPlatform.transform.position = new Vector3(newPlatform.transform.position.x, newPlatform.transform.position.y, 0);  // Ensure it stays flat
            platforms.Add(newPlatform);
        }
    }

    // Remove the first platform placed (FIFO)
    public void RemoveFirstPlatform()
    {
        if (platforms.Count > 0)
        {
            GameObject firstPlatform = platforms[0];
            platforms.RemoveAt(0);
            Destroy(firstPlatform);
        }
    }
}


