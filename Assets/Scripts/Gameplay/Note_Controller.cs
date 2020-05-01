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
    private bool buttoncanbepressed;

    public KeyCode keytopress;

    private void OnEnable()
    {
        EventController.AddListener<QteMissEvent>(QteMissEvent);

    }
    private void OnDisable()
    {
        EventController.RemoveListener<QteMissEvent>(QteMissEvent);

    }


    void Start()
    {
        buttoncanbepressed = true;
        canbepressed = false;
        PlayerOptions.QteInputEnabled = true;
    }

    void Update()
    {
        //Debug.Log("Can be pressed: " + canbepressed);
        //Debug.Log("Input enabled: " + PlayerOptions.QteInputEnabled);

        if (Input.GetKeyDown(keytopress))
        {
            if (canbepressed && buttoncanbepressed)

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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Activator"))
        {
            canbepressed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.StartsWith("Activator"))
        {
            canbepressed = false;
        }
    }

    private void QteMissEvent(QteMissEvent qtemiss)
    {
        if (this.gameObject.tag == "ArrowYellow" && qtemiss.color == 2)
        {
            buttoncanbepressed = qtemiss.enableinput;
        }
        else if (this.gameObject.tag == "ArrowRed" && qtemiss.color == 1)
        {
            buttoncanbepressed = qtemiss.enableinput;
        }
        else if (this.gameObject.tag == "ArrowGreen" && qtemiss.color == 3)
        {
            buttoncanbepressed = qtemiss.enableinput;
        }
        else if (this.gameObject.tag == "ArrowBlue" && qtemiss.color == 0)
        {
            buttoncanbepressed = qtemiss.enableinput;
        }
    }

    IEnumerator WaitEnableInput()
    {
        yield return waitenableinput;
        PlayerOptions.QteInputEnabled = true;

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

        ev_qtemiss.enableinput = PlayerOptions.QteInputEnabled;
        EventController.TriggerEvent(ev_qtemiss);
    }

}
