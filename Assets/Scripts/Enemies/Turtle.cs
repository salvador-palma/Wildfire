using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turtle : Enemy
{
    
    
    private void Start() {
        flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.01f,Speed,0.03f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health += (int) Math.Pow(x-20, 2);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-60, 2)/5000f) + 1f;
        }
     
        MaxHealth = Health;

    }
   
    

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}
