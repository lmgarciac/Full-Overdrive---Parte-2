using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Audio_Controller : MonoBehaviour
{

    private AudioSource audioplayer;

    void Start()
    {
        audioplayer = this.GetComponent<AudioSource>();
    }

    public void PlaySelectSound()
    {
        audioplayer.clip = (AudioClip)Resources.Load<AudioClip>($"Sound/so_menu_select");
        audioplayer.Play();
    }

    void Update()
    {
        
    }
}
