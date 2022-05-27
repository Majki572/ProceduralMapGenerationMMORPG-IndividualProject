using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapCreator : MonoBehaviour
{
    public BiomePreset[] biomes;
    public GameObject tilePrefab;
    
    private ObjectPooler objectPooler;
    private ObjectSpawner objectSpawner;
    public int[,] biomedMap;

    //[Header("SpawnableObjects")]
    //int staticObjectCount = 100;
    // TODO int dynamicObjectCount = 100;

    [Header("Dimensions")]
    public int width = 100;
    public int height = 100;
    [SerializeField] public float scale = 1.0f;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    private float[,] moistureMap;

    [Header("Heat Map")]
    public Wave[] heatWaves;
    private float[,] heatMap;

    public void Start()
    {
        objectPooler = ObjectPooler.Instance;
        objectPooler.Init();
        objectSpawner = ObjectSpawner.Instance;
        generateMap();
        previousScale = scale;
        
        objectSpawner.spawnStaticObjects();
    }
    
    #region DoUsunieciaModyfikacjaWGUI
    private float previousScale;
    private void Update()
    {
        if(previousScale != scale)
        {
            foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
            {
                if (!(o.tag == "MapGenerator") && !(o.tag == "MainCamera")) {
                    o.SetActive(false); 
                }
            }
            generateMap();
        }
        previousScale = scale;
    }
    #endregion

    private void generateMap()
    {
        // wysokosc
        heightMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, heightWaves, offset);
        // wilgotnosc
        moistureMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, moistureWaves, offset);
        // temperatura
        heatMap = NoiseGenerator.GenerateNoiseMap(width, height, scale, heatWaves, offset);
        putTiles();
        GameObject.FindWithTag("BiomeDataObj").GetComponent<BiomeDataObj>().biomedMap = biomedMap;
        
    }

    private void putTiles(){
        biomedMap = new int[width,height];
        BiomePreset biomeTMP;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                // pobierz obiekt z basenu wez jego sprite i naloz na niego sprite odpowiadajacemu danemu biomowi : )
                GameObject tile = objectPooler.SpawnFromPool("prefab", new Vector3(x, y, 0), Quaternion.identity);
                biomeTMP = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);
                tile.GetComponent<SpriteRenderer>().sprite = biomeTMP.GetTileSprite();
                biomedMap[x, y] = biomeTMP.getBiomeID();
            }
        }
    }

    private BiomePreset GetBiome(float height, float moisture, float heat)
    {
        List<BiomeTmpData> biomeTemp = new List<BiomeTmpData>();
        foreach (BiomePreset biome in biomes)
        {
            if (biome.MatchCondition(height, moisture, heat))
            {
                biomeTemp.Add(new BiomeTmpData(biome));
            }
        }

        float curVal = 0.0f;
        BiomePreset biomeToReturn = null;
        foreach (BiomeTmpData biome in biomeTemp)
        {
            if (biomeToReturn == null)
            {   
                biomeToReturn = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
            }
            else
            {
                if (biome.GetDiffValue(height, moisture, heat) < curVal)
                {
                    biomeToReturn = biome.biome;
                    curVal = biome.GetDiffValue(height, moisture, heat);
                }
            }
        }
        if (biomeToReturn == null)
            biomeToReturn = biomes[0];
        return biomeToReturn;
    }
}
