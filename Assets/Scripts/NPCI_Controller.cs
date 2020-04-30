using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using System.Linq;


public class NPCI_Controller : MonoBehaviour
{
    public Dialogue[] dialogTree;
    private Dialogue dialogue;
    private int dialoguecounter = 0;
    private readonly DialogueEvent ev_dialogue = new DialogueEvent();
    private readonly DialogueStatusEvent ev_dialoguestatus = new DialogueStatusEvent();
    private readonly PayMoneyEvent ev_paymoney = new PayMoneyEvent();


    private bool starttalking = false;
    private bool cantalk = false;

    private void Start()
    {
        dialogue = dialogTree[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cantalk)
        {
            if (!starttalking)
            {
                CheckQuestStatus();

                ev_dialogue.talking = false;
                ev_dialogue.dialogue = dialogue;
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

                UpdateQuestStatus(); //Verifica si hay una quest asociada al NPC y su estado
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

    private void CheckQuestStatus()
    {
        if (this.tag == "Gordon") //Si habla con el dueño de Milo, iniciar quest
        {
            bool found = false; //Buscar en que estado esta la quest
            int questatus = 0;
            int questIndex = 0;

            for (int i = 0; i < Player_Status.QuestList.Count; ++i)
            {
                var quest = Player_Status.QuestList.ElementAt(i);
                if (quest.questname == "MiloQuest")
                {
                    found = true;
                    questIndex = i;
                    questatus = quest.queststatus;
                }
            }

            Debug.Log("QuestStatus: " + questatus);

            if (found && questatus == 2) //Si cumplio el objetivo finalizar la quest y cambiar dialogo
            {
                Player_Status.QuestList[questIndex] = new Quests("MiloQuest", 3, 0);
                dialogue = dialogTree[1];
            }

            if (found && questatus == 3) //Cambiar dialogo
            {
                dialogue = dialogTree[2];
            }
        }

        if (this.tag == "Homeless") //Si habla con el dueño de Milo, iniciar quest
        {
            bool found = false; //Buscar en que estado esta la quest
            int questatus = 0;
            int questIndex = 0;
            int questCount = 0;
            questCount = Player_Status.QuestList.Count;

            for (int i = 0; i < questCount; ++i)
            {
                var quest = Player_Status.QuestList.ElementAt(i);
                if (quest.questname == "HomelessQuest")
                {
                    found = true;
                    questIndex = i;
                    questatus = quest.queststatus;
                }
            }

            Debug.Log("QuestStatus: " + questatus);

            if (found && questatus == 2) //Si cumplio el objetivo finalizar la quest y cambiar dialogo
            {
                Player_Status.QuestList[questIndex] = new Quests("HomelessQuest", 3, 0);
                dialogue = dialogTree[1];
            }

            if (found && questatus == 3) //Cambiar dialogo
            {
                dialogue = dialogTree[2];
            }
        }

        if (this.tag == "GameKid")
        {
            Quests currentQuest = Player_Status.FindQuest("GameKidQuest");
            if (currentQuest != null && currentQuest.queststatus == 1) //Si la quest esta activa pero aun no cumplio
            {
                dialogue = dialogTree[1];
            }
            if (currentQuest != null && currentQuest.queststatus == 2) //Si cumplio el objetivo finalizar la quest y cambiar dialogo
            {
                Player_Status.QuestList[Player_Status.FindQuestIndex("GameKidQuest")] = new Quests("GameKidQuest", 3, 0);
                dialogue = dialogTree[2];
            }
            if (currentQuest != null && currentQuest.queststatus == 3) //cambiar dialogo
            {
                dialogue = dialogTree[3];
            }
        }
    }

    private void UpdateQuestStatus()
    {
        //Special dialogues
        //Dog dialogue
        if (this.tag == "Gordon") //Si habla con el dueño de Milo, iniciar quest
        {
            bool found = false; //Buscar en que estado esta la quest
            int questatus = 0;
            int questIndex = 0;
            int questCount = 0;
            questCount = Player_Status.QuestList.Count;

            for (int i = 0; i < questCount; ++i)
            {
                var quest = Player_Status.QuestList.ElementAt(i);
                if (quest.questname == "MiloQuest")
                {
                    found = true;
                    questIndex = i;
                    questatus = quest.queststatus;
                }
            }

            if (!found) //Si no esta iniciada, iniciarla
            {
                Player_Status.QuestList.Add(new Quests("MiloQuest", 1, 0));
            }
        }

        if (this.tag == "Milo") //Si habla con Milo
        {
            bool found = false; //Buscar en que estado esta la quest
            int questatus = 0;
            int questIndex = 0;
            int questCount = 0;
            questCount = Player_Status.QuestList.Count;


            for (int i = 0; i < questCount; ++i)
            {
                var quest = Player_Status.QuestList.ElementAt(i);
                if (quest.questname == "MiloQuest")
                {
                    found = true;
                    questIndex = i;
                    questatus = quest.queststatus;
                }
            }

            if (found && questatus == 1) //Si esta iniciada la quest entonces objetivo cumplido
            {
                this.gameObject.SetActive(false);
                Player_Status.QuestList[questIndex] = new Quests("MiloQuest", 2, 0);
            }
        }

        if (this.tag == "Homeless")
        {
            bool found = false; //Buscar en que estado esta la quest
            int questatus = 0;
            int questIndex = 0;
            int questValue = 0;
            int questCount = 0;
            questCount = Player_Status.QuestList.Count;

            for (int i = 0; i < questCount; ++i)
            {
                var quest = Player_Status.QuestList.ElementAt(i);
                if (quest.questname == "HomelessQuest")
                {
                    found = true;
                    questIndex = i;
                    questatus = quest.queststatus;
                    questValue = quest.genericnumber;
                }
            }

            if (!found) //Si no esta iniciada, iniciarla
            {
                Player_Status.QuestList.Add(new Quests("HomelessQuest", 1, 1));
                ev_paymoney.moneypaid = 1;
                EventController.TriggerEvent(ev_paymoney);
            }

            if (found && questatus == 1) //Si esta iniciada la quest chequear el dinero recibido
            {
                if (questValue < 4) //Recibir dinero
                {
                    Player_Status.QuestList[questIndex] = new Quests("HomelessQuest", 1, questValue+1);
                    ev_paymoney.moneypaid = 1;
                    EventController.TriggerEvent(ev_paymoney);
                }
                else //Si ya recibio todo el dinero, finalizar la quest
                {
                    Player_Status.QuestList[questIndex] = new Quests("HomelessQuest", 2, questValue+1);
                    ev_paymoney.moneypaid = 1;
                    EventController.TriggerEvent(ev_paymoney);
                }
            }
        }

        if (this.tag == "GameKid")
        {
            Quests currentQuest = Player_Status.FindQuest("GameKidQuest");

            if (currentQuest == null) //Si no esta iniciada, iniciarla
            {
                Player_Status.QuestList.Add(new Quests("GameKidQuest", 1, 1));
            }

            if (currentQuest!= null && currentQuest.queststatus == 1) //Si esta iniciada la quest chequear el highscore
            {
                if (currentQuest.genericnumber > 2) //Finalizar la quest
                {
                    Player_Status.QuestList[Player_Status.FindQuestIndex("GameKidQuest")] = new Quests("GameKidQuest", 2, currentQuest.genericnumber);

                }
            }
        }

    }
}
