using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    
    private bool check = false;
    
    private void Start() {
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomGaussian(0.02f,0.1f);
        AttackDelay = 4f;
        AttackRange = 0.77f;
        Damage = 20;
        Health = 110;
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

