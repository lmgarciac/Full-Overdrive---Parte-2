using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
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

    public GameObject rotatingItem;

    private readonly UseItemEvent ev_useitem = new UseItemEvent();


    public virtual void Use()
    {
        if (this.type == 1) //Equipables
        {
            Inventory.instance.Equip(this);
        }

        if (this.type == 2) //Interactables
        {
            //Lo escucha PlayerController aunque podría esta tranquilamente aca el codigo
            ev_useitem.item = this;
            EventController.TriggerEvent(ev_useitem);
        }

        if (this.type == 3) //Consumables
        {
            Inventory.instance.Consume(this);
            Inventory.instance.Remove(this);
        }
    }

    public void RemoveFromInventory()
    {
        this.equiped = false;
        Inventory.instance.Remove(this);
    }
}
