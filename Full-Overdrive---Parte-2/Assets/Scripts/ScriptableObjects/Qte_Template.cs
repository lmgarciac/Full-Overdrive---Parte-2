using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "QTE_Template_xxx_n", menuName = "Qte Template")]
public class Qte_Template : ScriptableObject
{

    public string qteDescription = "Descripcion del QTE";
    public int speed = 5;
    public float arrowsHeight = 10f;
    public Vector3 QTEPosition = new Vector3(-30f, 8f, -9f);
    public Quaternion QTERotation = new Quaternion(-90f, 180, 0, 0);
    public DatosNotas[] noteData;
}

[System.Serializable]
public class DatosNotas
{
    public float notePosition;
    public float noteDelay;
}


