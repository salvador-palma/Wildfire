using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnDeath : IPoolable
{
    public string PoolName;
    public override string getReference()
    {
        return PoolName;
    }
    public float Timer = 1f;
    private void Update() {
        Timer-=Time.deltaTime;
        if(Timer <= 0){
            UnPool();
        }
    }
    public override void Pool()
    {
        Timer = 1f;
        GetComponent<ParticleSystem>().Clear();
        GetComponent<ParticleSystem>().Play();

        int n = ObjectPooling.ActiveAmount(this) + 1;
        
        if(n >= 10 && GameVariables.hasQuest(23)){

            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(23, "Rowl", 18);
            
        }
    }
   
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }
}
