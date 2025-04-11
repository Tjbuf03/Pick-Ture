using UnityEngine;
using System.Collections;

public class InfoTargetBase : MonoBehaviour
{
    private Transform spawnPoint;
    private InfoTargetSpawner spawner;
    private float visibleDuration = 3f;
    private float moveSpeed = 3f;
    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;

    public void SetSpawnPoint(Transform point, InfoTargetSpawner targetSpawner)
    {
        spawnPoint = point;
        spawner = targetSpawner;

        hiddenPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 1.5f, 0f);
        visiblePosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);

        transform.position = hiddenPosition;

        StartCoroutine(RiseUp());
    }

    private IEnumerator RiseUp()
    {
        while (Vector3.Distance(transform.position, visiblePosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, visiblePosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(visibleDuration);

        StartCoroutine(LowerDown());
    }

    private IEnumerator LowerDown()
    {
        while (Vector3.Distance(transform.position, hiddenPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, hiddenPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

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
        if (spawnPoint != null)
        {
            spawner.FreeSpawnPoint(spawnPoint);
        }
        Destroy(gameObject);
    }
}
