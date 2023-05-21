using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HomeMenu : MonoBehaviour
{

    public Player player;
    public UIDocument MenuUIDoc;
    public UIDocument OptionsUIDoc;
    public GameObject OptionsUI;
    public GameObject MainMenuUI;
    //public GameObject ContinueButton;
    // Start is called before the first frame update

    private void Awake()
    {
        MenuUIDoc.rootVisualElement.Q<Button>("NewGameMain").clicked += () => NewGame();
        MenuUIDoc.rootVisualElement.Q<Button>("ContinueMain").clicked += () => ContinueGame();
        MenuUIDoc.rootVisualElement.Q<Button>("ExitMain").clicked += () => ExitGame();
        MenuUIDoc.rootVisualElement.Q<Button>("OptionsMain").clicked += () => GoToOptions();
        OptionsUIDoc.rootVisualElement.Q<Button>("BackOptions").clicked += () => LeaveOptions();
    }

    void Start()
    {
        OptionsUIDoc.rootVisualElement.style.display = DisplayStyle.None;
        string path = Application.persistentDataPath + "/player.file";
        Debug.Log(path);
        if (File.Exists(path) && SceneManager.GetSceneByName("Menu").isLoaded)
        {
            MenuUIDoc.rootVisualElement.Q<Button>("ContinueMain").style.display = DisplayStyle.Flex;
        } else
        {
            MenuUIDoc.rootVisualElement.Q<Button>("ContinueMain").style.display = DisplayStyle.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
    }
    
    public void GoToOptions()
    {
        MenuUIDoc.rootVisualElement.style.display = DisplayStyle.None;
        OptionsUIDoc.rootVisualElement.style.display = DisplayStyle.Flex;
    }
    
    public void LeaveOptions()
    {
        MenuUIDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        OptionsUIDoc.rootVisualElement.style.display = DisplayStyle.None;
    }


    public void ContinueGame()
    {
        SceneManager.LoadScene("1");
        Time.timeScale = 1;
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("SeenTutorial", 0);
        Vector3 restart = new Vector3(-3f, -7f, 0);
        SaveSystem.SavePlayer(player, restart, false);
        SceneManager.LoadScene("1");
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
