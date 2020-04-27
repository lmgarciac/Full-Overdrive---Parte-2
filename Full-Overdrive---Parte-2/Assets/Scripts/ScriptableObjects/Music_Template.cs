using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Music_Configuration", menuName = "Music_Configuration")]
public class Music_Template : ScriptableObject
{
    public Canciones[] listaCanciones;
}

[System.Serializable]
public class Canciones
{
    public string sceneName = "Nombre de la escena";
    public AudioClip cancion;
    public float volume;
}
