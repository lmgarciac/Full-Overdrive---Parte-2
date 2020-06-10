using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Battle_Controller : FiniteStateMachine
{
    //[SerializeField] private float initcounter;
    [SerializeField] private float goseconds;
    [SerializeField] private float setseconds;
    [SerializeField] private float readyseconds;
    [SerializeField] private float ti_songstartseconds;
    [SerializeField] private float ti_totalsongtime;
    [SerializeField] private float ti_turntime;
    [SerializeField] private float ti_actiontime;

    [SerializeField] private int playerhp;//max hp
    [SerializeField] private int playersp;//max sp
    [SerializeField] private int playerinitsp;
    [SerializeField] private int playerinithp;
    [SerializeField] private int playerbuff;
    [SerializeField] private int playerheal;

    [SerializeField] private int enemyMaxHP;//max hp
    [SerializeField] private int enemyMaxSP;//max sp
    [SerializeField] private int enemyinitsp;
    [SerializeField] private int enemyinithp;
    [SerializeField] private int enemyheal;
    [SerializeField] private int enemybuff;

    [SerializeField] private int enemyAttack;
    [SerializeField] private int enemyDefense;


    [SerializeField] private int specialcost;


    [SerializeField] private float beatTempo; //en BPM
    [SerializeField] private int timesignature; //compas
    
    
    /*Por ahora solo 1 tipo QTE por Tipo Accion*/
    [SerializeField] private GameObject QTEprefab; //compas
    [SerializeField] private GameObject QTEprefabDef;
    [SerializeField] private GameObject QTEprefabSpecial;
    [SerializeField] private GameObject QTEprefabItem;
    /*----------------------------------------*/
    
    [SerializeField] private Vector3 positionQTE;
    [SerializeField] private Quaternion rotationQTE;

    [SerializeField] private GameObject spotlightplayer;
    [SerializeField] private GameObject spotlightenemy;

    [SerializeField] private string previous_scene;

    [SerializeField] private Animator anim_dialogue;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyObject;

    //[SerializeField] private Areas_Template areas;



    
    public enum characterid
    {
        player = 0,
        enemy = 1,
        none = -1,
    }

    public enum turnstate
    {
        turninfo = 0,
        chooseaction = 1,
        qte = 2,
        anim = 3,
        miss = 4,
    }

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


    private int countdownstate = 1; //1 - Ready
                                    //2 - Set
                                    //3 - Go
                                    //0 - None
    private int _turnstate;
    private float currentseconds;
    private float starttimer; 
    private float controllertimer;
    private float turntimer;       
    private float actiontimer;       
    private float buffplayer;
    private float buffenemy;
    private float defenseplayer;
    private float defenseenemy;

    private int qtecounter;
    private bool qtefinished;
    private int qteleavecounter;
    private int qtenoteamount;
    private float qteeffic;
    private int bonusdamage;
    private float bonusmultiplier;
    private int bonusHP;
    private int bonusSP;

    private int difficulty;

    private static float ti_coroutinetime = 0.1f;

    private bool playerturn;
    private bool enemyturn;
    private bool playing;
    private bool oncountdown;
    private bool selected;

// Countdown triggers
    private bool triggercountdown;
    private bool triggerturn;
    private bool triggerstate;
    private bool gameover;
    private bool enableitems;
    private enum countertype {inittimer = 0, sessiontimer =1,}
    private enum counterstatus {ready = 0, set =1, go=2}
    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(ti_coroutinetime);


    //Timer Settings
    [SerializeField] private float chooseTime = 6.0f;
    [SerializeField] private GameObject go_clock;
    [SerializeField] private GameObject go_clockParent;

    private Image im_clock;
    private WaitForSecondsRealtime chooseTimeTick = new WaitForSecondsRealtime(0.1f);
    private WaitForSecondsRealtime missTime = new WaitForSecondsRealtime(2f);
    private WaitForSecondsRealtime actionTimer = new WaitForSecondsRealtime(2f);
    private WaitForSecondsRealtime turnSwitchTimer = new WaitForSecondsRealtime(2f);
    private WaitForSecondsRealtime QTEShowWait = new WaitForSecondsRealtime(1f);
    private WaitForSecondsRealtime switchEnemyTimer = new WaitForSecondsRealtime(4f);

    

    private GameObject instantiatedQTE;

    private static Object qteBase;

    private readonly StartTimerEvent ev_starttimer = new StartTimerEvent();
    private readonly UpdateTimerEvent ev_updatettimer = new UpdateTimerEvent();
    private readonly FinishTimerEvent ev_finishtimer = new FinishTimerEvent();    
    private readonly CounterStatusEvent ev_counterstatus = new CounterStatusEvent();    
    private readonly BattleStartedEvent ev_battlestarted = new BattleStartedEvent();    
    private readonly EnableTurnEvent ev_enableturn = new EnableTurnEvent();    
    private readonly ActionEvent ev_action = new ActionEvent();    
    private readonly SelectActionEvent ev_selected = new SelectActionEvent();    
    private readonly GameOverEvent ev_gameover = new GameOverEvent();    
    private readonly QtePrizeEvent ev_qteprize = new QtePrizeEvent();
    private readonly AnimEvent ev_anim = new AnimEvent();
    private readonly LoadEnemyEvent ev_enemy = new LoadEnemyEvent();

    private SceneController sceneController;


    public struct Character {
        public int currentHP;
        public int currentSP;
        public int buff;
        public int heal;
    }

    private Character player;
    private Character enemy;

    //QTE movement data
    [SerializeField] private float qtelerpTime = 1f;
    [SerializeField] private float qtestep = 1f;

    private float qteelapsedTime = 0f;
    private bool qteisMoving = false;
    private bool qtecanMove = true;
    public bool qtein;
    public bool qteout;
    private Vector3 qtenextPosition;

    private bool actionButton;

    private bool iswaiting;

    [SerializeField] private Bar_Template currentBar;
    [SerializeField] private Enemy_Template currentEnemy;

    private GameObject currentEnemyObject;
    private int currentEnemyID;
    private bool enemyswitch;
    private bool enemywait = false; //Wait a little before switching enemies
    //Start Event

    void Start()
    {
        StartProcedure();

        im_clock = go_clock.GetComponent<Image>();
        StartCoroutine(StartCountdown());
        sceneController = FindObjectOfType<SceneController>();
      
        if(Player_Status.CurrentBar == 0) //Only for testing
        {
            Player_Status.CurrentBar = 3;
            Player_Status.CurrentArea = 1;
            Player_Status.AttackStat = 10;
            Player_Status.DefenseStat = 10;
            Player_Status.Buffs = 2;
            Player_Status.Heals = 2;
            Player_Status.MaxHPStat = 40;
            Player_Status.MaxSPStat = 60;
        }

        //Load Bar, Load Enemies and Load Player Stats
        if (Player_Status.CurrentBar != 0)
        {
            currentBar = (Bar_Template)Resources.Load<Bar_Template>($"so_Bars/Bar_{Player_Status.CurrentBar}");
            Instantiate((GameObject)Resources.Load<GameObject>($"Prefabs/{currentBar.prefabModelName}"));
            Instantiate((GameObject)Resources.Load<GameObject>($"Prefabs/{currentBar.lightsPrefabModelName}"));

            currentEnemyID = 0;
            if (currentBar.enemies.Length != 0)
            {
                LoadEnemy(currentBar.enemies[currentEnemyID]);
            }

            playerhp = Player_Status.MaxHPStat;
            playersp = Player_Status.MaxSPStat;
        }
    }

    private void LoadEnemy(Enemy_Template loadEnemy)
    {
        if (loadEnemy == null)
        {
            return;
        }

        if (currentEnemyObject != null) //Destroy current enemy
        {
            DestroyImmediate(currentEnemyObject);
            //Debug.Log("Objeto Destruido");
        }

        currentEnemy = loadEnemy;
        currentEnemyObject = Instantiate((GameObject)Resources.Load<GameObject>($"Prefabs/{currentEnemy.prefabName}"));

        if(currentEnemyObject != null && currentEnemy != null)
        {
            //Debug.Log("Objetos Instanciados");
        }

        enemy.currentHP = loadEnemy.enemyinitHP;
        enemy.currentSP = loadEnemy.enemyinitSP;
        enemyMaxHP = loadEnemy.enemyMaxHP;
        enemyMaxSP = loadEnemy.enemyMaxSP;
        enemy.heal = loadEnemy.enemyHeals;
        enemy.buff = loadEnemy.enemyBuffs;
        enemyAttack = loadEnemy.enemyAttack;
        enemyDefense = loadEnemy.enemyDefense;

        enemyObject = currentEnemyObject;

//        enemyObject = GameObject.FindGameObjectWithTag("EnemyAnim");

        ev_enemy.enemyTemplate = currentEnemy;
        ev_enemy.enemyGameObject = currentEnemyObject;

        EventController.TriggerEvent(ev_enemy);
    }

    //Enable / Disable Events

    private void OnEnable() {
        EventController.AddListener<QteHitEvent>(QteHitEvent);
        EventController.AddListener<QtePlayEvent>(QtePlayEvent);
        EventController.AddListener<QteLeaveEvent>(QteLeaveEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);

    }
    private void OnDisable() {
        EventController.RemoveListener<QteHitEvent>(QteHitEvent);
        EventController.RemoveListener<QtePlayEvent>(QtePlayEvent);
        EventController.RemoveListener<QteLeaveEvent>(QteLeaveEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);

    }

    //Update Event

    protected override void Update()
    {
        if (enemywait)
        {
            currentEnemyID++;
            LoadEnemy(currentBar.enemies[currentEnemyID]);
            enemywait = false;
            triggerstate = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && gameover)
        {
            anim_dialogue.SetBool("Open", false);
            sceneController.FadeAndLoadScene(previous_scene);
        }

        // Initial Countdown logic
        if (oncountdown && countdownstate == 1 && triggercountdown)
        {
            ev_counterstatus.counterstatus = (int)counterstatus.ready;
            EventController.TriggerEvent(ev_counterstatus);
            triggercountdown = false;
        }
        else if (oncountdown && countdownstate == 2 && triggercountdown)
        {
            ev_counterstatus.counterstatus = (int)counterstatus.set;
            EventController.TriggerEvent(ev_counterstatus);
            triggercountdown = false;
        }
        else if (oncountdown && countdownstate == 3 && triggercountdown)
        {
            ev_counterstatus.counterstatus = (int)counterstatus.go;
            EventController.TriggerEvent(ev_counterstatus);
            triggercountdown = false;
        }

        // Start game
        if (!oncountdown && !playing)
        {
            playing = true;
            EventController.TriggerEvent(ev_finishtimer);
            EventController.TriggerEvent(ev_enableturn);

            //Trigger anim
            ev_anim.playerturn = playerturn;
            ev_anim.animation = (int)animation.idle;

            ev_anim.dontshowUI = true;

            EventController.TriggerEvent(ev_anim);

            //SwitchState(new IdleState());
            //StartCoroutine(ChooseTime());
            StartCoroutine(StateSwitch());

            //StartCoroutine(TimeController());
        }

        // Turn controller: Cambia de turno y dispara los eventos asociados
        if (playing && playerturn && triggerturn)
        {
            selected = false;
            triggerturn = false;
            triggerstate = false;
            qtecounter = 0;
            qteleavecounter = 0;
            ev_enableturn.characterid = (int)characterid.player;

            ev_enableturn.turnstate = (int)turnstate.turninfo;

            //ev_enableturn.turnstate = (int)turnstate.chooseaction;

            //spotlightplayer.SetActive(true);
            //spotlightenemy.SetActive(false);

            // _turnstate = (int)turnstate.turninfo;
            EventController.TriggerEvent(ev_enableturn);

            StartCoroutine(StateSwitch());

            SwitchState(new IdleState());
            //SwitchState(new ChooseState());

            //Trigger anim

            //Debug.Log("Trigger animacion Turno!");

            ev_anim.playerturn = playerturn;
            ev_anim.animation = (int)animation.idle;

            ev_anim.dontshowUI = true;

            ev_anim.camshake = false;
            EventController.TriggerEvent(ev_anim);

        }
        else if (playing && !playerturn && triggerturn)
        {
            selected = false;
            triggerturn = false;
            triggerstate = false;
            qtecounter = 0;
            ev_enableturn.characterid = (int)characterid.enemy;


            ev_enableturn.turnstate = (int)turnstate.turninfo;
            //ev_enableturn.turnstate = (int)turnstate.chooseaction;



            //spotlightplayer.SetActive(false);
            //spotlightenemy.SetActive(true);

            // _turnstate = (int)turnstate.turninfo;
            EventController.TriggerEvent(ev_enableturn);

            StartCoroutine(StateSwitch());

            SwitchState(new IdleState());
            //SwitchState(new ChooseState());

            //Trigger anim
            ev_anim.playerturn = playerturn;
            ev_anim.animation = (int)animation.idle;

            ev_anim.dontshowUI = true;


            ev_anim.camshake = false;
            EventController.TriggerEvent(ev_anim);
        }

        
        
        // State controller: 
        if (playing && triggerstate && IsStateRunning(new IdleState().GetType()))
        {
            qtein = true;
            qteout = false;
            triggerstate = false;
            ev_enableturn.turnstate = (int)turnstate.chooseaction;
            EventController.TriggerEvent(ev_enableturn); 
            SwitchState(new ChooseState());

            //Trigger anim
            ev_anim.playerturn = playerturn;
            ev_anim.animation = (int)animation.idle;
            ev_anim.dontshowUI = false;

            ev_anim.camshake = false;
            EventController.TriggerEvent(ev_anim);

            StartCoroutine(ChooseTime());

        }
        else if (playing && triggerstate && IsStateRunning(new ChooseState().GetType()))
        {
            triggerstate = false;

            if (ev_action.action == (int)action.item) //Se quedo eligiendo item pero no llegó
            {
                ev_action.action = (int)action.none;
            }

            if (ev_action.action == (int)action.none)
            {
                ev_enableturn.turnstate = (int)turnstate.miss;
                StartCoroutine(MissedTurn());
                qteeffic = 0;
            }
            else
            {
                ev_enableturn.turnstate = (int)turnstate.qte;
            }

            EventController.TriggerEvent(ev_enableturn);


            if (playerturn && ev_action.action != (int)action.none)
            {
                instantiatedQTE = (GameObject)Instantiate(qteBase);
                QTE_Main_Controller qteController = instantiatedQTE.GetComponent<QTE_Main_Controller>();
                qteController.InitializeQte(SelectQTE());

                instantiatedQTE.transform.position = positionQTE;
                instantiatedQTE.transform.Rotate(rotationQTE.x, rotationQTE.y, rotationQTE.z, Space.Self);

                //instantiatedQTE.transform.rotation = Quaternion.identity;
                //instantiatedQTE.transform.rotation = Quaternion.AngleAxis(rotationQTE.x, Vector3.right);

                qtecanMove = false;
                StartCoroutine(ShowQTEWait()); // Waits a second before enabling QTE

                //qtein = true;


                qtefinished = false;

                ev_anim.animation = (int)animation.crowdout;
                EventController.TriggerEvent(ev_anim);
            }

            if (!playerturn)
            {
                StartCoroutine(StateSwitch());
            }

            if (ev_action.action == (int)action.none) // Si se le pasa el turno no quiero que pase por el evento siguiente
            {
                SwitchState(new AnimState());
            }
            else
            {
                SwitchState(new QTEState());
            }
            //SwitchState(new QTEState());

            //Trigger anim
            ev_anim.playerturn = playerturn;
            if (ev_action.action == (int)action.attack 
                || ev_action.action == (int)action.buff 
                || ev_action.action == (int)action.heal)
            {
                ev_anim.animation = (int)animation.play;
            }
            else if (ev_action.action == (int)action.special 
                || ev_action.action == (int)action.defend)
            {
                ev_anim.animation = (int)animation.special;
                if (ev_action.action == (int)action.special)
                {
                    ev_anim.camshake = true;
                }
            }
            else if (ev_action.action == (int)action.none)
            {
                ev_anim.animation = (int)animation.none;
            }
            else 
            {
                ev_anim.dontshowUI = true;

                ev_anim.animation = (int)animation.idle;
            }

            ev_anim.choosestate = true;
            EventController.TriggerEvent(ev_anim);
            ev_anim.choosestate = false;

        }
        //else if (playing && triggerstate && IsStateRunning(new QTEState().GetType()))
        else if ((playing && qtenoteamount!= 0 && (qteleavecounter+qtecounter == qtenoteamount) && IsStateRunning(new QTEState().GetType()))
                || (!playerturn && playing && triggerstate && IsStateRunning(new QTEState().GetType()))
                || (playing && ev_action.action == (int)action.none && triggerstate && IsStateRunning(new QTEState().GetType())))
        {

            //Debug.Log("PASO!!!");
            triggerstate = false;
            selected = false;
            qteout = true;
            qtecanMove = true;
            qteisMoving = true;

            ev_anim.animation = (int)animation.crowdin;

            EventController.TriggerEvent(ev_anim);

            //////Calculate Prizes////// (Comun para ambos)
            CalculatePrizes();

            bonusdamage = ev_qteprize.prizeDamage;
            bonusmultiplier = ev_qteprize.prizeMultiplier;
            bonusHP = ev_qteprize.prizeHP;
            bonusSP = ev_qteprize.prizeSP;


            ////////////////////Player actions/////////////////////////

            //Añado bonus de SP player e indico turno
            if (playerturn)
            {
                ev_qteprize.playerturn = true;
                player.currentSP += bonusSP;
                Mathf.Clamp(player.currentSP, 0, playersp);
            }

            //Añado defensa bonus player
            if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.defend)
            {
                defenseplayer = CalculateDefense();
                //defenseplayer += bonusmultiplier;
                Debug.Log($"Defense: {defenseplayer}");
                defenseenemy = 1;

            }

            if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.attack)
            {
                ev_action.damage = (int)(CalculateDamage() * buffplayer * defenseenemy);

                //enemy.hp = enemy.hp - ev_action.damage;
                enemy.currentHP = Mathf.Clamp(enemy.currentHP - ev_action.damage, 0, enemyMaxHP);
                defenseenemy = 1;

                buffplayer = 1;
            }
            else if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.special)
            {
                ev_action.damage = (int)(CalculateDamage() * buffplayer * defenseenemy);

                //enemy.hp = enemy.hp - ev_action.damage;
                enemy.currentHP = Mathf.Clamp(enemy.currentHP - ev_action.damage, 0, enemyMaxHP);

                player.currentSP -= specialcost;
                buffplayer = 1;
                defenseenemy = 1;

            }
            else if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.heal && playerheal > 0)
            {
                Debug.Log($"Heal: {CalculateHeal()}");
                ev_action.damage = (int)(CalculateHeal() * buffplayer);

                //player.hp = player.hp - ev_action.damage;//Negative damage
                player.currentHP = Mathf.Clamp(player.currentHP - ev_action.damage, 0, playerhp);
                playerheal = playerheal - 1;
                buffplayer = 1;
                defenseenemy = 1;

            }
            else if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.buff && playerbuff > 0)
            {
                
                playerbuff = playerbuff - 1;
                buffplayer = CalculateBuff();
                ev_action.buff = buffplayer;
                defenseenemy = 1;

            }


            //////////////////////////Enemy Actions/////////////////////////

            //Añado bonus SP enemy
            if (!playerturn)
            {
                ev_qteprize.playerturn = false;
                enemy.currentSP += bonusSP;
                Mathf.Clamp(enemy.currentSP, 0, enemyMaxSP);
            }
            //Añado defensa bonus enemy
            if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.defend)
            {
                defenseenemy = CalculateDefense();
                //Debug.Log($"Defense: {defenseenemy}");
                defenseplayer = 1;
            }

            if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.attack)
            {
                ev_action.damage = (int)(CalculateDamage() * buffenemy * defenseplayer);

                player.currentHP = Mathf.Clamp(player.currentHP - ev_action.damage,0,playerhp);

                buffenemy = 1;
                defenseplayer = 1;
            }
            else if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.special)
            {
                ev_action.damage = (int)(CalculateDamage() * buffenemy * defenseplayer);
                player.currentHP = Mathf.Clamp(player.currentHP - ev_action.damage, 0, playerhp);
                enemy.currentSP -= specialcost;
                buffenemy = 1;
                defenseplayer = 1;

            }
            else if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.heal)
            {
                Debug.Log($"Heal: {CalculateHeal()}");

                ev_action.damage = (int)(CalculateHeal() * buffenemy);
                enemy.currentHP = Mathf.Clamp(enemy.currentHP - ev_action.damage, 0, enemyMaxHP);

                Debug.Log($"EnemyHP: {enemy.currentHP}");

                buffenemy = 1;
                defenseplayer = 1;

            }
            else if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.buff)
            {
                buffenemy = CalculateBuff();
                ev_action.buff = buffenemy;
                defenseplayer = 1;
            }



            //Execute Actions
            ev_enableturn.turnstate = (int)turnstate.anim;
            EventController.TriggerEvent(ev_enableturn);             
            EventController.TriggerEvent(ev_action);
            ev_action.characterid = (int)characterid.none;

            EventController.TriggerEvent(ev_qteprize);


            //Trigger anim
            ev_anim.playerturn = playerturn;

            if (ev_action.action == (int)action.attack)
            {
                ev_anim.animation = (int)animation.play;
            }
            if (ev_action.action == (int)action.buff)
            {
                ev_anim.animation = (int)animation.play;
            }
            if (ev_action.action == (int)action.heal)
            {
                ev_anim.animation = (int)animation.play;
            }
            if (ev_action.action == (int)action.special)
            {
                ev_anim.animation = (int)animation.special;
            }
            if (ev_action.action == (int)action.defend)
            {
                ev_anim.animation = (int)animation.special;
            }
            if (ev_action.action == (int)action.none)
            {
                ev_anim.dontshowUI = true;
                ev_anim.animation = (int)animation.idle;
            }


            if (playerturn)
            {
                ev_anim.modelposition = new Vector3(playerObject.transform.position.x,
                                                    playerObject.transform.position.y,
                                                    playerObject.transform.position.z);
            }
            else
            {
                ev_anim.modelposition = new Vector3(enemyObject.transform.position.x,
                                                    enemyObject.transform.position.y,
                                                    enemyObject.transform.position.z);
            }
            ev_anim.animstate = true;
            EventController.TriggerEvent(ev_anim);
            ev_anim.animstate = false;


            ////Check State of Battle/////

            if (enemy.currentHP <= 0)
            {
                enemy.currentHP = 0;

                //Check all enemies beaten//
                if (currentEnemyID >= (currentBar.enemies.Length - 1))
                {
                    //if (areas.Areas[Player_Status.CurrentArea - 1].targetPicks >= Player_Status.Picks) //Si no se la dio, que se la de
                    //{
                    //    Player_Status.Picks++;
                    //}

                    // Check if Bar is finished
                    Bar_Serializable currentBar = Map_Status.FindBar(Player_Status.CurrentBar);

                    if (currentBar == null) //Si no esta agregado, agregarlo
                    {
                        Map_Status.FinishedBars.Add(new Bar_Serializable(Player_Status.CurrentBar, "", false));
                    }

                    currentBar = Map_Status.FindBar(Player_Status.CurrentBar);

                    if (currentBar != null && currentBar.finished == false) //Si aun no finalizó el bar, darle pua y finalizarlo
                    {
                        Player_Status.Picks++;
                        Debug.Log("Pua Obtenida");
                    }

                    currentEnemyID++;
                    gameover = true;
                    ev_gameover.playerwin = true;
                    EventController.TriggerEvent(ev_gameover);
                    ev_anim.animation = (int)animation.win;
                    ev_anim.camshake = false;
                    EventController.TriggerEvent(ev_anim);
                }
                else if (!iswaiting)
                {
                    StartCoroutine(SwitchEnemyWait());

                    enemyswitch = true;
                    ev_anim.playerturn = playerturn;
                    ev_anim.animation = (int)animation.win;
                    ev_anim.camshake = false;
                    EventController.TriggerEvent(ev_anim);
                }
            }
            if (player.currentHP <= 0)
            {
                player.currentHP = 0;
                ev_gameover.playerwin = false;
                EventController.TriggerEvent(ev_gameover);
                gameover = true;

                //Trigger anim Win
                ev_anim.playerturn = playerturn;
                ev_anim.animation = (int)animation.lose;
                ev_anim.camshake = false;
                EventController.TriggerEvent(ev_anim);
            }


            //Reset variables
            qtecounter = 0;
            qtenoteamount = 0;
            ev_action.action = (int)action.none;

            StartCoroutine(ActionTimer());

            SwitchState(new AnimState());


        }
        else if (playing && triggerstate && IsStateRunning(new AnimState().GetType()))
        {
            //Trigger anim
            ev_anim.playerturn = playerturn;
            ev_anim.dontshowUI = true;
            ev_anim.camshake = false;
            if (!enemyswitch) //if switching enemies, don't change turns
            {
                playerturn = !playerturn;
                ev_anim.animation = (int)animation.idle;
            }
            else
            {
                ev_anim.animation = (int)animation.win;
            }
            enemyswitch = false;

            EventController.TriggerEvent(ev_anim);
            triggerstate = false;

            StartCoroutine(TurnSwitch());


            //ev_enableturn.turnstate = (int)turnstate.turninfo;
            //EventController.TriggerEvent(ev_enableturn);             

            SwitchState(new IdleState());
            //SwitchState(new ChooseState());


        }

        /////////////////////Player Input logic////////////////////
        if (playerturn && IsStateRunning(new ChooseState().GetType()))
        {
            ResetPrizes();

            if (Input.GetKeyDown(KeyCode.A) && !selected) //Attack
            {
                ev_action.action = ev_selected.action = (int)action.attack;
                //ev_action.damage = (int)(CalculateDamage() * buffplayer * defenseenemy);
                ev_action.characterid = ev_selected.characterid = (int)characterid.player;
                EventController.TriggerEvent(ev_selected);
                selected = true;
            }
            if (Input.GetKeyDown(KeyCode.S) && player.currentSP >= specialcost && !selected) //Special
            {
                //ev_action.damage = (int)(CalculateDamage() * buffplayer * defenseenemy);
                //defenseenemy = 1;
                ev_action.action = ev_selected.action = (int)action.special;
                ev_action.characterid = ev_selected.characterid = (int)characterid.player;                
                EventController.TriggerEvent(ev_selected);
                selected = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && !selected) 
            {
                ev_action.action = ev_selected.action = (int)action.defend;
                //defenseplayer = CalculateDefense();
                ev_action.characterid = ev_selected.characterid = (int)characterid.player;
                EventController.TriggerEvent(ev_selected);
                selected = true;
            }
            //if (Input.GetKeyDown(KeyCode.W) && !selected) //Item
            //{
            //    enableitems = true;
            //    ev_action.action = ev_selected.action = (int)action.item;
            //    ev_action.characterid = ev_selected.characterid = (int)characterid.player;                
            //    EventController.TriggerEvent(ev_selected);
            //    selected = true;
            //}
            //else if (enableitems && Input.GetKeyDown(KeyCode.W)) //Back
            //{
            //    enableitems = false;
            //    ev_selected.action = (int)action.back;
            //    ev_selected.characterid = (int)characterid.player;
            //    EventController.TriggerEvent(ev_selected);
            //}
            enableitems = true;
            if (enableitems && Input.GetKeyDown(KeyCode.Q) && player.buff > 0) //Buff
            {
                enableitems = false;
                player.buff--;
                //buffplayer = CalculateBuff();
                ev_action.action = ev_selected.action = (int)action.buff;
                ev_action.characterid = ev_selected.characterid = (int)characterid.player;                
                EventController.TriggerEvent(ev_selected);
                selected = true;
            }
            if (enableitems && Input.GetKeyDown(KeyCode.E) && player.heal > 0) //Heal
            {
                enableitems = false;
                player.heal--;
                //ev_action.damage = (int)(CalculateHeal() * buffplayer) + bonusHP;
                ev_action.action = ev_selected.action = (int)action.heal;
                ev_action.characterid = ev_selected.characterid = (int)characterid.player;
                EventController.TriggerEvent(ev_selected);
                selected = true;
            }


        }

        /////////////////////AI Logic///////////////////////
        if (!playerturn && IsStateRunning(new ChooseState().GetType()) && !selected && !iswaiting) //De momento solo ataca
        {
            ResetPrizes();

            //Debug.Log($"Selected: {selected} - Playerbuff: {buffplayer}");
            //Debug.Log($"{enemy.currentHP} / {enemyMaxHP} = {(float)enemy.currentHP / (float)enemyMaxHP}");

            if (((float)enemy.currentHP / (float)enemyMaxHP < 0.4f && Random.Range(0, 1001) > 500) && enemy.heal > 0) //Se cura si tiene menos de 40% de vida
            {
                enemy.heal--;
                ev_action.action = ev_selected.action = (int)action.heal;
                selected = true;
            }
            else if (( buffplayer > 1f && Random.Range(0, 1001) > 500) || Random.Range(0, 1001) > 900) //Se defiende si el otro esta buffeado o si tiene baja SP o un 10% al azar
            {
                ev_action.action = ev_selected.action = (int)action.defend;
                selected = true;
            }
            else if (buffenemy == 1f && enemy.buff > 0 && Random.Range(0, 1001) > 700) //Se buffea 30% al azar
            {
                enemy.buff--;
                ev_action.action = ev_selected.action = (int)action.buff;
                selected = true;
            }
            else if (enemy.currentSP >= specialcost) //Tira el especial
            {
                ev_action.action = ev_selected.action = (int)action.special;
                selected = true;
            }
            //else if (Random.Range(0, 1001) >= 950) //Muy de vez en cuando pierde el turno
            //{
            //    ev_action.action = ev_selected.action = (int)action.none;
            //}
            else//Si no se cumple nada, Ataca normal
            {
                ev_action.action = ev_selected.action = (int)action.attack;
                selected = true;
            }

            //Debug.Log($"AccionEnemigo{ev_action.action}");

            ev_action.characterid = ev_selected.characterid = (int)characterid.enemy;
            EventController.TriggerEvent(ev_selected);

            selected = false; //Lo voy a poner en la corutina al selected
            StartCoroutine(ChoiceWait());

        }


        //QTE Movement Scroll    
        if (instantiatedQTE != null)
        {
            if (qtecanMove && qtein)
            {
                qtenextPosition = instantiatedQTE.transform.position + Vector3.right * qtestep;
                qteisMoving = true;
            }
            if (qtecanMove && qteout)
            {
                qtenextPosition = instantiatedQTE.transform.position + Vector3.left * qtestep;
                qteisMoving = true;
            }
            if (qteisMoving)
            {
                MoveQTE(instantiatedQTE);
            }

            // Destroy QTE
            if (instantiatedQTE != null && !qteisMoving && qteout) //
            {
                instantiatedQTE.transform.position = positionQTE;
                Destroy(instantiatedQTE);
                qtecanMove = true;
                qteout = false;
                qtein = true;
            }
        }
    
        base.Update();

        //Debug.Log(playerturn);
    }

    private void StartProcedure(){


        difficulty = PlayerOptions.Difficulty;
        currentseconds = 0;
        countdownstate = 0;
//      Para saber cuanto vale una negra en segundos, divido el BPM por 60 
//      que me da los BPS y divido 1 por eso.        
        //beatTempo = ( 1 / ( beatTempo / 60f)  ) * 100 * 2;  //Recalculo en decisegundos
        beatTempo = beatTempo / 60f * (1/ti_coroutinetime);  //Recalculo en decisegundos
        ti_actiontime = beatTempo;
        //ti_actiontime = 82;
        ti_turntime = beatTempo * (timesignature-1);
        //ti_turntime = 328;
        //Debug.Log(ti_turntime);
        // _turnstate = (int)turnstate.turninfo;
        ev_enableturn.characterid = (int)characterid.player;


        ev_enableturn.turnstate = (int)turnstate.turninfo;
        //ev_enableturn.turnstate = (int)turnstate.chooseaction;

        ev_action.characterid = (int)characterid.none; 


        playerturn = true;
        enemyturn = false;
        selected = false;
        playing = false;
        oncountdown = true;
        gameover = false;
        enableitems = false;
        player.currentHP = playerinithp;
        player.currentSP = playerinitsp;
        player.heal = playerheal;
        player.buff = playerbuff;
        enemy.currentHP = enemyinithp;
        enemy.currentSP = enemyinitsp;
        enemy.heal = enemyheal;
        enemy.buff = enemybuff;  
        triggercountdown = false;
        triggerturn = false;
        triggerstate = false;
        buffplayer = 1;
        buffenemy = 1;
        defenseplayer = 1;   
        defenseenemy = 1;   
        qtecounter = 0;

        qteBase = Resources.Load("Prefabs/Qte_Base");

        iswaiting = false;
        SwitchState(new IdleState());
        //SwitchState(new ChooseState());

        //Trigger anim
        //ev_anim.playerturn = playerturn;
        //ev_anim.animation = (int)animation.idle;
        //EventController.TriggerEvent(ev_anim);

        ////Trigger anim
        //ev_anim.playerturn = !playerturn;
        //ev_anim.animation = (int)animation.idle;
        //EventController.TriggerEvent(ev_anim);

        //positionQTE = new Vector3(0f,10f,-3f);
        //qteobject = GameObject.FindGameObjectWithTag("QTE");
        //qteobject.SetActive(false);

        ev_battlestarted.playerhp = playerhp;
        ev_battlestarted.playersp = playersp;
        ev_battlestarted.playerinitsp = playerinitsp;
        ev_battlestarted.enemyinitsp = enemyinitsp;
        ev_battlestarted.playerinithp = playerinithp;
        ev_battlestarted.enemyinithp = enemyinithp;
        ev_battlestarted.buffqty = player.buff;
        ev_battlestarted.healqty = player.heal;
        ev_battlestarted.enemyhp = enemyMaxHP;
        ev_battlestarted.enemysp = enemyMaxSP;
        ev_battlestarted.specialcost = specialcost;
        EventController.TriggerEvent(ev_battlestarted);

        ev_starttimer.countertype = (int)countertype.inittimer;
        ev_updatettimer.countertype = (int)countertype.inittimer;
        ev_finishtimer.countertype = (int)countertype.inittimer;

        EventController.TriggerEvent(ev_starttimer);




    }
    IEnumerator StartCountdown()
    {
        while (starttimer <= ti_songstartseconds)
        {
            if (starttimer == readyseconds)
            {
                countdownstate = 1;
                triggercountdown = true;
            }
            else if (starttimer == setseconds)
            {
                countdownstate = 2;
                triggercountdown = true;
            }
            else if (starttimer == goseconds)
            {
                countdownstate = 3;
                triggercountdown = true;
            }
            yield return waitforseconds;
            starttimer++;
        }
        oncountdown = false;
        countdownstate = 0;
    }
    IEnumerator TimeController()
    {
        while (controllertimer <= ti_totalsongtime && !gameover)
        {
            if (turntimer >= ti_turntime) //Esto en teoría ya no sirve... refactorizar
            {
                turntimer = 0;
                //actiontimer = 0;
                //playerturn = !playerturn;
                //triggerturn = true;
                //triggerstate = false;
            }
            if (actiontimer >= ti_actiontime && !triggerturn)
            {
                actiontimer = 0;
                triggerstate = true;
            }
            yield return waitforseconds;
           // Debug.Log($"Actiontimer: {actiontimer} Turntimer:{turntimer}");
            if (!(IsStateRunning(new QTEState().GetType()) && playerturn && ev_action.action != (int)action.none))
            {
                actiontimer++;
                turntimer++;
            }

            //actiontimer++;
            //turntimer++;
            controllertimer++;
        }
    }

    private float CalculateStats()
    {
        if (playerturn)
        {
            Debug.Log("Calculate Player Stat: " + ((Player_Status.AttackStat / 10.0f) * (10.0f / enemyDefense)));
            return ((Player_Status.AttackStat / 10.0f) * (10.0f / enemyDefense));
        }
        else
        {
            Debug.Log("Calculate Enemy Stat: " + ((enemyDefense / 10.0f) * (10.0f / Player_Status.DefenseStat)));
            return ((enemyDefense / 10.0f) * (10.0f / Player_Status.DefenseStat));
        }
    }

    private int CalculateDamage()
    {
        //Debug.Log($"Bonusdamage: {bonusdamage}");
        if (ev_action.action == (int)action.attack)
        {
            return ((int)((Random.Range(5, 8) + bonusdamage) * CalculateStats()));
        }
        else //Special
        {
            return ((int)((Random.Range(15, 21) + bonusdamage) * CalculateStats()));
        }
    }
    private float CalculateBuff()
    {
        return Random.Range(1.3f, 1.5f) + bonusmultiplier;
    }
    private int CalculateHeal()
    {
        return Random.Range(-20, -17) - bonusHP;//Negative damage
    }   
    private float CalculateDefense()
    {
        return Random.Range(0.65f, 0.75f) + bonusmultiplier; //es un porcentual de 1 (daño total)
    }       
    private void QteHitEvent(QteHitEvent qtehit)
    {
        if (qtehit.success)
        {
            qtecounter++;
        }
    }
    private void QteLeaveEvent(QteLeaveEvent qteleave)
    {
        qteleavecounter = qteleave.qtenoteleave;
    }
    private void MoveQTE(GameObject qte)
    {
        //Lerp Movement needs a percentage of movement from point A to point B.
        qteelapsedTime += Time.deltaTime;
        if (qteelapsedTime > qtelerpTime)
        {
            qteelapsedTime = qtelerpTime;
        }

        float qtepercentage = qteelapsedTime / qtelerpTime;
        qte.transform.position = Vector3.Lerp(qte.transform.position, qtenextPosition, qtepercentage);

        qtecanMove = false;

        if (qtepercentage == 1)
        {
            //qtecanMove = true;
            qteisMoving = false;
            qteelapsedTime = 0f;
        }
    }

    private void QtePlayEvent(QtePlayEvent qteplay)
    {
        qtenoteamount = qteplay.noteamount;
        //Debug.Log($"PlayEvent!");
    }
    private void CalculatePrizes()
    {
        int aidice = Random.Range(0,1001); //Dados enemigo

        if (playerturn) //Si es turno del player eficacia en base a QTE
        {
            if (qtenoteamount != 0)
            {
                //Debug.Log($"Counter:{qtecounter} - Amount:{qtenoteamount}");
                qteeffic = (float)qtecounter / (float)qtenoteamount;
            }
        }
        else //Si es turno enemigo, eficacia de AI
        {
            if (aidice>=0 && aidice<600) //Un 60% de las veces le sale bien
            {
                qteeffic = 0.7f;
            }
            else if (aidice>=600 && aidice<900) //Un 30% le sale masomenos
            {
                qteeffic = 0.3f;
            }
            else if (aidice > 900 && aidice <= 950) //Un 5% le sale mal
            {
                qteeffic = 0f;
            }
            else //Un 5% le sale perfecto
            {
                qteeffic = 1f;
            }
        }

        ev_qteprize.effic = qteeffic;

        //Debug.Log($"{ev_action.action} Effi: {qteeffic}");

        ev_qteprize.prizeDamage = 0;
        ev_qteprize.prizeSP = 0;
        ev_qteprize.prizeMultiplier = 0;
        ev_qteprize.prizeHP = 0;

        switch (ev_action.action)
        {
            case (int)action.attack:
                if (qteeffic < 0.3f)
                {
                    ev_qteprize.prizeDamage = -3;
                }
                else if (qteeffic >= 0.3f && qteeffic < 0.7f)
                {
                    ev_qteprize.prizeSP = 5;
                }
                else if (qteeffic >= 0.7f && qteeffic < 1f)
                {
                    ev_qteprize.prizeSP = 7;
                    ev_qteprize.prizeDamage = 2;
                }
                else //100%
                {
                    ev_qteprize.prizeDamage = 5;
                    ev_qteprize.prizeSP = 10;
                }
                break;
            case (int)action.defend:
                if (qteeffic < 0.3f)
                {
                    ev_qteprize.prizeMultiplier = -0.15f;//defense multipliers are negative
                }
                else if (qteeffic >= 0.3f && qteeffic < 0.7f)
                {
                    ev_qteprize.prizeSP = 5;
                    ev_qteprize.prizeMultiplier = -0.25f;
                }
                else if (qteeffic >= 0.7f && qteeffic < 1)
                {
                    ev_qteprize.prizeSP = 10;
                    ev_qteprize.prizeMultiplier = -0.35f;
                }
                else //100%
                {
                    ev_qteprize.prizeMultiplier = -0.5f;
                    ev_qteprize.prizeSP = 25;
                }
                break;
            case (int)action.special:
                if (qteeffic < 0.3f)
                {
                    ev_qteprize.prizeDamage = -7;
                }
                else if (qteeffic >= 0.3f && qteeffic < 0.7f)
                {
                    ev_qteprize.prizeDamage = 3;
                }
                else if (qteeffic >= 0.7f && qteeffic < 1)
                {
                    ev_qteprize.prizeDamage = 5;
                }
                else //100%
                {
                    ev_qteprize.prizeDamage = 10;
                }
                break;
            case (int)action.heal:
                if (qteeffic < 0.3f)
                {
                    ev_qteprize.prizeHP = -10;
                }
                else if (qteeffic >= 0.3f && qteeffic < 0.7f)
                {
                    ev_qteprize.prizeSP = 5;
                    ev_qteprize.prizeHP = 5;
                }
                else if (qteeffic >= 0.7f && qteeffic < 1)
                {
                    ev_qteprize.prizeSP = 10;
                    ev_qteprize.prizeHP = 10;
                }
                else //100%
                {
                    ev_qteprize.prizeSP = 10;
                    ev_qteprize.prizeHP = 20;
                }
                break;
            case (int)action.buff:
                if (qteeffic < 0.3f)
                {
                    ev_qteprize.prizeMultiplier = -0.2f;
                }
                else if (qteeffic >= 0.3f && qteeffic < 0.7f)
                {
                    ev_qteprize.prizeSP = 5;
                }
                else if (qteeffic >= 0.7f && qteeffic < 1)
                {
                    ev_qteprize.prizeSP = 10;
                    ev_qteprize.prizeMultiplier = 0.2f;
                }
                else //100%
                {
                    ev_qteprize.prizeSP = 15;
                    ev_qteprize.prizeMultiplier = 0.5f;
                }
                break;
            default:

                break;
        }
    }
    private void ResetPrizes()
    {
        ev_qteprize.prizeDamage = 0;
        ev_qteprize.prizeHP = 0;
        ev_qteprize.prizeMultiplier = 0;
        ev_qteprize.prizeSP = 0;
        bonusdamage = 0;
        bonusHP = 0;
        bonusSP = 0;
        bonusmultiplier = 0;
    }

    //private GameObject SelectQTE()
    //{
    //    switch (ev_action.action)
    //    {
    //        case (int)action.attack:
    //            return (GameObject)Resources.Load<GameObject>($"Qte_atk_{ Random.Range(1, 4)}");
    //            break;
    //        case (int)action.defend:
    //            return (GameObject)Resources.Load<GameObject>($"Qte_def_{Random.Range(1, 4)}");
    //            break;
    //        case (int)action.special:
    //            return (GameObject)Resources.Load<GameObject>($"Qte_special_{Random.Range(1, 4)}");
    //            break;
    //        case (int)action.heal:
    //            return (GameObject)Resources.Load<GameObject>($"Qte_item_{Random.Range(1, 4)}");
    //            break;
    //        case (int)action.buff:
    //            return (GameObject)Resources.Load<GameObject>($"Qte_item_{Random.Range(1, 4)}");
    //            break;
    //        default:
    //            return (GameObject)Resources.Load<GameObject>($"Qte_atk_{Random.Range(1, 4)}");
    //            break;
    //    }
    //}

    private string SelectQTE()
    {
        switch (ev_action.action)
        {
            case (int)action.attack:
                return ($"QTE_Template_atk_{ Random.Range(1, 4)}");
                break;
            case (int)action.defend:
                return ($"QTE_Template_def_{Random.Range(1, 4)}");
                break;
            case (int)action.special:
                return ($"QTE_Template_spe_{Random.Range(1, 4)}");
                break;
            case (int)action.heal:
                return ($"QTE_Template_ite_{Random.Range(1, 4)}");
                break;
            case (int)action.buff:
                return ($"QTE_Template_ite_{Random.Range(1, 4)}");
                break;
            default:
                return ($"QTE_Template_atk_{Random.Range(1, 4)}");
                break;
        }
    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        playerheal = Player_Status.Heals;
        playerbuff = Player_Status.Buffs;
        //Debug.Log(playerheal);
    }
    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {
        Player_Status.Heals = player.heal;
        Player_Status.Buffs = player.buff;
        //Debug.Log(playerheal);
    }

    IEnumerator ChooseTime()
    {

        //Espera 5 segundos y llena el timer
        im_clock.fillAmount = 0f;
        float fadeSpeed = Mathf.Abs(im_clock.fillAmount - 1f) / chooseTime;

        while (!Mathf.Approximately(im_clock.fillAmount, 1f) && !selected)
        {
            im_clock.fillAmount = Mathf.MoveTowards(im_clock.fillAmount, 1f,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }

        if (!gameover)
        {
            triggerstate = true;
        }
    }

    IEnumerator MissedTurn()
    {

        yield return missTime;

        if (!gameover)
        {
            triggerstate = true;
        }
    }

    IEnumerator ActionTimer()
    {

        yield return actionTimer;

        if (!gameover && !iswaiting)
        {
            triggerstate = true;
        }
    }

    IEnumerator TurnSwitch()
    {

        yield return turnSwitchTimer;

        if (!gameover)
        {
            triggerstate = true;
            triggerturn = true;
        }

    }

    IEnumerator StateSwitch()
    {

        yield return turnSwitchTimer;

        if(!gameover)
        {
            triggerstate = true;
        }
    }

    IEnumerator ChoiceWait()
    {
        iswaiting = true;
        yield return turnSwitchTimer;
        selected = true;
        iswaiting = false;
    }

    IEnumerator SwitchEnemyWait()
    {
        enemywait = false;
        iswaiting = true;
        yield return switchEnemyTimer;
        enemywait = true;
        iswaiting = false;
    }

    IEnumerator ShowQTEWait()
    {
        yield return QTEShowWait;
        qtecanMove = true;
    }
}
