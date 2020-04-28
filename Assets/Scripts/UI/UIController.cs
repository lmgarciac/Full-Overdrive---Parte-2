using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tx_scoreP1;
    [SerializeField] private TextMeshProUGUI tx_scoreP2;
    [SerializeField] private TextMeshProUGUI tx_congrats;
    [SerializeField] private TextMeshProUGUI tx_inittimer;
    [SerializeField] private TextMeshProUGUI tx_sessioncounter;
    [SerializeField] private TextMeshProUGUI tx_sessioncounterlabel;
    [SerializeField] private TextMeshProUGUI tx_keyinformation;

    private string[] congratlist;
    private int congratlenght;

    private int player1points;
    private int player2points;
    private enum countertype {inittimer = 0, sessiontimer =1,}

    void Start()
    {
        congratlist = new string[]{ "Nice!", 
                                    "Good!", 
                                    "Well Done!", 
                                    "Amazing!", 
                                    "Cool!",
                                    "Keep it up!" };
        congratlenght = congratlist.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable() {
        EventController.AddListener<ScoreEvent>(OnScoreEvent); 
        EventController.AddListener<HitEvent>(OnHitEvent); 
        EventController.AddListener<GameStartEvent>(OnGameStartEvent);
        EventController.AddListener<GameOverPongEvent>(GameOverPongEvent); 
        EventController.AddListener<StartTimerEvent>(StartTimerEvent); 
        EventController.AddListener<UpdateTimerEvent>(UpdateTimerEvent); 
        EventController.AddListener<FinishTimerEvent>(FinishTimerEvent); 
        EventController.AddListener<WaitEvent>(OnWaitEvent);
    }
    private void OnDisable() {
        EventController.RemoveListener<ScoreEvent>(OnScoreEvent);
        EventController.RemoveListener<HitEvent>(OnHitEvent); 
        EventController.RemoveListener<GameStartEvent>(OnGameStartEvent); 
        EventController.RemoveListener<GameOverPongEvent>(GameOverPongEvent); 
        EventController.RemoveListener<StartTimerEvent>(StartTimerEvent); 
        EventController.RemoveListener<UpdateTimerEvent>(UpdateTimerEvent); 
        EventController.RemoveListener<FinishTimerEvent>(FinishTimerEvent); 
        EventController.RemoveListener<WaitEvent>(OnWaitEvent);
    }

   private void OnScoreEvent(ScoreEvent score)
    {
        if (score.playerId == 1)
        {
            player1points = player1points + score.scoreData;
            tx_scoreP1.text = player1points.ToString();
            tx_congrats.text = $"Player {score.playerId} scores!"; //Interpolacion de cadenas
        }
        if (score.playerId == 2)
        {
            player2points = player2points + score.scoreData;            
            tx_scoreP2.text = player2points.ToString();  
            tx_congrats.text = $"Player {score.playerId} scores!"; //Interpolacion de cadenas
        }
    }
    private void OnHitEvent (HitEvent hit)
    {
        tx_congrats.text =  congratlist[Random.Range(0, congratlenght)];   
    }  
    private void OnGameStartEvent (GameStartEvent start)
    {
        player1points = 0;
        tx_scoreP1.text = player1points.ToString();
        player2points = 0;
        tx_scoreP2.text = player2points.ToString();

        tx_congrats.text = null;
        tx_keyinformation.text = null;
        tx_sessioncounterlabel.text = $"Remaining time";
        tx_congrats.text = $"Good luck!";
    }      
    private void OnWaitEvent(WaitEvent wait)
    {
        if (wait.waiting)
        {
            tx_keyinformation.text = $"Press \"{wait.startKey}\" to continue";
        }
        if (!wait.waiting)
        {
            tx_keyinformation.text = null;
        }

    }
    private void StartTimerEvent(StartTimerEvent timer)
    {
        if (timer.countertype == (int)countertype.inittimer)
        {
            tx_inittimer.text = $"{timer.totalseconds}";
            tx_sessioncounterlabel.text = null;
            tx_sessioncounter.text = null;
            tx_keyinformation.text = null;
            tx_scoreP1.text = "0";
            tx_scoreP2.text = "0";
        }
        if (timer.countertype == (int)countertype.sessiontimer)
        {
            tx_sessioncounter.text = $"{timer.totalseconds}";
            tx_inittimer.text = null;
        }
    }

    private void UpdateTimerEvent(UpdateTimerEvent timer)
    {
        if (timer.countertype == (int)countertype.inittimer)
        {
            if (timer.currentseconds == 0)
            {
            tx_inittimer.text = $"Go!";                
            }
            else
            {
            tx_inittimer.text = $"{timer.currentseconds}";                
            }
        }
        if (timer.countertype == (int)countertype.sessiontimer)
        {
            tx_sessioncounter.text = $"{timer.currentseconds}";
        }
    }
    private void FinishTimerEvent(FinishTimerEvent timer)
    {
        if (timer.countertype == (int)countertype.inittimer)
        {
            tx_inittimer.text = $"Go!";
        }
        if (timer.countertype == (int)countertype.sessiontimer)
        {
            tx_sessioncounter.text = $"Finished!";
        }
    }
    private void GameOverPongEvent(GameOverPongEvent evt)
    {
        if (evt.playerId != 0)
        {
        tx_inittimer.text = $"Player {evt.playerId} wins!";
        }
        else
        {
        tx_inittimer.text = $"Tie!";            
        }
        tx_congrats.text = null;         
        tx_keyinformation.text = $"Press \"{evt.reloadkey}\" to replay!";
    }

}
