using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farming_Area_Controller : MonoBehaviour
{
    private SceneController sceneController;
    private bool canenterfarming;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canenterfarming && PlayerOptions.InputEnabled)
        {
            sceneController.FadeAndLoadScene("_Test_Farming");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            canenterfarming = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canenterfarming = false;
        }
    }
}
