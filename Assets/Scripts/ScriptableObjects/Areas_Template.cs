using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Areas", menuName = "Areas")]
public class Areas_Template : ScriptableObject
{
    public List<Area_Class> Areas = new List<Area_Class>();
}

[System.Serializable]
public class Area_Class
{
    public int targetPicks;
    public int targetCollectables;

    //public int enemyAttack;
    //public int enemyDefense;
    //public int enemyinitHP;
    //public int enemyinitSP;
    //public int enemyMaxHP;
    //public int enemyMaxSP;
    //public int enemyHeals;
    //public int enemyBuffs;
}
