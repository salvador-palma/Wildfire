using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Enemy
{

    public bool Covered;
    public int ArmorCovered;
  
    // Start is called before the first frame update
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }

    // Update is called once per frame
    public override void UpdateEnemy()
    {
        Move();
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange ){
           Attacking = true;
           GetComponent<Animator>().SetTrigger("InRange");
           Cover();
        }
    }
    public void Cover(){
        Covered = true;
        Armor = ArmorCovered;
    }
    public int TitForTat = 3;
    int TitCounter = 3;
    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null){

        if(!ignoreArmor){
            float B = Dmg/(1+(Armor/100f));
            Dmg = (int)(B + (Dmg-B)*(onHit ? Flamey.Instance.ArmorPen : 0));
        }

        if(onHit && !Covered){Flamey.Instance.ApplyOnHit(Dmg, Health, this, except);}

        if(Covered && onHit){
            if(TitCounter <= 0){
                TitCounter = TitForTat;
            }else{
                 Attack();
            }
        }

        Health -= Dmg;
        Flamey.Instance.TotalDamage+=(ulong)Dmg;
        PlayHitAnimation(Dmg, TextID); 
    }


    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
