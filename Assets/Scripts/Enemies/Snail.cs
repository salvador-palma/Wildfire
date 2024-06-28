using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Snail : Enemy
{
    
    [SerializeField]  protected bool withShell;
    [SerializeField] private float maxSpeed;
    private void Start() {
        base.flame = Flamey.Instance;
        maxSpeed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        Speed = maxSpeed;
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-10, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-10, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-10, 2)/5000f) + 1f);
        }
        
        MaxHealth = Health;
    }
    bool Moving;
    public override void UpdateEnemy()
    {
        if(Moving){
            base.UpdateEnemy();
        }
    }

    public void SlideOn(){
        Moving = true;
    }
    public void SlideOff(){
        Moving = false;
    }
   

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}

