using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    
    private bool check = false;
    [SerializeField]  private bool withShell;
    private void Start() {
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomGaussian(0.02f,Speed);
        // AttackDelay = 4f;
        // AttackRange = 1.04f;
        // Damage = 20;
        // Health = 110;
        // Armor = 20;
        MaxHealth = Health;
        StartAnimations(withShell? 0 : 2);
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

