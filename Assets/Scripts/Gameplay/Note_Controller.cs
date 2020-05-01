using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Note_Controller : MonoBehaviour
{
    private readonly QteHitEvent ev_qtehit = new QteHitEvent();  
    private readonly QtePlayEvent ev_qteplay = new QtePlayEvent();
    private readonly QteMissEvent ev_qtemiss = new QteMissEvent();


    private WaitForSecondsRealtime waitenableinput = new WaitForSecondsRealtime(0.7f);

    public bool canbepressed;
    public bool inputenabled;

    public KeyCode keytopress;

    void Start()
    {
        canbepressed = false;
        inputenabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(keytopress))
        {
            if(canbepressed && inputenabled)
            {
                gameObject.SetActive(false);
                ev_qtehit.success = true;

                if(this.tag == "ArrowBlue")
                {
                    ev_qtehit.color = 0;
                }
                if (this.tag == "ArrowRed")
                {
                    ev_qtehit.color = 1;
                }
                if (this.tag == "ArrowYellow")
                {
                    ev_qtehit.color = 2;
                }
                if (this.tag == "ArrowGreen")
                {
                    ev_qtehit.color = 3;
                }

                EventController.TriggerEvent(ev_qtehit);
            }

            if (!canbepressed && inputenabled)
            {
                inputenabled = false;

                if (this.tag == "ArrowBlue")
                {
                    ev_qtemiss.color = 0;
                }
                if (this.tag == "ArrowRed")
                {
                    ev_qtemiss.color = 1;
                }
                if (this.tag == "ArrowYellow")
                {
                    ev_qtemiss.color = 2;
                }
                if (this.tag == "ArrowGreen")
                {
                    ev_qtemiss.color = 3;
                }

                ev_qtemiss.enableinput = inputenabled;
                EventController.TriggerEvent(ev_qtemiss);
                StartCoroutine(WaitEnableInput());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Activator"))
        {
            canbepressed = true;
        }
        //if (other.tag == "ActivatorBlue")
        //{
        //    canbepressed = true;
        //    ev_qtehit.color = 0;
        //}

        //else if (other.tag == "ActivatorRed")
        //{
        //    canbepressed = true;
        //    ev_qtehit.color = 1;
        //}

        //else if (other.tag == "ActivatorYellow")
        //{
        //    canbepressed = true;
        //    ev_qtehit.color = 2;
        //}

        //else if (other.tag == "ActivatorGreen")
        //{
        //    canbepressed = true;
        //    ev_qtehit.color = 3;
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.StartsWith("Activator"))
        {
            canbepressed = false;
        }
        //if (other.tag == "ActivatorBlue")
        //{
        //    canbepressed = false;
        //}

        //else if(other.tag == "ActivatorRed")
        //{
        //    canbepressed = false;
        //}

        //else if (other.tag == "ActivatorYellow")
        //{
        //    canbepressed = false;
        //}

        //else if (other.tag == "ActivatorGreen")
        //{
        //    canbepressed = false;
        //}
    }

    IEnumerator WaitEnableInput()
    {
        yield return waitenableinput;
        inputenabled = true;

        if (this.tag == "ArrowBlue")
        {
            ev_qtemiss.color = 0;
        }
        if (this.tag == "ArrowRed")
        {
            ev_qtemiss.color = 1;
        }
        if (this.tag == "ArrowYellow")
        {
            ev_qtemiss.color = 2;
        }
        if (this.tag == "ArrowGreen")
        {
            ev_qtemiss.color = 3;
        }

        ev_qtemiss.enableinput = inputenabled;
        EventController.TriggerEvent(ev_qtemiss);
    }

}
