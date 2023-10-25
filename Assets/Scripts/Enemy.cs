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
   
    public Transform HitCenter;

    public bool inEffect;

    
    public void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
    }

    public void StartAnimations(int ID){
        GetComponent<Animator>().SetInteger("EnemyID", ID);
    }
    
    public void HittedWithArmor(int dmg, bool onHit, string except = null){
        int effectiveDmg = (int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-Flamey.Instance.ArmorPen))) * dmg);
        if(onHit){Flamey.Instance.ApplyOnHit(effectiveDmg, Health, this, except);}
        Health -= effectiveDmg;
        
        if(Health <= 0){Die();}
        PlayHitAnimation(new Tuple<int, bool>(effectiveDmg, false));
       
    }
    public void HittedArmorless(int dmg, string except = null){
        
        //if(onHit){Flamey.Instance.ApplyOnHit(effectiveDmg, Health, this, except);}
        Health -= dmg;
        
        if(Health <= 0){Die();}
        PlayHitAnimation(new Tuple<int, bool>(dmg, false));
       
    }
    private void Hitted(Tuple<int,bool> res, Vector2 explosionPos){
        int effectiveDmg = (int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-Flamey.Instance.ArmorPen))) * res.Item1);
        
        
        Health -= effectiveDmg;
        Flamey.Instance.ApplyOnHit(effectiveDmg, Health, this);
        if(Health <= 0){this.Die();}
        PlayHitAnimation(new Tuple<int, bool>(effectiveDmg, res.Item2));
        PlayHitSoundFx();
        SpawnExplosion(explosionPos);
        //CameraShake.Shake(0.25f,0.1f);
    }
    private void PlayHitSoundFx(){
        AudioManager.Instance.PlayFX(1,1,0.3f, 0.5f);
    }
    public void PlayHitAnimation(Tuple<int,bool> res){
        GetComponent<Animator>().Play("EnemyHit");
        DamageUI.Instance.spawnTextDmg(transform.position, res.Item1.ToString(), res.Item2 ? 1 : 0);
    }

    public virtual void Die(){
        CameraShake.Shake(0.4f,0.15f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "FlareHit"){
           
            Hitted(other.GetComponent<FlareSpot>().DmgCrit, other.transform.position);
        }else if(other.tag == "OrbitalHit"){
            Hitted(new Tuple<int, bool>(FlameCircle.Instance.damage, false), other.transform.position);
        }
    }

    public void Attack(){
        flame.Hitted(Damage, ArmorPen);
        PlayAttackAnimation();
    }
    private void PlayAttackAnimation(){
        GetComponent<Animator>().Play("EnemyAttack");
    }
    public IEnumerator AttackRoutine(){
        yield return new WaitForSecondsRealtime(AttackDelay);
        Attack();
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
    private void SpawnExplosion(Vector2 explosionPos){
        GameObject g = Instantiate(EnemySpawner.Instance.ExplosionPrefab);
        g.transform.position = explosionPos;
    }


    public int CompareTo(Enemy other)
    {
        return Vector2.Distance(transform.position, flame.transform.position) < Vector2.Distance(other.transform.position, flame.transform.position)? -1 : 1; 
    }

    public void EndEnemy(){
        this.enabled = false;
    }
}
