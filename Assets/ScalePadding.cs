using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalePadding : MonoBehaviour
{
    public LayoutGroup UIGroup;
    public int paddingTop;
    public int paddingBottom;
    public int paddingLR;
    public int percentageSides;
    public int percentageTop;
    public int percentageBottom;

    // Start is called before the first frame update
    void Start()
    {

        paddingTop = (Screen.height / 100) * percentageTop;
        UIGroup.padding.top = paddingTop;
        paddingBottom = (Screen.height / 100) * percentageBottom;
        UIGroup.padding.bottom = paddingBottom;
        paddingLR = (Screen.height / 100) * percentageSides;
        UIGroup.padding.left = paddingLR;
        UIGroup.padding.right = paddingLR;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
