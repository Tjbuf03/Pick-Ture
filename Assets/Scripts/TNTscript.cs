using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTscript : MonoBehaviour
{

    [SerializeField] private float explodeDelay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        explodeDelay -= Time.deltaTime;

        if(explodeDelay <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
