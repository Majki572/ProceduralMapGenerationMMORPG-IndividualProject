using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset", order = 1)]
public class BiomePreset : ScriptableObject
{
    public Sprite[] tiles;
    public float minHeight;
    public float minMoisture;
    public float minHeat;
    public int biomeID;

    public Sprite GetTileSprite()
    {
        return tiles[Random.Range(0, tiles.Length)];
    }

    public bool MatchCondition(float height, float moisture, float heat)
    {
        return height >= minHeight && moisture >= minMoisture && heat >= minHeat;
    }

    public int getBiomeID(){
        return this.biomeID;
    }

}
