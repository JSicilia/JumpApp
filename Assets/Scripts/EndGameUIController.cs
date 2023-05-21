using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndGameUIController : MonoBehaviour
{

    private VisualElement Menu;
    private VisualElement[] MenuOptions;

    private const string POPUP_ANIMATION = "pop-animation-hide";


    // Start is called before the first frame update
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Menu = root.Q<VisualElement>("Menu");
        //MenuOptions = Menu.Q<VisualElement>("Body").Children();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        Menu.ToggleInClassList(POPUP_ANIMATION);
    }
}
