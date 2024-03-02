using System;
using System.Collections;
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
            Dmg = (int)(B + (Dmg-B)*Flamey.Instance.ArmorPen);
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
        Destroy(gameObject);
    }


    protected virtual int calculateEmbers(){
        if(MoneyMultipliers.Instance == null){
            return EmberDropRange[0] + Distribuitons.RandomBinomial(EmberDropRange[1] - EmberDropRange[0], 0.1f);
        }
        return (int)((EmberDropRange[0] + Distribuitons.RandomBinomial(EmberDropRange[1] - EmberDropRange[0], MoneyMultipliers.Instance.p)) * MoneyMultipliers.Instance.mult);
    }

    

    public virtual void Attack(){
        flame.Hitted(Damage, ArmorPen, this);
    }
    protected virtual void PlayAttackAnimation(){
        Debug.Log("ShootAnim");
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




    public void setTemporarySpeed(float seconds, float percReduced, Action<Enemy> beforeAction = null, Action<Enemy> afterAction = null, string augmentClass = null){
        //if(inEffect){return;}
        
        if(augmentClass == "IcePool"){
            if(timeSpeed <= 0){
                timeSpeed = Math.Max(timeSpeed, seconds);
                StartCoroutine(setSpeedTimer(percReduced,beforeAction,afterAction));
            }else{
                timeSpeed = Math.Max(timeSpeed, seconds);
            }
        }else{
            if(inEffect){return;}
            SetSpeedCouroutineEffect(seconds,percReduced,beforeAction,afterAction);
        }
        
    }

    private float timeSpeed = 0;
    private IEnumerator setSpeedTimer(float percReduced,Action<Enemy> beforeAction = null, Action<Enemy> afterAction = null){

        if(beforeAction != null){beforeAction(this);}
        //float current_Speed = getSpeed();
        setSpeed(getSpeed() * percReduced);

        while(timeSpeed > 0){
            timeSpeed -= Time.deltaTime;
            yield return null;
        }

        if(afterAction != null){afterAction(this);}
        setSpeed(getSpeed() / percReduced);
    }

    public virtual void setSpeed(float speed){
        Speed = speed;
    }
    public virtual float getSpeed(){
        return Speed;
    }
     private IEnumerator SetSpeedCouroutineEffect(float seconds, float percReduced,Action<Enemy> beforeAction = null, Action<Enemy> afterAction = null){
        inEffect = true;
        if(beforeAction != null){beforeAction(this);}

        //float current_Speed = getSpeed();
        setSpeed(getSpeed() * percReduced);
        yield return new WaitForSeconds(seconds);
        if(afterAction != null){afterAction(this);}
        setSpeed(getSpeed() / percReduced);

        inEffect = false;
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
