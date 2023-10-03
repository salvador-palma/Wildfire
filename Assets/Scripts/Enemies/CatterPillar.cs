using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatterPillar : Enemy
{
    
    private bool check = false;
    
    private void Start() {
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomGaussian(0.02f,0.2f);
        AttackDelay = 2f;
        AttackRange = 0.77f;
        Damage = 20;
        Health = 50;
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
