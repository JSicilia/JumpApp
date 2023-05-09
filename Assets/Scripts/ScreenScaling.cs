using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenScaling : MonoBehaviour
{
    private static bool created = false;

    public float resX;

    public float resY;

    private CanvasScaler can;

    // Start is called before the first frame update
    void Start()
    {
        
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        can = GetComponent<CanvasScaler>();

        ScaleScreenSize();
    }


    private void ScaleScreenSize()
    {
        resX = (float)Screen.currentResolution.width;
        resY = (float)Screen.currentResolution.height;

        can.referenceResolution = new Vector2(resX, resY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
