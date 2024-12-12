using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Cannonballscript cb = collision.gameObject.GetComponent<Cannonballscript>();
        CannonballLeftscript cbl = collision.gameObject.GetComponent<CannonballLeftscript>();

        if(cb || cbl)
        {
            Destroy(gameObject);
        }
    }
}
