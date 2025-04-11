using UnityEngine;

public class MovingBomb : TargetBase
{
    [Header("Movement Settings")]
    public float speed = 5f; // Adjust speed in Inspector
    public bool moveRight = true; // Set direction in Inspector
    public float rotationSpeed = 200f; // Adjust rotation speed in Inspector
    public GameObject explosionPrefab; // Assign in Inspector

    private Vector3 movementDirection;
    private int penalty = 100; // Same penalty as normal bomb

    void Start()
    {
        movementDirection = moveRight ? Vector3.right : Vector3.left;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        float rotationDirection = moveRight ? -1f : 1f;
        transform.Rotate(0, 0, rotationSpeed * rotationDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector3.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                SubtractPoints();
                ResetCombo();
                Explode();
                DestroySelf();
            }
        }
    }

    void SubtractPoints()
    {
        ScoreManager.Instance.SubtractScore(penalty);
    }

    void ResetCombo()
    {
        ScoreManager.Instance.ResetCombo();
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.15f, 0.1f); // You can tweak the duration and magnitude
        }
    }
}
