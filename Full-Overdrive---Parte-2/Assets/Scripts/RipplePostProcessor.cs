using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class RipplePostProcessor : MonoBehaviour
{
    public Material RippleMaterial;
    public float MaxAmount = 50f;

    [Range(0, 1)]
    public float Friction = .9f;

    private float Amount = 0f;

    private int currentaction;
    private bool showripple = false;

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

    private Vector3 modelposition;

    private void OnEnable()
    {
        EventController.AddListener<AnimEvent>(AnimEvent);
        EventController.AddListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.AddListener<SelectActionEvent>(SelectActionEvent);
        EventController.AddListener<ActionEvent>(ActionEvent);

    }
    private void OnDisable()
    {
        EventController.RemoveListener<AnimEvent>(AnimEvent);
        EventController.RemoveListener<EnableTurnEvent>(EnableTurnEvent);
        EventController.RemoveListener<SelectActionEvent>(SelectActionEvent);
        EventController.RemoveListener<ActionEvent>(ActionEvent);
    }

    void Update()
    {
        if (showripple)
        {
            this.Amount = this.MaxAmount;
            //Vector3 pos = Input.mousePosition;
            //this.RippleMaterial.SetFloat("_CenterX", pos.x);
            //this.RippleMaterial.SetFloat("_CenterY", pos.y);

            this.RippleMaterial.SetFloat("_CenterX", modelposition.x);
            this.RippleMaterial.SetFloat("_CenterY", modelposition.y);
            showripple = false;
            Debug.Log("Showripple " + showripple);
        }

        this.RippleMaterial.SetFloat("_Amount", this.Amount);
        this.Amount *= this.Friction;


    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, this.RippleMaterial);
    }

    private void AnimEvent(AnimEvent anim)
    {
        if (anim.animstate && currentaction == (int)action.attack) //This means fx should be played
        {
            Debug.Log("ShowRipple");
            showripple = true;
            modelposition = anim.modelposition;
        }
        if (anim.animstate && currentaction == (int)action.special) //This means fx should be played
        {
            Debug.Log("ShowRipple");
            showripple = true;
            modelposition = anim.modelposition;
        }
    }

    private void SelectActionEvent(SelectActionEvent timer)
    {
        currentaction = timer.action;
    }

    private void EnableTurnEvent(EnableTurnEvent enableturn)
    {
        if (enableturn.turnstate != (int)turnstate.qte && enableturn.turnstate != (int)turnstate.anim) //sino me lo borra cuando deberia estar activo
        {
            currentaction = (int)action.none;
            showripple = false;
        }
    }

    private void ActionEvent(ActionEvent ev_action)
    {
        currentaction = ev_action.action;
    }
}
