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

    private bool buttoncanbepressed;
    private bool blocked;
    private readonly QteMissEvent ev_qtemiss = new QteMissEvent();

    [SerializeField] private ParticleSystem ParticleNoteHit;
    [SerializeField] private GameObject ParticleKeyInput;

        private WaitForSecondsRealtime waitenableinput = new WaitForSecondsRealtime(1f);


    void Start()
    {
        button_renderer = GetComponent<SpriteRenderer>();
        ParticleNoteHit.Stop();
        buttoncanbepressed = false;
        blocked = false;
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

            if (buttoncanbepressed == false && !blocked)
            {
                //PlayerOptions.QteInputEnabled = false;

                if (this.tag == "ActivatorBlue")
                {
                    ev_qtemiss.color = 0;
                }
                if (this.tag == "ActivatorRed")
                {
                    ev_qtemiss.color = 1;
                }
                if (this.tag == "ActivatorYellow")
                {
                    ev_qtemiss.color = 2;
                }
                if (this.tag == "ActivatorGreen")
                {
                    ev_qtemiss.color = 3;
                }
                blocked = true;
                ev_qtemiss.enableinput = buttoncanbepressed;
                EventController.TriggerEvent(ev_qtemiss);
                StartCoroutine(WaitEnableInput());
            }

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

    IEnumerator WaitEnableInput()
    {
        yield return waitenableinput;
        //PlayerOptions.QteInputEnabled = true;

        if (this.tag == "ActivatorBlue")
        {
            ev_qtemiss.color = 0;
        }
        if (this.tag == "ActivatorRed")
        {
            ev_qtemiss.color = 1;
        }
        if (this.tag == "ActivatorYellow")
        {
            ev_qtemiss.color = 2;
        }
        if (this.tag == "ActivatorGreen")
        {
            ev_qtemiss.color = 3;
        }
        buttoncanbepressed = true;
        blocked = false;
        ev_qtemiss.enableinput = buttoncanbepressed;
        EventController.TriggerEvent(ev_qtemiss);
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.tag == "ActivatorYellow" && other.tag.StartsWith("ArrowYellow"))
        {
            buttoncanbepressed = true;
        }
        else if (this.gameObject.tag == "ActivatorRed" && other.tag.StartsWith("ArrowRed"))
        {
            buttoncanbepressed = true;
        }
        else if (this.gameObject.tag == "ActivatorGreen" && other.tag.StartsWith("ArrowGreen"))
        {
            buttoncanbepressed = true;
        }
        else if (this.gameObject.tag == "ActivatorBlue" && other.tag.StartsWith("ArrowBlue"))
        {
            buttoncanbepressed = true;
        }
        else
        {
            buttoncanbepressed = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.StartsWith("Arrow"))
        {
            buttoncanbepressed = false;
        }
    }

}
