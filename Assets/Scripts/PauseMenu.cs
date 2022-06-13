using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject PauseButton;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        LeanTween.scale(PauseMenuUI, new Vector3(0.9f, 0.5f, 1), 0.2f).setEaseInQuint().setIgnoreTimeScale(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        LeanTween.scale(PauseMenuUI, new Vector3(0, 0, 0), 0.2f).setEaseOutQuint().setIgnoreTimeScale(true).setOnComplete(ClosePauseMenu);
        
    }

    public void ClosePauseMenu()
    {
        PauseMenuUI.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        player.SavePlayer();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu"); 
    }

    public void Restart()
    {
        Vector3 restart = new Vector3(-3f, -7f, 0);
        SaveSystem.SavePlayer(player, restart, false);
        SceneManager.LoadScene("1");
        Time.timeScale = 1;
    }
}
