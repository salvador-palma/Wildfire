using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            deck.removeFromDeck("Kill to Heal");
            deck.removeFromDeck("Corpse Conduit");
            deck.removeFromDeck("Reaper's Reward");
        } 
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a " + prob*100 + "% chance of healing for " + perc*100 + "% of its max health";
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
        if(Random.Range(0f,1f) < prob){
            Collider2D[] targets = Physics2D.OverlapCircleAll(en.transform.position, 2.5f, FlareManager.EnemyMask);
            Flamey.Instance.SpawnObject(Prefab).transform.position = en.transform.position;
            foreach(Collider2D col in targets){
                col.GetComponent<Enemy>().Hitted(dmg, 1);
            }
            
        }
    }
    public void Stack(Explosion vampOnDeath){
        prob += vampOnDeath.prob;
        dmg += vampOnDeath.dmg;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 1f){
            prob = 1;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Bomb Rush");
            deck.removeFromDeck("Grenade Launcher");
            deck.removeFromDeck("Bombardment");
        } 
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a " + prob*100 + "% chance of generating an explosion that deals " + dmg + " damage to surrounding enemies";
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
    

    public static int AtkTimes = 5;
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
        if(Random.Range(0f,1f) < prob){
            Flamey.Instance.SpawnObject(Prefab).transform.position = en.transform.position;
        }
    }
    public void Stack(Necromancer necromancer){
        prob += necromancer.prob;
        dmgPerc += necromancer.dmgPerc;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 0.5f){
            prob = 0.5f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Wraith Walkers");
            deck.removeFromDeck("Soul Shepard");
            deck.removeFromDeck("Crypt of the Necromancer");
        } 
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a " + prob*100 + "% chance of summoning a friendly ghoul. Ghouls can attack enemies for up to 5 times with " + dmgPerc*100 + "% of your damage.";
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
        if(Random.Range(0f,1f) < prob){
            
            SpawnBullets(en.transform.position);
            Flamey.Instance.addEmbers(25);
        }
    }
    private void SpawnBullets(Vector2 pos){
        int randomRotation = Random.Range(0,360);
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
        if(prob >= 1f){
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Pirate Wannabe");
            deck.removeFromDeck("Yes, Captain!");
            deck.removeFromDeck("Shoot it, Loot it");
        } 
        if(amount >= 6){
            amount = 6;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Cannonball Pile");
        }
    }
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a " + prob*100 + "% chance of releasing " + amount + " bullets that deal " + dmg + " and apply On-Hit Effects. \n if this effect procs, you will also gain +25 embers";
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