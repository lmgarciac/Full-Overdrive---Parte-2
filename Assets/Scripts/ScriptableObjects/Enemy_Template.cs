using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_n", menuName = "Enemies")]
public class Enemy_Template : ScriptableObject
{
    public string prefabName;
    public int enemyAttack;
    public int enemyDefense;
    public int enemyinitHP;
    public int enemyinitSP;
    public int enemyMaxHP;
    public int enemyMaxSP;
    public int enemyHeals;
    public int enemyBuffs;
    public bool beaten;
}
