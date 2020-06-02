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

    //Area Fader
    [SerializeField] private GameObject area1;
    [SerializeField] private GameObject area2;
    [SerializeField] private GameObject area3;
    [SerializeField] private GameObject area4;
    [SerializeField] private GameObject area1Mesh;
    [SerializeField] private GameObject area2Mesh;
    [SerializeField] private GameObject area3Mesh;
    [SerializeField] private GameObject area4Mesh;

    [SerializeField] private float fadeDuration;
    [SerializeField] private float targetAlpha;

    private bool isFading;

    private WaitForSecondsRealtime waitenableinput = new WaitForSecondsRealtime(1f);

    private void OnEnable()
    {
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<ExpandBoundariesEvent>(ExpandBoundariesEvent);
        EventController.AddListener<QuitGameEvent>(QuitGameEvent);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<ExpandBoundariesEvent>(ExpandBoundariesEvent);
        EventController.RemoveListener<QuitGameEvent>(QuitGameEvent);

    }


    // Start is called before the first frame update
    void Start()
    {
        //if (Player_Status.CurrentArea > 1) //A futuro cambiar a algo genérico
        //{
        //    localscale.x = targetScale;
        //    localscale.y = targetScale;
        //    localscale.z = targetScale;

        //    boundaries.transform.localScale = localscale;
        //}
        if (Player_Status.CurrentArea == 1) //A futuro cambiar a algo genérico
        {
            area1.SetActive(true);
            area2.SetActive(false);
            area3.SetActive(false);
            area4.SetActive(false);
        }
        if (Player_Status.CurrentArea == 2) //A futuro cambiar a algo genérico
        {
            area1.SetActive(false);
            area2.SetActive(true);
        }
        if (Player_Status.CurrentArea == 3) //A futuro cambiar a algo genérico
        {
            area1.SetActive(false);
            area2.SetActive(false);
            area3.SetActive(true);
            area4.SetActive(false);
        }

    }

    private void Update()
    {
        SendMessage();
    }

    private void QuitGameEvent(QuitGameEvent quitgame)
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

        PlayerOptions.NewGame = false;

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

        PlayerOptions.NewGame = false;

    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {

        //Debug.Log("New Game: " + PlayerOptions.NewGame);

        //f (Map_Status.FirstTime)
        if (PlayerOptions.NewGame)   
        {
            ////Collectables
            activeCollectables = GameObject.FindGameObjectsWithTag("Collectable");
            collectablesIdentifiers = new int[activeCollectables.Length];
            int i = 0;
            foreach (GameObject activeCollectable in activeCollectables)
            {
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
        //StartCoroutine(ExpandBoundaries(targetScale, targetScaleDuration));
        if (Player_Status.CurrentArea == 1) //A futuro cambiar a algo genérico
        {
            area1.SetActive(true);
            area2.SetActive(false);
            area3.SetActive(false);
            area4.SetActive(false);
        }
        if (Player_Status.CurrentArea == 2) //A futuro cambiar a algo genérico
        {
            StartCoroutine(FadeArea(area1Mesh.GetComponent<MeshRenderer>(), 0.0f, area1, false));
            StartCoroutine(FadeArea(area2Mesh.GetComponent<MeshRenderer>(), targetAlpha, area2, true));

        }
        if (Player_Status.CurrentArea == 3) //A futuro cambiar a algo genérico
        {
            area1.SetActive(false);
            area2.SetActive(false);
            area3.SetActive(true);
            area4.SetActive(false);
        }

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
        //Debug.Log("alguina vez llego aca??");
        isScaling = false;
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
                PlayerOptions.InputEnabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && sendmessage && conversationbegin)
        {
            if (!starttalking)
            {
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
                sendmessage = false;
                StartCoroutine(WaitEnableInput());
            }
        }
    }

    IEnumerator WaitEnableInput()
    {
        yield return waitenableinput;
        PlayerOptions.InputEnabled = true;
    }

    private IEnumerator FadeArea(MeshRenderer mesh, float finalAlpha, GameObject areaObject, bool activate)
    {
        isFading = true;
        Color color;
        color = mesh.material.color;

        if (activate)
        {
            areaObject.SetActive(true);
            color.a = 0.0f;
            mesh.material.color = color;
        }

        float fadeSpeed = Mathf.Abs(color.a - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(color.a, finalAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
            mesh.material.color = color;
        }
        isFading = false;

        if (!activate)
        {
            areaObject.SetActive(false);
        }
    }
}
