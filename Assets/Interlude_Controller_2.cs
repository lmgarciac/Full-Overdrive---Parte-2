using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interlude_Controller_2 : MonoBehaviour
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
        
    }

    public void SwitchScene()
    {
        sceneController.FadeAndLoadScene("_Scene_Out");
    }
}
