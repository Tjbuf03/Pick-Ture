using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    //This script is currently placed on the Mona Lisa

    //space for scene name of win scene to be entered
    [SerializeField] private string SceneName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Checks if total number of pieces are collected, update piece number until we decide on a final one
        if(MainManager.Instance.PieceNumber == 16)
        {   
            SceneManager.LoadScene(SceneName);

            MainManager.Instance.isRestarting = true;
        }
    }
}
