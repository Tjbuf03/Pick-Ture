using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PieceCount : MonoBehaviour
{
    [SerializeField]private int pieces;
    [SerializeField]private TMP_Text piecesText;
    // Start is called before the first frame update
    void Start()
    {
        pieces = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Text UI displays number of collected pieces
        piecesText.text = "Pieces:" + pieces;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Piecescript ps = collision.gameObject.GetComponent<Piecescript>();
        if(ps)
        {
            pieces++;
        }
    }
}
