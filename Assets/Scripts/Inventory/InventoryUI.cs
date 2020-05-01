using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventoryUI : MonoBehaviour
{
    public Transform itemsParents;    //Le indica quien es el padre de los InventorySlots
    public GameObject inventoryUI;

    [SerializeField] private TextMeshProUGUI tx_attackStat;
    [SerializeField] private TextMeshProUGUI tx_defenseStat;
    [SerializeField] private TextMeshProUGUI tx_maxHPStat;
    [SerializeField] private TextMeshProUGUI tx_maxSPStat;
    [SerializeField] private TextMeshProUGUI tx_virtuosity;
    [SerializeField] private TextMeshProUGUI tx_redpills;
    [SerializeField] private TextMeshProUGUI tx_bluepills;


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
            bool open = inventoryUI.GetComponent<Animator>().GetBool("Open");

            if (!open && PlayerOptions.InputEnabled)
            {
                inventoryUI.GetComponent<Animator>().SetBool("Open", !open);
                PlayerOptions.InputEnabled = false;
            }

            if (open)
            {
                inventoryUI.GetComponent<Animator>().SetBool("Open", !open);
                PlayerOptions.InputEnabled = true;
            }

            UpdateUI();
        }
    }
    
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Items.Count)
            {
                slots[i].AddItem(inventory.Items[i]); //Llamo al script dentro de cada Slot parado en i y agrego el item de la variable inventory
                Debug.Log("Paso por el for");
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        //Update Stats
        tx_attackStat.text = Player_Status.AttackStat.ToString();
        tx_defenseStat.text = Player_Status.DefenseStat.ToString();
        tx_maxHPStat.text = Player_Status.MaxHPStat.ToString();
        tx_maxSPStat.text = Player_Status.MaxSPStat.ToString();
        tx_virtuosity.text = Player_Status.CurrentArea.ToString();

        tx_redpills.text = Player_Status.Heals.ToString();
        tx_bluepills.text = Player_Status.Buffs.ToString();


    }
}
