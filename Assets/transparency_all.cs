using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class transparency_all : MonoBehaviour
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
    Dictionary<int, GameObject> objectsFaded;
    Dictionary<int, GameObject> objectsHit;
    private Vector3 cam_player;

    private Vector3 screenPos;

    public float movSpeed;
    private bool verify;

    void Start()
    {
        objectsHit = new Dictionary<int, GameObject>();
        cam_player = gameCamera.transform.position - this.transform.position;

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

        RaycastHit[] hits;
        hits = Physics.RaycastAll(camRaycast);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "Floor")
            {
                // Si es nuevo, lo agrego al listado.
                if (!objectsHit.ContainsKey(hit.transform.gameObject.GetInstanceID()))
                {
                    objectsHit.Add(hit.transform.gameObject.GetInstanceID(), hit.transform.gameObject);
                }

                objectFaded = hit.transform.gameObject;
                color = objectFaded.GetComponent<MeshRenderer>().material.color;
                if (color.a >= targetAlpha)
                {
                    color.a -= Time.deltaTime * fadeSpeed;
                }
                objectFaded.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        //Si el objeto había estado bloqueando pero ya no está mas, le voy sacando alpha hasta que lo quito.
        for (int j = 0; j < objectsHit.Count; j++)
        {
            verify = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (objectsHit.ElementAt(j).Key == hits[i].transform.gameObject.GetInstanceID())
                {
                    verify = true; //Sigue estando, no hacer nada
                }
            }

            if (verify == false) //No está mas, devolver alpha y remover
            {
                //Devolver el Alpha
                color = objectsHit.ElementAt(j).Value.GetComponent<MeshRenderer>().material.color;

                if (color.a < 1.0f)
                {
                    color.a += Time.deltaTime * fadeSpeed;
                }
                objectsHit.ElementAt(j).Value.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        //Si ya volvió a alpha 1, quitarlo.
        for (int j = 0; j < objectsHit.Count; j++)
        {
            color = objectsHit.ElementAt(j).Value.GetComponent<MeshRenderer>().material.color;
            if (color.a >= 1.0f)
            {
                objectsHit.Remove(objectsHit.ElementAt(j).Key);
            }
        }
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Quaternion.AngleAxis(-45, Vector3.up) * direction;
        direction = direction.normalized * movSpeed;
        this.transform.Translate(direction * Time.deltaTime);

        gameCamera.transform.position = this.transform.position + cam_player;
    }

}
