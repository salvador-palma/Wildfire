using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Beaver : Enemy
{

    public int StealObjective;
    public int StealAmount;
    public bool Stealing;
    private bool gotAway;
    private void Start() {

        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }
    public override void UpdateEnemy()  {
        Move();
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange && !Stealing){
           GetComponent<Animator>().SetTrigger("InRange");
        }
        if((Math.Abs(transform.position.x) > 10f || Math.Abs(transform.position.y) > 6f) && Stealing){
            gotAway = true;
            Destroy(gameObject);
        }
    }
    public override void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime * (Stealing? -1f : 1));
    }
    public override void Attack()
    {
        if(Flamey.Instance.Health > StealObjective){
            StealAmount = StealObjective;
            Flamey.Instance.MaxHealth -= StealAmount;
            Flamey.Instance.Health = Math.Min(Flamey.Instance.MaxHealth,Flamey.Instance.Health);
        }else{
            base.Attack();
        }
        Stealing = true;

        StealAmount =   Flamey.Instance.removeEmbers(StealObjective);
        TurnBack();
    }
    private void TurnBack(){
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }

    public override void Die(bool onKill = true){
        if(!gotAway){ Flamey.Instance.addHealth(StealAmount, 0f);Flamey.Instance.addHealth(StealAmount);}
       
        base.Die();
    }


}