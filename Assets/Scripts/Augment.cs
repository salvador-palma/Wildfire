using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Tier{
    Silver = 0,
    Gold = 1,
    Prismatic = 2
}

[Serializable]
public class Augment
{
    public string Title;
    public string Description;
    [HideInInspector] public Tier tier;
    [HideInInspector] public Sprite icon;
    [HideInInspector] public UnityAction action;
    
    public string AugmentClass;
    bool baseStat;
    bool baseCard;
    bool baseCardUpgrade;
    public Augment(string augmentClass,string title, string desc, string ic, Tier ti, UnityAction action, bool baseStat = false, bool baseCard = false, bool baseCardUpgrade = false){
        Title = title;

        Description = desc;
        this.action = action;
        tier = ti;
        icon = Resources.Load<Sprite>("Icons/" + ic);
        
        AugmentClass = augmentClass;
        this.baseStat = baseStat;
        this.baseCard = baseCard;
        this.baseCardUpgrade = baseCardUpgrade;
    }
    public void Activate(){
        action();
    }
    
    public string getDescription(){
        return Description;
    }

    public bool playable(){
        return baseStat || ((baseCard||baseCardUpgrade) && SkillTreeManager.Instance.getLevel(AugmentClass) >= 0);
    }
    public string getAugmentClass(){return AugmentClass;}

    public bool isUnlockableMidGame(){
        return baseCard;
    }

    public SerializedAugment Serialize(){
        return new SerializedAugment(Title);
    }
    public string getDescription(int lvl){
        return Description;
        
    }
    public void Activate(int lvl){
        action();
        
    }
}


