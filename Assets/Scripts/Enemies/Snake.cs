using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snake : Enemy
{
    public bool isPoisonous;
    public int poisonTicks;
    private void Awake() {
        GetComponent<Animator>().SetBool("isPoisonous", isPoisonous);
    }
    private void Start() {
        VirtualPreStart(); 
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance; 
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-40, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-40, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-40, 2)/5000f) + 1f);
        }
        MaxHealth = Health;
        
    }

    public override void Attack(){
        if(isPoisonous){
            flame.Poison(poisonTicks);
            AudioManager.PlayOneShot(AttackSound, transform.position);
        }else{
            base.Attack();
        }
    }

    public override void Move()
    {
        if(IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1){
            if(getSlowInfo("IceLand")[0] <= 0){
                base.Move();
            }
        }else{base.Move();}
        
    }

    

    
}
