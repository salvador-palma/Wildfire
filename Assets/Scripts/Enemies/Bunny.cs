using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bunny : Enemy
{

    public bool jumping;
    float timer;
    public float jumpTimer;
    // Start is called before the first frame update
    void Start()
    {
        VirtualPreStart(); 
        timer = jumpTimer;
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-10, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-10, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-10, 2)/5000f) + 1f);
        }
        MaxHealth = Health;
    }

   
    // Update is called once per frame
    override public void UpdateEnemy()
    {
        if(jumping){
            base.UpdateEnemy();
        }else{
            if(timer > 0){
                timer-=Time.deltaTime;
            }else{
                if(IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1){
                    if(getSlowInfo("IceLand")[0] <= 0){
                        timer = jumpTimer;
                        jumping = true;
                        GetComponent<Animator>().Play("Jump");
                    }
                }else{
                    timer = jumpTimer;
                    jumping = true;
                    GetComponent<Animator>().Play("Jump");
                }
                
            }
        }
    }

    public override void Stun(float f, string source = null){
        if(source != null && source == "IceLand" && jumping){return;}
        base.Stun(f);
    }
    

    public void Landed(){
        jumping = false;
    }

    
}
