using UnityEngine;

public class SwingingPlatform : MonoBehaviour
{
    public Transform player;         // Reference to the player transform
    public float swingSpeed = 5f;    // Speed of the swing
    public float swingAmount = 5f;   // Amount of swing
    public float returnSpeed = 2f;   // Speed at which platform returns to original position

    private Vector3 initialPosition;
    private float direction;
    private bool playerOnPlatform = false; // Track if player is on platform

    void Start()
    {
        initialPosition = transform.position;  // Store the initial position of the platform
    }

    void Update()
    {
        if (playerOnPlatform)
        {
            // Determine the player's position relative to the platform
            float playerDirection = player.position.x > transform.position.x ? 1 : -1;

            // Lerp the direction smoothly
            direction = Mathf.Lerp(direction, playerDirection, Time.deltaTime * swingSpeed);

            // Swing the platform based on direction
            float swing = Mathf.Sin(Time.time * swingSpeed) * swingAmount * direction;
            transform.position = initialPosition + new Vector3(swing, 0, 0);
        }
        else
        {
            // Smoothly move the platform back to its original position when player is not on it
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * returnSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            playerOnPlatform = true; // Start swinging when player is on the platform
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            playerOnPlatform = false; // Stop swinging when player leaves the platform
            direction = 0; // Reset direction to prepare for swinging next time
        }
    }
}
