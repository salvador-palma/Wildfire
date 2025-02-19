using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogJumpFly : MonoBehaviour
{
    
    float timerCD = 1.2f;
    float timer;
    float distance = 2f;
    float speed = 0.5f;
    Vector2 dest;

    internal void Reset()
    {
        float angle = UnityEngine.Random.Range(0f,(float)(2*Math.PI));
        Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            vec *= distance;
            dest = new Vector2(transform.position.x + vec.x, transform.position.y + vec.y);
    }

    void Start()
    {
        timer = UnityEngine.Random.Range(0,10f);
        Reset();
    }
    // Update is called once per frame
    void Update()
    {
        if(timer<=0){
            timer = timerCD;
            float angle = UnityEngine.Random.Range(0f,(float)(2*Math.PI));
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            vec *= distance;
            dest = new Vector2(transform.position.x + vec.x, transform.position.y + vec.y);

        }else{
            timer -= Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, dest, Time.deltaTime*speed);
        }
    }
}
