using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogJumpSpike : MonoBehaviour
{
    public bool extraMoving;
    float speed = 2f;
    Vector2 ogPoint;
    bool dir;
    float distanceMax = 1f;
    void Start()
    {
        if(extraMoving){
            speed = 7f;
            distanceMax = 4f;
        }
        
        ogPoint = transform.position;
        ogPoint = new Vector2(ogPoint.x + UnityEngine.Random.Range(-1f,1f), ogPoint.y);
    }
    private void Update() {
        transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime * (dir?1:-1)),transform.position.y);

        if((ogPoint.x + distanceMax < transform.position.x && dir)||(ogPoint.x - distanceMax > transform.position.x && !dir)){
            dir = !dir;
        }
    }
    public void Reset()
    {

        ogPoint = transform.position;
        ogPoint = new Vector2(ogPoint.x + UnityEngine.Random.Range(-1f,1f), ogPoint.y);
        
    }
}
