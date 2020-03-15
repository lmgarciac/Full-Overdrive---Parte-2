using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bar_Controller : MonoBehaviour
{
    private bool canenterbar = false;
    private SceneController sceneController;
    private int[] collectablesIdentifiers;
    private GameObject[] activeCollectables;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canenterbar)
        {
            //SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
            //SaveMapState();
            sceneController.FadeAndLoadScene("Battle");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            canenterbar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canenterbar = false;
        }
    }

    private void SaveMapState()
    {
        //////Collectables
        //activeCollectables = GameObject.FindGameObjectsWithTag("Collectable");
        //collectablesIdentifiers = new int[activeCollectables.Length];

        //int i = 0;
        //foreach (GameObject activeCollectable in activeCollectables)
        //{
        //    collectablesIdentifiers[i] = activeCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier;
        //    i++;
        //}
        //Map_Status.CollectablesIdentifiers = collectablesIdentifiers;

        //////Player position
        //Map_Status.PlayerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;
        //Map_Status.PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        //////Camera position
        //Map_Status.CameraRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        //Map_Status.CameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        //Map_Status.FirstTime = false;
    }

}
