using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmog : IPoolable
{
    public string PoolName;
    public override string getReference()
    {
        return PoolName;
    }
    public float Timer = 10f;
    private void Update() {
        Timer-=Time.deltaTime;
        if(Timer <= 0){
            UnPool();
        }
    }
    public override void Pool()
    {
        Timer = 10f;
        GetComponent<ParticleSystem>().Clear();
        GetComponent<ParticleSystem>().Play();
    }

    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
        transform.localScale = new Vector3(args[2], args[2], args[2]);

    }
}
