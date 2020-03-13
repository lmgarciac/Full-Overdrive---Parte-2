using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Dialogue_Trigger : MonoBehaviour
{
    public Dialogue dialogue;
    private readonly DialogueEvent ev_dialogue = new DialogueEvent();
    private bool starttalking;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!starttalking)
                {
                    ev_dialogue.talking = false;
                    ev_dialogue.dialogue = dialogue;
                    EventController.TriggerEvent(ev_dialogue);
                    starttalking = true;
                }
                else
                {
                    ev_dialogue.talking = true;
                    EventController.TriggerEvent(ev_dialogue);
                }
            }
        }
    }
}
