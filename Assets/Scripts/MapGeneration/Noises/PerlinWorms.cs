using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PerlinWorms{

    private Vector2 currentDirection;
    private Vector2 currentPos;
    private Vector2 endPoint;
    private NoiseWormsSettings noiseWormsSettings;
    public bool moveToEndPoint = false;
    [Range(0.3f, 0.9f)]
    public float weight = 0.6f;

    public PerlinWorms(NoiseWormsSettings noiseWormsSettings, Vector2 startPos, Vector2 endPoint){
        currentDirection = Random.insideUnitCircle.normalized;
        this.noiseWormsSettings = noiseWormsSettings;
        this.currentPos = startPos;
        this.endPoint = endPoint;
        this.moveToEndPoint = true;
    }

    private Vector2 Move(){
        Vector3 direction = GetDirection();
        currentPos += (Vector2)direction;
        return currentPos;
    }
    
    private Vector2 MoveTowardEndPoint(){
        Vector3 direction = GetDirection();
        var directionToEndPoint = (this.endPoint - currentPos).normalized; // normalizing for length to be equal to 1
        var endDirection = ((Vector2)direction * (1 - weight) + directionToEndPoint * weight).normalized;
        currentPos += endDirection;
        return currentPos;
    }

    private Vector2 GetDirection(){
        float noise = NoiseGenerator.SumNoise(currentPos.x, currentPos.y, noiseWormsSettings);
        float angle = NoiseGenerator.RangeMap(noise, 0, 1, -80, 80); // to what extent does the worm move / rotation / for meandering
        currentDirection = (Quaternion.AngleAxis(angle, Vector3.forward) * currentDirection).normalized;
        return currentDirection;
    }

    public List<Vector2> MoveLength(int length){
        List<Vector2> list = new List<Vector2>();
        for(int i = 0; i < length; i++){
            if(moveToEndPoint){
                var result = MoveTowardEndPoint();
                list.Add(result);
                if(Vector2.Distance(this.endPoint, result) < 1){
                    break;
                }
            } else {
                var result = Move();
                list.Add(result);
            }
        }
        //never reached end point:
        if(moveToEndPoint){
            while(Vector2.Distance(this.endPoint, currentPos) > 1){
                weight = 0.9f;
                var result = MoveTowardEndPoint();
                list.Add(result);
                if(Vector2.Distance(this.endPoint, result) < 1){
                    break;
                }
            }
        }
        return list;
    }
}
