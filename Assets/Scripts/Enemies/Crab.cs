using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crab : Enemy{
    public int direction = 1;

    public bool burried;
    float timer;
    public float jumpTimer;


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

    override public void UpdateEnemy()
    {
        if(burried){
            return;
        }else{
            base.UpdateEnemy();
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer =  Random.Range(3f, 5f);
                direction *= Random.Range(0f, 1f) < 0.5f ? -1 : 1;

            }
        }
    }
    public override bool canTarget()
    {
        return !burried;
    }

    public override void Stun(float f, string source = null)
    {
        if (burried) { return; }
        base.Stun(f);
    }

    public IEnumerator Burry()
    {

        burried = true;
        untarget();
        if (Flamey.Instance.current_homing == this) { Flamey.Instance.current_homing = null; }
        GetComponent<Animator>().Play("Bury");

        yield return new WaitForSeconds(5f);
        if (this != null)
        {
            GetComponent<Animator>().Play("Unbury");
            burried = false;
        }
        

    }
    public int onHitToBurry = 3;
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if (!burried || (SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 && source != null && source.Equals("Lava Pool")))
        {
            if (onHit)
            {
                onHitToBurry--;
                if (onHitToBurry <= 0 && !Attacking)
                {
                    onHitToBurry = 3;
                    Flamey.Instance.StartCoroutine(Burry());
                }
            }
            return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
        }
        return 0;
    }
        
    float prevX;
    public float angleStep = -20f;
    public override void Move(){

        if(Stunned){return;}
        MoveSpiral(angleStep, direction == 1);
        CheckFlip();
        prevX = transform.position.x;
    }

    public override void CheckFlip(){   
        
        GetComponent<SpriteRenderer>().flipX = HitCenter.position.x < AttackTarget.getPosition().x;
        transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = HitCenter.position.x < AttackTarget.getPosition().x; 
    }


    
}


