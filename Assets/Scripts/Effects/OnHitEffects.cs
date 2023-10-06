using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public void ApplyEffect(float dmg = 0, float health = 0);
    
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
    public void ApplyEffect(float dmg, float health = 0)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            Flamey.Instance.addHealth(dmg * perc);
        }
    }
    public void Stack(VampOnHit vampOnHit){
        perc += vampOnHit.perc;
        prob += perc;
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
