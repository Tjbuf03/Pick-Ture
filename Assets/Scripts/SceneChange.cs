using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //public so it can be accessed by buttons or other scripts, the parameters are a smarter way to have the string be put in
    public void LoadScene(string SceneName)
    {
        //Always place on MainCamera or other object that remains active in the scene
        SceneManager.LoadScene(SceneName);
    }

}
