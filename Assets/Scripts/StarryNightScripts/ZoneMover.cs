using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MovableObject
{
    public GameObject gameObject;        // The GameObject to move
    public Vector3 direction = Vector3.one.normalized;  // The movement direction, default diagonal
    public float speed = 3f;             // Movement speed
}

public class ZoneMover : MonoBehaviour
{
    public List<MovableObject> objectsToMove;  // List of objects with directions and speeds
    public float zoneWidth = 2f;               // Width of the zone around the camera
    public float zoneHeight = 2f;              // Height of the zone around the camera

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Calculate the boundaries of the zone based on camera's position and zone size
        Vector3 cameraPos = mainCamera.transform.position;
        float minX = cameraPos.x - mainCamera.orthographicSize * mainCamera.aspect - zoneWidth;
        float maxX = cameraPos.x + mainCamera.orthographicSize * mainCamera.aspect + zoneWidth;
        float minY = cameraPos.y - mainCamera.orthographicSize - zoneHeight;
        float maxY = cameraPos.y + mainCamera.orthographicSize + zoneHeight;

        // Check each MovableObject to see if it's within the activation zone
        foreach (MovableObject obj in objectsToMove)
        {
            if (obj.gameObject != null)  // Ensure the object exists
            {
                Vector3 objPos = obj.gameObject.transform.position;

                // Start moving the object if it's within the zone boundaries
                if (objPos.x > minX && objPos.x < maxX && objPos.y > minY && objPos.y < maxY)
                {
                    MoveObject(obj);
                }
            }
        }
    }

    void MoveObject(MovableObject obj)
    {
        // Move the object in its specified direction and speed
        obj.gameObject.transform.Translate(obj.direction.normalized * obj.speed * Time.deltaTime);
    }
}
