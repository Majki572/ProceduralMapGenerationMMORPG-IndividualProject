using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "StaticObject Preset", menuName = "New StaticObject Preset", order = 2)]
public class StaticObjectPreset : ScriptableObject
{
    public Sprite[] stObj;
    public float minHeight;
    public float minMoisture;
    public float minHeat;

    public Sprite GetSprite()
    {
        return stObj[Random.Range(0, stObj.Length)];
    }
}