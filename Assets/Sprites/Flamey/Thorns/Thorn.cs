using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : IPoolable
{
    public override string getReference()
    {
        return "Thorns";
    }

    public override void Define(float[] args){
        transform.position = new Vector2(args[0], args[1]);
        gameObject.SetActive(true);
        GetComponent<Animator>().SetInteger("AOE", (int)args[2]);
       
    }
    public override void UnPool()
    {
        GetComponent<Animator>().SetInteger("AOE", 0);
        base.UnPool();
    }



}
