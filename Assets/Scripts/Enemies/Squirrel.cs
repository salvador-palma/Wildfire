using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Squirrel : Enemy
{
    

    public GameObject BombPrefab;
    
    public bool placedBomb;
    public float deltaBomb;
    protected virtual void Start() {
        VirtualPreStart(); 
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-30, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-30, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-30, 2)/2500f) + 1f);
        }
        MaxHealth = Health;
    }
    public override void UpdateEnemy()  {
        Move();
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !placedBomb){
            Attacking = true;
           GetComponent<Animator>().SetTrigger("InRange");
        }
        if((Math.Abs(transform.position.x) > 10f || Math.Abs(transform.position.y) > 6f) && placedBomb){
            Explode();
            Destroy(gameObject);
        }
    }
    public override void Move(){
        if(Stunned){return;}
        if (placedBomb)
        {
            transform.position = Vector2.MoveTowards(transform.position, Flamey.Instance.getPosition(), Speed* (1-SlowFactor)  * Time.deltaTime * -1f);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed* (1-SlowFactor)  * Time.deltaTime);
        }
        
    }
    public override void Attack()
    {
        placedBomb = true;
        BombPrefab = Instantiate(BombPrefab);
        Vector3 direction = (HitCenter.position - (Vector3)AttackTarget.getPosition()).normalized;
        BombPrefab.transform.position = (Vector3)AttackTarget.getPosition() + direction * deltaBomb;
        TurnBack();
         Attacking = false;
    }
    protected void TurnBack(){
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }

    protected void Explode(){
        ObjectPooling.Spawn(EnemySpawner.Instance.ExplosionPrefab, new float[]{BombPrefab.transform.position.x, BombPrefab.transform.position.y});
        AudioManager.PlayOneShot(AttackSound,transform.position);
        AttackTarget.Hitted(Damage, ArmorPen, this);
    }
    public override void Die(bool onKill = true){
        if(placedBomb){
            Destroy(BombPrefab);
        }
        base.Die();
    }
    protected override void ReturnWalk(){}


    
    
}
