using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Difficulty_Template_n", menuName = "Difficulty Template")]

public class Difficulty_Template : ScriptableObject
{
    public string qteDescription = "Descripción Dificultad";
    public float damageMultiplier = 1;
    public float defenseMultiplier = 1;
    public float hpMultiplier = 1;
    public float spMultiplier = 1;
    public float qteSpeedMultiplier = 1;
    public int additionalHeal = 0;
    public int additionalBuff = 0;
}
