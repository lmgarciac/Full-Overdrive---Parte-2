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
    private static int currentarea; //Area 1, Area 2 or Area 3

    //Player quests status

    private static List<Quests> questlist = new List<Quests>();

    //public stat Player_Status()
    //{
    //    questlist = new List<Quests>();
    //}


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
