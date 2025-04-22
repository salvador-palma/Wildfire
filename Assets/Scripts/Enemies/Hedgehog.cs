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
        VirtualPreStart(); 
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
            Damage = (int)(Damage * (float) (Math.Pow(x-20, 2)/2500f) + 1f);
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
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null){


        int n = 0;
        if(IceOnHit.Instance != null && SkillTreeManager.Instance.getLevel("Freeze") >= 2 && getSlowInfo("IceHit")[0] > 0){
            Dmg *= 2;
        }
        if(onHit && !Covered){
            n = Flamey.Instance.ApplyOnHit(Dmg, Health, this, except);
        }
        

        if(!ignoreArmor){
            float B = Dmg/(1+(Armor/100f));
            float armorPen = onHit || Character.Instance.isCharacter("Assassin")? Flamey.Instance.ArmorPen : 0;
            Dmg = (int)(B + (Dmg-B)*armorPen);
        }

        //============================

        //ACHIEVMENTS AND QUESTS
        if(Dmg >= 50000){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(17, "Naal", 13); //AZUREOTH UNLOCK
        }
        if(CritUnlock.Instance != null){
            if(Dmg >= Flamey.Instance.Dmg * 5f){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(18, "Rowl", 17); //POWERED UP UNLOCK
            }
        }

        if(source=="Thorns"){
            float DamageGiven = extraInfo[0];
            if(DamageGiven < Dmg){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(26, "Cloris", 13);
            }
        }

        //============================

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
        return n;
    }

    protected override void ReturnWalk(){}


    
}
