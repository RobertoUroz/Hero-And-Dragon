using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCharacterRotation (Quaternion newAngle)
    {
        switch (StaticClass.characterSelected)
        {
            case "Hero":
                StaticClass.heroPrefab.transform.localRotation = newAngle;
                break;
            case "Dragon":
                StaticClass.dragonPrefab.transform.localRotation = newAngle;
                break;
            default:
                break;
        }
    }

    public Quaternion getCharacterRotation ()
    {
        switch (StaticClass.characterSelected)
        {
            case "Hero":
                return StaticClass.heroPrefab.transform.localRotation;
            case "Dragon":
                return StaticClass.dragonPrefab.transform.localRotation;
            default:
                return Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    } 
}
