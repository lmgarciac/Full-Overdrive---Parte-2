using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Sound_Controller_Map : MonoBehaviour
{
    private AudioSource soundPlayer;
    private AudioSource musicPlayer;


    //Sounds // Songs
    [SerializeField] private AudioClip so_backsong;
    [SerializeField] private AudioClip so_collect;

    private void OnEnable()
    {

        EventController.AddListener<CollectEvent>(CollectEvent);

    }
    private void OnDisable()
    {

        EventController.RemoveListener<CollectEvent>(CollectEvent);

    }

    void Start()
    {
        // REACTIVAR ESTA LINEA DE CODIGO EN VERSION FINAL!!!
        //AudioListener.volume = PlayerOptions.Volume;
        //


        AudioSource[] audios = GetComponents<AudioSource>();
        soundPlayer = audios[0];
        musicPlayer = audios[1];
        musicPlayer.loop = true;
        musicPlayer.clip = so_backsong;
        musicPlayer.Play();

        //so_backsong = (AudioClip)Resources.Load<AudioClip>($"Music/so_battle_theme");

        musicPlayer.volume = 0.05f;
        soundPlayer.volume = 0.8f;

    }

    void Update()
    {

    }

    private void CollectEvent(CollectEvent collect)
    {
        soundPlayer.clip = so_collect;
        soundPlayer.Play();
    }

    private void QtePrizeEvent(QtePrizeEvent qteprize)
    {
        if (qteprize.effic < 0.3f)
        {
            soundPlayer.clip = (AudioClip)Resources.Load<AudioClip>($"Sound/so_crowdboo");
        }
        else if (qteprize.effic >= 0.3f && qteprize.effic < 0.7f)
        {
            soundPlayer.clip = (AudioClip)Resources.Load<AudioClip>($"Sound/so_crowdclap");
        }
        else if (qteprize.effic >= 0.7f && qteprize.effic < 1)
        {
            soundPlayer.clip = (AudioClip)Resources.Load<AudioClip>($"Sound/so_crowdcheer{Random.Range(1, 3)}");
        }
        else //100%
        {
            soundPlayer.clip = (AudioClip)Resources.Load<AudioClip>($"Sound/so_crowdhurra{Random.Range(1, 3)}");
        }

        soundPlayer.Play();
    }
}
