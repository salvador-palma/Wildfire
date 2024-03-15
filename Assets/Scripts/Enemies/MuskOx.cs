using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MuskOx : Enemy
{
    public int ArmorPerGrowl;
    
    public int TimesHealed;
    public float HealRadius= 2.7f;
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }
    bool howling;
    public override void UpdateEnemy()
    {
        if(Health < 2* MaxHealth/3 && TimesHealed ==0){
            TimesHealed++;
            howling = true;
            GetComponent<Animator>().Play("Howl");

        }else if(Health < MaxHealth/3 && TimesHealed ==1){
            TimesHealed++;
            howling = true;
            GetComponent<Animator>().Play("Howl");
        }
        if(howling){return;}
       
        Move();
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange ){
           Attacking = true;
           GetComponent<Animator>().SetTrigger("InRange");
           GetComponent<Animator>().SetBool("InRangeBool", true);
           InvokeRepeating("PlayAttackAnimation",0f, AttackDelay);
        }
    }
    public override void Attack()
    {
        Collider2D[] AnimalAround = Physics2D.OverlapCircleAll(HitCenter.position, HealRadius, FlareManager.EnemyMask);

        foreach(Collider2D col in AnimalAround){
            Enemy e = col.GetComponent<Enemy>();          
            if(e==null || e==this){continue;}
            e.Armor += ArmorPerGrowl;
            DamageUI.InstantiateTxtDmg(e.transform.position, ""+ArmorPerGrowl, 10);
            

        }
    }
    protected override void PlayAttackAnimation(){
        GetComponent<Animator>().Play("Howl");
    }

    public void EndHowl(){howling=false;}
}
