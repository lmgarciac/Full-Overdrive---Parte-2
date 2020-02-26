using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Music_Controller : MonoBehaviour
{
    private AudioSource musicPlayer;

    void Start()
    {

    }

    void Update()
    {
        
    }
    private void OnEnable() {
        // EventController.AddListener<GameStartEvent>(OnGameStartEvent);
        // EventController.AddListener<GameOverEvent>(GameOverEvent); 
        EventController.AddListener<StartTimerEvent>(StartTimerEvent); 
        // EventController.AddListener<UpdateTimerEvent>(UpdateTimerEvent); 
        // EventController.AddListener<FinishTimerEvent>(FinishTimerEvent); 
        // EventController.AddListener<CounterStatusEvent>(CounterStatusEvent); 
        EventController.AddListener<GameOverEvent>(GameOverEvent); 

    }
    private void OnDisable() {
        // EventController.RemoveListener<GameStartEvent>(OnGameStartEvent); 
        // EventController.RemoveListener<GameOverEvent>(GameOverEvent); 
        EventController.RemoveListener<StartTimerEvent>(StartTimerEvent); 
        // EventController.RemoveListener<UpdateTimerEvent>(UpdateTimerEvent); 
        // EventController.RemoveListener<FinishTimerEvent>(FinishTimerEvent); 
        // EventController.RemoveListener<CounterStatusEvent>(CounterStatusEvent); 
        EventController.RemoveListener<GameOverEvent>(GameOverEvent); 

    }    

    private void StartTimerEvent(StartTimerEvent timer)
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.Play();        
    }    
    private void GameOverEvent(GameOverEvent gameover)
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.Stop();        
    }        
}
