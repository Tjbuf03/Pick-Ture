using UnityEngine;

public class InfoBomb : MonoBehaviour
{
    private InfoTargetBase baseScript;

    void Start()
    {
        baseScript = GetComponent<InfoTargetBase>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null && Vector2.Distance(transform.position, crosshair.transform.position) < 0.75f)
            {
                baseScript.DestroySelf();
            }
        }
    }
}
