using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
