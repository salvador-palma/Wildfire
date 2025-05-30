using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : Enemy{
    public int direction = 1;

    public bool jumping;
    float timer;
    public float jumpTimer;
    public float flyingTime;

    public float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
        VirtualPreStart(); 
        timer = jumpTimer;
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-10, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-10, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-10, 2)/2500f) + 1f);
        }
        MaxSpeed = Speed;
        MaxHealth = Health;
        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;

    }
    protected override void ReturnWalk()
    {
        timer = jumpTimer;
        base.ReturnWalk();
    }
    bool check;
    override public void UpdateEnemy()
    {
        if(jumping){
            base.UpdateEnemy();
        }else{
            if(timer > 0){
                timer-=Time.deltaTime;
            }else if(!check){
                if (IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1)
                {
                    if (getSlowInfo("IceLand")[0] <= 0)
                    {
                        Flamey.Instance.StartCoroutine(Fly());
                    }
                }
                else
                {
                    Flamey.Instance.StartCoroutine(Fly());                    
                }
            }
        }
    }
    public IEnumerator Fly()
    {
        direction *= -1;
        jumping = true;
        GetComponent<Animator>().Play("Fly");

        yield return new WaitForSeconds(flyingTime);
        if (this != null)
        {
            jumping = false;
            timer = jumpTimer;
            check = false;
            GetComponent<Animator>().Play("Idle");
        }
    }

    public override void Stun(float f, string source = null)
    {
        if (source != null && source == "IceLand" && jumping) { return; }
        base.Stun(f);
    }
    

    float prevX;
    public float angleStep = -20f;
    public override void Move(){

        if(Stunned){return;}
        if (AttackTarget.Equals(Flamey.Instance))
        {

            MoveSpiral(angleStep, direction == 1);
            CheckFlip();
            prevX = transform.position.x;
        }
        else
        {
            base.Move();
        }
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }


    
}

