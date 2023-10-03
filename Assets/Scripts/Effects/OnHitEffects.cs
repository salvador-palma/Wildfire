using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public interface Effect{
    public string getText();
    public string getType();
    public string getDescription();
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
    public VampOnHit(float perc){
        
        this.perc = perc;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public void ApplyEffect(float dmg, float health = 0)
    {
        Flamey.Instance.addHealth(dmg * perc);
    }
    public void Stack(VampOnHit vampOnHit){
        perc += vampOnHit.perc;
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "VampFire";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "Heal " + perc + "% of the damage you deal each shot. This applies to critical damage aswell.";
    }
}
