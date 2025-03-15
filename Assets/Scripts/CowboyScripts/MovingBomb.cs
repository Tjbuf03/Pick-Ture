using UnityEngine;

public class MovingBomb : TargetBase
{
    [Header("Movement Settings")]
    public float speed = 5f; // Adjust speed in Inspector
    public bool moveRight = true; // Set direction in Inspector

    private Vector3 movementDirection;
    private int penalty = 100; // Same penalty as normal bomb

    void Start()
    {
        // Set movement direction based on moveRight toggle
        movementDirection = moveRight ? Vector3.right : Vector3.left;

        // Ensure the bomb is at Z = 0 so it's visible
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        // Move the bomb in the chosen direction
        transform.Translate(movementDirection * speed * Time.deltaTime);

        // Check if player presses P and the crosshair is near the bomb
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector3.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                SubtractPoints();
                ResetCombo(); // Reset combo when bomb is hit
                DestroySelf(); // Destroy the bomb
            }
        }
    }

    void SubtractPoints()
    {
        // Subtract points when bomb is hit
        ScoreManager.Instance.SubtractScore(penalty);
    }

    void ResetCombo()
    {
        // Reset the combo in the ScoreManager
        ScoreManager.Instance.ResetCombo();
    }
}
