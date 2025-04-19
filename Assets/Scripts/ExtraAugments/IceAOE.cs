using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAOE : IPoolable
{
    public float Slow;

    public float LastingTime;
    float lt;

    
    void Update()
    {
        lt-=Time.deltaTime;
        if(lt<=0){
            lt = LastingTime;
            GetComponent<Animator>().Play("EndAOEBurn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){

            Enemy e = collider.GetComponent<Enemy>();
            if(!e.canTarget()){return;}
            float[] info = e.getSlowInfo("IceLand");
            
            float lastingAmount = Math.Max(lt, info==null? -1 : info[0]);
            e.SlowDown(lastingAmount, Slow, "IceLand");
  
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            collider.GetComponent<Enemy>().removeSlow("IceLand");
        }
    }

    public override void Pool()
    {
        
        lt = IceOnLand.Instance.lasting;
        Slow = IceOnLand.Instance.slow;
        Vector2 scale = new Vector2(0.2232152f,0.2232152f) * IceOnLand.Instance.size;
        transform.localScale = scale;
        if(EnemySpawner.Instance.isOnAugments){UnPool();}

        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 1;
        GetComponent<SpriteRenderer>().color = c;

        int n = ObjectPooling.ActiveAmount(this) + 1;
       
        if(n >= 100 && GameVariables.hasQuest(19)){

            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(19, "Cloris", 11);
            
        }

    }
    public override string getReference()
    {
        return "Ice Pool";
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }
}
