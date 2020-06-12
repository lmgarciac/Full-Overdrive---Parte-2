using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Scene_Controller : MonoBehaviour
{
    private SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneController.FadeAndLoadScene("Start_Menu");
        }
    }
}
