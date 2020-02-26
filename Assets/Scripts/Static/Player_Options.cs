using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerOptions
{
    private static int difficulty;
    private static float volume;

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

}
