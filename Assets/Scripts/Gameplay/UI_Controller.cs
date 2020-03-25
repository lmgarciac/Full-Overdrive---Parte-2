using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tx_centralinfo;
    [SerializeField] private TextMeshProUGUI tx_prizeinfo;

    [SerializeField] private TextMeshProUGUI tx_playerhp;
    [SerializeField] private TextMeshProUGUI tx_enemyhp;
    [SerializeField] private TextMeshProUGUI tx_playersp;
    [SerializeField] private TextMeshProUGUI tx_enemysp;
    [SerializeField] private TextMeshProUGUI tx_info;
    [SerializeField] private TextMeshPro tx_enemydamage;
    [SerializeField] private TextMeshPro tx_playerdamage;
    [SerializeField] private TextMeshProUGUI tx_buffqty;
    [SerializeField] private TextMeshProUGUI tx_healqty;

    [SerializeField] private GameObject go_actionspanel;
    [SerializeField] private GameObject go_itempanel;

    [SerializeField] private GameObject go_dialogue;
    [SerializeField] private TextMeshProUGUI tx_result;
    [SerializeField] private Animator anim_dialogue;


    private enum countertype { inittimer = 0, sessiontimer = 1, }
    private enum counterstatus { ready = 0, set = 1, go = 2 }

    public enum turnstate
    {
        turninfo = 0,
        chooseaction = 1,
        qte = 2,
        anim = 3,
        miss = 4,

    }
    public struct Character
    {
        public int hp;
        public int sp;
        public int buff;
        public int heal;
    }
    public enum characterid
    {
        player = 0,
        enemy = 1,
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
    private Character player;
    private Character enemy;



    //PARA HP PERSONAJE
    private int max_hp_player;
    private int max_hp_enemy;
    private int max_sp_player = 0;
    private int max_sp_enemy = 0;

    private int specialcost;

    public Image ProgressHP_Player;
    public Image ProgressHP_Enemy;
    public Image ProgressSP_Player;
    public Image ProgressSP_Enemy;

    /*Agregado para la barra de progreso del HP/SP*/

    void Start()
    {

    }
    void Update()
    {
        UpgradeHPBar(); //Con esto actualizo la barra de HP
        UpgradeSPBar();
    }

    private void OnEnable()
    {
        // EventController.AddListener<GameStartEvent>(OnGameStartEvent);
        // EventController.AddListener<GameOverEvent>(GameOverEvent); 
        EventController.AddListener<StartTimerEvent>(StartTimerEvent);
        EventController.AddListener<UpdateTimerEvent>(UpdateTimerEvent);
        EventController.AddListener<FinishTimerEvent>(FinishTimerEvent);
        EventController.AddListener<CounterStatusEvent>(CounterStatusEvent);
        EventController.AddListener<ActionEvent>(ActionEvent);
        EventController.AddListener<BattleStartedEvent>(BattleStartedEvent);
        EventController.AddListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.AddListener<GameOverEvent>(GameOverEvent);
        EventController.AddListener<SelectActionEvent>(SelectActionEvent);
        EventController.AddListener<QteHitEvent>(QteHitEvent);
        EventController.AddListener<QtePrizeEvent>(QtePrizeEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);

    }
    private void OnDisable()
    {
        // EventController.RemoveListener<GameStartEvent>(OnGameStartEvent); 
        // EventController.RemoveListener<GameOverEvent>(GameOverEvent); 
        EventController.RemoveListener<StartTimerEvent>(StartTimerEvent);
        EventController.RemoveListener<UpdateTimerEvent>(UpdateTimerEvent);
        EventController.RemoveListener<FinishTimerEvent>(FinishTimerEvent);
        EventController.RemoveListener<CounterStatusEvent>(CounterStatusEvent);
        EventController.RemoveListener<ActionEvent>(ActionEvent);
        EventController.RemoveListener<BattleStartedEvent>(BattleStartedEvent);
        EventController.RemoveListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.RemoveListener<GameOverEvent>(GameOverEvent);
        EventController.RemoveListener<SelectActionEvent>(SelectActionEvent);
        EventController.RemoveListener<QteHitEvent>(QteHitEvent);
        EventController.RemoveListener<QtePrizeEvent>(QtePrizeEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);

    }
    private void StartTimerEvent(StartTimerEvent timer)
    {
        go_itempanel.SetActive(false);
    }

    private void CounterStatusEvent(CounterStatusEvent status)
    {
        if (status.counterstatus == (int)counterstatus.ready)
        {
            tx_centralinfo.text = $"READY!";
        }
        if (status.counterstatus == (int)counterstatus.set)
        {
            tx_centralinfo.text = $"SET!";
        }
        if (status.counterstatus == (int)counterstatus.go)
        {
            tx_centralinfo.text = $"GO!";
        }
    }
    private void UpdateTimerEvent(UpdateTimerEvent timer)
    {

    }
    private void FinishTimerEvent(FinishTimerEvent timer)
    {
        tx_centralinfo.text = null;
    }

    private void BattleStartedEvent(BattleStartedEvent battlestart)
    {
        player.hp = battlestart.playerinithp;
        player.sp = battlestart.playerinitsp;
        enemy.hp = battlestart.enemyinithp;
        enemy.sp = battlestart.enemyinitsp;
        //player.buff = battlestart.buffqty;
        //player.heal = battlestart.healqty;

        max_hp_player = battlestart.playerhp;
        max_hp_enemy = battlestart.enemyhp;
        max_sp_player = battlestart.playersp;
        max_sp_enemy = battlestart.enemysp;

        specialcost = battlestart.specialcost;

        tx_playerhp.text = $"{player.hp}";
        tx_playersp.text = $"{player.sp}";
        tx_enemyhp.text = $"{enemy.hp}";
        tx_enemysp.text = $"{enemy.sp}";
        //tx_buffqty.text = $"{player.buff}";
        //tx_healqty.text = $"{player.heal}";
    }

    private void ActionEvent(ActionEvent ev_action)
    {
        /////////////////////////// Player Actions////////////////////

        if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.attack)
        {
            enemy.hp -= ev_action.damage;

            if (enemy.hp < 0)
            {
                enemy.hp = 0;
            }
            tx_enemydamage.text = $"-{ev_action.damage}HP";
            tx_enemyhp.text = $"{enemy.hp}";
        }
        if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.special)
        {
            enemy.hp -= ev_action.damage;
            player.sp -= specialcost;
            if (enemy.hp < 0)
            {
                enemy.hp = 0;
            }
            tx_enemydamage.text = $"-{ev_action.damage}HP";
            tx_enemyhp.text = $"{enemy.hp}";
            tx_playersp.text = $"{player.sp}";
        }
        if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.heal)
        {
            //player.hp -= ev_action.damage;
            player.hp = Mathf.Clamp(player.hp - ev_action.damage, 0, max_hp_player);


            tx_playerdamage.text = $"+{-ev_action.damage}HP";
            tx_playerhp.text = $"{player.hp}";
        }

        if (ev_action.characterid == (int)characterid.player && ev_action.action == (int)action.buff)
        {
            tx_playerdamage.text = $"Buffed!\n" + ev_action.buff.ToString("F1");
        }

        ////////////////////////Enemy actions///////////////////

        if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.attack)
        {
            player.hp -= ev_action.damage;
            if (player.hp < 0)
            {
                player.hp = 0;
            }
            tx_playerdamage.text = $"-{ev_action.damage}HP";
            tx_playerhp.text = $"{player.hp}";
        }

        if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.special)
        {
            player.hp -= ev_action.damage;
            enemy.sp -= specialcost;
            if (player.hp < 0)
            {
                player.hp = 0;
            }
            tx_playerdamage.text = $"-{ev_action.damage}HP";
            tx_playerhp.text = $"{player.hp}";
            tx_enemysp.text = $"{enemy.sp}";
        }

        if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.heal)
        {
            //enemy.hp -= ev_action.damage;
            enemy.hp = Mathf.Clamp(enemy.hp - ev_action.damage, 0, max_hp_enemy);

            tx_enemydamage.text = $"+{-ev_action.damage}HP";
            tx_enemyhp.text = $"{enemy.hp}";
        }

        if (ev_action.characterid == (int)characterid.enemy && ev_action.action == (int)action.buff)
        {
            tx_enemydamage.text = $"Buffed!\n" + ev_action.buff.ToString("F1");
        }

    }


    private void EnableTurnEvent(EnableTurnEvent enableturn)
    {
        tx_prizeinfo.text = null;
        tx_enemydamage.text = null;
        tx_playerdamage.text = null;
        if (enableturn.characterid == (int)characterid.player)
        {
            switch (enableturn.turnstate)
            {
                case (int)turnstate.turninfo:
                    tx_info.text = $"Get Ready!";
                    break;
                case (int)turnstate.chooseaction:
                    tx_info.text = $"Choose!";
                    break;
                case (int)turnstate.qte:
                    tx_info.text = $"Play!";
                    go_actionspanel.SetActive(true);
                    go_itempanel.SetActive(false);
                    tx_centralinfo.text = null;
                    break;
                case (int)turnstate.anim:
                    tx_info.text = $"Action!";
                    break;
                case (int)turnstate.miss:
                    tx_info.text = $"Turn missed!";
                    break;
                default:
                    break;
            }
        }
        if (enableturn.characterid == (int)characterid.enemy)
        {
            switch (enableturn.turnstate)
            {
                case (int)turnstate.turninfo:
                    tx_info.text = $"Enemy Turn!";
                    tx_enemydamage.text = null;
                    tx_playerdamage.text = null;
                    break;
                case (int)turnstate.chooseaction:
                    tx_info.text = $"Enemy Choice!";
                    break;
                case (int)turnstate.qte:
                    tx_info.text = $"Enemy Play!";
                    go_actionspanel.SetActive(true);
                    go_itempanel.SetActive(false);
                    tx_centralinfo.text = null;
                    break;
                case (int)turnstate.anim:
                    tx_info.text = $"Enemy Action!";
                    break;
                default:
                    break;
            }
        }

    }

    private void GameOverEvent(GameOverEvent gameover)
    {
        if (gameover.playerwin)
        {
            tx_result.text = $"You Win!";
        }
        else
        {
            tx_result.text = $"You Lose!";
        }

        anim_dialogue.SetBool("Open", true);
        

        //tx_centralinfo.text = $"GameOver!";
        go_actionspanel.SetActive(false);
        go_itempanel.SetActive(false);
        tx_info.text = null;
        tx_prizeinfo.text = null;
        tx_enemydamage.text = null;
        tx_playerdamage.text = null;
    }
    private void SelectActionEvent(SelectActionEvent select)
    {
        if (select.characterid == (int)characterid.player && select.action == (int)action.item)
        {
            go_actionspanel.SetActive(false);
            go_itempanel.SetActive(true);
        }
        if (select.characterid == (int)characterid.player && select.action == (int)action.buff)
        {
            go_actionspanel.SetActive(true);
            go_itempanel.SetActive(false);

            if (player.buff > 0)
            {
                player.buff--;
            }
            else
            {
                player.buff = 0;
            }
            tx_buffqty.text = $"{player.buff}";
        }

        if (select.characterid == (int)characterid.player && select.action == (int)action.heal)
        {
            go_actionspanel.SetActive(true);
            go_itempanel.SetActive(false);

            if (player.heal > 0)
            {
                player.heal--;
            }
            else
            {
                player.heal = 0;
            }
            tx_healqty.text = $"{player.heal}";
        }
        if (select.characterid == (int)characterid.player && select.action == (int)action.back)
        {
            go_actionspanel.SetActive(true);
            go_itempanel.SetActive(false);
        }

        if (select.characterid != (int)characterid.player)
        {
            Debug.Log("Estas pasando por aca mucho?");
            if (select.action == (int)action.attack)
            {
                tx_centralinfo.text = $"Attack!";
            }
            if (select.action == (int)action.special)
            {
                tx_centralinfo.text = $"Special!";
            }
            if (select.action == (int)action.defend)
            {
                tx_centralinfo.text = $"Defend!";
            }
            if (select.action == (int)action.heal)
            {
                tx_centralinfo.text = $"Heal!";
            }
            if (select.action == (int)action.buff)
            {
                tx_centralinfo.text = $"Buff!";
            }
        }
    }
    private void QteHitEvent(QteHitEvent qtehit)
    {
        if (qtehit.success)
        {

        }
    }
    private void QtePrizeEvent(QtePrizeEvent qteprize)
    {

        string _tx_effic = null;
        string _tx_prizehp = null;
        string _tx_prizesp = null;
        string _tx_prizemult = null;
        string _tx_prizedam = null;

        //player.sp += qteprize.prizeSP;
        //player.hp += qteprize.prizeHP;
        if (qteprize.playerturn)
        {
            player.sp = Mathf.Clamp(player.sp + qteprize.prizeSP, 0, max_sp_player);
            //player.hp = Mathf.Clamp(player.hp + qteprize.prizeHP, 0, max_hp_player);
        }
        else
        {
            enemy.sp = Mathf.Clamp(enemy.sp + qteprize.prizeSP, 0, max_sp_enemy);
            //enemy.hp = Mathf.Clamp(enemy.hp + qteprize.prizeHP, 0, max_hp_enemy);
        }

        tx_playersp.text = $"{player.sp}";
        tx_playerhp.text = $"{player.hp}";
        tx_enemysp.text = $"{enemy.sp}";
        tx_enemyhp.text = $"{enemy.hp}";

        if (qteprize.effic < 0.3f)
        {
            _tx_effic = "You suck!";
        }
        else if (qteprize.effic >= 0.3f && qteprize.effic < 0.7f)
        {
            _tx_effic = "Nice!";
        }
        else if (qteprize.effic >= 0.7f && qteprize.effic < 1)
        {
            _tx_effic = "Great!";
        }
        else //100%
        {
            _tx_effic = "You rock!";
        }

        if (qteprize.prizeHP != 0)
        {
            _tx_prizehp = $"\n" + qteprize.prizeHP.ToString("+0;-#") + $" HP! ";
        }
        if (qteprize.prizeSP != 0)
        {
            _tx_prizesp = $"\n" + qteprize.prizeSP.ToString("+0;-#") + $" SP! ";
        }
        if (qteprize.prizeMultiplier != 0)
        {
            _tx_prizemult = $"\n" + qteprize.prizeMultiplier.ToString("F1") + $" Multiplier! ";
        }
        if (qteprize.prizeDamage != 0)
        {
            _tx_prizedam = $"\n" + qteprize.prizeDamage.ToString("+0;-#") + $" Damage! ";
        }

        //tx_prizeinfo.text = $"{_tx_effic}" +
        //                    $"\n {qteprize.prizeSP} SP! " +
        //                    $"\n {qteprize.prizeHP} HP! " +
        //                    $"\n {qteprize.prizeMultiplier} Multiplier" +
        //                    $"\n {qteprize.prizeDamage} Damage";

        tx_prizeinfo.text = $"{_tx_effic}" +
                            _tx_prizesp +
                            _tx_prizehp +
                            _tx_prizemult +
                            _tx_prizedam;

    }


    void UpgradeHPBar()
    {
        float _calc_hpdown_enemy = (float)enemy.hp / (float)max_hp_player;
        float _calc_hpdown_player = (float)player.hp / (float)max_hp_enemy;

        if (float.IsNaN(_calc_hpdown_enemy))
        {
            _calc_hpdown_enemy = 0;
        }

        if (float.IsNaN(_calc_hpdown_player))
        {
            _calc_hpdown_player = 0;
        }

        if (player.hp <= (float)max_hp_player)
        {
            ProgressHP_Player.transform.localScale = new Vector3(Mathf.Clamp((_calc_hpdown_player), 0, 1), ProgressHP_Player.transform.localScale.y, ProgressHP_Player.transform.localScale.z); //Ajusto HP Player

        }

        if (enemy.hp <= (float)max_hp_enemy)
        {
            ProgressHP_Enemy.transform.localScale = new Vector3(Mathf.Clamp((_calc_hpdown_enemy), 0, 1), ProgressHP_Enemy.transform.localScale.y, ProgressHP_Enemy.transform.localScale.z); //Ajusto HP Enemy
        }
    }

    void UpgradeSPBar()
    {
        float _calc_spdown_enemy = (float)enemy.sp / (float)max_sp_player;
        float _calc_spdown_player = (float)player.sp / (float)max_sp_enemy;

        if (float.IsNaN(_calc_spdown_player))
        {
            _calc_spdown_player = 0;
        }

        if (float.IsNaN(_calc_spdown_enemy))
        {
            _calc_spdown_enemy = 0;
        }


        if (player.sp <= (float)max_sp_player)
        {
            ProgressSP_Player.transform.localScale = new Vector3(Mathf.Clamp((_calc_spdown_player), 0f, 1f), ProgressSP_Player.transform.localScale.y, ProgressSP_Player.transform.localScale.z); //Ajusto SP Player

        }

        if (enemy.sp <= (float)max_sp_enemy)
        {
            ProgressSP_Enemy.transform.localScale = new Vector3(Mathf.Clamp((_calc_spdown_enemy), 0f, 1f), ProgressSP_Enemy.transform.localScale.y, ProgressSP_Enemy.transform.localScale.z); //Ajusto SP Enemy
        }
    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        tx_healqty.text = Player_Status.Heals.ToString();
        player.heal = Player_Status.Heals;

        tx_buffqty.text = Player_Status.Buffs.ToString();
        player.buff = Player_Status.Buffs;
    }

}