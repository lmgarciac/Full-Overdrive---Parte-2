using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparency : MonoBehaviour
{
    public float fadeSpeed = 1f;
    public float targetAlpha;

    private Color color;

    public Camera gameCamera;
    private RaycastHit hit;
    private bool isBlocking;
    private Ray camRaycast;
    private GameObject objectHit;
    private GameObject objectFaded;
    private Vector3 screenPos;

    void Start()
    {
    }

    void Update()
    {
        screenPos = gameCamera.WorldToScreenPoint(this.transform.position);
        camRaycast = gameCamera.ScreenPointToRay(screenPos);

        Debug.DrawRay(camRaycast.origin, camRaycast.direction * 100, Color.yellow);

        if (Physics.Raycast(camRaycast, out hit))
        {
            objectHit = hit.transform.gameObject;

            if (objectHit.tag != "Player" && objectHit.tag != "Floor")
            {
                objectFaded = objectHit;
                color = objectFaded.GetComponent<MeshRenderer>().material.color;
                Debug.Log("Hit!");

                if (color.a >= targetAlpha)
                {
                    color.a -= Time.deltaTime * fadeSpeed;
                }
                objectFaded.GetComponent<MeshRenderer>().material.color = color;
            }
            if (objectHit.tag == "Player" && objectFaded != null)
            {
                Debug.Log("Player!");
                if (color.a < 1)
                {
                    color.a += Time.deltaTime * fadeSpeed;
                }
                objectFaded.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }
}
