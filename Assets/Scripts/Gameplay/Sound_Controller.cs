using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Sound_Controller : MonoBehaviour
{
    private AudioSource soundPlayer;
    private AudioSource lickPlayer;
    private AudioSource musicPlayer;

    [SerializeField] private AudioClip so_ready;
    [SerializeField] private AudioClip so_set;
    [SerializeField] private AudioClip so_go;
    [SerializeField] private AudioClip so_select;
    [SerializeField] private AudioClip so_win;
    [SerializeField] private AudioClip so_qtehit;
    [SerializeField] private AudioClip so_qtemiss;

    private enum counterstatus {ready = 0, set =1, go=2}
    private AudioClip so_lick;
    private AudioClip so_backsong;

    public bool licktriggered;

    void Start()
    {
        AudioListener.volume = PlayerOptions.Volume;
        AudioSource[] audios = GetComponents<AudioSource>();
        lickPlayer = audios[0];
        soundPlayer = audios[1];
        musicPlayer = audios[2];
        licktriggered = false;
        //so_backsong = (AudioClip)Resources.Load<AudioClip>($"so_backsong");
        so_backsong = (AudioClip)Resources.Load<AudioClip>($"Music/so_battle_theme");
        musicPlayer.volume = 0.3f;
        lickPlayer.volume = 0.5f;
        soundPlayer.volume = 0.8f;
        musicPlayer.clip = so_backsong;
        musicPlayer.loop = true;
        musicPlayer.Play();            
    }

    void Update()
    {
        if (!licktriggered && !lickPlayer.isPlaying)
        {
            musicPlayer.volume = 0.3f;
        }
    }
    private void OnEnable() {
        // EventController.AddListener<GameStartEvent>(OnGameStartEvent);
        // EventController.AddListener<GameOverEvent>(GameOverEvent); 
        //EventController.AddListener<StartTimerEvent>(StartTimerEvent); 
        EventController.AddListener<UpdateTimerEvent>(UpdateTimerEvent); 
        EventController.AddListener<FinishTimerEvent>(FinishTimerEvent); 
        EventController.AddListener<CounterStatusEvent>(CounterStatusEvent); 
        EventController.AddListener<SelectActionEvent>(SelectActionEvent); 
        EventController.AddListener<GameOverEvent>(GameOverEvent); 
        EventController.AddListener<QteHitEvent>(QteHitEvent); 
        EventController.AddListener<QtePlayEvent>(QtePlayEvent); 
        EventController.AddListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.AddListener<QtePrizeEvent>(QtePrizeEvent);

    }
    private void OnDisable() {
        // EventController.RemoveListener<GameStartEvent>(OnGameStartEvent); 
        // EventController.RemoveListener<GameOverEvent>(GameOverEvent); 
        //EventController.RemoveListener<StartTimerEvent>(StartTimerEvent); 
        EventController.RemoveListener<UpdateTimerEvent>(UpdateTimerEvent); 
        EventController.RemoveListener<FinishTimerEvent>(FinishTimerEvent); 
        EventController.RemoveListener<CounterStatusEvent>(CounterStatusEvent); 
        EventController.RemoveListener<SelectActionEvent>(SelectActionEvent); 
        EventController.RemoveListener<GameOverEvent>(GameOverEvent); 
        EventController.RemoveListener<QteHitEvent>(QteHitEvent); 
        EventController.RemoveListener<QtePlayEvent>(QtePlayEvent); 
        EventController.RemoveListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.RemoveListener<QtePrizeEvent>(QtePrizeEvent);

    }
    private void CounterStatusEvent(CounterStatusEvent status)
    {
        if (status.counterstatus == (int)counterstatus.ready)
        {
            soundPlayer.clip = so_ready;
            soundPlayer.Play();
        }
        if (status.counterstatus == (int)counterstatus.set)
        {
            soundPlayer.clip = so_set;
            soundPlayer.Play();            
        }
        if (status.counterstatus == (int)counterstatus.go)
        {
            soundPlayer.clip = so_go;
            soundPlayer.Play();            
        }
    }

    private void UpdateTimerEvent(UpdateTimerEvent timer)
    {

    }
    private void FinishTimerEvent(FinishTimerEvent timer)
    {

    }
    private void SelectActionEvent(SelectActionEvent timer)
    {
        soundPlayer.clip = so_select;
        soundPlayer.Play();
    }
    private void GameOverEvent(GameOverEvent gameover)
    {
        soundPlayer.clip = so_win;
        soundPlayer.Play();
        musicPlayer.Stop();
    }    
    private void QteHitEvent(QteHitEvent qtehit)
    {
        if(qtehit.success)
        {
        soundPlayer.clip = so_qtehit;
        soundPlayer.Play();
        }
        else
        {
        soundPlayer.clip = so_qtemiss;
        soundPlayer.Play();            
        }
    }

    private void EnableTurnEvent (EnableTurnEvent enableturn)
    {
        licktriggered = false;
        so_lick = (AudioClip)Resources.Load<AudioClip>($"Sound/so_lick{Random.Range(1, 7)}");            
    }

    private void QtePlayEvent(QtePlayEvent qteplay)
    {
        //if (!licktriggered)
        //{
        //    musicPlayer.volume = 0.15f;
        //    lickPlayer.clip = so_lick;
        //    lickPlayer.Play();
        //    licktriggered = true;
        //}
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
