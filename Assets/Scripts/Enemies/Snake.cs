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
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance; 
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health += (int) Math.Pow(x-40, 2);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-60, 2)/5000f) + 1f;
        }
        MaxHealth = Health;
        
    }

    public override void Attack(){
        if(isPoisonous){
            flame.Poison(poisonTicks);
        }else{
            base.Attack();
        }
    }

   

    protected override void PlayAttackAnimation(){
       if(isPoisonous){
        GetComponent<Animator>().Play("EnemyAttackPoison");
       }else{
        GetComponent<Animator>().Play("EnemyAttack");
       }
    }

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
