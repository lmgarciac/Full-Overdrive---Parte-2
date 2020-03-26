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

    void Start()
    {
        PlayerOptions.Volume = AudioListener.volume;
        PlayerOptions.Difficulty = 1;

        //musica = this.GetComponent<AudioSource>();

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

    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Fadeout();
        Player_Status.Buffs = 2;
        Player_Status.Heals = 2;
        Player_Status.Money = 100;
        Player_Status.CurrentArea = 1;

    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }


}
