using UnityEngine;
using System;

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
    public int[] collectablesIdentifiers; //Collectables active

    //Player Status
    public int collectables;
    public int picks;
    public int heals;
    public int buffs;
    public int money;
    public int currentarea; //Area 1, Area 2 or Area 3 (Virtuosity)

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
        collectablesIdentifiers = Map_Status.CollectablesIdentifiers;

        Debug.Log("Player rotation: " + playerRotation);
        Debug.Log("Player position: " + playerPosition);

        //PlayerStats
        collectables = Player_Status.Collectables;
        picks = Player_Status.Picks;
        heals = Player_Status.Heals;
        buffs = Player_Status.Buffs;
        money = Player_Status.Money;
        currentarea = Player_Status.CurrentArea;

        //Game Options
        difficulty = PlayerOptions.Difficulty;
        volume = PlayerOptions.Volume;
    }

    public void LoadData()
    {

        //Map Status
        Map_Status.FirstTime = firstTime;
        Map_Status.PlayerPosition = playerPosition;
        Map_Status.PlayerRotation = playerRotation;

        Debug.Log("Player rotation: " + playerRotation);
        Debug.Log("Player position: " + playerPosition);

        Map_Status.CameraPosition = cameraPosition;
        Map_Status.CameraRotation = cameraRotation;
        Map_Status.CollectablesIdentifiers = collectablesIdentifiers;

        //PlayerStats
        Player_Status.Collectables = collectables;
        Player_Status.Picks = picks;
        Player_Status.Heals = heals;
        Player_Status.Buffs = buffs;
        Player_Status.Money = money;
        Player_Status.CurrentArea = currentarea;

        //Game Options
        PlayerOptions.Difficulty = difficulty;
        PlayerOptions.Volume = volume;

        Debug.Log("Data Loaded");

        Debug.Log("First Time: " + firstTime);
        Debug.Log("Heals: " + heals);
    }
}