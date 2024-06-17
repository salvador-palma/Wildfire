using System;
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
            deck.removeClassFromDeck("LavaPoolProb");
        }
        if(size >= 2.5f){
            size = 2.5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("LavaPoolSize");
        }
        if(lasting >= 10){
            lasting = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("LavaPoolDuration");
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
        return "Whenever a shot lands, there's a chance of spawning a Lava Pool. The Lava Pool deals damage per second to enemies stepping on it, ignoring Armor completely.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Pool Size: {1} units (Max. 25 units)<br>Pool Duration: {2}s (Max. 10s)<br>Damage: {3}/s", Mathf.Round(prob*100f), size*10, lasting , damage);
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
        if(UnityEngine.Random.Range(0f,1f) < prob){
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
            deck.removeClassFromDeck("IcePoolProb");
        }
        if(slow >= 0.5F){
            slow = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("IcePoolSlow");
        }
        if(size >= 2.5f){
            size = 2.5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("IcePoolSize");
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
        return "Whenever a shot lands, there's a chance of spawning an Ice Pool. Ice Pools slow down enemies stepping on it for " + Mathf.Round(slow*100) + "%. Ice Pools' slow does not stack with other Ice Pools.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Pool Size: {1} units (Max. 25 units)<br>Pool Duration: {2}s (Max. 10s)<br>Slow: {3}% (Max. 50%)", Mathf.Round(prob*100f), size*10, lasting , Mathf.Round(slow*100));
    }

    public string getIcon()
    {
        return "IcePoolUnlock";
    }
}

public class DrainOnLand : OnLandEffect
{
    public static DrainOnLand Instance;
    public GameObject prefab;
    public float size;
    public float perc;
    public float timegap;
    public float prob;
    public float lasting;
    public DrainOnLand(float size, float perc, float prob, float lasting){
        this.prob = prob;
        this.size = size;
        this.perc = perc;
        this.lasting = lasting;
       
        if(Instance == null){
            Instance = this;
            prefab = Resources.Load<GameObject>("Prefab/DrainAOE");
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
    public void Stack(DrainOnLand burnOnLand){
        size += burnOnLand.size;
        prob += burnOnLand.prob;
        perc += burnOnLand.perc;
        lasting += burnOnLand.lasting;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 0.5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("DrainPoolProb");
        }
        if(size >= 2.5f){
            size = 2.5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("DrainPoolSize");
        }
        if(lasting >= 10){
            lasting = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("DrainPoolDuration");
        }
        if(perc >= 1f){
            perc = 1f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("DrainPoolDuration");
        }
        
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Flower Field";
    }

    public string getType()
    {
        return "On-Land Effect";
    }

    public string getDescription()
    {
        return "Whenever a shot lands, there's a chance of sprouting a Flower. Everytime an enemy steps on a flower, you heal part of their Max HP";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Flower Size: {1} units (Max. 25 units)<br>Flower Lifespan: {2}s (Max. 10s)<br>Enemy Max HP drained: {3}/s", Mathf.Round(prob*100f), size*10, lasting , Mathf.Round(perc*100f));
    }

    public string getIcon()
    {
        return "DrainPoolUnlock";
    }
}
