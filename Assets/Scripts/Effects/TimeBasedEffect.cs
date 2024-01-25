using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Flamey.Instance.addHealth(perSec);
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
        return "You regen " + perSec + " health per second and " + perRound + " at the end of each round";
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

