using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    void Start()
    {
        PlayerOptions.Volume = AudioListener.volume;
        PlayerOptions.Difficulty = 1;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }


}
