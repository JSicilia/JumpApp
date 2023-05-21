using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialScreen : MonoBehaviour
{
    public UIDocument TutorialUI;
    private int TutorialShown;
    private int TutorialsOff;

    private void Awake()
    {
        TutorialUI.rootVisualElement.Q<Label>("MainText").RegisterCallback<ClickEvent>(Exit);
    }

    // Start is called before the first frame update
    void Start()
    {
        TutorialUI.rootVisualElement.style.display = DisplayStyle.None;
        TutorialsOff = PlayerPrefs.GetInt("TutorialsOff", 0);
        TutorialShown = PlayerPrefs.GetInt("SeenTutorial", 0);
        Debug.Log(TutorialShown + " + " + TutorialsOff);
        if (TutorialShown == 0 & TutorialsOff == 0) {
            TutorialUI.rootVisualElement.style.display = DisplayStyle.Flex;
            TutorialShown = 1;
            SeenTutorial();
        }
    }

    void SeenTutorial()
    {
        PlayerPrefs.SetInt("SeenTutorial", TutorialShown);
    }

    private void Exit(ClickEvent evt)
    {
        TutorialUI.rootVisualElement.style.display = DisplayStyle.None;

    }
}
