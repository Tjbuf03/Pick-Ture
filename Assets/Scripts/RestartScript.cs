using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MainManager.Instance.isRestarting == true)
        {
            //Bool Armageddon
            MainManager.Instance.GlideUnlocked = false;
            MainManager.Instance.CannonUnlocked = false;
            MainManager.Instance.TNTUnlocked = false;
            MainManager.Instance.Collected0 = false;
            MainManager.Instance.Collected1 = false;
            MainManager.Instance.Collected2 = false;
            MainManager.Instance.Collected3 = false;
            MainManager.Instance.Collected4 = false;
            MainManager.Instance.Collected5 = false;
            MainManager.Instance.Collected6 = false;
            MainManager.Instance.Collected7 = false;
            MainManager.Instance.Collected8 = false;
            MainManager.Instance.Collected9 = false;
            MainManager.Instance.Collected10 = false;
            MainManager.Instance.Collected11 = false;
            MainManager.Instance.Collected12 = false;
            MainManager.Instance.Collected13 = false;
            MainManager.Instance.Collected14 = false;
            MainManager.Instance.Collected15 = false;
            MainManager.Instance.Collected16 = false;
            MainManager.Instance.Collected17 = false;
            MainManager.Instance.CollectedS0 = false;
            MainManager.Instance.CollectedS1 = false;
            MainManager.Instance.CollectedS2 = false;
            MainManager.Instance.CollectedC0 = false;
            MainManager.Instance.CollectedC1 = false;
            MainManager.Instance.CollectedC2 = false;
            MainManager.Instance.CollectedC3 = false;
            MainManager.Instance.CollectedC4 = false;
            MainManager.Instance.CollectedC5 = false;
            MainManager.Instance.CollectedC6 = false;
            MainManager.Instance.CollectedC7 = false;
            MainManager.Instance.CollectedC8 = false;

            //Piece number reset
            MainManager.Instance.PieceNumber = 0;
        }
    }
}
