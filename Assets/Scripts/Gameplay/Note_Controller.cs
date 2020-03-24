using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Note_Controller : MonoBehaviour
{
    private readonly QteHitEvent ev_qtehit = new QteHitEvent();  
    private readonly QtePlayEvent ev_qteplay = new QtePlayEvent();  

    public bool canbepressed;
    public KeyCode keytopress;

    void Start()
    {
        canbepressed = false;     
    }

    void Update()
    {
        if (Input.GetKeyDown(keytopress))
        {
            if(canbepressed)
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

}
