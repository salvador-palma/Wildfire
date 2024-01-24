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
            deck.removeFromDeck("Kill to Heal");
            deck.removeFromDeck("Corpse Conduit");
            deck.removeFromDeck("Reaper's Reward");
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
