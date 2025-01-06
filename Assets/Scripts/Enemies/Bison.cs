using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using FMODUnity;
public class Bison : Enemy
{
    public int maxCharge;
    public int chargeAmount;
    public int dmgPerCharge;
    
    public bool running = false;
   
    [field: SerializeField] public EventReference SweepSound { get; private set; }
    [field: SerializeField] public EventReference FirstHitSound { get; private set; }
    private void Start() {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-45, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-45, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-45, 2)/5000f) + 1f);
        }
        MaxHealth = Health;

    }


    override public void UpdateEnemy() {
        if(Health < MaxHealth/2 || chargeAmount >= maxCharge){
            GetComponent<Animator>().Play("Run");
            running = true;
            
        }
        if(running){
            base.UpdateEnemy();
        }
    }
    
    public void charge(){
        chargeAmount++;
        AudioManager.PlayOneShot(SweepSound, transform.position);
    }

    bool AttackedAlready = false;
    public override void Attack(){
        if(AttackedAlready){
            flame.Hitted(Damage/2, ArmorPen, this);
        }else{
            AttackedAlready = true;
            AudioManager.PlayOneShot(FirstHitSound, transform.position);
            flame.Hitted(Damage + (chargeAmount*dmgPerCharge), 1, this);
        }
       
    }
    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null){
        if(source!= null && source.Equals("Lava Pool")){
            base.Hitted(SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 ? Dmg/2 : Dmg/10, TextID, ignoreArmor, onHit, except);
        }else{
            base.Hitted(Dmg, TextID, ignoreArmor, onHit, except);
        }
    }

    public override void CheckFlip()
    {
        base.CheckFlip();
        if(transform.position.x < 0){
            
            transform.Find("SmokeFront").GetComponent<SpriteRenderer>().flipX = true;
            transform.Find("SmokeBack").GetComponent<SpriteRenderer>().flipX = true;
        }
        

    }

    
}
