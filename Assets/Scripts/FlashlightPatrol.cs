using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPatrol : MonoBehaviour
{   
    //Credit: altered version of this tutorial https://youtu.be/4mzbDk4Wsmk?si=ffURKh0KXe_dvMNz

    //Creates array of waypoints, add empty objects to array as transforms
    [SerializeField]private Transform[] Waypoints;
    [SerializeField]private int targetPoint;
    [SerializeField]private float speed;
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Decides random next point if the flashlight gets to one point
        if(transform.position == Waypoints[targetPoint].position)
        {
            RandomPosition();
        }
        
        //In MoveTowards you first put the position you want it to move from then the position you want it to move to
        transform.position = Vector3.MoveTowards(transform.position, Waypoints[targetPoint].position, speed * Time.deltaTime); 
    }

    void RandomPosition()
    {
        targetPoint = Random.Range(0, Waypoints.Length);
    }
}
