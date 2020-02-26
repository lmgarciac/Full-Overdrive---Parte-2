using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 8f, 
                                            Camera.main.gameObject.transform.position.y + 4.5f,
                                            0f);
    }
}
