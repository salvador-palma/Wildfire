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
                timer = jumpTimer;
                jumping = true;
                GetComponent<Animator>().Play("Jump");
            }
        }
    }

    public void Landed(){
        jumping = false;
    }

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
