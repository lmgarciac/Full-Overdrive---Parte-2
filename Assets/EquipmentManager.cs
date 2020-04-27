using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
   #region Singleton
   public static EquipmentManager instance;

   private void Awake()
   {
      instance = this;
   }
   #endregion

   public Equipment[] currentEquipment; //Array con el equipamiento

   public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

   public OnEquipmentChanged onEquipmentChanged;
   
    Inventory inventory;
   
   void Start()
   {
      inventory = Inventory.instance;
      int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
      
      Debug.Log(System.Enum.GetNames(typeof(EquipmentSlot)).Length);
      //Sirve para saber la cantidad de intems q tiene el enum en el Scriptable object
      currentEquipment = new Equipment[numSlots];
   }

   public void Equip(Equipment newItem)
   {
      int slotIndex = (int)newItem.equipSlot;
      Equipment olditem = null;

      if (currentEquipment[slotIndex] != null) //si ya hay algo en el slot del inventario
      {
         olditem = currentEquipment[slotIndex];
         inventory.Add(olditem);
      }

      if (onEquipmentChanged != null)
      {
         onEquipmentChanged.Invoke(newItem, olditem); 
      }
      
      currentEquipment[slotIndex] = newItem;
   }

   public void Unequip(int slotIndex)
   {
      if (currentEquipment[slotIndex]!=null)
      {
         Equipment olditem = currentEquipment[slotIndex];
         inventory.Add(olditem);
         currentEquipment[slotIndex] = null;
         if (onEquipmentChanged != null)
         {
            onEquipmentChanged.Invoke(null, olditem); 
         }
      }
   }
   
   public void UnequipAll()
   {
      for (int i = 0; i < currentEquipment.Length; i++)
      {
         Unequip(i);
      }
   }

   void Update()
   {
      if (Input.GetKeyDown(KeyCode.U))
      {
         UnequipAll();
      }
   }
}
