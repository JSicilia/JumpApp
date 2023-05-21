using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCheck : MonoBehaviour
{
    public GameObject Checker;

    //public Image ChargeRight;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LeanTween.alpha(Checker.GetComponent<Image>().rectTransform, 0.2f, 0.5f);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LeanTween.alpha(Checker.GetComponent<Image>().rectTransform, 1f, 0.5f);
    }
}
