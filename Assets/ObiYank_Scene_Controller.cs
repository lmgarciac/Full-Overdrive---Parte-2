using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class ObiYank_Scene_Controller : MonoBehaviour
{

    [SerializeField] private Animator camera;
    [SerializeField] private Animator player;

    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(5f);
    private WaitForSecondsRealtime waitforsecond = new WaitForSecondsRealtime(1f);

    private SceneController sceneController;

    public Dialogue dialogue;
    private int dialoguecounter = 0;
    private readonly DialogueEvent ev_dialogue = new DialogueEvent();
    private readonly DialogueStatusEvent ev_dialoguestatus = new DialogueStatusEvent();

    private bool starttalking = false;
    private bool cantalk = false;
    private bool conversationbegin = false;

    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();

        StartCoroutine(InitScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (cantalk && !conversationbegin)
        {
            if (!starttalking)
            {
                Debug.Log("TAlkin");
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

        if (Input.GetKeyDown(KeyCode.E) && cantalk && conversationbegin)
        {
            if (!starttalking)
            {
                Debug.Log("TAlkin");
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

                sceneController.FadeAndLoadScene("_Test_Navigation");

            }
        }
    }

    IEnumerator InitScene()
    {

        yield return waitforseconds;
        camera.SetBool("Switch", true);
        player.SetBool("GetUp", true);

        yield return waitforseconds;
        yield return waitforseconds;

        player.SetBool("StartIdle", true);

        cantalk = true;

        //Comenzar dialogo
    }
}
