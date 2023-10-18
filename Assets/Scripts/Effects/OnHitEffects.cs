using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public interface Effect{
    public string getText();
    public string getType();
    public string getDescription();
    public string getIcon();
}
public interface OnHitEffects: Effect
{
    

    public bool addList();
    public void ApplyEffect(float dmg = 0, float health = 0, Enemy en = null);
    
}
public class VampOnHit : OnHitEffects
{
    public static VampOnHit Instance;
    public float perc;
    public float prob;
    public VampOnHit(float perc, float prob){
        
        this.perc = perc;
        this.prob = prob;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            Flamey.Instance.addHealth(dmg * perc);
        }
    }
    public void Stack(VampOnHit vampOnHit){
        perc += vampOnHit.perc;
        prob += vampOnHit.prob;
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "The Blood Mage";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "You have a " + prob*100 + "% chance per shot of healing " + perc*100 + "% of the damage you dealt. This applies for critical damage aswell.";
    }

    public string getIcon()
    {
        return "vampfire";
    }
}

public class IceOnHit : OnHitEffects

{
    private Color IceColor;
    public static IceOnHit Instance;
    public int duration;
    public float prob;
    public IceOnHit(int duration, float prob){
       
        this.duration = duration;
        this.prob = prob;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public async void ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null){return;}
        if(en.inEffect){return;}
        if(UnityEngine.Random.Range(0f,1f) < prob){
            
            en.inEffect = true;
            en.GetComponent<Animator>().Play("EnemyEffectice");
            float f = en.Speed;
            float percentage = Mathf.Min(0.99f,Flamey.Instance.MaxHealth * 0.00033f);
            
            en.Speed -= en.Speed * percentage;
            DamageUI.Instance.spawnTextDmg(en.transform.position, "SLOWED", 4);
            await Task.Delay(duration);
            if(en == null){return;}
            en.GetComponent<Animator>().Play("EnemyEffectClear");
            
            
            en.Speed = f;
            en.inEffect = false;
        }
    }
    public void Stack(IceOnHit iceOnHit){
        duration += iceOnHit.duration;
        prob += iceOnHit.prob;
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Ice Soul";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        
        float percentage = Mathf.Min(0.99f,Flamey.Instance.MaxHealth * 0.00033f);
        
        return "You have a " + prob*100 + "% chance per shot of slowing the enemy for " + percentage * 100 + "% of it's speed for a duration of "+ duration/1000 + " seconds. This effect scales with Max Health (+1% slow per 20 Extra Max Health)" ;
    }

    public string getIcon()
    {
        return "ice";
    }
}

public class ShredOnHit : OnHitEffects
{
    
    public static ShredOnHit Instance;
    public float percReduced;
    public float prob;
    public ShredOnHit(float prob, float percReduced){
       
        this.percReduced = percReduced;
        this.prob = prob;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null){return;}
        
        if(UnityEngine.Random.Range(0f,1f) < prob){
            en.Armor -=  (int)(en.Armor *  percReduced);
            Debug.Log("Shred");
        }
    }
    public void Stack(ShredOnHit shredOnHit){
        percReduced += shredOnHit.percReduced;
        prob += shredOnHit.prob;
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Shredding Flames";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {

        return "You have a " + prob*100 + "% chance per shot of reducing the target's armor for " + percReduced * 100 + "%" ;
    }

    public string getIcon()
    {
        return "shred";
    }
}

public class ExecuteOnHit : OnHitEffects
{
    
    public static ExecuteOnHit Instance;
    public float percToKill;
    public ExecuteOnHit(float percToKill){
       
        this.percToKill = percToKill;
        
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public async void  ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null){return;}
        if(en.Health < en.MaxHealth * percToKill){
            float f = en.Health;
            Vector2 v = en.transform.position;
            en.Health = 0;
            await Task.Delay(50);
            DamageUI.Instance.spawnTextDmg(v, "EXECUTED",5);
        }
        
    }
    public void Stack(ExecuteOnHit executeOnHit){
        percToKill = Mathf.Min(0.5f, percToKill + executeOnHit.percToKill);
        
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Assassin's Path";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "You penetrate through " + Flamey.Instance.ArmorPen * 100 + "% of enemy armor. When you hit an enemy below " + percToKill*100 + "% of it's Max Health, you execute them." ;
    }

    public string getIcon()
    {
        return "assassin";
    }
}

public class StatikOnHit : OnHitEffects
{
    
    public static StatikOnHit Instance;
    public GameObject prefab;
    public float prob;
    public int dmg;
    public int ttl;
    public StatikOnHit(float prob, int dmg, int ttl){
        
        this.prob = prob;
        this.dmg = dmg;
        this.ttl = ttl;
        prefab = Resources.Load<GameObject>("Prefab/StatikShiv");
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null){return;}
        
        if(UnityEngine.Random.Range(0f,1f) < prob){
            GameObject g = Flamey.Instance.SpawnObject(prefab);
            g.transform.position = en.HitCenter.position;
            StatikShiv s = g.GetComponent<StatikShiv>();
            s.TTL = ttl;
            s.currentTarget = en;
            s.locationOfEnemy = en.HitCenter.position;
            s.Started = true;
        }
        
    }
    public void Stack(StatikOnHit statikOnHit){
        prob += statikOnHit.prob;
        dmg += statikOnHit.dmg;
        ttl += statikOnHit.ttl;
        
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Statik Energy";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "When you hit an enemy, you have a chance of " + prob * 100 + "% of unleashing a statik chain that passes through a max of " + ttl + " enemies near by, dealing " +dmg+" damage to each.";
    }

    public string getIcon()
    {
        return "statik";
    }
}