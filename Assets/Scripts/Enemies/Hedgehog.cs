using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;
public class Hedgehog : Enemy
{

    public bool Covered;
    public int ArmorCovered;
    [field: SerializeField] public EventReference RollSound { get; private set; }
  
    // Start is called before the first frame update
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-20, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-20, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-20, 2)/5000f) + 1f);
        }
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
        AudioManager.PlayOneShot(RollSound,transform.position);
        Armor = ArmorCovered;
    }
    public int TitForTat = 10;
    int TitCounter = 0;
    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null){



        if(IceOnHit.Instance != null && SkillTreeManager.Instance.getLevel("Freeze") >= 2 && getSlowInfo("IceHit")[0] > 0){
            Dmg *= 2;
        }
        if(onHit && !Covered){
            Flamey.Instance.ApplyOnHit(Dmg, Health, this, except);
        }
        

        if(!ignoreArmor){
            float B = Dmg/(1+(Armor/100f));
            float armorPen = onHit || Character.Instance.isCharacter("Assassin")? Flamey.Instance.ArmorPen : 0;
            Dmg = (int)(B + (Dmg-B)*armorPen);
        }

        

        if(Covered && onHit){
            if(TitCounter > 0){
                TitCounter--;
            }else{
                TitCounter = TitForTat;
                 Attack();
            }
        }


        Health -= Dmg;
        Flamey.Instance.TotalDamage+=(ulong)Dmg;
        PlayHitAnimation(Dmg, TextID); 
    }


    
}
