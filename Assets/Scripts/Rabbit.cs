using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy
{
    
    private bool check = false;
    
    private void Start() {
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomGaussian(0.02f,0.1f);
        AttackDelay = 2f;
        AttackRange = 0.77f;
        Damage = 5;
        Health = 10;
    }
    private void Update() {
        
        base.Move();
        if(Vector2.Distance(flame.transform.position, transform.position) < AttackRange && !check){
            check = true;
            InvokeRepeating("Attack",0f, AttackDelay);
        }
    }

    
    
}
