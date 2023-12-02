using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEvolving : Enemy
{
    
    private bool check = false;
    
    private void Start() {
        int x = EnemySpawner.Instance.current_round;
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.01f,0.0012f * x,0.03f);
        AttackDelay = 5f;
        AttackRange = 1.55f;
        Damage = (int) 1.6f*x;
        Health = (int) 41.6f * x;
        Armor = (int) 5.83f * x;
        MaxHealth = Health;
        StartAnimations(5);
    }
    private void Update() {
        
        base.Move();
        if(Vector2.Distance(flame.transform.position, transform.position) < AttackRange && !check){
            check = true;
            Speed = 0.00001f;
            InvokeRepeating("Attack",0f, AttackDelay);
        }
    }

    
    
}
