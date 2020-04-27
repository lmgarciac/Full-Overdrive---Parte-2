using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Animator animFader;
    private bool fadesound = false;
    public AudioSource musica;
    public float soundFadespeed = 0.3f;
    private bool nodata;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject confirmScreen;


    void Start()
    {
        if (SessionData.LoadData())
        {
            nodata = false;
            continueButton.SetActive(true);
        }
        else
        {
            nodata = true;
            continueButton.SetActive(false);
        }

        Debug.Log("NoData" + nodata);

        PlayerOptions.Volume = AudioListener.volume;
        PlayerOptions.Difficulty = 1;
    }

    void Update()
    {
        if (fadesound == true)
        {
            musica.volume -= Time.deltaTime * soundFadespeed;
        }
    }

    private void Fadeout()
    {
        animFader.SetBool("Leave", true);
        fadesound = true;
    }

    public void PlayGame() // New Game
    {
        if(!nodata)
        {
            //Indicar que se borraran datos
            mainMenu.SetActive(false);
            confirmScreen.SetActive(true);
        }
        else
        {
            PlayerOptions.NewGame = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Fadeout();
            Player_Status.Buffs = 2;
            Player_Status.Heals = 2;
            Player_Status.Money = 100;
            Player_Status.CurrentArea = 1;
            Player_Status.Collectables = 0;
            Player_Status.Picks = 0;
        }
    }

    public void Confirm()
    {
        SessionData.EraseData();

        PlayerOptions.NewGame = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Fadeout();
        Player_Status.Buffs = 2;
        Player_Status.Heals = 2;
        Player_Status.Money = 100;
        Player_Status.CurrentArea = 1;
        Player_Status.Collectables = 0;
        Player_Status.Picks = 0;

        Debug.Log("Player Collectables: " + Player_Status.Collectables);

    }

    public void ContinueGame()
    {
        PlayerOptions.NewGame = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Fadeout();

    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }


}
