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
        //Aca tendria q haber algun evento al cual le pueda pasar el item q Equipé y estoy sacando del inventario para dibujarlo en el boton
        RemoveFromInventory();
        
    }
    
}

public enum EquipmentSlot
{
    Headphone,
    Amplificator
}