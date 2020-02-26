using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Arrow_Controller : MonoBehaviour
{
    public float beatTempo;

    void Start()
    {
        beatTempo = beatTempo / 60f;  
    }

    void Update()
    {
        transform.localPosition -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
    }
}
