using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParents;    //Le indica quien es el padre de los InventorySlots
    public GameObject inventoryUI;
    
    Inventory inventory;

    
    private InventorySlot[] slots; //Declara el array de Slots
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; //despues de hacer el OnItemChanged en el Script del Inventario llama a UpdateUI

        slots = itemsParents.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        //Debug.Log("UPDATING UI INVENTORY");
    }
    
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Items.Count)
            {
                slots[i].AddItem(inventory.Items[i]); //Llamo al script dentro de cada Slot parado en i y agrego el item de la variable inventory
                Debug.Log(slots[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        
    }
    
    
}
