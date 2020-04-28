using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Player : MonoBehaviour //Hereda de Monobehaviour, por ende puede modificar el transform directamente
{
    [SerializeField] private KeyCode leftbutton;
    [SerializeField] private KeyCode rightbutton;
    [SerializeField] private float playermargin;
    [SerializeField] private float playerforce;
    [SerializeField] private float playerdeltaforce;

    public ScoreEvent ev_score = new ScoreEvent();
    private readonly HitEvent ev_hit = new HitEvent();
    private float playerinitforce;
    private GameObject go_leftlimit;
    private GameObject go_rightlimit;
    private bool readytoplay;
    private Vector3 balldirection = Vector3.one;
    private enum countertype {inittimer = 0, sessiontimer =1,}

    private float direction = 1;

    void Start()
    {
        go_leftlimit = GameObject.FindGameObjectWithTag("LeftWall");
        go_rightlimit = GameObject.FindGameObjectWithTag("RightWall");
        balldirection.z = 0;
        playerinitforce = playerforce;
        readytoplay = true;
    }

    private void OnEnable() {
        EventController.AddListener<ScoreEvent>(OnScoreEvent);
        EventController.AddListener<GameStartEvent>(OnGameStartEvent);
        EventController.AddListener<StartTimerEvent>(OnStartTimerEvent);
        EventController.AddListener<GameOverEvent>(OnGameOverEvent);
        EventController.AddListener<WaitEvent>(OnWaitEvent);
    }

    private void OnDisable() {
        EventController.RemoveListener<ScoreEvent>(OnScoreEvent);
        EventController.RemoveListener<GameStartEvent>(OnGameStartEvent);
        EventController.RemoveListener<StartTimerEvent>(OnStartTimerEvent);
        EventController.RemoveListener<GameOverEvent>(OnGameOverEvent);
        EventController.RemoveListener<WaitEvent>(OnWaitEvent);
    }

    // Update is called once per frame
    void Update()
    {

        if (this.gameObject.tag == "AIPlayer") //AI Movement
        {
            if (readytoplay)
            {
                transform.position += Vector3.left * direction;
            }

            if ((go_leftlimit.transform.position.x - transform.position.x > -1 * playermargin) || 
                (go_rightlimit.transform.position.x - transform.position.x < playermargin && readytoplay))
            {
                direction = direction * -1;
            }
        }
        else //Player movement
        {
            if (Input.GetKey(leftbutton) && (go_leftlimit.transform.position.x - transform.position.x < -1 * playermargin)
                                        && readytoplay)
            {
                transform.position += Vector3.left;
            }
            if (Input.GetKey(rightbutton) && (go_rightlimit.transform.position.x - transform.position.x > playermargin)
                                         && readytoplay)
            {
                transform.position += Vector3.right;
            }
        }
    }

    private void OnWaitEvent(WaitEvent evt)
    {
        if (evt.waiting)
        {
            readytoplay = false;
        }
        else
        {
            readytoplay = true;
        }
    }

    private void OnGameStartEvent(GameStartEvent start)
    {
        readytoplay = true;
        playerinitforce = playerforce;
    }
   private void OnGameOverEvent(GameOverEvent evt)
    {
        readytoplay = false;
    }

    private void OnScoreEvent(ScoreEvent evt)
    {
        playerforce = playerinitforce;
    }
 
    private void OnStartTimerEvent(StartTimerEvent start)
    {
        if (start.countertype == (int)countertype.inittimer)
        {
        readytoplay = false;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.CompareTag("Ball"))
        {
            foreach (ContactPoint contact in other.contacts)
                {
                    other.rigidbody.AddForce(contact.normal* playerforce *-1);
                }
            playerforce += playerdeltaforce;
            EventController.TriggerEvent(ev_hit);
        }
    }

}
