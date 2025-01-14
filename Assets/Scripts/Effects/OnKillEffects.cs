using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public interface OnKillEffects : Effect
{
    public bool addList();
    public void ApplyEffect(Vector2 en);
}


public class Explosion : OnKillEffects
{
    public float prob;
    public int dmg;
    public static Explosion Instance;
    
    public static IPoolable Prefab;
    float radiusExplosion;
    
    private int ExplosionsUntilTrueDamage=20;
    private int ExplosionsDone;
    private Image cooldownImage;
    
    public Explosion(float prob, int dmg){
        this.prob = prob;
        this.dmg = dmg;
        if(Instance == null){
            Instance = this;
            if(SkillTreeManager.Instance.getLevel("Explosion")>=1){
                Prefab = Resources.Load<GameObject>("Prefab/ExplosionOnDeathGiant").GetComponent<IPoolable>();
                radiusExplosion = 1.8f;
            }else{
                Prefab = Resources.Load<GameObject>("Prefab/ExplosionOnDeath").GetComponent<IPoolable>();
                radiusExplosion = 1.2f;
            }
            if(SkillTreeManager.Instance.getLevel("Explosion")>=2){
                cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/ExplodeUnlock"));
            }
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {
        
        if(Random.Range(0f,1f) < prob){
            

            Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radiusExplosion, Flamey.EnemyMask);
            ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y});
            AudioManager.PlayOneShot(FMODEvents.Instance.Explosion, Vector2.zero);

            foreach(Collider2D col in targets){
                col.GetComponent<Enemy>().Hitted(dmg, 1, ignoreArmor:ExplosionsDone>=ExplosionsUntilTrueDamage, onHit:false);
            }

            if(Character.Instance.isCharacter("Explosion")){
                ExplodeCampfire(Flamey.Instance.transform.position);
            }

            if(SkillTreeManager.Instance.getLevel("Explosion")>=2){
                ExplosionsDone++;
                if(ExplosionsDone >= ExplosionsUntilTrueDamage){
                    ExplosionsDone=0;
                }
                
                cooldownImage.fillAmount = ((float)ExplosionsDone)/ExplosionsUntilTrueDamage;
            }
            
            
        }
    }
    public void ExplodeCampfire(Vector2 pos){
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radiusExplosion, Flamey.EnemyMask);
        ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y});

        foreach(Collider2D col in targets){
            col.GetComponent<Enemy>().Hitted(dmg, 1, ignoreArmor:false, onHit:false);
        }
    }
    public void Stack(Explosion vampOnDeath){
        prob += vampOnDeath.prob;
        dmg += vampOnDeath.dmg;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= .5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ExplodeProb");
        } 
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(prob >= .5f && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Explosion");
            maxed = true;
        }
    }

    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of generating a <color=#FFCC7C>massive explosion</color> that <color=#FF5858>damages</color> nearby enemies";
    }
    public string getCaps()
    {
         return string.Format("Chance: {0}% (Max. 50%) <br>Damage: +{1}", Mathf.Round(prob * 100), dmg);
    }

    public string getIcon()
    {
        return "ExplodeUnlock";
    }

    public string getText()
    {
        return "Explosion";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class Necromancer : OnKillEffects
{
    

    public static int AtkTimes = 3;
    public float prob;
    public float dmgPerc;
    public static Necromancer Instance;
    static GameObject Prefab;
    static GameObject PrefabMega;
    public float MegaGhoulProbability;

    public Necromancer(float prob, float dmgPerc){
        this.prob = prob;
        this.dmgPerc = dmgPerc;
        if(Instance == null){
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/Ghoul");
            PrefabMega = Resources.Load<GameObject>("Prefab/MegaGhoul");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {
        
        if(Random.Range(0f,1f) < prob){
            if(MegaGhoulProbability > Random.Range(0f,1f)){
                Flamey.Instance.SpawnObject(PrefabMega).transform.position = pos;
            }else{
                Flamey.Instance.SpawnObject(Prefab).transform.position = pos;
            }
            
        }
    }
    public void Stack(Necromancer necromancer){
        prob += necromancer.prob;
        dmgPerc += necromancer.dmgPerc;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= .5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("NecroProb");
        } 
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(prob >= .5f && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Necro");
            maxed = true;
        }
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of summoning a <color=#FFCC7C>friendly ghoul</color>. Ghouls can attack enemies for up to <color=#FFCC7C>3 times</color> with a percentage of your <color=#FF5858>base damage.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Base Damage Ratio: {1}%", Mathf.Round(prob * 100), Mathf.Round(dmgPerc * 100));
    }

    public static int getAttackTimes(){
        
        switch(SkillTreeManager.Instance.getLevel("Necromancer")){
            case 1: return 5;
            case 2: return 7;
            case 0: default: return 3 ;
        }
    }
    public string getIcon()
    {
        return "NecroUnlock";
    }

    public string getText()
    {
        return "Necromancer";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class Bullets : OnKillEffects
{
    

    
    public float prob;
    public int dmg;
    public int amount;
    public static Bullets Instance;
    static IPoolable Prefab;

    public Bullets(float prob, int dmg, int amount){
        this.prob = prob;
        this.dmg = dmg;
        this.amount = amount;
        if(Instance == null){
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/Bullet").GetComponent<IPoolable>();
            if(SkillTreeManager.Instance.getLevel("Pirate") >= 2){ this.amount *= 2;}
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {
        
        if(Random.Range(0f,1f) < prob){
            
            SpawnBullets(pos);
            Flamey.Instance.addEmbers(10);
        }
    }
    private void SpawnBullets(Vector2 pos){
        AudioManager.PlayOneShot(FMODEvents.Instance.RoundShot, Vector2.zero);
        float randomRotation = Random.Range(0,360);
        
        if(SkillTreeManager.Instance.getLevel("Pirate") >= 1){

            for(int i = 0; i != amount; i++){
                Enemy e = Enemy.getClosestEnemy(pos, i);
                if(e!=null){
                    randomRotation = Vector2.SignedAngle( Vector2.up, (Vector2)e.HitCenter.position - pos);
                }
                ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y, randomRotation});
            }          
            
        }else{
            for(int i =0; i != amount; i++){
                ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y, i*(360/amount) + randomRotation});
            }
        }
       
        

    }
    public void Stack(Bullets necromancer){
        prob += necromancer.prob;
        dmg += necromancer.dmg;
        amount += necromancer.amount * (SkillTreeManager.Instance.getLevel("Pirate") >= 2 ? 2 : 1);
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= .5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BulletsProb");
        } 
        if(amount >= 6){
            amount = 6;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BulletsAmount");
        }
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(prob >= .5f && amount >= 6 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Pirate");
            maxed = true;
        }
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of shooting <color=#FFCC7C>Cannon Balls</color> out of the enemy's corpse, that deal damage and apply <color=#FF99F3>On-Hit effects</color> whenever they hit another creature. If this effect procs, you will also gain <color=#FFCC7C>+10 embers</color>. <color=#AFEDFF>Cannon Balls' speed</color> scales with <color=#AFEDFF>Bullet Speed";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Amount of Cannon Balls: {1} (Max. 6)<br>Damage: +{2}", Mathf.Round(prob*100f), amount, dmg);
    }

    public string getIcon()
    {
        return "BulletsUnlock";
    }

    public string getText()
    {
        return "Pirate";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}