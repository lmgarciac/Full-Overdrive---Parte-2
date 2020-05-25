using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class FX_Controller : MonoBehaviour
{
    private EnemyFXScript enemyFX;
    [SerializeField] private GameObject fx_hearts_player;
    [SerializeField] private GameObject fx_buff_player;
    [SerializeField] private GameObject fx_defend_player;

    [SerializeField] private GameObject fx_attack_player;
    [SerializeField] private GameObject fx_special_player;
    [SerializeField] private GameObject fx_pre_attack_player;
    [SerializeField] private GameObject fx_pre_special_player;

    [SerializeField] private GameObject fx_hearts_enemy;
    [SerializeField] private GameObject fx_buff_enemy;
    [SerializeField] private GameObject fx_defend_enemy;

    [SerializeField] private GameObject fx_attack_enemy;
    [SerializeField] private GameObject fx_special_enemy;
    [SerializeField] private GameObject fx_pre_attack_enemy;
    [SerializeField] private GameObject fx_pre_special_enemy;


    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(0.4f);
    private WaitForSecondsRealtime waitforsecondsspecial = new WaitForSecondsRealtime(0.1f);


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

    public enum turnstate
    {
        turninfo = 0,
        chooseaction = 1,
        qte = 2,
        anim = 3,
        miss = 4,
    }

    private int currentaction;

    private void OnEnable()
    {
        EventController.AddListener<AnimEvent>(AnimEvent);
        EventController.AddListener<ActionEvent>(ActionEvent);
        EventController.AddListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.AddListener<SelectActionEvent>(SelectActionEvent);
        EventController.AddListener<LoadEnemyEvent>(LoadEnemyEvent);

    }
    private void OnDisable()
    {
        EventController.RemoveListener<AnimEvent>(AnimEvent);
        EventController.RemoveListener<ActionEvent>(ActionEvent);
        EventController.RemoveListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.RemoveListener<SelectActionEvent>(SelectActionEvent);
        EventController.RemoveListener<LoadEnemyEvent>(LoadEnemyEvent);

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void LoadEnemyEvent(LoadEnemyEvent currentEnemy)
    {
        enemyFX = GameObject.FindGameObjectWithTag("EnemyFX").GetComponent<EnemyFXScript>();
        fx_hearts_enemy = enemyFX.fx_hearts_enemy;
        fx_buff_enemy = enemyFX.fx_buff_enemy;
        fx_defend_enemy = enemyFX.fx_defend_enemy;
        fx_attack_enemy = enemyFX.fx_attack_enemy;
        fx_special_enemy = enemyFX.fx_special_enemy;
        fx_pre_attack_enemy = enemyFX.fx_pre_attack_enemy;
        fx_pre_special_enemy = enemyFX.fx_pre_special_enemy;
    }

    private void AnimEvent(AnimEvent anim)
    {
        if (anim.playerturn)
        {
            if (anim.animstate && currentaction == (int)action.heal) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_hearts_player, 3));
            }
            if (anim.animstate && currentaction == (int)action.buff) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_buff_player, 3));
            }
            if (anim.animstate && currentaction == (int)action.attack) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_attack_player, 1));
            }
            if (anim.animstate && currentaction == (int)action.defend) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_defend_player, 3));
            }
            else if (anim.choosestate && currentaction == (int)action.attack) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_pre_attack_player, 1));
            }

            if (anim.animstate && currentaction == (int)action.special) //This means fx should be played
            {
                StartCoroutine(RepeatFXSpecial(fx_special_player, 1));
            }
            else if (anim.choosestate && currentaction == (int)action.special) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_pre_special_player, 1));
            }
        }
        else
        {
            if (anim.animstate && currentaction == (int)action.heal) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_hearts_enemy, 3));
            }
            if (anim.animstate && currentaction == (int)action.buff) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_buff_enemy, 3));
            }
            if (anim.animstate && currentaction == (int)action.attack) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_attack_enemy, 1));
            }
            if (anim.animstate && currentaction == (int)action.defend) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_defend_enemy, 3));
            }
            else if (anim.choosestate && currentaction == (int)action.attack) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_pre_attack_enemy, 1));
            }

            if (anim.animstate && currentaction == (int)action.special) //This means fx should be played
            {
                StartCoroutine(RepeatFXSpecial(fx_special_enemy, 1));
            }
            else if (anim.choosestate && currentaction == (int)action.special) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_pre_special_enemy, 1));
            }
        }
    }

    private void ActionEvent(ActionEvent ev_action)
    {
        currentaction = ev_action.action;
    }

    private void SelectActionEvent(SelectActionEvent ev_selectaction)
    {
        currentaction = ev_selectaction.action;
    }

    private void EnableTurnEvent(EnableTurnEvent turn)
    {
        if (turn.turnstate != (int)turnstate.qte)
        {
            currentaction = (int)action.none;
        }
    }

    IEnumerator RepeatFX(GameObject fx, int times)
    {
        int counter = 0;

        while (counter < times)
        {
            fx.SetActive(true);
            yield return waitforseconds;
            fx.SetActive(false);

            counter++;
        }
    }

    IEnumerator RepeatFXSpecial(GameObject fx, int times)
    {
        int counter = 0;

        while (counter < times)
        {
            fx.SetActive(true);
            yield return waitforsecondsspecial;
            fx.SetActive(false);

            counter++;
        }
    }
}
