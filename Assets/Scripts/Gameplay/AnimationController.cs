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

    //private Animator cameraanim;

    public enum animation
    {
        none = 0,
        idle = 1,
        play = 2,
        special = 3,
        win = 4,
        lose = 5,
    }

    private void OnEnable()
    {
        EventController.AddListener<AnimEvent>(AnimEvent);
    }
    private void OnDisable()
    {
        EventController.RemoveListener<AnimEvent>(AnimEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        an_player = go_player.GetComponent<Animator>();
        an_enemy = go_enemy.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
                }
            }
            else if (anim.animation == (int)animation.special)
            {
                an_player.SetBool("Idle", false);
                an_player.SetBool("Attack", false);
                an_player.SetBool("Special", true);
            }
            else //Idle
            {
                an_player.SetBool("Idle", true);
                an_player.SetBool("Attack", false);
                an_player.SetBool("Special", false);
                an_player.SetBool("Win", false);
                an_player.SetBool("Lose", false);
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
                }
            }
            else if (anim.animation == (int)animation.special)
            {
                an_enemy.SetBool("Idle", false);
                an_enemy.SetBool("Attack", false);
                an_enemy.SetBool("Special", true);
            }
            else //Idle
            {
                an_enemy.SetBool("Idle", true);
                an_enemy.SetBool("Attack", false);
                an_enemy.SetBool("Special", false);
                an_enemy.SetBool("Win", false);
                an_enemy.SetBool("Lose", false);
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

            an_enemy.SetBool("Idle", false);
            an_enemy.SetBool("Attack", false);
            an_enemy.SetBool("Special", false);
            an_enemy.SetBool("Win", false);
            an_enemy.SetBool("Lose", true);
        }
        else if (anim.animation == (int)animation.lose)
        {
            an_player.SetBool("Idle", false);
            an_player.SetBool("Attack", false);
            an_player.SetBool("Special", false);
            an_player.SetBool("Win", false);
            an_player.SetBool("Lose", true);

            an_enemy.SetBool("Idle", false);
            an_enemy.SetBool("Attack", false);
            an_enemy.SetBool("Special", false);
            an_enemy.SetBool("Lose", false);
            an_enemy.SetBool("Win", true);
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

    }

}
