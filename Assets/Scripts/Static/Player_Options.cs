using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerOptions
{
    private static int difficulty;
    private static float volume;
    private static bool newGame;

    private static bool inputEnabled;

    private static bool qteInputEnabled; //For QTE usage

    public static bool InputEnabled
    {
        get
        {
            return inputEnabled;
        }
        set
        {
            inputEnabled = value;
        }
    }

    public static bool QteInputEnabled
    {
        get
        {
            return qteInputEnabled;
        }
        set
        {
            qteInputEnabled = value;
        }
    }


    public static int Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }

    public static float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = value;
        }
    }

    public static bool NewGame
    {
        get
        {
            return newGame;
        }
        set
        {
            newGame = value;
        }
    }
}
