using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClass
{    
    public static string characterSelected = "";

    public static GameObject heroPrefab;

    public static GameObject dragonPrefab;

    public static void setCharacterRotation (Quaternion newAngle)
    {
        switch (characterSelected)
        {
            case "Hero":
                heroPrefab.transform.localRotation = newAngle;
                break;
            case "Dragon":
                dragonPrefab.transform.localRotation = newAngle;
                break;
            default:
                break;
        }
    }

    public static Quaternion getCharacterRotation ()
    {
        switch (characterSelected)
        {
            case "Hero":
                return heroPrefab.transform.localRotation;
            case "Dragon":
                return dragonPrefab.transform.localRotation;
            default:
                return Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    }

    public static void setCharacterScale (float value)
    {
        switch (characterSelected)
        {
            case "Hero":
                heroPrefab.transform.localScale = Vector3.one * value;
                break;
            case "Dragon":
                dragonPrefab.transform.localScale = Vector3.one * value;
                break;
            default:
                break;
        }
    }

    public static float getCharacterScale ()
    {
        switch (characterSelected)
        {
            case "Hero":
                return heroPrefab.transform.localScale.x;
            case "Dragon":
                return dragonPrefab.transform.localScale.x;
            default:
                return 0;
        }
    } 
}

