
using System;
using UnityEngine;

public interface TimeBasedEffect : Effect
{
    public bool addList();
    public void ApplyEffect();
    public void ApplyRound();
}


public class HealthRegen : TimeBasedEffect
{
    public static HealthRegen Instance;

    public float perSec;
    public float perRound;

    public int tickAmount;
    public HealthRegen(float perSec, float perRound){
        this.perSec = perSec;
        this.perRound = perRound;
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

    public void ApplyEffect()
    {
        if(tickAmount <= 0){
            tickAmount = 4;
            if(Flamey.Instance.Health < Flamey.Instance.MaxHealth){
                Flamey.Instance.addHealth(perSec);
            }
            
        }else{
            tickAmount--;
        }
        
    }
    public void ApplyRound()
    {
        Flamey.Instance.addHealth(perRound);
    }

    public void Stack(HealthRegen healthRegen){
        perSec += healthRegen.perSec;
        perRound += healthRegen.perRound;
        
    }
    
    public string getDescription()
    {
        return "You can regenerate health per second and at the end of each round";
    }
    public string getCaps()
    {
        return string.Format("Regen/s: {0}/s <br>Regen/round: {1}/round", Mathf.Round(perSec * 100.0f) * 0.01f, Mathf.Round(perRound * 100.0f) * 0.01f);
    }

    public string getIcon()
    {
        return "Regen";
    }

    public string getText()
    {
        return "Health Regen";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
}

public class LightningEffect : TimeBasedEffect

{
    public static LightningEffect Instance;
    public static GameObject lightning;
    public int interval;
    int current_interval;
    public int dmg;


    public LightningEffect(int interval, int dmg, float percRed){
        this.dmg = dmg;
        this.interval = interval;
        if(Instance == null){
            Instance = this;
            lightning = Resources.Load<GameObject>("Prefab/Lightning");
        }else{
            Instance.Stack(this, percRed);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        if(current_interval <=0 ){
            current_interval = interval;
            for(int i = 0; i < 3; i++)
            {
                Vector2 v = Flamey.Instance.getRandomHomingPosition();
                GameObject go =  Flamey.Instance.SpawnObject(lightning);
                go.transform.position = new Vector2(v.x, v.y + 10.91f);
            }
            

        }else{
            current_interval--;
        }
    }
    public void ApplyRound(){}

    public void Stack(LightningEffect lightningEffect, float percRed){
        if(percRed > 0){
            if((int)Math.Ceiling(interval * (1-percRed)) == interval){
                interval-=1;
            }else{
                interval = (int)Math.Ceiling(interval * (1-percRed));
            }
            
        }
        
        dmg += lightningEffect.dmg;
        RemoveUselessAugments();
    }

    void RemoveUselessAugments(){
        if(interval <= 1){
            interval = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ThunderInterval");
        } 
    }
    
    public string getDescription()
    {
        return "Each few amount of seconds, 3 thunders will spawn at a convenient location dealing damage to enemies struck by it. This ability applies On-Land Effects";
    }
    public string getCaps()
    {
        return string.Format("Interval: {0}s (Min 0.25s)<br>Damage: +{1}", Mathf.Round((float)interval/4 * 100.0f) * 0.01f, dmg);
    }

    public string getIcon()
    {
        return "ThunderUnlock";
    }

    public string getText()
    {
        return "Thunder";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
}

public class Immolate : TimeBasedEffect
{
    public static Immolate Instance;
    public static GameObject ring;
    public int interval;
    int current_interval;
    public int dmg;
    public float radius;

    public Immolate(int interval, int dmg, float radius, float percRed){
        this.dmg = dmg;
        this.interval = interval;
        this.radius = radius;
        if(Instance == null){
            Instance = this;
            ring = Resources.Load<GameObject>("Prefab/Ring");
        }else{
            Instance.Stack(this, percRed);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        if(current_interval <=0 ){
            current_interval = interval;
            
            Flamey.Instance.SpawnObject(ring);
            

        }else{
            current_interval--;
        }
    }
    public void ApplyRound(){}

    public void Stack(Immolate immolate, float percRed){
        if(percRed > 0){
            if((int)Math.Ceiling(interval * (1-percRed)) == interval){
                interval-=1;
            }else{
                interval = (int)Math.Ceiling(interval * (1-percRed));
            }
            
        }
        
        dmg += immolate.dmg;
        radius += immolate.radius;
        RemoveUselessAugments();
    }

    void RemoveUselessAugments(){
        if(interval <= 16){
            interval = 16;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ImmolateInterval");
        } 
        if(radius >= 2f){
            radius = 2f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ImmolateRadius");
        }
    }
    
    public string getDescription()
    {
        return "Each few amount of seconds, you will release a wave of energy that travels through the campsite dealing damage to enemies caught by it";
    }
    public string getCaps()
    {
        return string.Format("Interval: {0}s (Min. 4s)<br>Travel Radius: {1} units (Max 200 units)<br>Damage: +{1}", Mathf.Round((float)interval/4 * 100.0f) * 0.01f, Mathf.Round(radius*100), dmg);
    }

    public string getIcon()
    {
        return "ImmolateUnlock";
    }

    public string getText()
    {
        return "Immolate";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
}

