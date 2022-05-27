using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeTmpData
{
    public BiomePreset biome;
    public BiomeTmpData(BiomePreset preset)
    {
        biome = preset;
    }

    public float GetDiffValue(float height, float moisture, float heat)
    {
        return (height - biome.minHeight) + (moisture - biome.minMoisture) + (heat - biome.minHeat);
    }
}
