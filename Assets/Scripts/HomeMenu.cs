using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public Player player;
    public GameObject OptionUI;
    public GameObject MainMenuUI;
    public GameObject ContinueButton;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/player.file";
        Debug.Log(path);
        if (File.Exists(path) && SceneManager.GetSceneByName("Menu").isLoaded)
        {
            ContinueButton.SetActive(true);
        } else
        {
            ContinueButton.SetActive(false);
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
        LeanTween.scale(MainMenuUI, new Vector3(0, 0, 0), 0.4f).setEaseOutQuint().setOnComplete(CloseMenuForOptions);
        OptionUI.SetActive(true);
    }

    public void CloseMenuForOptions()
    {
        MainMenuUI.SetActive(false);
        LeanTween.scale(OptionUI, new Vector3(0.8f, 0.6f, 1), 0.3f).setEaseInQuint();
        
    }

    public void LeaveOptions()
    {
        LeanTween.scale(OptionUI, new Vector3(0, 0, 0), 0.4f).setEaseOutQuint().setOnComplete(CloseOptionsForMenu);
        MainMenuUI.SetActive(true);
    }

    public void CloseOptionsForMenu()
    {
        OptionUI.SetActive(false);
        LeanTween.scale(MainMenuUI, new Vector3(0.8f, 0.6f, 1), 0.3f).setEaseInQuint();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("1");
        Time.timeScale = 1;
    }

    public void NewGame()
    {
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
