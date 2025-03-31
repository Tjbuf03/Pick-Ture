using UnityEngine;

public class Target : TargetBase
{
    private int maxPoints = 150;
    private int minPoints = 75;
    private float lifetime = 3f;
    private float timeAlive = 0f;

    void Update()
    {
        timeAlive += Time.deltaTime;
        int currentScore = Mathf.RoundToInt(Mathf.Lerp(maxPoints, minPoints, timeAlive / lifetime));

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                ScoreManager.Instance.AddScore(currentScore);
                ScoreManager.Instance.UpdateCombo(); // Call to update combo
                DestroySelf(); // Call to destroy the target
            }
        }
    }
}
