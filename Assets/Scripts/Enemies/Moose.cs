using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moose : Enemy
{
    public int HealPerGrowl;
    
    public int TimesHealed;
    public float HealRadius= 2.7f;
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-50, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-50, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-50, 2)/5000f) + 1f);
        }
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
            if(e==null || e is Moose){continue;}
            int healing = Math.Min(HealPerGrowl, e.MaxHealth - e.Health);
            e.Health += healing;
            if(healing>0){
                DamageUI.InstantiateTxtDmg(e.transform.position, ""+healing, 3);
            }

        }
    }
    protected override void PlayAttackAnimation(){
        GetComponent<Animator>().Play("Howl");
    }

    public void EndHowl(){howling=false;}


    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
