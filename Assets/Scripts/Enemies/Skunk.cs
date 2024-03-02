using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skunk : Enemy
{

    public float StunDuration;
    private void Start() {

        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
         base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;

        //StartAnimations(6);
    }

    
   
    public override void Attack(){
        Flamey.Instance.Stun(StunDuration);
       
    }


    override protected void PlayAttackAnimation(){
        GetComponent<Animator>().Play("Stun");
    }
}
