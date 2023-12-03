using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnLandEffect: Effect
{
    public bool addList();
    public void ApplyEffect(Vector2 pos);
    
}

public class BurnOnLand : OnLandEffect
{
    public static BurnOnLand Instance;
    public GameObject prefab;
    public float size;
    public int damage;
    public float timegap;
    public float prob;
    public float lasting;
    public BurnOnLand(float size, int damage, float prob, float lasting){
        this.prob = prob;
        this.size = size;
        this.damage = damage;
        this.lasting = lasting;
       
        if(Instance == null){
            Instance = this;
            prefab = Resources.Load<GameObject>("Prefab/BurnAOE");
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(Vector2 pos)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            GameObject go = Flamey.Instance.SpawnObject(prefab);
            go.transform.position = pos;
        }
    }
    public void Stack(BurnOnLand burnOnLand){
        size += burnOnLand.size;
        prob += burnOnLand.prob;
        damage += burnOnLand.damage;
        lasting += burnOnLand.lasting;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 0.5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Hot Steps");
            deck.removeFromDeck("Lava here, Lava there");
            deck.removeFromDeck("The Apocalypse");
        }
        if(size >= 2.5f){
            size = 2.5f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Heat Area");
            deck.removeFromDeck("Lava Lakes");
            deck.removeFromDeck("Inside the volcano");
        }
        
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Lava Pool";
    }

    public string getType()
    {
        return "On-Land Effect";
    }

    public string getDescription()
    {
        return "Whenever a shot lands, there's a " + prob * 100 + "% chance of spawning a Lava Pool with x" + size + " size that lasts for " + lasting + " seconds . The Lava Pool deals " + damage + " damage per second to enemies stepping on it.";
    }

    public string getIcon()
    {
        return "lava";
    }
}