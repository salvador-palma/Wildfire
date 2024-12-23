using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurnAOE : IPoolable
{
    public int Damage;
    public float Timer =1f;
    float t;
    public float LastingTime;
    float lt;
    List<Enemy> colliding;
    public bool EverLasting;
   
    void Update()
    {
        t-=Time.deltaTime;
        
        if(t<=0){
            t= Timer;
            try{
                if(SkillTreeManager.Instance.getLevel("Lava Pool") >= 2){
                    foreach (Enemy item in colliding)
                    {
                        if(item == null){continue;}
                        item.Hitted(EverLasting ? BurnOnLand.Instance.damage : Damage,9, ignoreArmor:false, onHit: false, source: "Lava Pool");
                        item.Armor -= Math.Max(0, (int)(item.Armor * .02f));
                    }
                }else{
                    foreach (Enemy item in colliding)
                    {
                        if(item == null){continue;}

                        item.Hitted(Damage,9, ignoreArmor:false, onHit: false, source: "Lava Pool");
                        
                    }
                }
                
            }catch(InvalidOperatorException e){
                Debug.Log(e);
            }
            
        }
        if(!EverLasting){
            lt-=Time.deltaTime;
            if(lt<=0){
                lt = LastingTime;
                GetComponent<Animator>().Play("EndAOEBurn");
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Add(collider.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Remove(collider.GetComponent<Enemy>());
        }
    }
    public override void Pool()
    {
        colliding = new List<Enemy>();
        lt = BurnOnLand.Instance.lasting;
        Damage = BurnOnLand.Instance.damage;
        if(!EverLasting){
            Vector2 scale = new Vector2(0.2232152f,0.2232152f) * BurnOnLand.Instance.size;
            transform.localScale = scale;
        }
        if(EnemySpawner.Instance.isOnAugments){UnPool();}

        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 1;
        GetComponent<SpriteRenderer>().color = c;
    }
    public override string getReference()
    {
        return "Lava Pool";
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }

    
}
