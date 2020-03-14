using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class NPCI_Controller : MonoBehaviour
{
    public Dialogue dialogue;
    private int dialoguecounter = 0;
    private readonly DialogueEvent ev_dialogue = new DialogueEvent();
    private bool starttalking = false;
    private bool cantalk = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cantalk)
        {
            if (!starttalking)
            {
                ev_dialogue.talking = false;
                ev_dialogue.dialogue = dialogue;
                EventController.TriggerEvent(ev_dialogue);
                starttalking = true;
                dialoguecounter++;
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
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            cantalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            cantalk = false;
        }
    }
}
