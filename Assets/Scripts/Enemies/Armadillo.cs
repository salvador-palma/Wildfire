using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armadillo : Enemy
{

    public float rollingSpeedMultiplier;
    public int hitsUntilUnroll;
    public int ArmorNotRolling;

    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-30, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-30, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-30, 2)/5000f) + 1f);
        }
        MaxHealth = Health;
    }
    override public void UpdateEnemy(){
        base.UpdateEnemy();
    }

    public override void Move(){
        if(hitsUntilUnroll> 0){
            transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * (1-SlowFactor) * rollingSpeedMultiplier * Time.deltaTime);
        }else{
            transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * (1-SlowFactor) * Time.deltaTime);
        }
        if(hitsUntilUnroll==0){
            UnRoll();
            hitsUntilUnroll = -1;
        }
    }

    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null){

        if(hitsUntilUnroll > 0 && source!= null && source.Equals("Lava Pool")){

            Dmg = SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 ? Dmg/2 : Dmg/10;
        }
        base.Hitted(Dmg, TextID, ignoreArmor, hitsUntilUnroll <= 0 ? onHit : false, except);
        if(onHit && hitsUntilUnroll > 0){
            hitsUntilUnroll--;
        }
        
    }

    public void UnRoll(){
        GetComponent<Animator>().SetTrigger("Unroll");
        Armor = ArmorNotRolling;
    }

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
