using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject go_ball;
    [SerializeField] private GameObject go_player1;
    [SerializeField] private GameObject go_player2;
    [SerializeField] private float ballforce;
    [SerializeField] private KeyCode startkey;
    [SerializeField] private KeyCode reloadkey;
    [SerializeField] private int initcounter;
    [SerializeField] private int sessioncounter;
    [SerializeField] private int paramendscreencounter;
    [SerializeField] private int maxscore;
    private MeshRenderer mr_player1;
    private Vector3 balldirection = Vector3.one;
    private Vector3 player1initposition;
    private Vector3 player2initposition;
    private int player1points;
    private int player2points;
    private int currentseconds;
    private int endscreencounter;    
    private int gametime;
    private bool oncountdown;
    private bool playing;
    private bool waiting;
    private bool gameover;
    private bool nextscreen;
    private bool scoresaved;
    private enum countertype {inittimer = 0, sessiontimer =1,}
    private WaitForSeconds waitforseconds = new WaitForSeconds(1);
    private readonly GameStartEvent ev_gamestart = new GameStartEvent();
    private readonly GameOverPongEvent ev_gameover = new GameOverPongEvent();
    private readonly StartTimerEvent ev_starttimer = new StartTimerEvent();
    private readonly UpdateTimerEvent ev_updatettimer = new UpdateTimerEvent();
    private readonly FinishTimerEvent ev_finishtimer = new FinishTimerEvent();
    private readonly WaitEvent ev_waitevent = new WaitEvent();

    private bool waitingcpu;

    public AudioSource audiosource;
    public AudioClip so_win;
    public AudioClip so_score;
    public AudioClip so_hit;

    public struct highScore {
        public int playerwin;
        public int p1score;
        public int p2score;
    }

    private highScore auxscore;

    private Dictionary<int,highScore> highScorelist = new Dictionary<int,highScore>();

    void Start()
    {
        ev_gamestart.startKey = startkey;
        ev_waitevent.startKey = startkey;
        ev_gameover.reloadkey = reloadkey;
        player1initposition = go_player1.transform.position;
        player2initposition = go_player2.transform.position;
        StartProcedure();
    }
    private void OnEnable() {
        EventController.AddListener<ScoreEvent>(OnScoreEvent); //Se agrega como listener
    }

    private void OnDisable() {
        EventController.RemoveListener<ScoreEvent>(OnScoreEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (!oncountdown && !playing)
            {
            playing = true;
            EventController.TriggerEvent(ev_gamestart);
            StartBall();
            StartPlayers();
            }
        if ((waiting && Input.GetKey(startkey) && !gameover)) 
            //|| (waitingcpu && !gameover))
            {
            waiting = false;
            waitingcpu = false;
            ev_waitevent.waiting = false;
            EventController.TriggerEvent(ev_waitevent);
            StartBall();
            }
        if (gametime == 0)
        {
            if (player1points > player2points)
            {
                ev_gameover.playerId = 1;
                ev_gameover.p1score = player1points;
                ev_gameover.p2score = player2points;
            }
            if (player2points > player1points)
            {
                ev_gameover.playerId = 2;
                ev_gameover.p1score = player1points;
                ev_gameover.p2score = player2points;                
            }
            if (player2points == player1points)
            {
                ev_gameover.playerId = 0;
                ev_gameover.p1score = player1points;
                ev_gameover.p2score = player2points;
            }            
            EventController.TriggerEvent(ev_gameover);
            gameover = true;
        }                    
        if (gameover && !scoresaved)
        {
            EndProcedure();
        }
        if (gameover && Input.GetKey(reloadkey))
        {            
            gameover = false;
            StartProcedure();
        }

        //if (nextscreen)
        //{
        //    StopAllCoroutines();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }
    void StartBall()
    {
        balldirection = new Vector3 (Random.Range(0, 2) == 0 ? Random.Range(-0.5f, -0.2f) : Random.Range(0.2f, 0.5f) ,  //x
                                     Random.Range(0, 2) == 0 ? -1 : 1, //y
                                     0); //z
        balldirection.Normalize();
        if (!go_ball.activeSelf)
        {
            go_ball.SetActive(true);                
        }
        go_ball.GetComponent<Rigidbody>().AddForce(balldirection * ballforce);            
    }

    void StartPlayers()
        {
            go_player1.transform.position = player1initposition;
            go_player2.transform.position = player2initposition;
            go_player1.GetComponent<MeshRenderer>().enabled = true;
            go_player2.GetComponent<MeshRenderer>().enabled = true;
        }
    void StopPlayers()
        {
            go_player1.GetComponent<MeshRenderer>().enabled = false;
            go_player2.GetComponent<MeshRenderer>().enabled = false;
        }

    void StartProcedure()
    {
        nextscreen = false;
        oncountdown = true;
        playing = false;
        waiting = false;
        waitingcpu = false;
        gameover = false;
        scoresaved = false;
        go_ball.SetActive(false);
        player1points = 0;
        player2points = 0;
        gametime = sessioncounter;
        go_ball.transform.position = Vector3.zero;        
        go_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        go_ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        StartPlayers();
        GetScores();
        StartCoroutine(StartCountdown());
    }

    void EndProcedure()
    {
        go_ball.SetActive(false);
        StopPlayers();
        UpdatedScores.p1score = player1points;
        UpdatedScores.p2score = player2points;
        UpdatedScores.winplayerID = ev_gameover.playerId;
        SaveScores();
        StartCoroutine(EndScreenCountdown());
    }
    IEnumerator StartCountdown()
    {
        currentseconds = initcounter;
        ev_starttimer.totalseconds = initcounter;
        ev_starttimer.countertype = (int)countertype.inittimer;
        ev_updatettimer.countertype = (int)countertype.inittimer;
        ev_finishtimer.countertype = (int)countertype.inittimer;
        EventController.TriggerEvent(ev_starttimer);

        while (currentseconds >= 0)
        {
          yield return waitforseconds;
          currentseconds--;
          ev_updatettimer.currentseconds = currentseconds;
          EventController.TriggerEvent(ev_updatettimer);
        }
        EventController.TriggerEvent(ev_finishtimer);
        oncountdown = false;
        StartCoroutine(SessionCountdown());
    }

    IEnumerator SessionCountdown()
    {
        currentseconds = sessioncounter;
        ev_starttimer.totalseconds = sessioncounter;
        ev_starttimer.countertype = (int)countertype.sessiontimer;
        ev_updatettimer.countertype = (int)countertype.sessiontimer;
        ev_finishtimer.countertype = (int)countertype.sessiontimer;
        EventController.TriggerEvent(ev_starttimer);
        while (currentseconds > 0)
        {
                yield return waitforseconds;
                if (!waiting)
                {
                    currentseconds--;
                    ev_updatettimer.currentseconds = currentseconds;
                    EventController.TriggerEvent(ev_updatettimer);
                    gametime--;            
                }
        }
        EventController.TriggerEvent(ev_finishtimer);
    }

    IEnumerator EndScreenCountdown()
    {
        endscreencounter = paramendscreencounter;

        while (endscreencounter > 0)
        {
          yield return waitforseconds;
          endscreencounter--;
        }

        nextscreen = true;
    }

    private void OnScoreEvent(ScoreEvent score)
    {

        audiosource.clip = so_score;
        audiosource.Play();

        if (score.playerId == 1)
        {
            player1points = player1points + score.scoreData;
            go_ball.transform.position = new Vector3 (score.playerPosition.x, 
                                                   score.playerPosition.y - (go_player1.transform.localScale.y +0.1f),
                                                   score.playerPosition.z);
            go_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            go_ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            waiting = true;
            //waitingcpu = true;

            //StartCoroutine()

            ev_waitevent.waiting = true;
            EventController.TriggerEvent(ev_waitevent);
        }
        if (score.playerId == 2) 
        {
            player2points = player2points + score.scoreData;
            go_ball.transform.position = new Vector3 (score.playerPosition.x, 
                                                   score.playerPosition.y + (go_player2.transform.localScale.y +0.1f),
                                                   score.playerPosition.z);            
            go_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            go_ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 
            waiting = true;
            ev_waitevent.waiting = true;
            EventController.TriggerEvent(ev_waitevent);
        }
        if (player1points == maxscore)
        {
            ev_gameover.playerId = 1;
            ev_gameover.p1score = player1points;
            ev_gameover.p2score = player2points;
            EventController.TriggerEvent(ev_gameover);
            gameover = true;

            audiosource.clip = so_win;
            audiosource.Play();
        }
        if (player2points == maxscore)
        {
            ev_gameover.playerId = 2;
            ev_gameover.p1score = player1points;
            ev_gameover.p2score = player2points;            
            EventController.TriggerEvent(ev_gameover);
            gameover = true;

            audiosource.clip = so_win;
            audiosource.Play();
        }
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    } 

    public void GetScores(){
        highScorelist.Clear();
        for (int i = 0; i<=4; i++)
        {
            auxscore.playerwin = PlayerPrefs.GetInt($"Winner{i}", 0);
            auxscore.p1score = PlayerPrefs.GetInt($"P1Score{i}", 0);
            auxscore.p2score = PlayerPrefs.GetInt($"P2Score{i}", 0);
            highScorelist.Add(i,auxscore);
        }
    }

    public void SaveScores(){
        int ih = 0;
        int ig = 0;
        bool flag = false;

        for (ih = 0; ih < highScorelist.Count; ih++)
        {
        Debug.Log($"{ih}: Winner: Player {highScorelist[ih].playerwin} {highScorelist[ih].p1score} - {highScorelist[ih].p2score}");
        }


        for (ih = 0; ih < highScorelist.Count; ih++)
        { 
            auxscore = highScorelist[ih];
            //auxscore2 = highScorelist[ih];
            if ((Mathf.Abs(player1points - player2points) > Mathf.Abs(auxscore.p1score - auxscore.p2score)) && !flag)
            {

                //for (ig=highScorelist.Count-1-ih; ig>ih; ig--)
                for (ig=highScorelist.Count-1; ig>=ih; ig--)
                    {
                        auxscore = highScorelist[ig];
                        highScorelist.Remove(ig+1);
                        highScorelist.Add(ig+1,auxscore);
                    }

                auxscore.playerwin = ev_gameover.playerId;
                auxscore.p1score = player1points;
                auxscore.p2score = player2points;
                highScorelist.Remove(ih);                
                highScorelist.Add(ih,auxscore); 

                flag = true;
            }

        }

        for (ih = 0; ih < highScorelist.Count; ih++)
        {
            PlayerPrefs.SetInt($"Winner{ih}", highScorelist[ih].playerwin);
            PlayerPrefs.SetInt($"P1Score{ih}", highScorelist[ih].p1score);
            PlayerPrefs.SetInt($"P2Score{ih}", highScorelist[ih].p2score);  

            PlayerPrefs.Save();
                      
            Debug.Log($"{ih}: Winner: Player {highScorelist[ih].playerwin} {highScorelist[ih].p1score} - {highScorelist[ih].p2score}");
        }
        scoresaved = true;
    }
    
}
