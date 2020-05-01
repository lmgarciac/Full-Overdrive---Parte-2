using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
