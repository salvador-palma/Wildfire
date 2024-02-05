using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        return "Whenever a shot lands, there's a " + Mathf.Round(prob * 100) + "% chance of spawning a Lava Pool with x" + size + " size that lasts for " + lasting + " seconds . The Lava Pool deals " + damage + " damage per second to enemies stepping on it.";
    }

    public string getIcon()
    {
        return "LavaPoolUnlock";
    }
}


public class IceOnLand : OnLandEffect
{
    public static IceOnLand Instance;
    public GameObject prefab;
    public float size;
    public float slow;
    public float timegap;
    public float prob;
    public float lasting;
    public IceOnLand(float size, float slow, float prob, float lasting){
        this.prob = prob;
        this.size = size;
        this.slow = slow;
        this.lasting = lasting;
       
        if(Instance == null){
            Instance = this;
            prefab = Resources.Load<GameObject>("Prefab/IceAOE");
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(Vector2 pos)
    {
        if(Random.Range(0f,1f) < prob){
            GameObject go = Flamey.Instance.SpawnObject(prefab);
            go.transform.position = pos;
        }
    }
    public void Stack(IceOnLand iceOnLand){
        size += iceOnLand.size;
        prob += iceOnLand.prob;
        slow += iceOnLand.slow;
        lasting += iceOnLand.lasting;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 0.5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Cold Steps");
            deck.removeFromDeck("Ice here, Ice there");
            deck.removeFromDeck("The North Pole");
        }
        if(slow >= 0.5F){
            slow = .5f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Glacial Grip");
            deck.removeFromDeck("Frozen Stasis");
            deck.removeFromDeck("Cold Bath");
        }
        if(size >= 2.5f){
            size = 2.5f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Cold breeze");
            deck.removeFromDeck("Frozen Lakes");
            deck.removeFromDeck("Inside the iceberg");
        }
        
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Ice Pool";
    }

    public string getType()
    {
        return "On-Land Effect";
    }

    public string getDescription()
    {
        return "Whenever a shot lands, there's a " + Mathf.Round(prob * 100) + "% chance of spawning an Ice Pool with x" + size + " size that lasts for " + lasting + " seconds . The Ice Pool slows down enemies stepping on it for " + Mathf.Round(slow*100) + "%";
    }

    public string getIcon()
    {
        return "IcePoolUnlock";
    }
}