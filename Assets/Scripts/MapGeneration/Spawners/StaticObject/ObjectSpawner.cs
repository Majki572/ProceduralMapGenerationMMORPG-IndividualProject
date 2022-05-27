using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private int chunkMinX = 0;
    private int chunkMaxX = 200;
    private int chunkMinY = 0;
    private int chunkMaxY = 200;
    private int staticObjectCount = 200;
    
    public Dictionary<int, StaticObjectPreset> trees;
    public Dictionary<int, StaticObjectPreset> rocks;
    
    private GameObject objectPooler;
    private int[,] whatBiome;
    
    #region Singleton
    public static ObjectSpawner Instance;

    private void Awake()
    {
        Instance = this;
        objectPooler = GameObject.FindWithTag("ObjPooler");
    }
    #endregion

    public void spawnStaticObjects(){
        whatBiome = GameObject.FindWithTag("BiomeDataObj").GetComponent<BiomeDataObj>().biomedMap;
        spawnStaticObject("Tree");
        spawnStaticObject("Rock");
    }

    private void spawnStaticObject(string name){
        //bool canSpawn;

        for (int i = 0; i < staticObjectCount; ++i){
            Vector3 pos = new Vector3(Random.Range(chunkMinX,chunkMaxX), Random.Range(chunkMinY,chunkMaxY), 0);
            //canSpawn = PreventOverlap(pos);
            GameObject tree = objectPooler.GetComponent<ObjectPooler>().SpawnFromPool(name, pos, Quaternion.identity);

            //tree.GetComponent<SpriteRenderer>().sprite = GetSprite(name, pos);
        }
    }

    private bool PreventOverlap(Vector3 pos){
        return false;
    }

    private int GetSprite(string name, Vector3 pos){
        Sprite sprite; // to return
        return 0;
    }
}
