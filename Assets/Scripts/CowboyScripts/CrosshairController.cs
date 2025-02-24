using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
        transform.position += move;
    }
}
