using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange && !Stealing){
           GetComponent<Animator>().SetTrigger("InRange");
        }
        if((Math.Abs(transform.position.x) > 10f || Math.Abs(transform.position.y) > 6f) && Stealing){
            gotAway = true;
            Destroy(gameObject);
        }
    }
    public override void Move(){
        if(Stunned){return;}
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * (1-SlowFactor) * Time.deltaTime * (Stealing? -1f : 1));
    }
    public override void Attack()
    {
        if(Flamey.Instance.MaxHealth > StealObjective){
            StealAmount = StealObjective;
            Flamey.Instance.MaxHealth -= StealAmount;
            Flamey.Instance.Health = Math.Min(Flamey.Instance.MaxHealth,Flamey.Instance.Health);
        }

        AudioManager.PlayOneShot(AttackSound, transform.position);
        Stealing = true;

        
        TurnBack();
    }
    private void TurnBack(){
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }
    protected override void ReturnWalk(){}

    public override void Die(bool onKill = true){
        if(!gotAway && Stealing){ Flamey.Instance.addHealth((int)(StealAmount*0.95f), 0f);}
       
        base.Die();
    }


    


}
