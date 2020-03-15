﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player_Status
{
    private static int collectables;
    private static int picks;
    private static int heals;
    private static int buffs;
    private static int money;

    public static int Collectables
    {
        get
        {
            return collectables;
        }
        set
        {
            collectables = value;
        }
    }

    public static int Picks
    {
        get
        {
            return picks;
        }
        set
        {
            picks = value;
        }
    }

    public static int Heals
    {
        get
        {
            return heals;
        }
        set
        {
            heals = value;
        }
    }

    public static int Buffs
    {
        get
        {
            return buffs;
        }
        set
        {
            buffs = value;
        }
    }

    public static int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
        }
    }
}
