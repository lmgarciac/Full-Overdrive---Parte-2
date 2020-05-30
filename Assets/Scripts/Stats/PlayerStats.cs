using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            defense.AddModifier(newItem.defModifier);
            attack.AddModifier(newItem.dmgModifier);
        }

        if (oldItem != null)
        {
            defense.AddModifier(oldItem.defModifier);
            attack.AddModifier(oldItem.dmgModifier);
        }
    }
}
