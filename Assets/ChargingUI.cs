using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingUI : MonoBehaviour
{
    public GameObject LeftCharge;
    public GameObject RightCharge;
    private float screenHeight;
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        Debug.Log("screen height: " + screenHeight);
        Debug.Log("screen width: " + screenWidth);
        RectTransform RightRt = RightCharge.GetComponent<RectTransform>();
        RightRt.sizeDelta = new Vector2((screenWidth/100) * 48, (screenHeight/100) * 10);
        RightRt.anchoredPosition = new Vector3(-((screenWidth / 100) * 26), (screenHeight / 100) * 7, 0);

        RectTransform LeftRt = LeftCharge.GetComponent<RectTransform>();
        LeftRt.sizeDelta = new Vector2((screenWidth / 100) * 48, (screenHeight / 100) * 10);
        LeftRt.anchoredPosition = new Vector3((screenWidth / 100) * 26, (screenHeight / 100) * 7, 0);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
