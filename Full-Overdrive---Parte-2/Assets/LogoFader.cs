using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoFader : MonoBehaviour
{

    public Animator animFader;
    private bool fadesound = false;
    private AudioSource musica;
    public float soundFadespeed = 0.3f;

    private void Start()
    {
        musica = this.GetComponent<AudioSource>();
    }
    private void Update()
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
}
