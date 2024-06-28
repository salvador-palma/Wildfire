using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bear : Enemy
{


    public int BearState;
    public int HealIncrement;

    // Start is called before the first frame update
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-40, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-40, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-40, 2)/5000f) + 1f);
        }
        MaxHealth = Health;
    }

    // Update is called once per frame
    override public void UpdateEnemy()
    {
        switch(BearState){
            case 0:
                base.UpdateEnemy();
                if(Health < MaxHealth/3 && !Attacking){BearState = 1; GetComponent<Animator>().Play("Eating");}
                break;
            case 1:
                if(Health >= MaxHealth){BearState = 2;GetComponent<Animator>().Play("Walk");}
                break;
            case 2:
                base.UpdateEnemy();
                break;
        } 
    }

    public void EatHoney(){
        Health += HealIncrement;
        DamageUI.InstantiateTxtDmg(transform.position, "+"+HealIncrement, 3);
    }

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
