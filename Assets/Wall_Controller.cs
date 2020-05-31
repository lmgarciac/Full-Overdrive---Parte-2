using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Wall_Controller : MonoBehaviour
{

    ///Message Variables
    public Dialogue dialogue;
    private int dialoguecounter = 0;
    private readonly DialogueEvent ev_dialogue = new DialogueEvent();
    private readonly DialogueStatusEvent ev_dialoguestatus = new DialogueStatusEvent();
    private bool starttalking = false;
    private bool cantalk = false;
    private bool conversationbegin = false;
    private bool sendmessage;

    [SerializeField] private Animator playerAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SendMessage();
    }

    private void OnTriggerEnter(Collider other)
    {
        sendmessage = true;
        playerAnimator.SetBool("Walking", false);
        //Debug.Log("EnterNiebla!");
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("ExitNiebla!");
        sendmessage = false;
    }

    private void SendMessage()
    {
        if (sendmessage && !conversationbegin)
        {
            if (!starttalking)
            {
                ev_dialogue.talking = false;
                ev_dialogue.dialogue = dialogue;
                Debug.Log(dialogue.name);
                ev_dialogue.isshop = false;
                EventController.TriggerEvent(ev_dialogue);
                starttalking = true;
                dialoguecounter++;

                ev_dialoguestatus.dialogueactive = true;
                EventController.TriggerEvent(ev_dialoguestatus);

                conversationbegin = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && sendmessage && conversationbegin)
        {
            if (!starttalking)
            {
                ev_dialogue.talking = false;
                ev_dialogue.dialogue = dialogue;
                Debug.Log(dialogue.name);
                ev_dialogue.isshop = false;
                EventController.TriggerEvent(ev_dialogue);
                starttalking = true;
                dialoguecounter++;

                ev_dialoguestatus.dialogueactive = true;
                EventController.TriggerEvent(ev_dialoguestatus);

            }
            else
            {
                ev_dialogue.talking = true;
                EventController.TriggerEvent(ev_dialogue);
                dialoguecounter++;
            }

            if (dialoguecounter > dialogue.sentences.Length)
            {
                dialoguecounter = 0;
                starttalking = false;

                ev_dialoguestatus.dialogueactive = false;
                EventController.TriggerEvent(ev_dialoguestatus);

                conversationbegin = false;
                sendmessage = false;
            }
        }
    }
}
