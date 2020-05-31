using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    // Start is called before the first frame update
    public Item item;
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking up an item");
        //ADD to Inventory
        //FindObjectOfType<Inventory>().Add();
        bool wasPickedUp = Inventory.instance.Add(item,true);
        if (wasPickedUp)
        {
            Destroy(gameObject);    
        }
        
    }
}
