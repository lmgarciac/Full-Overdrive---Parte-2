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

    public float movSpeed;

    void Start()
    {
    }

    void Update()
    {
        WorldTransparency();
        PlayerMovement();
    }

    private void WorldTransparency()
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

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);

        direction = Quaternion.AngleAxis(-45, Vector3.up) * direction;
        this.transform.Translate(direction.normalized * Time.deltaTime * movSpeed);
        gameCamera.transform.Translate(direction.normalized * Time.deltaTime * movSpeed);
    }

}
