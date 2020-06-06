using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Events;
using UnityEngine.AI;

public class Player_Controller : MonoBehaviour
{
    private SceneController sceneController;

    public float fadeSpeed = 1f;
    public float targetAlpha;

    private Color color;
    private Shader shader;

    public Camera gameCamera;
    private RaycastHit hit;
    private bool isBlocking;
    private Ray camRaycast;
    private GameObject objectHit;
    private GameObject objectFaded;
    Dictionary<int, GameObject> objectsFaded;
    Dictionary<int, GameObject> objectsHit;
    private Vector3 cam_player;
    private Vector3 screenPos;
    // public bool ortograph;
    public float movSpeed;
    private bool verify;
    public Material opaqueMat;
    public Material transMat;
    private Material[] currentMats;
    public float MaxTurnSpeed;

    [SerializeField] private float zoomlevel;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private int targetPicks;
    [SerializeField] private int targetCollectables;

    [SerializeField] private Areas_Template areas;


    //[SerializeField] private Item healItem;
    //[SerializeField] private Item buffItem;


    private int currentarea;
    
    private Item item;
    
    public enum camstate
    {
        idle = 0,
        zoomin = 1,
        zoomout = 2,
    }

    private bool cameraplaying = false;

    private int _camstate = 0;

    public int collectables;
    public int picks;
    private int heals;
    private int buffs;
    public int money;

    private bool dialogueactive;
    private bool checkingarea;
    private bool enableinput = false;

    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(0.5f);

    private WaitForSecondsRealtime waitenableinput = new WaitForSecondsRealtime(1f);

    //// Events ////
    private readonly CollectEvent ev_collect = new CollectEvent();
    private readonly BuyEvent ev_buy = new BuyEvent();
    private readonly ExpandBoundariesEvent ev_expand = new ExpandBoundariesEvent();

    private bool skiparea = false;

    //Ragdoll
    public List<Collider> RagdollParts = new List<Collider>();

    private void Awake()
    {
        SetRagdollParts();
    }

