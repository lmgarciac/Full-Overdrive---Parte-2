using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Map_Controller : MonoBehaviour
{
    private GameObject[] currentCollectables;
    private bool collectableActive;
    private GameObject[] activeCollectables;
    private int[] collectablesIdentifiers;
    private Collectable_Controller collectableController;
    private bool isScaling;
    private Vector3 localscale;

    [SerializeField] private GameObject boundaries;
    [SerializeField] private float targetScale;
    [SerializeField] private float targetScaleDuration;


    ///Message Variables
    public Dialogue dialogue;
    private int dialoguecounter = 0;
    private readonly DialogueEvent ev_dialogue = new DialogueEvent();
    private readonly DialogueStatusEvent ev_dialoguestatus = new DialogueStatusEvent();
    private bool starttalking = false;
    private bool cantalk = false;
    private bool conversationbegin = false;
    private bool sendmessage;

    private void OnEnable()
    {
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<ExpandBoundariesEvent>(ExpandBoundariesEvent);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<ExpandBoundariesEvent>(ExpandBoundariesEvent);

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        SendMessage();
    }

    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {

        ////Collectables
        activeCollectables = GameObject.FindGameObjectsWithTag("Collectable");
        collectablesIdentifiers = new int[activeCollectables.Length];

        int i = 0;
        foreach (GameObject activeCollectable in activeCollectables)
        {
            collectablesIdentifiers[i] = activeCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier;
            i++;
        }
        Map_Status.CollectablesIdentifiers = collectablesIdentifiers;

        Map_Status.FirstTime = false;

    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        if (Map_Status.FirstTime)
        {
            ////Collectables
            activeCollectables = GameObject.FindGameObjectsWithTag("Collectable");
            collectablesIdentifiers = new int[activeCollectables.Length];
            int i = 0;
            foreach (GameObject activeCollectable in activeCollectables)
            {
                Debug.Log(activeCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier);
                collectableController = activeCollectable.GetComponent<Collectable_Controller>();
                collectablesIdentifiers[i] = collectableController.uniqueIdentifier;
                i++;
            }

            Map_Status.CollectablesIdentifiers = collectablesIdentifiers;
        }
        else
        {
            ////Collectables
            currentCollectables = GameObject.FindGameObjectsWithTag("Collectable");

            foreach (GameObject currentCollectable in currentCollectables)
            {
                collectableActive = false;
                foreach (int currentCollectableId in Map_Status.CollectablesIdentifiers)
                {
                    if (currentCollectableId == currentCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier)
                    {
                        collectableActive = true;
                    }
                }
                if (collectableActive == false)
                {
                    currentCollectable.SetActive(false);
                }
            }
        }
    }

    private void ExpandBoundariesEvent(ExpandBoundariesEvent expand)
    {
        sendmessage = true;
        StartCoroutine(ExpandBoundaries(targetScale, targetScaleDuration));
    }


    private IEnumerator ExpandBoundaries(float finalScale, float scaleDuration)
    {
        isScaling = true;
        float scaleSpeed = Mathf.Abs(boundaries.transform.localScale.x - finalScale) / scaleDuration;

        while (!Mathf.Approximately(boundaries.transform.localScale.x, finalScale))
        {
            localscale.x = Mathf.MoveTowards(boundaries.transform.localScale.x, finalScale,
                                               scaleSpeed * Time.deltaTime);
            localscale.y = Mathf.MoveTowards(boundaries.transform.localScale.y, finalScale,
                                               scaleSpeed * Time.deltaTime);
            localscale.z = Mathf.MoveTowards(boundaries.transform.localScale.z, finalScale,
                                               scaleSpeed * Time.deltaTime);

            boundaries.transform.localScale = localscale;

            yield return null;
        }
        Debug.Log("alguina vez llego aca??");
        isScaling = false;
    }

    private void SendMessage()
    {
        if (sendmessage && !conversationbegin)
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

        if (Input.GetKeyDown(KeyCode.E) && sendmessage && conversationbegin)
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

                sendmessage = false;
            }
        }
    }
}
