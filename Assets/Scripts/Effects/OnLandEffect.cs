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
    public IPoolable prefab;
    public GameObject prefabEverlast;
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
            prefab = Resources.Load<GameObject>("Prefab/BurnAOE").GetComponent<IPoolable>();
            prefabEverlast = Resources.Load<GameObject>("Prefab/AbilityCharacter/BurnAOE Everlasting");

        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(Vector2 pos)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            ObjectPooling.Spawn(prefab, new float[]{pos.x, pos.y});
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
        
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(prob >= .5f && size >= 2.5f && lasting >= 10 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Lava");
            maxed = true;
        }
    }
    public void SpawnExtraAssets(){
        Flamey.Instance.SpawnObject(prefabEverlast);
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
        return "Whenever a shot lands, there's a chance of spawning a <color=#FFCC7C>Lava Pool</color>. The <color=#FFCC7C>Lava Pool</color> deals damage per second to enemies <color=#FFCC7C>stepping</color> on it, ignoring <color=#919191>Armor</color> completely.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Pool Size: {1} units (Max. 25 units)<br>Pool Duration: {2}s (Max. 10s)<br>Damage: {3}/s", Mathf.Round(prob*100f), size*10, lasting , damage);
    }

    public string getIcon()
    {
        return "LavaPoolUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class IceOnLand : OnLandEffect
{
    public static IceOnLand Instance;
    public IPoolable prefab;
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
            prefab = Resources.Load<GameObject>("Prefab/IceAOE").GetComponent<IPoolable>();
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(Vector2 pos)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            ObjectPooling.Spawn(prefab, new float[]{pos.x, pos.y});
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
        if(slow >= 0.5f){
            slow = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("IcePoolSlow");
        }
        if(size >= 2.5f){
            size = 2.5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("IcePoolSize");
        }
        if(lasting >= 10){
            lasting = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("IcePoolDuration");
        }
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(prob >= .5f && size >= 2.5f && slow >= 0.5f && lasting >= 10 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Snow Pool");
            maxed = true;
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Snow Pool";
    }

    public string getType()
    {
        return "On-Land Effect";
    }

    public string getDescription()
    {
        return "Whenever a shot lands, there's a chance of spawning an <color=#FFCC7C>Ice Pool</color>. <color=#FFCC7C>Ice Pools</color> <color=#AFEDFF>slow</color> down enemies <color=#FFCC7C>stepping</color> on it for " + Mathf.Round(slow*100) + "%. <color=#FFCC7C>Ice Pools'</color> <color=#AFEDFF>slow</color> does not stack with other <color=#FFCC7C>Ice Pools.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Pool Size: {1} units (Max. 25 units)<br>Pool Duration: {2}s (Max. 10s)<br>Slow: {3}% (Max. 50%)", Mathf.Round(prob*100f), size*10, lasting , Mathf.Round(slow*100));
    }

    public string getIcon()
    {
        return "IcePoolUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class DrainOnLand : OnLandEffect
{
    public static DrainOnLand Instance;
    public IPoolable prefab;
    public IPoolable prefabCarnivore;
    public float size;
    public float perc;
    public float timegap;
    public float prob;
    public float lasting;
    public float carnivoreChance;
    public DrainOnLand(float size, float perc, float prob, float lasting){
        this.prob = prob;
        this.size = size;
        this.perc = perc;
        this.lasting = lasting;
       
        if(Instance == null){
            Instance = this;
            prefab = Resources.Load<GameObject>("Prefab/DrainAOE").GetComponent<IPoolable>();
            prefabCarnivore = Resources.Load<GameObject>("Prefab/DrainAOECarnivore").GetComponent<IPoolable>();
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(Vector2 pos)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            if(Character.Instance.isCharacter("Flower Field") && UnityEngine.Random.Range(0f,1f) < carnivoreChance){
                ObjectPooling.Spawn(prefabCarnivore, new float[]{pos.x, pos.y});
            }else{
                ObjectPooling.Spawn(prefab, new float[]{pos.x, pos.y});
            }
            
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
        if(prob >= .25f){
            prob = .25f;
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
        
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(prob >= .25f && size >= 2.5f && lasting >= 10 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Flower Field");
            
            maxed = true;
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
        return "Whenever a shot lands, there's a chance of sprouting a <color=#FFCC7C>Flower</color>. Everytime an enemy <color=#FFCC7C>steps</color> on a <color=#FFCC7C>flower</color>, you heal part of their <color=#0CD405>Max HP";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 50%) <br>Flower Size: {1} units (Max. 25 units)<br>Flower Lifespan: {2}s (Max. 10s)<br>Enemy Max HP drained: {3}%/s", Mathf.Round(prob*100f), size*10, lasting , Mathf.Round(perc*100f));
    }

    public string getIcon()
    {
        return "DrainPoolUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}
