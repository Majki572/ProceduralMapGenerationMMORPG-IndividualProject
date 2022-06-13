using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NoiseGenerator : MonoBehaviour
{
    // scale - to zoom in or out from the noisemap to cut out specified widthXheight rectangle
    public static float[,] GenerateNoiseMap(int width, int height, float scale, Wave[] waves, Vector2 offset){
        float[,] noiseMap = new float[width, height];
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                float newX = (float)x * scale + offset.x;
                float newY = (float)y * scale + offset.y;
                float normalize = 0.0f;

                foreach (Wave wave in waves){
                    noiseMap[x, y] += wave.amplitude * Mathf.PerlinNoise(newX * wave.frequency + 
                        wave.seed, newY * wave.frequency + wave.seed);
                    normalize += wave.amplitude;
                }
                noiseMap[x, y] /= normalize;
            }
        }
        return noiseMap;
    }

    public static List<Vector2Int> FindLocalMaxima(float[,] noiseMap, int[,] biomedMap){
        List<Vector2Int> maximas = new List<Vector2Int>();
        List<Vector2Int> correctedMaximas = new List<Vector2Int>();
        for (int x = 0; x < noiseMap.GetLength(0); x++){
            for (int y = 0; y < noiseMap.GetLength(1); y++){
                float noiseValue = noiseMap[x,y];
                if(CheckNeighbours(x, y, noiseMap, (neighbourNoise) => neighbourNoise > noiseValue)){ //weź wartość neighbour noise i sprawdz czy jest wieksza od noiseVal
                    maximas.Add(new Vector2Int(x, y));
                }
            }
        }
        
        for(int i = 0; i < maximas.Count; i++){
            if(biomedMap[maximas[i].x, maximas[i].y] == 3 || biomedMap[maximas[i].x, maximas[i].y] == 1/* || biomedMap[maximas[i].x, maximas[i].y] == 5*/){ // ID of a mountains biome is 3
                correctedMaximas.Add(maximas[i]);
            }
        }
        //correctedMaximas.RemoveRange((int)(correctedMaximas.Count/2), (int)(correctedMaximas.Count - correctedMaximas.Count/2));
        if(correctedMaximas == null){
            return maximas;
        } 

        return correctedMaximas;
    }

    public static List<Vector2Int> FindLocalMinimum(float[,] noiseMap, int[,] biomedMap){
        List<Vector2Int> minima = new List<Vector2Int>();
        List<Vector2Int> correctedMinima = new List<Vector2Int>();
        for (int x = 0; x < noiseMap.GetLength(0); x++){
            for (int y = 0; y < noiseMap.GetLength(1); y++){
                float noiseValue = noiseMap[x, y];
                if (CheckNeighbours(x, y, noiseMap, (neighbourNoise) => neighbourNoise < noiseValue)){
                    minima.Add(new Vector2Int(x, y));
                }
            }
        }
        
        for(int i = 0; i < minima.Count; i++){
            if(biomedMap[minima[i].x, minima[i].y] == 4){ // ID of a mountains biome is 3
                correctedMinima.Add(minima[i]);
            }
        }
        //correctedMinima.RemoveRange((correctedMinima.Count)/4, correctedMinima.Count - (correctedMinima.Count)/4); // trim water endpoints
        if(correctedMinima == null){
            return minima;
        }
        return correctedMinima;
    }

    public static float SumNoise(float x, float y, NoiseWormsSettings noiseWormsSettings){
        float amplitude = 1;
        float freq = noiseWormsSettings.startFreq;
        float noiseSum = 0;
        float amplitudeSum = 0;
        for(int i = 0; i < noiseWormsSettings.octaves; i++){
            noiseSum += amplitude * Mathf.PerlinNoise(x * freq, y * freq);
            amplitudeSum += amplitude;
            amplitude *= noiseWormsSettings.persistance;
            freq *= 2;
        }
        return noiseSum / amplitudeSum;
    }

    public static float RangeMap(float inValue, float inMin, float inMax, float outMin, float outMax){
        return outMin + (inValue - inMin) * (outMax - outMin) / (inMax - inMin);
    }


    private static List<Vector2Int> directions = new List<Vector2Int>{
        new Vector2Int(0, 1),   //N
        new Vector2Int(1, 1),   //NE
        new Vector2Int(1, 0),   //E
        new Vector2Int(-1, 1),  //SE
        new Vector2Int(-1, 0),  //S
        new Vector2Int(-1, -1), //SW
        new Vector2Int(0, -1),  //W
        new Vector2Int(1, -1)   //NW
    };

    private static bool CheckNeighbours(int x, int y, float[,] noiseMap, Func<float, bool> failCondition){
        foreach (var dir in directions){
            Vector2Int newPosition = new Vector2Int(x + dir.x, y + dir.y);

            if (newPosition.x < 0 || newPosition.x >= noiseMap.GetLength(0) || newPosition.y < 0 || newPosition.y >= noiseMap.GetLength(1)){
                continue;
            }

            if(failCondition(noiseMap[x + dir.x, y + dir.y])){
                return false;
            }
        }
        return true;
    }
}
