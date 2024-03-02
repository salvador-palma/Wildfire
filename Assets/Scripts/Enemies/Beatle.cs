using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatle : Enemy
{
    public GameObject Ball;
    public float growthRate;
    public Transform BallFinit;
    public float BallReleaseSpeed;
    public bool hidden;
    public float ballHitRange;

    private void Start() {

        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }

    public override void UpdateEnemy()  {
        if(hidden){
            moveBall();
        }else{
            growBall();
            Move();
            if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange ){
                Attacking = true;
                Ball.GetComponent<Animator>().Play("Stop");
                GetComponent<Animator>().SetTrigger("InRange");
            }
        }
        
    }
    public override void Attack(){
       Attacking = false;
       Ball.GetComponent<Animator>().Play("Shoot");
       Ball.transform.parent = null;
       hidden = true;
       if(Flamey.Instance.current_homing == this){ Flamey.Instance.current_homing  = null; untarget();}
       
    }
    protected override void PlayAttackAnimation(){
        GetComponent<Animator>().Play("Hide");
        
    }
    public override bool canTarget()
    {
        return !hidden;
    }
    public void growBall(){
        Ball.transform.position = Vector2.MoveTowards(Ball.transform.position, BallFinit.position, growthRate * Time.deltaTime);
        Ball.transform.localScale = Vector2.MoveTowards(Ball.transform.localScale, BallFinit.localScale, growthRate * Time.deltaTime);
    }
    public void moveBall(){
        Ball.transform.position = Vector2.MoveTowards(Ball.transform.position, flame.transform.position, BallReleaseSpeed * Time.deltaTime);
        if(Vector2.Distance(flame.transform.position, Ball.transform.position) < ballHitRange){
            Attacking = true;
            Destroy(Ball);
            base.Attack();
            Die();
        }
    }

    protected override int calculateEmbers()
    {
        if(hidden){EmberDropRange[0] = 0; EmberDropRange[1] = 0;}
        return base.calculateEmbers();
    }

    public override void CheckFlip()
    {
        base.CheckFlip();
        if(transform.position.x < 0){
            Ball.transform.localPosition = new Vector2(-Ball.transform.localPosition.x,Ball.transform.localPosition.y);
            Ball.GetComponent<SpriteRenderer>().flipX = true;
            BallFinit.transform.localPosition = new Vector2(-BallFinit.transform.localPosition.x,BallFinit.transform.localPosition.y);
        }
    }
}
