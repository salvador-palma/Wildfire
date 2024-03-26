using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface OnKillEffects : Effect
{
    public bool addList();
    public void ApplyEffect(Enemy en = null);
}

public class VampOnDeath : OnKillEffects
{
    public float prob;
    public float perc;
    public static VampOnDeath Instance;

    public VampOnDeath(float prob, float perc){
        this.prob = prob;
        this.perc = perc;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Enemy en = null)
    {
        if(Random.Range(0f,1f) < prob){
            Flamey.Instance.addHealth(en.MaxHealth * perc);
        }
    }
    public void Stack(VampOnDeath vampOnDeath){
        prob += vampOnDeath.prob;
        perc += vampOnDeath.perc;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 1f){
            prob = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("VampDeathProb");
        } 
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of healing yourself for a percentage of its max health";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 100%) <br>Percentage Healed: {1} (Max. 100%)", Mathf.Round(prob * 100), Mathf.Round(perc * 100));
    }

    public string getIcon()
    {
        return "VampDeathUnlock";
    }

    public string getText()
    {
        return "Essence Eater";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
}

public class Explosion : OnKillEffects
{
    public float prob;
    public int dmg;
    public static Explosion Instance;
    public static GameObject Prefab;

    public Explosion(float prob, int dmg){
        this.prob = prob;
        this.dmg = dmg;
        if(Instance == null){
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/ExplosionOnDeath");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Enemy en = null)
    {
        if(en==null){return;}
        if(UnityEngine.Random.Range(0f,1f) < prob){
            Collider2D[] targets = Physics2D.OverlapCircleAll(en.transform.position, 1.8f, FlareManager.EnemyMask);
            Flamey.Instance.SpawnObject(Prefab).transform.position = en.transform.position;
            foreach(Collider2D col in targets){
                col.GetComponent<Enemy>().Hitted(dmg, 1, ignoreArmor:false, onHit:false);
            }
            
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
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of generating a massive explosion that damages surrounding enemies";
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
}


public class Necromancer : OnKillEffects
{
    

    public static int AtkTimes = 3;
    public float prob;
    public float dmgPerc;
    public static Necromancer Instance;
    static GameObject Prefab;

    public Necromancer(float prob, float dmgPerc){
        this.prob = prob;
        this.dmgPerc = dmgPerc;
        if(Instance == null){
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/Ghoul");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Enemy en = null)
    {
        if(en==null){return;}
        if(UnityEngine.Random.Range(0f,1f) < prob){
            Flamey.Instance.SpawnObject(Prefab).transform.position = en.transform.position;
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
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of summoning a friendly ghoul. Ghouls can attack enemies for up to 3 times with a percentage of your base damage.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Base Damage Ratio: {1}%", Mathf.Round(prob * 100), Mathf.Round(dmgPerc * 100));
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
}


public class Bullets : OnKillEffects
{
    

    
    public float prob;
    public int dmg;
    public int amount;
    public static Bullets Instance;
    static GameObject Prefab;

    public Bullets(float prob, int dmg, int amount){
        this.prob = prob;
        this.dmg = dmg;
        this.amount = amount;
        if(Instance == null){
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/Bullet");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Enemy en = null)
    {
        if(en==null){return;}
        if(UnityEngine.Random.Range(0f,1f) < prob){
            
            SpawnBullets(en.transform.position);
            Flamey.Instance.addEmbers(25);
        }
    }
    private void SpawnBullets(Vector2 pos){
        int randomRotation = UnityEngine.Random.Range(0,360);
        for(int i =0; i != amount; i++){
            GameObject go = Flamey.Instance.SpawnObject(Prefab);
            go.transform.position = pos;
            go.transform.rotation = Quaternion.Euler(0,0,i*(360/amount) + randomRotation);
            
        }

    }
    public void Stack(Bullets necromancer){
        prob += necromancer.prob;
        dmg += necromancer.dmg;
        amount += necromancer.amount;
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
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of shooting Cannon Balls out of the enemy's corpse, that deal damage and apply On-Hit effects whenever they hit another creature. If this effect procs, you will also gain +10 embers. Cannon Balls' speed scales with Bullet Speed";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 100%) <br>Amount of Cannon Balls: {1} (Max. 6)<br>Damage: +{2}", Mathf.Round(prob*100f), amount, dmg);
    }

    public string getIcon()
    {
        return "BulletsUnlock";
    }

    public string getText()
    {
        return "Bullet Shooter";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
}