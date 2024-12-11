using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{   
    //Creates list to add all  in this scene to
    public List<GameObject> PieceList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MainManager.Instance.Collected0){
            Destroy(PieceList[0]);
        }
        if(MainManager.Instance.Collected1){
            Destroy(PieceList[1]);
        }
        if(MainManager.Instance.Collected2){
            Destroy(PieceList[2]);
        }
        if(MainManager.Instance.Collected3){
            Destroy(PieceList[3]);
        }
        if(MainManager.Instance.Collected4){
            Destroy(PieceList[4]);
        }
        if(MainManager.Instance.Collected5){
            Destroy(PieceList[5]);
        }
        if (MainManager.Instance.Collected6)
        {
            Destroy(PieceList[6]);
        }
        if (MainManager.Instance.Collected7)
        {
            Destroy(PieceList[7]);
        }
        if (MainManager.Instance.Collected8)
        {
            Destroy(PieceList[8]);
        }
        if (MainManager.Instance.Collected9)
        {
            Destroy(PieceList[9]);
        }
        if (MainManager.Instance.Collected10)
        {
            Destroy(PieceList[10]);
        }
        if (MainManager.Instance.Collected11)
        {
            Destroy(PieceList[11]);
        }
        if (MainManager.Instance.Collected12)
        {
            Destroy(PieceList[12]);
        }
        if (MainManager.Instance.Collected13)
        {
            Destroy(PieceList[13]);
        }
        if (MainManager.Instance.Collected14)
        {
            Destroy(PieceList[14]);
        }
        if (MainManager.Instance.Collected15)
        {
            Destroy(PieceList[15]);
        }
        if (MainManager.Instance.Collected16)
        {
            Destroy(PieceList[16]);
        }
        if (MainManager.Instance.Collected17)
        {
            Destroy(PieceList[17]);
        }

    }
}
