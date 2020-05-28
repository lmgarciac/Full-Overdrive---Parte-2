using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Map_Status
{
    private static bool firstTime = true; //Indicates first time entering Scene

    private static Vector3 playerPosition;
    private static Quaternion playerRotation;

    private static Vector3 miloPosition;
    private static Quaternion miloRotation;

    private static Vector3 cameraPosition;
    private static Quaternion cameraRotation;

    private static int[] collectablesIdentifiers;

    private static List<Bar_Serializable> finishedBars = new List<Bar_Serializable>();

    public static List<Bar_Serializable> FinishedBars
    {
        get
        {
            return finishedBars;
        }
        set
        {
            finishedBars = value;
        }
    }

    public static Bar_Serializable FindBar(int barIdent)
    {
        for (int i = 0; i < finishedBars.Count; ++i)
        {
            var bar = finishedBars.ElementAt(i);
            if (bar.barIdentifier == barIdent)
            {
                return bar;
            }
        }
        return null;
    }

    public static int FindBarIndex(int barIdent)
    {
        for (int i = 0; i < finishedBars.Count; ++i)
        {
            var bar = finishedBars.ElementAt(i);
            if (bar.barIdentifier == barIdent)
            {
                return i;
            }
        }
        return 999;
    }

    public static Vector3 PlayerPosition
    {
        get
        {
            return playerPosition;
        }
        set
        {
            playerPosition = value;
        }
    }

    public static Quaternion PlayerRotation
    {
        get
        {
            return playerRotation;
        }
        set
        {
            playerRotation = value;
        }
    }

    public static Vector3 MiloPosition
    {
        get
        {
            return miloPosition;
        }
        set
        {
            miloPosition = value;
        }
    }

    public static Quaternion MiloRotation
    {
        get
        {
            return miloRotation;
        }
        set
        {
            miloRotation = value;
        }
    }

    public static Vector3 CameraPosition
    {
        get
        {
            return cameraPosition;
        }
        set
        {
            cameraPosition = value;
        }
    }

    public static Quaternion CameraRotation
    {
        get
        {
            return cameraRotation;
        }
        set
        {
            cameraRotation = value;
        }
    }

    public static int[] CollectablesIdentifiers
    {
        get
        {
            return collectablesIdentifiers;
        }
        set
        {
            collectablesIdentifiers = value;
        }
    }

    public static bool FirstTime
    {
        get
        {
            return firstTime;
        }
        set
        {
            firstTime = value;
        }
    }

}
