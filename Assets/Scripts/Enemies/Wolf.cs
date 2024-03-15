using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Wolf : Enemy
{
    public bool isStray;
    public bool isAlpha;
    public float RunningSpeed;
    public float HowlRadius;

    public bool howling;
    
    public Vector2Int AlphaToDefaultRatio; // 1,4
    public float AlphaRatio;
    private void Awake() {
        if(isStray){
            GetComponent<Animator>().SetBool("isAlpha", true);
        }
        if(isAlpha){
            StartCoroutine(StartAlphaDelay());  
        }
    }
    private void Start() {
        AlphaRatio = AlphaToDefaultRatio[0]/(AlphaToDefaultRatio[1]*1.0f);
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
        if(!isStray){checkForAlphaSelf();}else{Speed= RunningSpeed;}
    }


    override public void UpdateEnemy() {
        if(!howling){
            base.UpdateEnemy();
        }
    }
    public IEnumerator StartAlphaDelay(){
        yield return new WaitForSeconds(3f);
        howling = true;
        GetComponent<Animator>().Play("Howl");
        Speed = RunningSpeed;
    }

    public void endHowl(){
        howling = false;
        scaleSprite();
    }
    public void Howl(){
        
        
        Collider2D[] AnimalAround = Physics2D.OverlapCircleAll(HitCenter.position, HowlRadius, FlareManager.EnemyMask);

        foreach(Collider2D col in AnimalAround){
            Enemy e = col.GetComponent<Enemy>();          
            if(e==null || e==this){continue;}
            if(e is Wolf){
                ((Wolf)e).Activate();
            }
        }
    }

    public void Activate(){
        if(isAlpha || isStray){
            return;
        }
        GetComponent<Animator>().SetTrigger("isActive");
        Speed = RunningSpeed;
    }

    public void TurnAlpha(){
        isAlpha = true;
        StartCoroutine(StartAlphaDelay());  
        
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

    protected override void PlayAttackAnimation(){
        if(isStray){
            GetComponent<Animator>().Play("StrayAttack");
        }else if(isAlpha){
            GetComponent<Animator>().Play("AlphaAttack");
        }else{
            GetComponent<Animator>().Play("DefaultAttack");
        }
  
    }

    public void scaleSprite(){
        Vector2 s = transform.localScale;
        s *= 1.5f;
        transform.localScale = s;
    }
}
