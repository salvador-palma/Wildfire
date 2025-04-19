using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareSpot : IPoolable
{
    float Timer = 10f;
    private void Update() {
        Timer-=Time.deltaTime;
        if(Timer <= 0){
            UnPool();
        }
    }

    public override string getReference()
    {
        return "FlareSpot";
    }
    public override void Pool()
    {
        Timer = 10f;
        GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }

    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
        GetComponent<SpriteRenderer>().enabled=true;
    }
}
