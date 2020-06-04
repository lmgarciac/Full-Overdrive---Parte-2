using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    public GameObject SeñalVerde;
    public GameObject SeñalAmarilla;
    public GameObject SeñalRojo;
    public Light Luz;

    // Start is called before the first frame update
    void Start()
    {
        CerrarSeñal();
    }

    public void CerrarSeñal()
    {
        SeñalVerde.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        SeñalAmarilla.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        SeñalRojo.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        Luz.color = Color.red;
    }

    public void AlertaSeñal()
    {
        SeñalVerde.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        SeñalAmarilla.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        SeñalRojo.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        Luz.color = Color.yellow;
    }

    public void AbrirSeñal()
    {
        SeñalVerde.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        SeñalAmarilla.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        SeñalRojo.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        Luz.color = Color.green;
    }

}
