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
            Damage = (int)(Damage * (float) (Math.Pow(x-30, 2)/5000f) + 1f);
        }
        MaxHealth = Health;
    }
    public override void UpdateEnemy()  {
        Move();
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange && !placedBomb){
            Attacking = true;
           GetComponent<Animator>().SetTrigger("InRange");
        }
        if((Math.Abs(transform.position.x) > 10f || Math.Abs(transform.position.y) > 6f) && placedBomb){
            Explode();
            Destroy(gameObject);
        }
    }
    public override void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime * (placedBomb? -1f : 1));
    }
    public override void Attack()
    {
        placedBomb = true;
        BombPrefab = Instantiate(BombPrefab);
        Vector3 direction = (HitCenter.position - Flamey.Instance.transform.position).normalized;
        BombPrefab.transform.position = Flamey.Instance.transform.position + direction * deltaBomb;
        TurnBack();
         Attacking = false;
    }
    private void TurnBack(){
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }

    protected void Explode(){
        Instantiate(EnemySpawner.Instance.ExplosionPrefab).transform.position = BombPrefab.transform.position;
        flame.Hitted(Damage, ArmorPen, this);
    }
    public override void Die(bool onKill = true){
        if(placedBomb){
            Destroy(BombPrefab);
        }
        base.Die();
    }


    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}
