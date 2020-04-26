using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Events;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    
    public GameObject pauseMenuUI;
    private readonly QuitGameEvent ev_quitgame = new QuitGameEvent();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Aprete la P");
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            } 
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        
        //Debug.Log("Menu");
    }

    public void QuitGame()
    {
        EventController.TriggerEvent(ev_quitgame);

        SessionData.SaveData();



        Application.Quit();
    }
    
}
