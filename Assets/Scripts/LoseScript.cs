using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScript : MonoBehaviour
{

    //This script is placed on objects that can cause you to lose 

    //space for scene name of lose scene to be entered
    [SerializeField] private string SceneName;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
        if(pm)
        {
             SceneManager.LoadScene(SceneName); 

             // Player is not returning from painting
            MainManager.Instance.isReturning = false;
        }
       
    }
}
