using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private GameObject go_player;
    private Animator an_player;

    [SerializeField] private GameObject go_enemy;
    private Animator an_enemy;


    [SerializeField] private GameObject go_UIAttack;
    private Animator an_UIAttack;

    [SerializeField] private GameObject go_UIDefend;
    private Animator an_UIDefend;

    [SerializeField] private GameObject go_UISpecial;
    private Animator an_UISpecial;

    //[SerializeField] private GameObject go_UIItem;
    //private Animator an_UIItem;

    [SerializeField] private GameObject go_UIHeal;
    private Animator an_UIHeal;

    [SerializeField] private GameObject go_UIBuff;
    private Animator an_UIBuff;

    [SerializeField] private GameObject go_UIClock;
    private Animator an_UIClock;

    [SerializeField] private GameObject go_Crowd;
    private Animator an_Crowd;

    [SerializeField] private GameObject go_Camera;
    private Animator an_Camera;

    [SerializeField] private GameObject go_cintaPlayer;
    private Animator an_cintaPlayer;

    [SerializeField] private GameObject go_cintaEnemy;
    private Animator an_cintaEnemy;

    private bool cintaActivada = false;

    public enum animation
    {
        none = 0,
        idle = 1,
        play = 2,
        special = 3,
        win = 4,
        lose = 5,
        crowdin = 6,
        crowdout = 7,
    }

    private int currentaction;

    public enum action
    {
        attack = 1,
        defend = 2,
        special = 3,
        item = 4,
        buff = 5,
        heal = 6,
        back = 7,
        none = 0,
    }

    private void OnEnable()
    {
        EventController.AddListener<AnimEvent>(AnimEvent);
        EventController.AddListener<ActionEvent>(ActionEvent);
        EventController.AddListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.AddListener<QteMissEvent>(QteMissEvent);
        EventController.AddListener<LoadEnemyEvent>(LoadEnemyEvent);
    }
    private void OnDisable()
    {
        EventController.RemoveListener<AnimEvent>(AnimEvent);
        EventController.RemoveListener<ActionEvent>(ActionEvent);
        EventController.RemoveListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.RemoveListener<QteMissEvent>(QteMissEvent);
        EventController.RemoveListener<LoadEnemyEvent>(LoadEnemyEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        an_player = go_player.GetComponent<Animator>();

        an_UIAttack = go_UIAttack.GetComponent<Animator>();
        an_UIDefend = go_UIDefend.GetComponent<Animator>();
        an_UISpecial = go_UISpecial.GetComponent<Animator>();
        //an_UIItem = go_UIItem.GetComponent<Animator>();
        an_UIHeal = go_UIHeal.GetComponent<Animator>();
        an_UIBuff = go_UIBuff.GetComponent<Animator>();

        an_UIClock = go_UIClock.GetComponent<Animator>();
        an_Crowd = go_Crowd.GetComponent<Animator>();
        an_Camera = go_Camera.GetComponent<Animator>();
        an_cintaPlayer = go_cintaPlayer.GetComponent<Animator>();
        an_cintaEnemy = go_cintaEnemy.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        an_Camera.SetBool("Shake", false);
        an_enemy.SetBool("Damage", false);
        an_player.SetBool("Damage", false);

    }

    private void LoadEnemyEvent(LoadEnemyEvent currentEnemy)
    {
        //Debug.Log("Enemigos: " + GameObject.FindGameObjectsWithTag("EnemyAnim").Length);
        go_enemy = GameObject.FindGameObjectWithTag("EnemyAnim");
        an_enemy = go_enemy.GetComponent<Animator>();
        an_enemy.SetBool("Lose", false);
        //if (an_enemy != null)
        //{
        //    Debug.Log("Animator Cargado");
        //}
    }


    private void AnimEvent(AnimEvent anim)
    {

        if (anim.playerturn) //Player
        {
            if (anim.animation == (int)animation.play)
            {
                if (an_player.GetBool("Attack") != true)
                {
                    an_player.SetBool("Attack", true);
                    an_player.SetBool("Idle", false);
                    an_player.SetBool("Special", false);
                    an_player.SetBool("Win", false);
                    an_player.SetBool("Lose", false);                  

                    if (anim.animstate && currentaction == (int)action.attack) //significa que debe shakear camara
                    {
                        an_Camera.SetBool("Shake",true);
                        an_enemy.SetBool("Damage", true);
                    }
                    CloseAttackUI();
                }
            }
            else if (anim.animation == (int)animation.special)
            {
                an_player.SetBool("Idle", false);
                an_player.SetBool("Attack", false);
                an_player.SetBool("Special", true);
                an_player.SetBool("Win", false);
                an_player.SetBool("Lose", false);

                if (anim.animstate && currentaction == (int)action.special) //significa que debe shakear camara
                {
                    an_Camera.SetBool("Shake", true);
                    an_enemy.SetBool("Damage", true);
                }
                CloseAttackUI();
            }
            else if (anim.animation == (int)animation.none) //Miss
            {
                an_player.SetBool("Idle", false);
                an_player.SetBool("Attack", false);
                an_player.SetBool("Special", false);
                an_player.SetBool("Win", false);
                an_player.SetBool("Lose", true);

                an_enemy.SetBool("Damage", false);

                CloseAttackUI();
            }
            else //Idle
            {
                //Debug.Log("InicializarUI");

                an_player.SetBool("Idle", true);
                an_player.SetBool("Attack", false);
                an_player.SetBool("Special", false);
                an_player.SetBool("Win", false);
                an_player.SetBool("Lose", false);

                an_enemy.SetBool("Lose", false);
                an_enemy.SetBool("Damage", false);

                if (!anim.dontshowUI)
                {
                    InitializeAttackUI();
                }
            }
        }
        else //Enemy
        {
            if (anim.animation == (int)animation.play)
            {
                if (an_enemy.GetBool("Attack") != true)
                {
                    an_enemy.SetBool("Attack", true);
                    an_enemy.SetBool("Idle", false);
                    an_enemy.SetBool("Special", false);
                    an_enemy.SetBool("Win", false);
                    an_enemy.SetBool("Lose", false);

                    if (anim.animstate && currentaction == (int)action.attack) //significa que debe shakear camara
                    {
                        an_player.SetBool("Damage", true);
                        an_Camera.SetBool("Shake", true);
                    }

                    an_Camera.SetBool("ZoomEnemy", false);

                }
            }
            else if (anim.animation == (int)animation.special)
            {
                an_enemy.SetBool("Idle", false);
                an_enemy.SetBool("Attack", false);
                an_enemy.SetBool("Special", true);
                an_enemy.SetBool("Win", false);
                an_enemy.SetBool("Lose", false);

                an_player.SetBool("Damage", true);

                an_Camera.SetBool("ZoomEnemy", false);

            }
            else //Idle
            {
                an_enemy.SetBool("Idle", true);
                an_enemy.SetBool("Attack", false);
                an_enemy.SetBool("Special", false);
                an_enemy.SetBool("Win", false);

                an_enemy.SetBool("Lose", false);

                an_player.SetBool("Damage", false);


                if (!anim.dontshowUI)
                {
                    an_Camera.SetBool("ZoomEnemy", true);
                }
            }
        }

        // WIN / LOSE ANIMATIONS

        if (anim.animation == (int)animation.win)
        {
            an_player.SetBool("Idle", false);
            an_player.SetBool("Attack", false);
            an_player.SetBool("Special", false);
            an_player.SetBool("Lose", false);
            an_player.SetBool("Win", true);
            an_player.SetBool("Damage", false);


            an_enemy.SetBool("Idle", false);
            an_enemy.SetBool("Attack", false);
            an_enemy.SetBool("Special", false);
            an_enemy.SetBool("Win", false);

            an_enemy.SetBool("Damage", true);
            an_enemy.SetBool("Lose", true);

            if (cintaActivada == false)
            {
                an_cintaEnemy.SetBool("CintaIn", true);
                cintaActivada = true;
                StartCoroutine(CintaOut());
            }


            CloseAttackUI();
        }
        else if (anim.animation == (int)animation.lose)
        {
            an_player.SetBool("Idle", false);
            an_player.SetBool("Attack", false);
            an_player.SetBool("Special", false);
            an_player.SetBool("Win", false);

            an_player.SetBool("Damage", true);
            an_player.SetBool("Lose", true);


            an_enemy.SetBool("Idle", false);
            an_enemy.SetBool("Attack", false);
            an_enemy.SetBool("Special", false);

            an_enemy.SetBool("Lose", false);
            an_enemy.SetBool("Win", true);
            an_enemy.SetBool("Damage", false);
            
            if (cintaActivada == false)
            {
                an_cintaPlayer.SetBool("CintaIn", true);
                cintaActivada = true;
                StartCoroutine(CintaOut());
            }


            CloseAttackUI();
        }

        //Camshake

        if (anim.camshake)
        {
            //cameraanim.runtimeAnimatorController = Resources.Load("Camera_Shake") as RuntimeAnimatorController;
        }
        else
        {
            //cameraanim.runtimeAnimatorController = null;
        }

        if (anim.animation == (int)animation.crowdin)
        {
            an_Crowd.SetBool("MoveOut", false);
        }
        if (anim.animation == (int)animation.crowdout)
        {
            an_Crowd.SetBool("MoveOut", true);
        }

    }

    IEnumerator CintaOut()
    {
        yield return new WaitForEndOfFrame();

        an_cintaEnemy.SetBool("CintaIn", false);
        an_cintaPlayer.SetBool("CintaIn", false);

        yield return new WaitForSecondsRealtime(8f);
        cintaActivada = false;
    }

    private void InitializeAttackUI()
    {
        go_UIAttack.SetActive(true);
        go_UIDefend.SetActive(true);
        go_UISpecial.SetActive(true);
        go_UIHeal.SetActive(true);
        go_UIBuff.SetActive(true);
        //go_UIItem.SetActive(true);
        go_UIClock.SetActive(true);


        an_UIAttack.SetBool("SlideIn", true);
        an_UIDefend.SetBool("SlideIn", true);
        an_UISpecial.SetBool("SlideIn", true);
        an_UIHeal.SetBool("SlideIn", true);
        an_UIBuff.SetBool("SlideIn", true);
        //an_UIItem.SetBool("SlideIn", true);
        an_UIClock.SetBool("SlideIn", true);

        an_UIAttack.SetBool("SlideOut", false);
        an_UIDefend.SetBool("SlideOut", false);
        an_UISpecial.SetBool("SlideOut", false);
        //an_UIItem.SetBool("SlideOut", false);
        an_UIHeal.SetBool("SlideOut", false);
        an_UIBuff.SetBool("SlideOut", false);
        an_UIClock.SetBool("SlideOut", false);

        an_Camera.SetBool("ZoomPlayer",true);


    }

    private void CloseAttackUI()
    {
        an_UIAttack.SetBool("SlideOut", true);
        an_UIDefend.SetBool("SlideOut", true);
        an_UISpecial.SetBool("SlideOut", true);
        //an_UIItem.SetBool("SlideOut", true);
        an_UIHeal.SetBool("SlideOut", true);
        an_UIBuff.SetBool("SlideOut", true);
        an_UIClock.SetBool("SlideOut", true);

        an_UIAttack.SetBool("SlideIn", false);
        an_UIDefend.SetBool("SlideIn", false);
        an_UISpecial.SetBool("SlideIn", false);
        an_UIHeal.SetBool("SlideIn", false);
        an_UIBuff.SetBool("SlideIn", false);
        //an_UIItem.SetBool("SlideIn", false);
        an_UIClock.SetBool("SlideIn", false);

        an_Camera.SetBool("ZoomPlayer", false);

        //go_UIAttack.SetActive(false);
        //go_UIDefend.SetActive(false);
        //go_UISpecial.SetActive(false);
        //go_UIHeal.SetActive(false);
        //go_UIBuff.SetActive(false);
        ////go_UIItem.SetActive(true);
        //go_UIClock.SetActive(false);

        //an_uiattack.setbool("slidein", false);
        //an_uidefend.setbool("slidein", false);
        //an_uispecial.setbool("slidein", false);
        //an_uiheal.setbool("slidein", false);
        //an_uibuff.setbool("slidein", false);
        ////an_uiitem.setbool("slidein", true);
        //an_uiclock.setbool("slidein", false);

    }

    private void ActionEvent(ActionEvent ev_action)
    {
        currentaction = ev_action.action;
    }

    private void EnableTurnEvent(EnableTurnEvent turn)
    {
        currentaction = (int)action.none;
    }

    private void QteMissEvent(QteMissEvent qtemiss)
    {
        if (!qtemiss.enableinput)
        {
            an_Camera.SetBool("Shake", true);
        }
    }
}
