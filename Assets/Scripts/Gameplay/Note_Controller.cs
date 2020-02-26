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
                EventController.TriggerEvent(ev_qtehit);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Activator")
        {
            canbepressed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Activator")
        {
            canbepressed = false;
        }
    }

}
