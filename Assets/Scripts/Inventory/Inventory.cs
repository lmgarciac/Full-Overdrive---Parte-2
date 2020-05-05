using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Inventory : MonoBehaviour
{
    #region Singleton
    private readonly ObtainItemEvent ev_obtainitem = new ObtainItemEvent();

    public static Inventory instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory Found");
            return;
        }
        instance = this;

        Return_Items();
    }

    private void OnEnable()
    {
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
    }

    #endregion

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallback;


    public int space = 20;
    public List<Item> Items = new List<Item>();

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (Items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }
            item.equiped = false;
            Items.Add(item);

            //Evento para que escuche quien lo necesite
            ev_obtainitem.item = item;
            EventController.TriggerEvent(ev_obtainitem);
            Debug.Log(item.name + " obtained");

            //Llamo al delegate
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void Consume(Item item)
    {
        if (item.name == "UNKNOWN DRINK")
        {
            Player_Status.MaxHPStat += item.bonusMaxHP;
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void Equip(Item item)
    {
        if (item.equiped == true)
        {
            item.equiped = false;
            if (item.name == "AMP OF BATTLE")
            {
                Player_Status.AttackStat -= item.bonusAttack;
            }
            if (item.name == "BATTLE HEADPHONES")
            {
                Player_Status.DefenseStat -= item.bonusDefense;
            }
        }
        else
        {
            item.equiped = true;

            if (item.name == "AMP OF BATTLE")
            {
                Player_Status.AttackStat += item.bonusAttack;
            }
            if (item.name == "BATTLE HEADPHONES")
            {
                Player_Status.DefenseStat += item.bonusDefense;
            }
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent scene)
    {
        foreach (var item in Items)
        {
            Debug.Log("Antes Item: " + item.name);
        }
        Player_Status.ItemList = Items;
    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent scene)
    {
        Items = Player_Status.ItemList;

        //foreach (var item in Items)
        //{
        //    Debug.Log("Despues Item: " + item.name);
        //}

        if (onItemChangedCallback != null)
        {
            Debug.Log("Callback OK!" + onItemChangedCallback);

            onItemChangedCallback.Invoke();
        }
    }


    public void Return_Items()
    {
        Inventory.instance.Items.Clear();

        Debug.Log("Items Static: " + Player_Status.Items_Serializable);

        if (Player_Status.Items_Serializable != null)
        {

            foreach (Item_Serializable item_Serializable in Player_Status.Items_Serializable)
            {
                var item = new Item();
                //(AudioClip)Resources.Load<AudioClip>($"Music/so_battle_theme");
                item = (Item)Resources.Load<Item>($"Items/{item_Serializable.name}");
                Debug.Log("Item: " + item.name);
                Items.Add(item);
            }
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }


}
