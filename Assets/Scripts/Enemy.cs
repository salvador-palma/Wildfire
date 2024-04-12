using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
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
    public Vector2Int EmberDropRange;
    public bool Attacking;
   
    public Transform HitCenter;

    public bool inEffect;

    
    public virtual void UpdateEnemy()  {
        Move();
        
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange ){
           Attacking = true;
           removeSlowingEffects();
           GetComponent<Animator>().SetTrigger("InRange");
           InvokeRepeating("PlayAttackAnimation",0f, AttackDelay);
        }
    }
    public virtual void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
    }

    public void StartAnimations(int ID){

        GetComponent<Animator>().SetInteger("EnemyID", ID);
    }
   

    public virtual void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null){

        if(!ignoreArmor){
            float B = Dmg/(1+(Armor/100f));
            Dmg = (int)(B + (Dmg-B)*(onHit ? Flamey.Instance.ArmorPen : 0));
        }

        if(onHit){Flamey.Instance.ApplyOnHit(Dmg, Health, this, except);}


        Health -= Dmg;
        Flamey.Instance.TotalDamage+=(ulong)Dmg;
        PlayHitAnimation(Dmg, TextID); 
    }

    
    private void PlayHitSoundFx(){
        AudioManager.Instance.PlayFX(1,1,0.3f, 0.5f);
    }
    public void PlayHitAnimation(int dmg, int textID){
        GetComponent<Animator>().Play("EnemyHit");
        DamageUI.InstantiateTxtDmg(transform.position, dmg.ToString(), textID);
    }

    public virtual void Die(bool onKill = true){
        if(this==null){return;}
        if(onKill){Flamey.Instance.ApplyOnKill(this);}
        Flamey.Instance.addEmbers(calculateEmbers());
        flame.TotalKills++;
        PlayHitSoundFx();
        CameraShake.Shake(0.4f,0.15f);
        incDeathAmount();
        Destroy(gameObject);
    }


    protected virtual int calculateEmbers(){
        if(MoneyMultipliers.Instance == null){
            return EmberDropRange[0] + Distribuitons.RandomBinomial(EmberDropRange[1] - EmberDropRange[0], 0.1f);
        }
        return (int)((EmberDropRange[0] + Distribuitons.RandomBinomial(EmberDropRange[1] - EmberDropRange[0], MoneyMultipliers.Instance.p)) * MoneyMultipliers.Instance.mult);
    }

    public abstract int getDeathAmount();
    public abstract void incDeathAmount();
    public abstract void ResetStatic();
    

    public virtual void Attack(){
        flame.Hitted(Damage, ArmorPen, this);
    }
    protected virtual void PlayAttackAnimation(){
       
        GetComponent<Animator>().Play("EnemyAttack");
    }
    
    
    public void target(){
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void untarget(){
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public virtual bool canTarget(){return true;}

    

    

    protected virtual void OnMouseDown() {
        
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

    public virtual void CheckFlip(){
        if(transform.position.x < 0){
            bool flipped = !GetComponent<SpriteRenderer>().flipX;
            GetComponent<SpriteRenderer>().flipX = flipped;
            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = flipped;
        }
    }



    public Dictionary<string, float[]> SlowEffectsDuration;
    public virtual void SlowDown(float seconds, float percentage, string SlowEffect){
        if(SlowEffectsDuration== null){SlowEffectsDuration = new Dictionary<string, float[]>();}
        float[] prevInfo = SlowEffectsDuration.GetValueOrDefault(SlowEffect, null);
        if(prevInfo == null || prevInfo[0] <= 0){
            Speed *= percentage;
            
        }
        SlowEffectsDuration[SlowEffect] = new float[2]{seconds, percentage};
        CheckEnemyIceSkin();
    }  
    public float[] getSlowInfo(string SlowEffect){
        return SlowEffectsDuration == null ? null : SlowEffectsDuration.GetValueOrDefault(SlowEffect, null);
    }
    public void ApplySlowUpdate(){
        if(SlowEffectsDuration== null){SlowEffectsDuration = new Dictionary<string, float[]>();}

        foreach (string slow in SlowEffectsDuration.Keys)
        {   
            if(SlowEffectsDuration[slow][0] > 0){
                SlowEffectsDuration[slow][0] -= Time.deltaTime;
                if(SlowEffectsDuration[slow][0] <= 0){
                    Speed /= SlowEffectsDuration[slow][1];
                    CheckEnemyIceSkin();
                }
            }
            
        }
    } 
    private void removeSlowingEffects(){
        SlowEffectsDuration = new Dictionary<string, float[]>();
        CheckEnemyIceSkin();
    }
    public void removeSlow(string effect){
        SlowEffectsDuration[effect][0] = 0;
        CheckEnemyIceSkin();
    }
    private void CheckEnemyIceSkin(){
        
        if(SlowEffectsDuration.Any(k => k.Value[0] > 0)){
            
            transform.Find("Effect").GetComponent<SpriteRenderer>().enabled = true;
        }else{
            transform.Find("Effect").GetComponent<SpriteRenderer>().enabled = false;
        }
    }



    public static Enemy getClosestEnemy(Vector2 pos){
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        if(go.Length == 0){return null;}
        try{
            GameObject minimum = go[0];
            float minDist = float.PositiveInfinity;
            foreach(GameObject enemy in go){
                float calc = Vector2.Distance(enemy.GetComponent<Enemy>().HitCenter.position, pos);
                if(minDist > calc){
                    minDist = calc;
                    minimum = enemy;
                }
            }
            return minimum.GetComponent<Enemy>();
        }catch{
            Debug.Log("Covered Error! Flamey.getRandomHomingEnemy()");

        }
        return null;
    }

}
