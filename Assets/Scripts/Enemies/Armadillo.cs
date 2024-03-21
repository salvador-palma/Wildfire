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
        MaxHealth = Health;
    }
    override public void UpdateEnemy(){
        base.UpdateEnemy();
    }

    public override void Move(){
        if(hitsUntilUnroll> 0){
            transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * rollingSpeedMultiplier * Time.deltaTime);
        }else{
            transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
        }
        if(hitsUntilUnroll==0){
            UnRoll();
            hitsUntilUnroll = -1;
        }
    }

    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null){

        base.Hitted(Dmg, TextID, ignoreArmor, false, except);
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
