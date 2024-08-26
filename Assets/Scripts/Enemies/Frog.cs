using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Frog : Enemy
{

    public bool jumping;
    float timer;
    public float jumpTimer;
    public LineRenderer Tongue;
    public float TongueRange;
    public bool shooting;
    public bool retracting;
    public Vector2 tongueDesiredPos;
    public float TongueSpeed;
    public Transform MouthPos;
    public Transform TongueTip;
    public bool hasReachedRange;
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
            Health = (int)(Health * (float) (Math.Pow(x-40, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-40, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-40, 2)/5000f) + 1f);
        }
        MaxHealth = Health;
    }
   
    // Update is called once per frame
    override public void UpdateEnemy()
    {
        if(shooting){
            if(retracting){
                Tongue.SetPosition(0, Vector2.MoveTowards(Tongue.GetPosition(0), Tongue.GetPosition(1), TongueSpeed * Time.deltaTime));
                if(Vector2.Distance(Tongue.GetPosition(1), Tongue.GetPosition(0)) < 0.1f ){
                    Tongue.gameObject.SetActive(false);
                    TongueTip.gameObject.SetActive(false);
                    retracting=false;
                    shooting=false;
                    GetComponent<Animator>().SetTrigger("CloseMouth");
                }
            }else{
                Tongue.SetPosition(0, Vector2.MoveTowards(Tongue.GetPosition(0), Vector3.zero, TongueSpeed * Time.deltaTime));
                if(Tongue.GetPosition(0).magnitude < TongueRange ){
                    base.Attack();
                    retracting=true;
                }
            } 
            TongueTip.position = Tongue.GetPosition(0);
        }else{
            if(hasReachedRange){return;}
            if(jumping){
                Move();
                if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange ){
                    Attacking = true;
                    hasReachedRange = true;
                    GetComponent<Animator>().SetTrigger("InRange");
                    StartCoroutine(PlayAttackAnimation(AttackDelay));
                }
            }else{
                if(timer > 0){
                    timer-=Time.deltaTime;
                }else{
                    if(IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1){
                        if(getSlowInfo("IceLand")[0] <= 0){
                            timer = jumpTimer;
                            jumping = true;
                            GetComponent<Animator>().Play("Jump");
                        }
                    }else{
                        timer = jumpTimer;
                        jumping = true;
                        GetComponent<Animator>().Play("Jump");
                    }
                    
                }
            }
        }
        
    }

    public override void Stun(float f, string source = null){
        if(source != null && source == "IceLand" && jumping){return;}
        base.Stun(f);
    }

    public override void Attack(){
        Attacking = false;
        Tongue.SetPosition(0, MouthPos.position);
        Tongue.SetPosition(1, MouthPos.position);
        TongueTip.position = MouthPos.position;
        Tongue.gameObject.SetActive(true);
        TongueTip.gameObject.SetActive(true);
        Vector3 direction = (MouthPos.position - Vector3.zero).normalized;
        tongueDesiredPos = Vector3.zero + direction * TongueRange;
        
        shooting = true;
    }

    public void Landed(){
        jumping = false;
    }

    public override void CheckFlip(){
        if(transform.position.x < 0){
            Tongue.transform.localScale = new Vector3(-1, 1, 1);
        }
        base.CheckFlip();
    }


    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}

