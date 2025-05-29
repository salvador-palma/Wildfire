using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Armadillo : Enemy
{

    public float rollingSpeedMultiplier;
    public int hitsUntilUnroll;
    public int ArmorNotRolling;
    [field: SerializeField] public EventReference PopOutSound { get; private set; }
    [field: SerializeField] public EventReference DigSound { get; private set; }
    EventInstance DigSoundInstance;

    void Start()
    {
        VirtualPreStart();
        
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        DigSoundInstance = AudioManager.CreateInstance(DigSound);
        DigSoundInstance.start();
        
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-30, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-30, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-30, 2)/2500f) + 1f);
        }
        MaxHealth = Health;
    }
    override public void UpdateEnemy(){
        base.UpdateEnemy();
    }

    public override void Move(){
        
        if(hitsUntilUnroll> 0){
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1-SlowFactor) * rollingSpeedMultiplier * Time.deltaTime);
        }else{
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1-SlowFactor) * Time.deltaTime);
        }
        if(hitsUntilUnroll==0){
            UnRoll();
            hitsUntilUnroll = -1;
        }
    }

    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null){

        if(hitsUntilUnroll > 0 && source!= null && source.Equals("Lava Pool")){

            Dmg = SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 ? Dmg/2 : Dmg/10;
        }
        int n = base.Hitted(Dmg, TextID, ignoreArmor, hitsUntilUnroll <= 0 ? onHit : false, except, source, extraInfo);
        if(onHit && hitsUntilUnroll > 0){
            hitsUntilUnroll--;
        }
        return n;
        
    }
    public override void KnockBack(Vector2 origin, bool retracting, float power, float time = 0.5f,  bool stopOnOrigin = false, float angleMissStep = 0 ){
        if(hitsUntilUnroll <= 0){
            base.KnockBack(origin, retracting, power, time, stopOnOrigin, angleMissStep);
        }
    }

    public void UnRoll(){
        DigSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        DigSoundInstance.release();
        AudioManager.PlayOneShot(PopOutSound,transform.position);

        GetComponent<Animator>().SetTrigger("Unroll");
        Armor = ArmorNotRolling;
    }

    
}
