using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManagerCannon3 : MonoBehaviour
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
        if(MainManager.Instance.CollectedC6){
            Destroy(PieceList[0]);
        }
        if(MainManager.Instance.CollectedC7){
            Destroy(PieceList[1]);
        }
        if(MainManager.Instance.CollectedC8){
            Destroy(PieceList[2]);
        }
    }
}
