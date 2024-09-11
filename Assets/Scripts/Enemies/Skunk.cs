using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Skunk : Enemy
{

    public float StunDuration;
    private void Start() {

        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
         base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-30, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-30, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-30, 2)/5000f) + 1f);
        }
        MaxHealth = Health;

        //StartAnimations(6);
    }

    
   
    public override void Attack(){
        Flamey.Instance.Stun(StunDuration);
        base.Attack();
    }


    
    override protected IEnumerator PlayAttackAnimation(float delay){
        while(Health>0){
            GetComponent<Animator>().Play("Stun");
            yield return new WaitForSeconds(delay);
            yield return new WaitForSeconds(extraAtkSpeedDelay);
        }
    }


    
}
