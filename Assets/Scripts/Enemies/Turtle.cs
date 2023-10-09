using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    
    private bool check = false;
    
    private void Start() {
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomGaussian(0.01f,Speed);
        // AttackDelay = 6f;
        // AttackRange = 1.55f;
        // Damage = 50;
        // Health = 1000;
        // Armor = 50;
        MaxHealth = Health;
        StartAnimations(2);
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
