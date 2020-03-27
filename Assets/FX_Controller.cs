using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class FX_Controller : MonoBehaviour
{
    [SerializeField] private GameObject fx_hearts_player;
    [SerializeField] private GameObject fx_buff_player;
    [SerializeField] private GameObject fx_attack_player;
    [SerializeField] private GameObject fx_pre_attack_player;

    [SerializeField] private GameObject fx_hearts_enemy;
    [SerializeField] private GameObject fx_buff_enemy;
    [SerializeField] private GameObject fx_attack_enemy;
    [SerializeField] private GameObject fx_pre_attack_enemy;

    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(0.4f);

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

    }
    private void OnDisable()
    {
        EventController.RemoveListener<AnimEvent>(AnimEvent);
        EventController.RemoveListener<ActionEvent>(ActionEvent);
        EventController.RemoveListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.RemoveListener<SelectActionEvent>(SelectActionEvent);

    }

    void Start()
    {
        
    }

    void Update()
    {
        
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
            else if (anim.choosestate && currentaction == (int)action.attack) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_pre_attack_player, 1));
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
            else if (anim.choosestate && currentaction == (int)action.attack) //This means fx should be played
            {
                StartCoroutine(RepeatFX(fx_pre_attack_enemy, 1));
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
}
