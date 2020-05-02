using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class SessionData
{

    private static GameData GAME_DATA;

    public static bool EraseData()
    {
        const bool valid = false;

        try
        {
            PlayerPrefs.SetString("data", "");
            PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }

        return valid;
    }

    public static bool LoadData()
    {
        var valid = false;

        var data = PlayerPrefs.GetString("data", "");
        if (data != "")
        {
            var success = DESEncryption.TryDecrypt(data, out var original);
            if (success)
            {
                GAME_DATA = JsonUtility.FromJson<GameData>(original);
                GAME_DATA.LoadData();
                valid = true;
            }
            else
            {
                GAME_DATA = new GameData();
            }

        }
        else
        {
            GAME_DATA = new GameData();
        }

        return valid;
    }

    public static bool SaveData()
    {
        const bool valid = false;

        try
        {
            GAME_DATA.SaveData();
            var result = DESEncryption.Encrypt(JsonUtility.ToJson(SessionData.GAME_DATA));
            PlayerPrefs.SetString("data", result);
            PlayerPrefs.Save();
            Debug.Log("DataSaved");
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }

        return valid;
    }

    public static GameData Data
    {
        get
        {
            if (GAME_DATA == null)
                LoadData();
            return GAME_DATA;
        }
    }

}


[Serializable]
public class GameData
{
    //Put attributes that you want to save during your game.
    //public int currentCharacterLevel = 0;
    //public int[] abilitiesLevel = { 0, 0 , 0 };

    //Map Status
    public bool firstTime = true; //Indicates first time entering map
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;

    public Vector3 miloPosition;
    public Quaternion miloRotation;

    public int[] collectablesIdentifiers; //Collectables active

    //Player Status
    //Collectables
    public int collectables;
    public int picks;
    public int heals;
    public int buffs;
    public int money;

    //Stats
    public int attackStat;
    public int defenseStat;
    public int maxHPStat;
    public int maxSPStat;
    public int currentarea; //Area 1, Area 2 or Area 3 (Virtuosity)

    //Inventory
    public List<Item> itemlist = new List<Item>();
    

    //Game Options
    public int difficulty;
    public float volume;

    public GameData()
    {
        //currentCharacterLevel = -1;
        //abilitiesLevel[0] = -1;
        //abilitiesLevel[1] = -1;
    }

    public void SaveData()
    {

        //Map Status
        firstTime = Map_Status.FirstTime;

        playerPosition = Map_Status.PlayerPosition;
        playerRotation = Map_Status.PlayerRotation;
        cameraPosition = Map_Status.CameraPosition;
        cameraRotation = Map_Status.CameraRotation;

        miloPosition = Map_Status.MiloPosition;
        miloRotation = Map_Status.MiloRotation;


        collectablesIdentifiers = Map_Status.CollectablesIdentifiers;

        Debug.Log("Player rotation: " + playerRotation);
        Debug.Log("Player position: " + playerPosition);

        Debug.Log("Milo position: " + miloPosition);

        //PlayerStatus
        //Collectables
        collectables = Player_Status.Collectables;
        picks = Player_Status.Picks;
        heals = Player_Status.Heals;
        buffs = Player_Status.Buffs;
        money = Player_Status.Money;

        //Inventario

        itemlist = Player_Status.ItemList;

        //Stats

        attackStat = Player_Status.AttackStat;
        defenseStat = Player_Status.DefenseStat;
        maxHPStat = Player_Status.MaxHPStat;
        maxSPStat = Player_Status.MaxSPStat;
        currentarea = Player_Status.CurrentArea;

        //Game Options
        difficulty = PlayerOptions.Difficulty;
        volume = PlayerOptions.Volume;
        //PlayerOptions.NewGame = false; esta variable no se controla aca asi que no guardar
    }

    public void LoadData()
    {

        //Map Status
        Map_Status.FirstTime = firstTime;
        Map_Status.PlayerPosition = playerPosition;
        Map_Status.PlayerRotation = playerRotation;
        Map_Status.CameraPosition = cameraPosition;
        Map_Status.CameraRotation = cameraRotation;

        Map_Status.MiloPosition = miloPosition;
        Map_Status.MiloRotation = miloRotation;

        Map_Status.CollectablesIdentifiers = collectablesIdentifiers;

        //PlayerStatus
        Player_Status.Collectables = collectables;
        Player_Status.Picks = picks;
        Player_Status.Heals = heals;
        Player_Status.Buffs = buffs;
        Player_Status.Money = money;

        //Inventario

        Player_Status.ItemList = itemlist;

        //Stats

        Player_Status.AttackStat = attackStat;
        Player_Status.DefenseStat = defenseStat;
        Player_Status.MaxHPStat = maxHPStat;
        Player_Status.MaxSPStat = maxSPStat;
        Player_Status.CurrentArea = currentarea;

        //Game Options
        PlayerOptions.Difficulty = difficulty;
        PlayerOptions.Volume = volume;
        //PlayerOptions.NewGame = false; esta variable no se controla aca asi que no guardar

        Debug.Log("Data Loaded");

        Debug.Log("First Time: " + firstTime);
        Debug.Log("Heals: " + heals);
    }
}