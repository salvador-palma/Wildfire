using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour,IComparable<Enemy>
{
    public Flamey flame;
    public int Damage;
    public float AttackDelay;
    public float AttackRange;
    public float Speed;
    public int Health;
    public int MaxHealth;
    public int Armor;
    public float ArmorPen;
    public bool Attacking;
   
    public Transform HitCenter;

    public bool inEffect;

    
    public virtual void UpdateEnemy()  {
        Move();
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange ){
           Attacking = true;
           GetComponent<Animator>().SetTrigger("InRange");
           InvokeRepeating("Attack",0f, AttackDelay);
        }
    }
    public void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
    }

    public void StartAnimations(int ID){
        GetComponent<Animator>().SetInteger("EnemyID", ID);
    }
   
    public void HittedWithArmor(int dmg, bool onHit, int TextID, string except = null){
        float B = dmg/(1+Armor/100);
        int effectiveDmg = (int)(B + (Damage-B)*flame.ArmorPen);
        //int effectiveDmg = (int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-Flamey.Instance.ArmorPen))) * dmg);
        
        
        if(onHit){Flamey.Instance.ApplyOnHit(effectiveDmg, Health, this, except);}
        HittedArmorless(effectiveDmg,TextID);       
    }
    
    public void HittedArmorless(int dmg, int textID){
        try{
            Health -= dmg;
            flame.TotalDamage+=(ulong)dmg;
            PlayHitAnimation(dmg, textID);
        }catch{
            Debug.Log("Error Ocurred");
        }
        
    }
    public void Hitted(int Dmg, int TextID){
        int effectiveDmg = (int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-Flamey.Instance.ArmorPen))) * Dmg);
        Flamey.Instance.ApplyOnHit(effectiveDmg, Health, this);
        HittedArmorless(effectiveDmg,TextID);   
    }
    private void PlayHitSoundFx(){
        AudioManager.Instance.PlayFX(1,1,0.3f, 0.5f);
    }
    public void PlayHitAnimation(int dmg, int textID){
        GetComponent<Animator>().Play("EnemyHit");
        DamageUI.Instance.spawnTextDmg(transform.position, dmg.ToString(), textID);
    }

    public virtual void Die(){
        flame.TotalKills++;
        PlayHitSoundFx();
        CameraShake.Shake(0.4f,0.15f);
        Destroy(gameObject);
    }

    

    public void Attack(){
        flame.Hitted(Damage, ArmorPen);
        PlayAttackAnimation();
    }
    private void PlayAttackAnimation(){
        GetComponent<Animator>().Play("EnemyAttack");
    }
    
    
    public void target(){
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void untarget(){
        transform.GetChild(0).gameObject.SetActive(false);
    }


    private void OnMouseDown() {
        
        Flamey.Instance.target(this);
    }
    public static void SpawnExplosion(Vector2 explosionPos){
        GameObject g = Instantiate(EnemySpawner.Instance.ExplosionPrefab);
        g.transform.position = explosionPos;
    }


    public int CompareTo(Enemy other)
    {
        return Vector2.Distance(HitCenter.position, flame.transform.position) < Vector2.Distance(other.HitCenter.position, flame.transform.position)? -1 : 1; 
    }

    public void EndEnemy(){
        this.enabled = false;
    }
}
