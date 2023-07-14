using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCheck : MonoBehaviour
{
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject tutorialText;

    private RectTransform controllerWrap;
    private HorizontalLayoutGroup layoutGroup;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        controllerWrap = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();


        float ScreenWidth = Screen.width / 10f;

        //Dynamic Box Collider 
        //boxCollider.size = new Vector2(2000f, ScreenWidth * 3.5f);

        //Dynamic UI buttons
        left.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidth * 3f, ScreenWidth * 3f);
        jump.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidth * 3.5f, ScreenWidth * 3.5f);
        right.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidth * 3f, ScreenWidth * 3f);
        layoutGroup.spacing = (ScreenWidth / 10f) * 2.5f;
        controllerWrap.anchoredPosition = new Vector2(0f, (ScreenWidth * 4f) / 2f);
        tutorialText.GetComponent<HorizontalLayoutGroup>().spacing = (ScreenWidth / 10f) * 2.5f;
        tutorialText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, (ScreenWidth * 5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LeanTween.alpha(left.GetComponent<Image>().rectTransform, 0.2f, 0.3f);
        LeanTween.alpha(jump.GetComponent<Image>().rectTransform, 0.2f, 0.3f);
        LeanTween.alpha(right.GetComponent<Image>().rectTransform, 0.2f, 0.3f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LeanTween.alpha(left.GetComponent<Image>().rectTransform, 0.7f, 0.3f);
        LeanTween.alpha(jump.GetComponent<Image>().rectTransform, 0.7f, 0.3f);
        LeanTween.alpha(right.GetComponent<Image>().rectTransform, 0.7f, 0.3f);
    }
}
