using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RiverGenerator : MonoBehaviour
{
    public NoiseWormsSettings riverSettings;
    public Vector2 riverStartPosition;
    public int riverLength = 50;
    public bool bold = true;
    //public bool endPointOn = true;

    private ObjectPooler objectPooler;

    public void CreateRiver(Vector2Int startPosition, List<Vector2Int> minimas){
        objectPooler = GameObject.FindWithTag("ObjPooler").GetComponent<ObjectPooler>();
        riverSettings = new NoiseWormsSettings();
        PerlinWorms worm;
        var closestWaterPos = minimas.OrderBy(pos => Vector2.Distance(pos, startPosition)).First();
        worm = new PerlinWorms(riverSettings, startPosition, closestWaterPos);
        List<Vector2> position = worm.MoveLength(riverLength);
        putRivers(position);
    }

    private void putRivers(List<Vector2> positons){
        var mapGenerator = GameObject.FindWithTag("MapGenerator").GetComponent<MapCreator>();
        int[,] biomedMap = GameObject.FindWithTag("BiomeDataObj").GetComponent<BiomeDataObj>().biomedMap;
        foreach (Vector2 pos in positons){
            int sizeX = mapGenerator.width;
            int sizeY = mapGenerator.height;
            if (pos.x < 0 || pos.x >= sizeX || pos.y < 0 || pos.y >= sizeY)
                break;
            GameObject tile = objectPooler.SpawnFromPool("River", new Vector3(pos.x, pos.y, 0), Quaternion.identity);
            tile.GetComponent<SpriteRenderer>().sprite = mapGenerator.riverMinimumAndFlowSprite;
            if (bold && (biomedMap[(int)pos.x, (int)pos.y] != 3)){
                GameObject tileUP = objectPooler.SpawnFromPool("River", new Vector3(pos.x, pos.y + 1, 0), Quaternion.identity);
                GameObject tileDOWN = objectPooler.SpawnFromPool("River", new Vector3(pos.x, pos.y - 1, 0), Quaternion.identity);
                GameObject tileLEFT = objectPooler.SpawnFromPool("River", new Vector3(pos.x - 1, pos.y, 0), Quaternion.identity);
                GameObject tileRIGHT = objectPooler.SpawnFromPool("River", new Vector3(pos.x + 1, pos.y, 0), Quaternion.identity);
                tileUP.GetComponent<SpriteRenderer>().sprite = mapGenerator.riverMinimumAndFlowSprite;
                tileDOWN.GetComponent<SpriteRenderer>().sprite = mapGenerator.riverMinimumAndFlowSprite;
                tileLEFT.GetComponent<SpriteRenderer>().sprite = mapGenerator.riverMinimumAndFlowSprite;
                tileRIGHT.GetComponent<SpriteRenderer>().sprite = mapGenerator.riverMinimumAndFlowSprite;
            }
        }
    }
}
