using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEvolving : Enemy
{
    
    
    
    public void Start() {
        
        int x = EnemySpawner.Instance.current_round;
        flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.01f,0.0015f * x,0.03f);
        AttackDelay = 5f;
        AttackRange = 1.55f;
        Damage = (int) 1.6f*x;
        Health = (int) (3 * 41.6f * x - 5000);
        Armor = (int) (2*5.83f * x - 350);
        MaxHealth = Health;
        StartAnimations(5);
        
    }
    
    

}
