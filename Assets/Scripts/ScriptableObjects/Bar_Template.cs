using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bar_n", menuName = "Bar")]
public class Bar_Template : ScriptableObject
{
    public int barIdentifier;
    public string prefabModelName;
    public bool finished;
    public Enemy_Template[] enemies;
}

[System.Serializable]
public class Bar_Serializable
{
    public int barIdentifier;
    public string barName;
    public bool finished;

    public Bar_Serializable(int barIdent, string barNam, bool finish)
    {
        barIdentifier = barIdent;
        barName = barNam;
        finished = finish;
    }
}
