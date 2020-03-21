using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Tutorial_Controller : MonoBehaviour
{
    private GameObject instantiatedQTE;
    private static Object qteBase;
    [SerializeField] private Vector3 positionQTE;
    [SerializeField] private Quaternion rotationQTE;

    private WaitForSecondsRealtime waitforsecondsinitial = new WaitForSecondsRealtime(6f);
    private WaitForSecondsRealtime waitforsecondsqte = new WaitForSecondsRealtime(3f);

    public float timescaleconfig = 1f;
    private bool isFading;
    public float fadeDurationinitial = 2f;
    public float fadeDurationfinal = 2f;

    public float targetTimeScaleSlow = 0.3f;
    public float targetTimeScaleFast = 0.3f;

    private float qteelapsedTime = 0f;
    private bool qteisMoving = false;
    private bool qtecanMove = true;
    public bool qtein;
    public bool qteout;


    private bool startdialogue = false;

    private Vector3 qtenextPosition;

    private int tutorialphase; //0 Initial
                               //1 Slow
                               //2 Sped Up

    private int qtehitcount = 0;
    private int qteleavecount = 0;

    [SerializeField] private GameObject soundController;

    private AudioSource musicPlayer;
    private AudioSource dialoguePlayer;

    private AudioClip so_dialogueGero;

    [SerializeField] private float qtelerpTime = 1f;
    [SerializeField] private float qtestep = 1f;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private Animator infoTextAnimator;

    private SceneController sceneController;


    void Start()
    {
        playerAnimator.SetBool("Special", true);
        AudioSource[] audios = soundController.GetComponents<AudioSource>();
        dialoguePlayer = audios[1];
        musicPlayer = audios[2];

        so_dialogueGero = (AudioClip)Resources.Load<AudioClip>($"Sound/so_dialogue_gero");

        sceneController = FindObjectOfType<SceneController>();

        StartCoroutine(InitTutorial());
    }


    private void OnEnable()
    {
        EventController.AddListener<QtePlayEvent>(QtePlayEvent);
        EventController.AddListener<QteLeaveEvent>(QteLeaveEvent);
        EventController.AddListener<QteHitEvent>(QteHitEvent);

    }
    private void OnDisable()
    {
        EventController.RemoveListener<QtePlayEvent>(QtePlayEvent);
        EventController.RemoveListener<QteLeaveEvent>(QteLeaveEvent);
        EventController.RemoveListener<QteHitEvent>(QteHitEvent);

    }


    void Update()
    {
        //QTE Movement Scroll    
        if (instantiatedQTE != null)
        {
            if (qtecanMove && qtein)
            {
                qtenextPosition = instantiatedQTE.transform.position + Vector3.right * qtestep;
                qteisMoving = true;
            }
            if (qtecanMove && qteout)
            {
                qtenextPosition = instantiatedQTE.transform.position + Vector3.left * qtestep;
                qteisMoving = true;
            }
            if (qteisMoving)
            {
                MoveQTE(instantiatedQTE);
            }
        }

        if ((qtehitcount + qteleavecount) >= 4 && tutorialphase == 0)
        {
            StartCoroutine(FadeTime(1f, fadeDurationinitial));
            infoTextAnimator.SetBool("Slide", false);

            tutorialphase = 1;
        }

        if (qtehitcount + qteleavecount >= 8 && tutorialphase == 1)
        {
            StartCoroutine(FadeTime(targetTimeScaleFast, fadeDurationfinal));
            tutorialphase = 2;
        }
        if (qtehitcount + qteleavecount >= 36 && tutorialphase == 2)
        {
            cameraAnimator.SetBool("ZoomIn", true);
        }
        if (qtehitcount + qteleavecount >= 42 && tutorialphase == 2)
        {
            tutorialphase = 3;
            startdialogue = true;
            StartCoroutine(FadeTime(0.01f, fadeDurationinitial));
            //Dialogo
        }
        if (tutorialphase == 3 && !dialoguePlayer.isPlaying && startdialogue == false)
        {
            StartCoroutine(FadeTime(1f, fadeDurationinitial));
            sceneController.FadeAndLoadScene("_Test_Navigation");
        }

    }

    private void FixedUpdate()
    {
        //if (startdialogue)
        //{
        //    startdialogue = false;
        //    musicPlayer.volume = 0f;
        //    Debug.Log("Start Dialogue!");
        //    dialoguePlayer.clip = so_dialogueGero;
        //    dialoguePlayer.Play();
        //}
    }

    private IEnumerator FadeTime(float finalTimeScale, float fadeDuration)
    {
        isFading = true;
        float fadeSpeed = Mathf.Abs(Time.timeScale - finalTimeScale) / fadeDuration;
        while (!Mathf.Approximately(Time.timeScale, finalTimeScale))
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, finalTimeScale,
                                               fadeSpeed * Time.deltaTime);
            musicPlayer.pitch = Time.timeScale;
            yield return null;
        }
        Debug.Log("alguina vez llego aca??");
        isFading = false;


        if (startdialogue)
        {
            startdialogue = false;
            musicPlayer.volume = 0f;
            Debug.Log("Start Dialogue!");
            dialoguePlayer.clip = so_dialogueGero;
            dialoguePlayer.Play();
        }
    }



    IEnumerator InitTutorial()
    {
        yield return waitforsecondsinitial;

        qteBase = Resources.Load("Prefabs/Qte_Base");
        instantiatedQTE = (GameObject)Instantiate(qteBase);
        QTE_Main_Controller qteController = instantiatedQTE.GetComponent<QTE_Main_Controller>();
        qteController.InitializeQte("QTE_Template_tutorial");
        instantiatedQTE.transform.position = positionQTE;
        instantiatedQTE.transform.Rotate(rotationQTE.x, rotationQTE.y, rotationQTE.z, Space.Self);
        qtein = true;

        infoTextAnimator.SetBool("Slide", true);

        yield return waitforsecondsqte;

        StartCoroutine(FadeTime(targetTimeScaleSlow, fadeDurationinitial));
    }

    private void MoveQTE(GameObject qte)
    {
        qteelapsedTime += Time.deltaTime;
        if (qteelapsedTime > qtelerpTime)
        {
            qteelapsedTime = qtelerpTime;
        }

        float qtepercentage = qteelapsedTime / qtelerpTime;
        qte.transform.position = Vector3.Lerp(qte.transform.position, qtenextPosition, qtepercentage);

        qtecanMove = false;

        if (qtepercentage == 1)
        {
            //qtecanMove = true;
            qteisMoving = false;
            qteelapsedTime = 0f;
        }
    }

    private void QteHitEvent(QteHitEvent qtehit)
    {
        qtehitcount++;
    }

    private void QteLeaveEvent(QteLeaveEvent qteleave)
    {
        qteleavecount++;
    }

    private void QtePlayEvent(QtePlayEvent qteplay)
    {

    }
}
