using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    
    public int defModifier;
    public int dmgModifier;

    public override void Use()
    {
        base.Use();
        //Equip the Item
        EquipmentManager.instance.Equip(this);
        //Remove from Inventory
        RemoveFromInventory();
        
    }
    
}

public enum EquipmentSlot{Headphone,Amplificator,Microphone}