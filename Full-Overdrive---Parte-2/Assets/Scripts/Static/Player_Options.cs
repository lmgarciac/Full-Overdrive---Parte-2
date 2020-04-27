using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerOptions
{
    private static int difficulty;
    private static float volume;
    private static bool newGame;

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
