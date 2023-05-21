using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public GameObject DeathDisplay;
    public GameObject PauseMenuUI;
    public GameObject PauseButton;
    public UIDocument InGameMenu;
    public Player player;

    private void Awake()
    {
        InGameMenu.rootVisualElement.Q<Button>("ResumeIG").clicked += () => Resume();
        InGameMenu.rootVisualElement.Q<Button>("RestartIG").clicked += () => Restart();
        InGameMenu.rootVisualElement.Q<Button>("ExitIG").clicked += () => Exit();
    }

    private void Start()
    {
        InGameMenu.rootVisualElement.style.display = DisplayStyle.None;
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
        InGameMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        PauseButton.SetActive(false);
        Time.timeScale = 0;
        DeathDisplay.SetActive(true);
        LeanTween.alpha(DeathDisplay.GetComponent<RectTransform>(), 1f, 0f);
        //LeanTween.value(DeathCount.gameObject, a => DeathCount.color = a, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0f);
    }

    public void Resume()
    {
        InGameMenu.rootVisualElement.style.display = DisplayStyle.None;
        PauseButton.SetActive(true);
        Time.timeScale = 1;
        DeathDisplay.SetActive(false);
        LeanTween.alpha(DeathDisplay.GetComponent<RectTransform>(), 0f, 0f);
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
