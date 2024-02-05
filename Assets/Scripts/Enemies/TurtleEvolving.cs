using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEvolving : Enemy
{
    
    
    
    public void Start() {
        
        int x = EnemySpawner.Instance.current_round;
        flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.01f,(float)(0.075f + Math.Pow(x-60, 2)/50000f),0.03f);
        AttackDelay = 5f;
        AttackRange = 1.55f;
        Damage = (int)(100 + Math.Pow(x-60, 2)/100f);
        Health = (int) (5000 + Math.Pow(x-60, 2)/2f);
        Armor = (int) (2*5.83f * x - 350);
        MaxHealth = Health;
        StartAnimations(5);
        
    }
    
    

}
