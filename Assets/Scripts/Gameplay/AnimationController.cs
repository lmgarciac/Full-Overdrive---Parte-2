using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject camera;

    [SerializeField] private GameObject objplayerattack;
    [SerializeField] private GameObject objplayeridle;
    [SerializeField] private GameObject objplayerspecial;

    [SerializeField] private GameObject objenemyattack;
    [SerializeField] private GameObject objenemyidle;
    [SerializeField] private GameObject objenemyspecial;

    private Animator playeranim;
    private Animator enemyanim;
    private Animator cameraanim;

    public enum animation
    {
        none = 0,
        idle = 1,
        play = 2,
        special = 3,
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
        playeranim = player.GetComponent<Animator>();
        enemyanim = enemy.GetComponent<Animator>();
        cameraanim = camera.GetComponent<Animator>();
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
                //playeranim.runtimeAnimatorController = Resources.Load("ANI_Main_Atack") as RuntimeAnimatorController;
                objplayeridle.SetActive(false);
                objplayerspecial.SetActive(false);
                objplayerattack.SetActive(true);
            }
            else if (anim.animation == (int)animation.special)
            {
                //playeranim.runtimeAnimatorController = Resources.Load("ANI_Main_Special") as RuntimeAnimatorController;
                objplayeridle.SetActive(false);
                objplayerspecial.SetActive(true);
                objplayerattack.SetActive(false);
            }
            else //Idle
            {
                //playeranim.runtimeAnimatorController = Resources.Load("ANI_Main_idle") as RuntimeAnimatorController;
                objplayeridle.SetActive(true);
                objplayerspecial.SetActive(false);
                objplayerattack.SetActive(false);
            }
        }
        else //Enemy
        {
            if (anim.animation == (int)animation.play)
            {
                //enemyanim.runtimeAnimatorController = Resources.Load("ANI_Enemy_Atack") as RuntimeAnimatorController;
                objenemyidle.SetActive(false);
                objenemyspecial.SetActive(false);
                objenemyattack.SetActive(true);
            }
            else if (anim.animation == (int)animation.special)
            {
                //enemyanim.runtimeAnimatorController = Resources.Load("ANI_Enemy_Special") as RuntimeAnimatorController;
                objenemyidle.SetActive(false);
                objenemyspecial.SetActive(true);
                objenemyattack.SetActive(false);
            }
            else
            {
                //enemyanim.runtimeAnimatorController = Resources.Load("ANI_Enemy_idle") as RuntimeAnimatorController;
                objenemyidle.SetActive(true);
                objenemyspecial.SetActive(false);
                objenemyattack.SetActive(false);
            }
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
