using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public Player player;
    public GameObject OptionUI;
    public GameObject MainMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        //LeanTween.
    }

    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void NewGame()
    {
        Vector3 restart = new Vector3(-3f, -7f, 0);
        SaveSystem.SavePlayer(player, restart, false);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
