using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();  // List to track all placed platforms

    // Add a platform to the list
    public void AddPlatform(GameObject platform)
    {
        platforms.Add(platform);
    }

    // Remove the last placed platform
    public void RemoveLastPlatform()
    {
        if (platforms.Count > 0)
        {
            // Get the last platform in the list
            GameObject lastPlatform = platforms[platforms.Count - 1];

            // Remove it from the list
            platforms.RemoveAt(platforms.Count - 1);

            // Destroy the platform
            Destroy(lastPlatform);
        }
    }
}

