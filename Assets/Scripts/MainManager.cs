using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    //Declared static to make sure there is only one instance across all MainManagers
    public static MainManager Instance;

    public int PieceNumber;

    public Vector3 PlayerPos;

    public bool isReturning;

    public bool isRestarting;

    [Header("Upgrade Bools")]
    public bool GlideUnlocked;
    public bool CannonUnlocked;
    public bool TNTUnlocked;

    [Header("Piece Bools")]
    public bool Collected0;
    public bool Collected1;
    public bool Collected2;
    public bool Collected3;
    public bool Collected4;
    public bool Collected5;
    public bool Collected6;
    public bool Collected7;
    public bool Collected8;
    public bool Collected9;
    public bool Collected10;
    public bool Collected11;
    public bool Collected12;
    public bool Collected13;
    public bool Collected14;
    public bool Collected15;
    public bool Collected16;
    public bool Collected17;
    public bool CollectedS0;
    public bool CollectedS1;
    public bool CollectedS2;
    public bool CollectedC0;
    public bool CollectedC1;
    public bool CollectedC2;
    public bool CollectedC3;
    public bool CollectedC4;
    public bool CollectedC5;
    public bool CollectedC6;
    public bool CollectedC7;
    public bool CollectedC8;

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

        //Bool begins unchecked
        isReturning = false;

        //Bool begins unchecked
        isRestarting = false;

        //Bool begins unchecked
        GlideUnlocked = false;

        //Bool begins unchecked
        CannonUnlocked = false;

        //Bool begins unchecked
        TNTUnlocked = false;
    }
}
