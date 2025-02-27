using UnityEngine;

public class Bomb : TargetBase
{
    private int penalty = 100;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.5f)
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
