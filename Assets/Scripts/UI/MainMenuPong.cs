using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuPong : MonoBehaviour
{

    private GameObject go_scores; 
    private TextMeshProUGUI tx_scores; 
    private string tx_string;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void QuitGame()
    {
 //       Debug.Log("Quit!");
        Application.Quit();
        SceneManager.LoadScene(0);
    }

    public void PrintScores()
    {
        tx_string = null;
        tx_scores = GameObject.FindGameObjectWithTag("TopScores").GetComponent<TextMeshProUGUI>();        
            for (int i = 0; i<=4; i++)
            {
                tx_string = tx_string + ($"{i+1}: Player{PlayerPrefs.GetInt($"Winner{i}", 0)} -> {PlayerPrefs.GetInt($"P1Score{i}", 0)} - {PlayerPrefs.GetInt($"P2Score{i}", 0)} \n");                                                                            
            }
                tx_scores.text = tx_string;
    }
}
