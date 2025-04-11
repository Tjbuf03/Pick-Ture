using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject shotEffectPrefab; // Assign your prefab here in the Inspector

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
        transform.position += move;

        if (Input.GetKeyDown(KeyCode.P))
        {
            TryShowShotEffect(); // By default, try to show effect
        }
    }

    public void TryShowShotEffect()
    {
        if (shotEffectPrefab != null)
        {
            GameObject effect = Instantiate(shotEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.1f); // Auto destroy after brief time
        }
    }

    public void SuppressShotEffect()
    {
        // Nothing needed here anymore since the prefab is spawned once
        // But you can still keep this method if bombs want to override default behavior
    }
}
