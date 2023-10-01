using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    
    private bool check = false;
    
    private void Start() {
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomGaussian(0.02f,0.04f);
        AttackDelay = 6f;
        AttackRange = 1f;
        Damage = 5;
        Health = 100;
    }
    private void Update() {
        
        base.Move();
        if(Vector2.Distance(flame.transform.position, transform.position) < AttackRange && !check){
            check = true;
            InvokeRepeating("Attack",0f, AttackDelay);
        }
    }

    
    
}
