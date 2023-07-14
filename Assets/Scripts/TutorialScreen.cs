using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private GameObject TutorialUI;
    [SerializeField] private GameObject TutorialUIButtons;
    [SerializeField] private Text TutorialTag;
    [SerializeField] private Text TutorialContTag;
    [SerializeField] private Text leftTag;
    [SerializeField] private Text jumpTag;
    [SerializeField] private Text rightTag;
    private int TutorialShown;
    private int TutorialsOff;

    // Start is called before the first frame update
    void Start()
    {
        TutorialUI.SetActive(false);
        TutorialUIButtons.SetActive(false);
        TutorialsOff = PlayerPrefs.GetInt("TutorialsOff", 0);
        TutorialShown = PlayerPrefs.GetInt("SeenTutorial", 0);
        Debug.Log(TutorialShown + " + " + TutorialsOff);
        if (TutorialShown == 0 & TutorialsOff == 0) {
            Time.timeScale = 0;
            TutorialUI.SetActive(true);
            TutorialUIButtons.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Time.timeScale = 1;
            LeanTween.alphaText(TutorialContTag.rectTransform, 0f, 2f).setEase(LeanTweenType.linear);
            LeanTween.alphaText(leftTag.rectTransform, 0f, 2f).setEase(LeanTweenType.linear);
            LeanTween.alphaText(jumpTag.rectTransform, 0f, 2f).setEase(LeanTweenType.linear);
            LeanTween.alphaText(rightTag.rectTransform, 0f, 2f).setEase(LeanTweenType.linear);
            LeanTween.alphaText(TutorialTag.rectTransform, 0f, 2f).setEase(LeanTweenType.linear).setOnComplete(SeenTutorial);

        }
    }

    void SeenTutorial()
    {
        TutorialShown = 1;
        PlayerPrefs.SetInt("SeenTutorial", TutorialShown);
        TutorialUI.SetActive(false);
    }
}
