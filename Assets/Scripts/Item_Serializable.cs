using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item_Serializable
{
    new public string name = "New Item";
    //public Sprite icon = null;
    public bool isDefaultItem = false;
    public int type; //0: Static
                     //1: Equipable
                     //2: Interactable
                     //3: Consumable

    public int bonusAttack;
    public int bonusDefense;
    public int bonusMaxHP;
    public int bonusMaxSP;

    public bool equiped;

    //public GameObject rotatingItem;

}
