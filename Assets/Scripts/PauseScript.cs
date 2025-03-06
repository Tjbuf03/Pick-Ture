using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{

    public bool paused;
    public GameObject PauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            if(paused)
            {
                PauseGame();
            }

            if(!paused)
            {
                ResumeGame();
            }
        }

        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }
}
