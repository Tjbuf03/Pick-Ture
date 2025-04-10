using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeIconScript : MonoBehaviour
{
    [SerializeField] private GameObject GlideIcon;
    [SerializeField] private GameObject CannonIcon;
    [SerializeField] private GameObject TNTIcon;

    //This script is located on the UpgradeIcons parent in the MainCanvas prefab

    void Start()
    {
        //Upgrade Icons begin hidden
        GlideIcon.SetActive(false);
        CannonIcon.SetActive(false);
        TNTIcon.SetActive(false);  
    }

    void Update()
    {
        if(MainManager.Instance.GlideUnlocked == true)
        {
            //Turns on Glide Icon
            GlideIcon.SetActive(true);
        }

        if(MainManager.Instance.CannonUnlocked == true)
        {
            //Turns on Cannon Icon
            CannonIcon.SetActive(true);
        }

        if(MainManager.Instance.TNTUnlocked == true)
        {
            //Turns on TNT Icon
            TNTIcon.SetActive(true);
        }
    }
}
