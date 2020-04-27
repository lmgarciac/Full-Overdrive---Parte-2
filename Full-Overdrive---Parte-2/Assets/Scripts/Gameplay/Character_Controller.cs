using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : FiniteStateMachine
{
    Quaternion ogRotation;

    // Start is called before the first frame update
    void Awake()
    {
        ogRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = ogRotation;
        base.Update();
    }
}
