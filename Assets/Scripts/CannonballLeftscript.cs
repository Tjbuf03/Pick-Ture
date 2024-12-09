using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballLeftscript : MonoBehaviour
{   
    [SerializeField] private float speed;
    [SerializeField] private float despawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;

        despawnTimer -= Time.deltaTime;

        if(despawnTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy(gameObject);
    }
}
