using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Tier{
    Silver,
    Gold,
    Prismatic
}

[Serializable]
public class Augment
{
    public string Title;
    public string[] Description;
    [HideInInspector] public Tier tier;
    [HideInInspector] public Sprite icon;
    [HideInInspector] public UnityAction[] actions;
    [SerializeField] int level;    
    string AugmentClass;
    bool baseStat;
    bool baseCard;
    public Augment(string augmentClass,string t, string[] d, string i, Tier ti, UnityAction[] a, bool baseStat = false, bool baseCard = false){
        Title = t;

        Description = d;
        actions = a;
        tier = ti;
        icon = Resources.Load<Sprite>(i);
        level = baseStat ? 0 : -1;
        AugmentClass = augmentClass;
        this.baseStat = baseStat;
        this.baseCard = baseCard;
    }
    public void Activate(){
        actions[level]();
    }

    public void Upgrade(){
        if(level == actions.Length - 1){return;}
        level++;
    }
    public string getDescription(){
        return Description[level];
    }
   

    public bool playable(){
        return baseStat || (baseCard && level >= 0);
    }
    public string getAugmentClass(){return AugmentClass;}
}


