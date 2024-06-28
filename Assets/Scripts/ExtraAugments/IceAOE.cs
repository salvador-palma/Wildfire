using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAOE : MonoBehaviour
{
    public float Slow;

    public float LastingTime;
    float lt;

    private void Start() {
        
        lt = IceOnLand.Instance.lasting;
        Slow = IceOnLand.Instance.slow;
        Vector2 scale = transform.localScale * IceOnLand.Instance.size;
        transform.localScale = scale;

    }
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
            float[] info = e.getSlowInfo("IcePool");
            
            float lastingAmount = Math.Max(lt, info==null? -1 : info[0]);
            e.SlowDown(lastingAmount, 1-Slow, "IcePool");
  
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            collider.GetComponent<Enemy>().removeSlow("IcePool");
        }
    }

    
}
