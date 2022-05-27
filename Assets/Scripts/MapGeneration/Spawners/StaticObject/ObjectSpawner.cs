using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private int chunkMinX = 0;
    private int chunkMaxX = 200;
    private int chunkMinY = 0;
    private int chunkMaxY = 200;
    public int staticObjectCount = 400;
    
    public StaticObjectPreset[] trees;
    public StaticObjectPreset[] rocks;
    
    private Dictionary<int, StaticObjectPreset> treesD;
    private Dictionary<int, StaticObjectPreset> rocksD;

    private Dictionary<string, Dictionary<int, StaticObjectPreset>> dictionaryHolder = new Dictionary<string, Dictionary<int, StaticObjectPreset>>();
    
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
        treesD = copyTable(trees);
        rocksD = copyTable(rocks);
        dictionaryHolder.Add("Tree", treesD);
        dictionaryHolder.Add("Rock", rocksD);
        spawnStaticObject("Tree");
        spawnStaticObject("Rock");
    }

    private void spawnStaticObject(string name){
        //bool canSpawn;

        for (int i = 0; i < staticObjectCount; ++i){
            Vector3 pos = new Vector3((int)Random.Range(chunkMinX,chunkMaxX), (int)Random.Range(chunkMinY,chunkMaxY), 0);
            //canSpawn = PreventOverlap(pos);
            GameObject tree = objectPooler.GetComponent<ObjectPooler>().SpawnFromPool(name, pos, Quaternion.identity);
            tree.GetComponent<SpriteRenderer>().sprite = GetSprite(name, pos);
        }
    }

    private bool PreventOverlap(Vector3 pos){
        return false;
    }
    // From the list of dictionaries pick the one that has biome-classified object in it
    // Choose texture of the wanted object considering biome
    private Sprite GetSprite(string name, Vector3 pos){
        Sprite sprite;
        Dictionary<int, StaticObjectPreset> dict = dictionaryHolder[name];
        sprite = dict[whatBiome[(int)pos[0],(int)pos[1]]].GetSprite();
        return sprite;
    }

    private Dictionary<int, StaticObjectPreset> copyTable(StaticObjectPreset[] src){
        Dictionary<int, StaticObjectPreset> dst = new Dictionary<int, StaticObjectPreset>();
        for(int i = 0; i < src.Length; i++){
            dst.Add(i, src[i]);
        }
        return dst;
    }
}
