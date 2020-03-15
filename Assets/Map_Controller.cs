using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Map_Controller : MonoBehaviour
{
    private GameObject[] currentCollectables;
    private bool collectableActive;
    private GameObject[] activeCollectables;
    private int[] collectablesIdentifiers;
    private Collectable_Controller collectableController;

    private void OnEnable()
    {
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
    }


    // Start is called before the first frame update
    void Start()
    {

        ////Player position
        //GameObject.FindGameObjectWithTag("Player").transform.position = Map_Status.PlayerPosition;
        //GameObject.FindGameObjectWithTag("Player").transform.rotation = Map_Status.PlayerRotation;

        ////Camera position
        //GameObject.FindGameObjectWithTag("MainCamera").transform.position = Map_Status.CameraPosition;
        // GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = Map_Status.CameraRotation;


    }

    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {

        ////Collectables
        activeCollectables = GameObject.FindGameObjectsWithTag("Collectable");
        collectablesIdentifiers = new int[activeCollectables.Length];

        int i = 0;
        foreach (GameObject activeCollectable in activeCollectables)
        {
            collectablesIdentifiers[i] = activeCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier;
            i++;
        }
        Map_Status.CollectablesIdentifiers = collectablesIdentifiers;

        Map_Status.FirstTime = false;

    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        if (Map_Status.FirstTime)
        {
            ////Collectables
            activeCollectables = GameObject.FindGameObjectsWithTag("Collectable");
            collectablesIdentifiers = new int[activeCollectables.Length];
            int i = 0;
            foreach (GameObject activeCollectable in activeCollectables)
            {
                Debug.Log(activeCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier);
                collectableController = activeCollectable.GetComponent<Collectable_Controller>();
                collectablesIdentifiers[i] = collectableController.uniqueIdentifier;
                i++;
            }

            Map_Status.CollectablesIdentifiers = collectablesIdentifiers;
        }
        else
        {
            ////Collectables
            currentCollectables = GameObject.FindGameObjectsWithTag("Collectable");

            foreach (GameObject currentCollectable in currentCollectables)
            {
                collectableActive = false;
                foreach (int currentCollectableId in Map_Status.CollectablesIdentifiers)
                {
                    if (currentCollectableId == currentCollectable.GetComponent<Collectable_Controller>().uniqueIdentifier)
                    {
                        collectableActive = true;
                    }
                }
                if (collectableActive == false)
                {
                    currentCollectable.SetActive(false);
                }
            }
        }
    }
}
