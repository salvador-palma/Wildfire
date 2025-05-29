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
        VirtualPreStart(); 
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
            Damage = (int)(Damage * (float) (Math.Pow(x-50, 2)/2500f) + 1f);
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
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange ){
           Attacking = true;
           GetComponent<Animator>().SetTrigger("InRange");
           GetComponent<Animator>().SetBool("InRangeBool", true);
           StartCoroutine(PlayAttackAnimation(AttackDelay));
        }
    }
    public override void Attack()
    {
        
        
        Collider2D[] AnimalAround = Physics2D.OverlapCircleAll(HitCenter.position, HealRadius, Flamey.EnemyMask);

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
    

    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null){
        if(source!= null && source.Equals("Lava Pool")){
            return base.Hitted(SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 ? Dmg/2 : Dmg/10, TextID, ignoreArmor, onHit, except, source, extraInfo);
        }else{
            return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
        }
    }

    public void EndHowl(){howling=false;}


    
}
