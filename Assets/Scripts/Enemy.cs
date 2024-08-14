using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public abstract class Enemy : MonoBehaviour,IComparable<Enemy>
{
    public Flamey flame;
    public string Name;
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

    [SerializeField] private float slowfactor;
    protected float SlowFactor{
        get{
            return slowfactor;
        }
        set{
            slowfactor = Math.Clamp(value,0f,.99f);
        }
    }

    
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
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * (1-SlowFactor) * Time.deltaTime);
    }

    public void StartAnimations(int ID){

        GetComponent<Animator>().SetInteger("EnemyID", ID);
    }
   

    public virtual void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null){

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
        
        Flamey.Instance.addEmbers(calculateEmbers());
        flame.TotalKills++;
        PlayHitSoundFx();
        CameraShake.Shake(0.4f,0.05f);
        incDeathAmount();
        if(onKill){Flamey.Instance.ApplyOnKill(HitCenter.position);}
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



    public Dictionary<string, float[]> SlowEffectsDuration = new Dictionary<string, float[]>{{"IceHit", new float[2]},{"IceLand", new float[2]}};
    public virtual void SlowDown(float seconds, float percentage, string SlowEffect){

        float[] prevInfo = getSlowInfo(SlowEffect);

        if(prevInfo == null || prevInfo[0] <= 0){
            SlowFactor += percentage; 
        }

        SlowEffectsDuration[SlowEffect] = new float[2]{seconds, percentage};
        CheckEnemyIceSkin();
    }  
    public float[] getSlowInfo(string SlowEffect){
        return SlowEffectsDuration == null ? null : SlowEffectsDuration.GetValueOrDefault(SlowEffect, null);
    }
    public void ApplySlowUpdate(){
        foreach (string slow in SlowEffectsDuration.Keys)
        {   
            if(SlowEffectsDuration[slow][0] > 0){
                SlowEffectsDuration[slow][0] -= Time.deltaTime;
                if(SlowEffectsDuration[slow][0] <= 0){
                    SlowFactor -= SlowEffectsDuration[slow][1];
                    CheckEnemyIceSkin();
                }
            }
        }
    } 
    private void removeSlowingEffects(){
        SlowEffectsDuration = new Dictionary<string, float[]>{{"IceHit", new float[2]},{"IceLand", new float[2]}};
        CheckEnemyIceSkin();
    }
    public void removeSlow(string effect){
        if(SlowEffectsDuration.ContainsKey(effect)){
            SlowEffectsDuration[effect][0] = 0;
            CheckEnemyIceSkin();
        }
    }
    private void CheckEnemyIceSkin(){
        if(SlowEffectsDuration.Any(k => k.Value[0] > 0)){
            transform.Find("Effect").GetComponent<SpriteRenderer>().enabled = true;
        }else{
            transform.Find("Effect").GetComponent<SpriteRenderer>().enabled = false;
            SlowFactor = 0;
        }
    }


    public static Vector2 getPredicatedEnemyPosition(Comparison<Enemy> sortingFactor){
        List<Enemy> selected = GameObject.FindGameObjectsWithTag("Enemy").Select(I => I.GetComponent<Enemy>()).ToList();
        if(selected.Count == 0){return Vector2.zero;}
        selected.Sort(sortingFactor);
        return selected.First().HitCenter.position;
        
    }
    public static Enemy getClosestEnemy(Vector2 pos, int index = 0){
        Enemy[] go = GameObject.FindGameObjectsWithTag("Enemy").Select(e=>e.GetComponent<Enemy>()).Where(e=>e.canTarget()).ToArray();
        Array.Sort(go, (a,b)=> Vector2.Distance(a.HitCenter.position, pos) < Vector2.Distance(b.HitCenter.position, pos)? -1 : 1);
        if(go.Length == 0){return null;}
        if(go.Length <= index){return go[go.Length - 1];}
        return go[index];
    }

}
