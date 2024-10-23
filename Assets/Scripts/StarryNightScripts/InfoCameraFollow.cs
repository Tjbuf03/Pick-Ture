using UnityEngine;

public class InfoCameraFollow : MonoBehaviour
{
    public float scrollSpeed = 2f;  // Speed at which the camera moves sideways

    void Update()
    {
        // Move the camera to the right at a constant speed
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
    }
}
