using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    //Declared static to make sure there is only one instance across all MainManagers
    public static MainManager Instance;

    public int PieceNumber;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Awake happens one time when the object is created
    private void Awake()
    {
        //If there is already an instance of the MainManager object, the second one will delete itself
        //This makes sure there is only one instance no matter how many times you come back to the same scenes, called a singleton
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Tells game not to destroy object this is attached to as the scene changes
        DontDestroyOnLoad(gameObject);
    }
}
