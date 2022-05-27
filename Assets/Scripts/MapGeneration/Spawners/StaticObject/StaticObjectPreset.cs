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

    public Sprite GetTileSprite()
    {
        return stObj[Random.Range(0, stObj.Length)];
    }

    public bool MatchCondition(float height, float moisture, float heat)
    {
        return height >= minHeight && moisture >= minMoisture && heat >= minHeat;
    }
}