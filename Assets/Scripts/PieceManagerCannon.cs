using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManagerCannon : MonoBehaviour
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
        if(MainManager.Instance.CollectedC0){
            Destroy(PieceList[0]);
        }
        if(MainManager.Instance.CollectedC1){
            Destroy(PieceList[1]);
        }
        if(MainManager.Instance.CollectedC2){
            Destroy(PieceList[2]);
        }
    }
}
