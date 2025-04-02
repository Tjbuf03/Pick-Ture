using UnityEngine;

public class Bomb : TargetBase
{
    private int penalty = 100;
    public GameObject explosionPrefab; // Assign in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                SubtractPoints();
                ResetCombo(); // Reset combo when bomb is hit
                Explode(); // Show explosion effect
                DestroySelf(); // Destroy the bomb
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
            Destroy(explosion, 0.5f); // Adjust timing to match animation length
        }
    }
}
