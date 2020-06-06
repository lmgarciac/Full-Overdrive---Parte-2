using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Collectable_Controller : MonoBehaviour
{

    public int uniqueIdentifier;
    public GameObject effects;

    private void OnEnable()
    {
        EventController.AddListener<CollectEvent>(CollectEvent);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<CollectEvent>(CollectEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DisableItem());
        }
    }

    private void CollectEvent(CollectEvent collect)
    {
        //StartCoroutine(DisableItem());
    }

    private IEnumerator DisableItem()
    {
        effects.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
    }

}


