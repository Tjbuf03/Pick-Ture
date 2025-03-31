using UnityEngine;
using System.Collections;

public class TargetBase : MonoBehaviour
{
    private Transform spawnPoint;
    private TargetSpawner spawner;
    private float visibleDuration = 3f; // Time the target stays up before descending
    private float moveSpeed = 3f; // Speed at which the target moves up/down
    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;

    public void SetSpawnPoint(Transform point, TargetSpawner targetSpawner)
    {
        spawnPoint = point;
        spawner = targetSpawner;

        // Set hidden and visible positions
        hiddenPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 1.5f, spawnPoint.position.z);
        visiblePosition = spawnPoint.position;

        // Start at the hidden position
        transform.position = hiddenPosition;

        // Move up and start the cycle
        StartCoroutine(RiseUp());
    }

    private IEnumerator RiseUp()
    {
        // Move from hidden position to visible position
        while (Vector3.Distance(transform.position, visiblePosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, visiblePosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait at the top before lowering back down
        yield return new WaitForSeconds(visibleDuration);

        // Start moving down
        StartCoroutine(LowerDown());
    }

    private IEnumerator LowerDown()
    {
        // Move back down to hidden position
        while (Vector3.Distance(transform.position, hiddenPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, hiddenPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Despawn after moving down
        Despawn();
    }

    private void Despawn()
    {
        if (spawnPoint != null)
        {
            spawner.FreeSpawnPoint(spawnPoint);
        }
        Destroy(gameObject);
    }

    public void DestroySelf()
    {
        // If destroyed early (hit), free the spawn point and remove instantly
        if (spawnPoint != null)
        {
            spawner.FreeSpawnPoint(spawnPoint);
        }
        Destroy(gameObject);
    }
}