    private IEnumerator StartRagdoll()
    {
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(3f);
        TurnOnRagdoll();
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Rigidbody>().AddForce(200f * Vector3.up, ForceMode.Impulse);

    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.GetComponentsInChildren<Collider>();

        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
            }
        }
    }

    private void TurnOnRagdoll()
    {
        //this.GetComponent<Rigidbody>().useGravity = false;

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //this.gameObject.GetComponent<SphereCollider>().enabled = false;

        playerAnimator.enabled = false;
        playerAnimator.avatar = null;


        foreach (Collider c in RagdollParts)
        {
            c.isTrigger = false;
            c.attachedRigidbody.velocity = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<DialogueStatusEvent>(DialogueStatusEvent);
        EventController.AddListener<QuitGameEvent>(QuitGameEvent);
        EventController.AddListener<PayMoneyEvent>(PayMoneyEvent);
        EventController.AddListener<UseItemEvent>(UseItemEvent);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<DialogueStatusEvent>(DialogueStatusEvent);
        EventController.RemoveListener<QuitGameEvent>(QuitGameEvent);
        EventController.RemoveListener<PayMoneyEvent>(PayMoneyEvent);
        EventController.RemoveListener<UseItemEvent>(UseItemEvent);

    }

    void Start()
    {
        //StartCoroutine(StartRagdoll());

        objectsHit = new Dictionary<int, GameObject>();
        cam_player = gameCamera.transform.position - this.transform.position;

        StartCoroutine(WaitEnableInput());

        //Por el momento para testear
        //money = 1000;
        sceneController = FindObjectOfType<SceneController>();
        PlayerOptions.InputEnabled = true;

        if (Player_Status.CurrentArea == 0)
        {
            Player_Status.CurrentArea = currentarea = 1;
        }
    }

    void Update()
    {

        WorldTransparency();

        if (Input.GetKeyUp(KeyCode.L) && Input.GetKeyUp(KeyCode.O))
        {
            skiparea = true;
        }

        if(dialogueactive)
        {
            return;
        }

        if (!checkingarea)
        {
            checkingarea = true;
            StartCoroutine(CheckCurrentArea()); //Checks if the player has met the requisites to advance to the next area
        }


        //Input routines
        if ((Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D)) && enableinput && PlayerOptions.InputEnabled)
        {
            playerAnimator.SetBool("Walking", true);
            PlayerMovement();
        }
        else
        {
            playerAnimator.SetBool("Walking", false);
        }

        if (Input.GetKey(KeyCode.UpArrow) && enableinput && PlayerOptions.InputEnabled)
        {
            if (_camstate == (int)camstate.idle && !cameraplaying)
            {
                cameraAnimator.SetBool("CamIdle", false);
                cameraAnimator.SetBool("CamOut", false);
                cameraAnimator.SetBool("CamIn", true);
                _camstate = (int)camstate.zoomin;
                cameraplaying = true;
                StartCoroutine(WaitforCamera());
            }
            if (_camstate == (int)camstate.zoomout && !cameraplaying)
            {
                cameraAnimator.SetBool("CamIdle", true);
                cameraAnimator.SetBool("CamOut", false);
                cameraAnimator.SetBool("CamIn", false);
                _camstate = (int)camstate.idle;
                cameraplaying = true;
                StartCoroutine(WaitforCamera());
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) && enableinput && PlayerOptions.InputEnabled)
        {
            if (_camstate == (int)camstate.idle && !cameraplaying)
            {
                cameraAnimator.SetBool("CamIdle", false);
                cameraAnimator.SetBool("CamOut", true);
                cameraAnimator.SetBool("CamIn", false);
                _camstate = (int)camstate.zoomout;
                cameraplaying = true;
                StartCoroutine(WaitforCamera());
            }
            if (_camstate == (int)camstate.zoomin && !cameraplaying)
            {
                cameraAnimator.SetBool("CamIdle", true);
                cameraAnimator.SetBool("CamOut", false);
                cameraAnimator.SetBool("CamIn", false);
                _camstate = (int)camstate.idle;
                cameraplaying = true;
                StartCoroutine(WaitforCamera());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Collectable")
        {
            //Debug.Log("Collectable!");
            //Sumar puntos

            collectables++;
            Player_Status.Collectables++;

            //other.gameObject.SetActive(false);
            EventController.TriggerEvent(ev_collect);
        }
        
        if (other.tag == "Interactable")
        {
            //Debug.Log("Interactable!");
            //Deberia de agregar al inventario el objeto
            item =  other.gameObject.GetComponent<ItemPickup>().item;
            Inventory.instance.Add(item,true); //Llama a la instancia del Inventario y la agrega! de pechio (Se podria agregar un parametro mas con el ID para otro prc y agregarlo como Key item debajo)
            other.gameObject.SetActive(false); //No lo borro para agregarlo al inventario
            //EventController.TriggerEvent(ev_items);
        }
    }





    private void WorldTransparency()
    {

        screenPos = gameCamera.WorldToScreenPoint(this.transform.position);
        camRaycast = gameCamera.ScreenPointToRay(screenPos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(camRaycast);

        for (int i = 0; i < hits.Length; i++)
        {

            RaycastHit hit = hits[i];
            //Debug.Log($"Raycast target: {hit.transform.gameObject.name}");
            if (hit.transform.gameObject.tag == "Transparent")
            {
                //Debug.Log("Hit!");
                objectFaded = hit.transform.gameObject;

                // Si es nuevo, lo agrego al listado.
                if (!objectsHit.ContainsKey(hit.transform.gameObject.GetInstanceID()))
                {
                    //Debug.Log("Asignar material transparente");
                    //currentMats = objectFaded.GetComponent<MeshRenderer>().materials;
                    //currentMats[0] = transMat;
                    objectFaded.GetComponent<MeshRenderer>().material = transMat;
                    objectsHit.Add(hit.transform.gameObject.GetInstanceID(), hit.transform.gameObject);
                }
                color = objectFaded.GetComponent<MeshRenderer>().material.color;

                if (color.a >= targetAlpha) //Quitar Alpha
                //if (Mathf.Approximately(color.a, targetAlpha))                        
                {
                    color.a -= Time.deltaTime * fadeSpeed;
                }
                objectFaded.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        //Si el objeto había estado bloqueando pero ya no está mas, le voy sacando alpha hasta que lo quito.
        for (int j = 0; j < objectsHit.Count; j++)
        {
            verify = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (objectsHit.ElementAt(j).Key == hits[i].transform.gameObject.GetInstanceID())
                {
                    verify = true; //Sigue estando, no hacer nada
                }
            }

            if (verify == false) //No está mas, devolver alpha
            {
                //Devolver el Alpha
                color = objectsHit.ElementAt(j).Value.GetComponent<MeshRenderer>().material.color;

                if (color.a < 1.0f)
                {
                    color.a += Time.deltaTime * fadeSpeed;
                }
                objectsHit.ElementAt(j).Value.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        List<int> keys = new List<int>(objectsHit.Keys);
        foreach (int key in keys)
        {
            //dict[key] = ...

            color = objectsHit[key].GetComponent<MeshRenderer>().material.color;
            if (color.a >= 1.0f)
            //if (Mathf.Approximately(color.a, 1.0f))
            {
                //Debug.Log("Te devuelvo el opaco");
                objectsHit[key].GetComponent<MeshRenderer>().material = opaqueMat;
                objectsHit.Remove(key);
            }
        }
        //Si ya volvió a alpha 1, quitarlo.
        //for (int j = 0; j < objectsHit.Count; j++)
        //{
        //    //objectFaded.GetComponent<MeshRenderer>().material = opaqueMat;
        //    color = objectsHit.ElementAt(j).Value.GetComponent<MeshRenderer>().material.color;
        //    if (color.a >= 1.0f)
        //    //if (Mathf.Approximately(color.a, 1.0f))
        //    {
        //        Debug.Log("Te devuelvo el opaco");
        //        objectFaded.GetComponent<MeshRenderer>().material = opaqueMat;
        //        objectsHit.Remove(objectsHit.ElementAt(j).Key);
        //    }
        //    //Debug.Log(objectsHit.ElementAt(j).Key);
        //}
    }

    private void PlayerMovement()
    {
        float changeangle = 45f;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Quaternion.AngleAxis(changeangle, Vector3.up) * direction;
        direction = direction.normalized * movSpeed;
        //this.transform.Translate(direction * Time.deltaTime);

        Quaternion wanted_rotation = Quaternion.LookRotation(direction);

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, wanted_rotation, MaxTurnSpeed * Time.deltaTime);

        this.transform.Translate(Vector3.forward * Time.deltaTime);

        gameCamera.transform.position = this.transform.position + cam_player;
    }

    bool AnimatorIsPlaying(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    IEnumerator WaitforCamera()
    {
        yield return waitforseconds;
        cameraplaying = false;
    }

    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {
        //Items
        Player_Status.Collectables = collectables;
        Player_Status.Picks = picks;
        Player_Status.Heals = heals;
        Player_Status.Buffs = buffs;
        Player_Status.Money = money;
        Player_Status.CurrentArea = currentarea;

        ////Player position
        Map_Status.PlayerRotation = this.transform.rotation;
        Map_Status.PlayerPosition = this.transform.position;

        ////Camera position
        Map_Status.CameraRotation = gameCamera.transform.rotation;
        Map_Status.CameraPosition = gameCamera.transform.position;
    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        //Restore Items
        collectables = Player_Status.Collectables;
        picks = Player_Status.Picks;
        heals = Player_Status.Heals;
        buffs = Player_Status.Buffs;
        money = Player_Status.Money;
        currentarea = Player_Status.CurrentArea;

        //Restore positions
        //if (!Map_Status.FirstTime)
        if (!PlayerOptions.NewGame)
        {
            this.transform.position = Map_Status.PlayerPosition;
            this.transform.rotation = Map_Status.PlayerRotation;
            gameCamera.transform.position = Map_Status.CameraPosition;
            gameCamera.transform.rotation = Map_Status.CameraRotation;
            this.transform.GetComponent<NavMeshAgent>().Warp(this.transform.position);
        }
    }

    public void BuyHeal(int price)
    {
        if (money > price)
        {
            ev_buy.isheal = true;
            ev_buy.price = price;

            money -= price;
            Player_Status.Money -= price;

            heals++;
            Player_Status.Heals++;
            //Inventory.instance.Add(healItem);

            EventController.TriggerEvent(ev_buy);
        }
    }

    public void BuyBuff(int price)
    {
        if (money > price)
        {
            ev_buy.isheal = false;
            ev_buy.price = price;

            money -= price;
            Player_Status.Money -= price;

            buffs++;
            Player_Status.Buffs++;
            //Inventory.instance.Add(buffItem);

            EventController.TriggerEvent(ev_buy);
        }
    }

    private void DialogueStatusEvent(DialogueStatusEvent status)
    {
        dialogueactive = status.dialogueactive;
    }

    private void PayMoneyEvent(PayMoneyEvent paymoney)
    {
        if (money > 0)
        {
            money -= 1;
            Player_Status.Money -= 1;
        }
    }

    private void QuitGameEvent(QuitGameEvent quitgame)
    {
        ////Player position
        Map_Status.PlayerRotation = this.transform.rotation;
        Map_Status.PlayerPosition = this.transform.position;

        ////Camera position
        Map_Status.CameraRotation = gameCamera.transform.rotation;
        Map_Status.CameraPosition = gameCamera.transform.position;
    }

    //Inventory Usage

    private void UseItemEvent(UseItemEvent useitem)
    {
        if (useitem.item.name == "GAMEKID")
        {
            sceneController.FadeAndLoadScene("GameKid");
        }
    }


    IEnumerator WaitEnableInput()
    {
        yield return waitenableinput;
        enableinput = true;
    }

    IEnumerator CheckCurrentArea() // A futuro, mejorar con un diccionario, lista, array o lo que sea
    {
        yield return waitenableinput;

        if ((picks >= areas.Areas[currentarea - 1].targetPicks && collectables >= areas.Areas[currentarea - 1].targetCollectables && currentarea < 3) || skiparea)
        {
            currentarea++;
            ev_expand.currentarea = currentarea;

            Player_Status.CurrentArea = currentarea;

            EventController.TriggerEvent(ev_expand);
            skiparea = false;
        }
        checkingarea = false;
    }
}
