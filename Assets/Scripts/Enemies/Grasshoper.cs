using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Grasshoper : Enemy
{

    public int direction = 1;

    public bool jumping;
    float timer;
    public float jumpTimer;

    public float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
 
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
            Damage = (int)(Damage * (float) (Math.Pow(x-10, 2)/5000f) + 1f);
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
                if(IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1){
                    if(getSlowInfo("IceLand")[0] <= 0){
                        
                        GetComponent<Animator>().Play("Jump");
                    }
                }else{
                    
                    GetComponent<Animator>().Play("Jump");
                }
            }
        }
    }

    public override void Stun(float f, string source = null){
        if(source != null && source == "IceLand" && jumping){return;}
        base.Stun(f);
    }
    
    public void Landed(){
        jumping = false;
        timer = jumpTimer;
        check = false;
    }
    public void Jumping(){
        jumping = true;
        check = true;
    }

    float prevX;
    public float angleStep = 0.39f;
    public override void Move(){

        if(Stunned){return;}

        Vector2 hk = flame.transform.position;
        Vector2 cv = HitCenter.position;

        double g = Math.Atan2(cv.y - hk.y, cv.x - hk.x);
        g = g < 0 ? g+2*Math.PI : g;
        g += angleStep * direction;
        int w = Math.PI/2 < g && g < 2*Math.PI - Math.PI/2 ? -1 : 1;

        double r = Math.Sqrt(  Math.Pow(cv.x - hk.x, 2)/4f  + Math.Pow(cv.y - hk.y, 2));
        double a = Math.Sqrt(4*r*r);
        double b = r;

        float nx = (float)(w*a*b/Math.Sqrt(b*b + a*a*Math.Pow(Math.Tan(g),2)));
        float ny = (float)(nx*Math.Tan(g));

        Vector2 dest = new Vector2(nx, ny);

        prevX = transform.position.x;
        transform.position = Vector2.MoveTowards(transform.position, dest, Speed * (1-SlowFactor) * Time.deltaTime);
        
        CheckFlip();
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }


    
}
