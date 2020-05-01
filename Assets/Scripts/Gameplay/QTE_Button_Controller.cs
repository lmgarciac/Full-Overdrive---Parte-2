using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class QTE_Button_Controller : MonoBehaviour
{
    private SpriteRenderer button_renderer;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public KeyCode actionkey;

    [SerializeField] private ParticleSystem ParticleNoteHit;
    [SerializeField] private GameObject ParticleKeyInput;


    void Start()
    {
        button_renderer = GetComponent<SpriteRenderer>();
        ParticleNoteHit.Stop();
    }

    private void OnEnable()
    {
        EventController.AddListener<QteHitEvent>(QteHitEvent);
        EventController.AddListener<QteMissEvent>(QteMissEvent);

    }
    private void OnDisable()
    {
        EventController.RemoveListener<QteHitEvent>(QteHitEvent);
        EventController.RemoveListener<QteMissEvent>(QteMissEvent);

    }

    void Update()
    {
        if(Input.GetKeyDown(actionkey))
        {
            button_renderer.sprite = pressedImage;
        }
        if(Input.GetKeyUp(actionkey))
        {
            button_renderer.sprite = defaultImage;
        }
    }

    private void QteHitEvent(QteHitEvent qtehit)
    {
        Debug.Log("Emit!");

        if(qtehit.color == 0 && this.tag == "ActivatorBlue")
        {
            ParticleNoteHit.Play();
        }
        if (qtehit.color == 1 && this.tag == "ActivatorRed")
        {
            ParticleNoteHit.Play();
        }
        if (qtehit.color == 2 && this.tag == "ActivatorYellow")
        {
            ParticleNoteHit.Play();
        }
        if (qtehit.color == 3 && this.tag == "ActivatorGreen")
        {
            ParticleNoteHit.Play();
        }
    }

    private void QteMissEvent(QteMissEvent qtemiss)
    {

        if (qtemiss.color == 0 && this.tag == "ActivatorBlue")
        {
            if (qtemiss.enableinput)
            {
                ParticleKeyInput.SetActive(true);
            }
            else
            {
                ParticleKeyInput.SetActive(false);
            }
        }
        if (qtemiss.color == 1 && this.tag == "ActivatorRed")
        {
            if (qtemiss.enableinput)
            {
                ParticleKeyInput.SetActive(true);
            }
            else
            {
                ParticleKeyInput.SetActive(false);
            }
        }
        if (qtemiss.color == 2 && this.tag == "ActivatorYellow")
        {
            if (qtemiss.enableinput)
            {
                ParticleKeyInput.SetActive(true);
            }
            else
            {
                ParticleKeyInput.SetActive(false);
            }
        }
        if (qtemiss.color == 3 && this.tag == "ActivatorGreen")
        {
            if (qtemiss.enableinput)
            {
                ParticleKeyInput.SetActive(true);
            }
            else
            {
                ParticleKeyInput.SetActive(false);
            }
        }


    }
}
