using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{   
    //Declared static to make sure there is only one instance across all PieceManagers
    public static PieceManager Instance;
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

    }
}
