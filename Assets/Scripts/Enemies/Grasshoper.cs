using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Grasshoper : Enemy
{

    public Transform centerPoint;
    public float initialRadiusX = 9.39f;
    public float initialRadiusY = 5.07f;

    public Vector2 shrinkLimits;
    private float shrinkRate;

    private float radiusX;
    private float radiusY;
    private float angle = 0f;
    public int direction = 1;

    public bool jumping;
    float timer;
    public float jumpTimer;

    public float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        radiusX = initialRadiusX;
        radiusY = initialRadiusY;
        centerPoint = Flamey.Instance.transform;
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
        Vector3 relativePosition = transform.position - centerPoint.position;
        angle = Mathf.Atan2(relativePosition.y, relativePosition.x);

        shrinkRate = Random.Range(shrinkLimits[0], shrinkLimits[1]);
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
    public override void Move(){

        if(Stunned){return;}

        float x = centerPoint.position.x + Mathf.Cos(angle) * radiusX;
        float y = centerPoint.position.y + Mathf.Sin(angle) * radiusY;

        prevX = transform.position.x;

        transform.position = new Vector3(x, y, transform.position.z);


        angle += Speed * (1-SlowFactor) * direction * Time.deltaTime;
        radiusX -= (Speed/MaxSpeed) * shrinkRate * Time.deltaTime;
        radiusY -= (Speed/MaxSpeed) * shrinkRate * Time.deltaTime;
        
        CheckFlip();
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }


    
}
