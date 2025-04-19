using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using FMODUnity;

public class Wolf : Enemy
{
    public bool isStray;
    public bool isAlpha;
    public float RunningSpeed;
    public float HowlRadius;

    public bool howling;
    
    public Vector2Int AlphaToDefaultRatio; // 1,4
    public float AlphaRatio;
    [field: SerializeField] public EventReference HowlSound { get; private set; }
    private void Awake() {
        if(isStray){
            GetComponent<Animator>().SetBool("isAlpha", true);
        }
        if(isAlpha){
            TurnAlpha();
        }
    }
    private void Start() {
        VirtualPreStart(); 
        AlphaRatio = AlphaToDefaultRatio[0]/(AlphaToDefaultRatio[1]*1.0f);
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
        if(!isStray){checkForAlphaSelf();}else{Speed= RunningSpeed;}
    }


    override public void UpdateEnemy() {
        if(!howling){
            base.UpdateEnemy();
        }
    }


    public void endHowl(){
        howling = false;
        scaleSprite();
    }
    public void Howl(){
        
        AudioManager.PlayOneShot(HowlSound,transform.position);
        Collider2D[] AnimalAround = Physics2D.OverlapCircleAll(HitCenter.position, HowlRadius, Flamey.EnemyMask);

        foreach(Collider2D col in AnimalAround){
            Enemy e = col.GetComponent<Enemy>();          
            if(e==null || e==this){continue;}
            if(e is Wolf){
                ((Wolf)e).Activate();
            }
        }
    }

    public void Activate(){
        if(isStray){
            return;
        }
        GetComponent<Animator>().SetTrigger("isActive");
        Speed = RunningSpeed;
        Damage += 50;
        ArmorPen = Math.Min(1,ArmorPen+.25f);
    }

    public void TurnAlpha(){
        isAlpha = true;
        
        Flamey.Instance.target(this);
        Speed = RunningSpeed;
        Damage += 50;
        ArmorPen = Math.Min(1,ArmorPen+.25f);
        howling = true;
        GetComponent<Animator>().Play("Howl");
        
        
    }

    [System.Obsolete]
    public void checkForAlpha(){

       Wolf[] wolves = FindObjectsOfType<Wolf>();
            int TotalWolves = wolves.Count();
            if(TotalWolves > 3){
                int AlphaWolves = wolves.Count( e => e.isAlpha );
                
                if(AlphaRatio > AlphaWolves/(TotalWolves * 1.0f)){
                    Wolf sucessor = wolves[Random.Range(0, wolves.Count() - 1)];
                    while(sucessor == this){
                        sucessor = wolves[Random.Range(0, wolves.Count() - 1)];
                    }
                    sucessor.TurnAlpha();
                }
            }
        
    }

    public void checkForAlphaSelf(){
        Wolf[] wolves = FindObjectsOfType<Wolf>();
            int TotalWolves = wolves.Count();
            if(TotalWolves > 3){
                int AlphaWolves = wolves.Count( e => e.isAlpha );
                
                if(AlphaRatio >= (AlphaWolves+1)/(TotalWolves * 1.0f)){
                    
                    TurnAlpha();
                }
            }
    }

    public override void Die(bool onKill = true)
    {
        if(isAlpha){
            checkForAlpha();
        }
        base.Die(onKill);
    }
    override protected IEnumerator PlayAttackAnimation(float delay){
        if(!isStray){
            if(isAlpha){
                AttackAnimationName = "AlphaAttack";
                WalkAnimationName = "AlphaRun";
            }else{
                AttackAnimationName = "DefaultAttack";
                WalkAnimationName = "DefaultRun";
            }
        }
        
        return base.PlayAttackAnimation(delay);
    }
    

    public void scaleSprite(){
        Vector2 s = transform.localScale;
        s *= 1.5f;
        transform.localScale = s;
    }


    
}
