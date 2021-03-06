﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update
    bool active;
    Canvas canvas;
    private SceneController sceneController;


    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            active = !active;
            canvas.enabled = active;
            Time.timeScale = (active) ? 0 : 1f;
        }
        
        if (Input.GetKeyDown("escape"))
        {
            //Application.LoadLevel("Menu");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
            sceneController.FadeAndLoadScene("_Test_Navigation");
        }
    }
}
