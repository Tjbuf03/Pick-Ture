using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera; // Assign your camera in the Inspector
    public float zoomOutSize = 10f; // Desired zoom-out size
    public float zoomSpeed = 5f; // Speed of the zoom transition
    private float originalSize; // Stores the original camera size
    private float targetSize; // The size the camera is zooming towards

    void Start()
    {
        // Initialize the camera and set the original size
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        originalSize = mainCamera.orthographicSize;
        targetSize = originalSize; // Start with the original size as the target
    }

    void Update()
    {
        // Update the target size based on input
        if (Input.GetKey(KeyCode.W))
        {
            targetSize = zoomOutSize;
        }
        else
        {
            targetSize = originalSize;
        }

        // Gradually zoom the camera using Lerp
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            targetSize,
            Time.deltaTime * zoomSpeed
        );
    }
}
