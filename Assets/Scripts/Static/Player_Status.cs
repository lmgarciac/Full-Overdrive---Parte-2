using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Player_Status
{
    //Player collectables
    private static int collectables;
    private static int picks;
    private static int heals;
    private static int buffs;
    private static int money;

    //Player quests status

    private static List<Quests> questlist = new List<Quests>();

    //Player Inventory

    public static List<Item> itemlist = new List<Item>();

    //Player Stats
    private static int attackStat;
    private static int defenseStat;
    private static int maxHPStat;
    private static int maxSPStat;
    private static int currentarea; //Area 1, Area 2 or Area 3

    public static int AttackStat
    {
        get
        {
            return attackStat;
        }
        set
        {
            attackStat = value;
        }
    }

    public static int DefenseStat
    {
        get
        {
            return defenseStat;
        }
        set
        {
            defenseStat = value;
        }
    }
    public static int MaxHPStat
    {
        get
        {
            return maxHPStat;
        }
        set
        {
            maxHPStat = value;
        }
    }
    public static int MaxSPStat
    {
        get
        {
            return maxSPStat;
        }
        set
        {
            maxSPStat = value;
        }
    }

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

    public static List<Quests> QuestList
    {
        get
        {
            return questlist;
        }
        set
        {
            questlist = value;
        }
    }

    public static List<Item> ItemList
    {
        get
        {
            return itemlist;
        }
        set
        {
            itemlist = value;
        }
    }

    public static Quests FindQuest (string questname)
    {
        for (int i = 0; i < questlist.Count; ++i)
        {
            var quest = questlist.ElementAt(i);
            if (quest.questname == questname)
            {
                return quest;
            }
        }
        return null;
    }

    public static int FindQuestIndex(string questname)
    {
        for (int i = 0; i < questlist.Count; ++i)
        {
            var quest = questlist.ElementAt(i);
            if (quest.questname == questname)
            {
                return i;
            }
        }
        return 999;
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

    public static int CurrentArea
    {
        get
        {
            return currentarea;
        }
        set
        {
            currentarea = value;
        }
    }
}

public class Quests
{
    public string questname;
    public int queststatus; //0: Not started
                            //1: Started
                            //2: Accomplished
                            //3: Finished
    public int genericnumber;

    public Quests(string questName, int questStatus, int genericNumber)
    {
        questname = questName;
        queststatus = questStatus;
        genericnumber = genericNumber; //Used for anything the quest needs to keep track on
    }
}
