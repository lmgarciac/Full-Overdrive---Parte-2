using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item item;
    public Image icon;
    public Button removeButton;
    public Image equipedImage;
    
    public void AddItem(Item newItem)
    {
        item = newItem;
        //Debug.Log("Paso por aca el: " + item.name);
        icon.sprite = item.icon;
        icon.enabled = true;
        icon.preserveAspect = true;
        //removeButton.interactable = true;

        if (item.equiped == true)
        {
            equipedImage.enabled = true;
        }
        else
        {
            equipedImage.enabled = false;
        }

    }
    
    
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();            
        }
            
    }
    

}
