using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera; // Assign your camera in the Inspector
    public float zoomOutSize = 10f; // Set this to the desired zoomed-out size
    private float originalSize;

    void Start()
    {
        // Save the camera's original size at the start
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        originalSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        // Check if "W" is pressed and adjust the camera size
        if (Input.GetKey(KeyCode.W))
        {
            mainCamera.orthographicSize = zoomOutSize;
        }
        else
        {
            mainCamera.orthographicSize = originalSize;
        }
    }
}
