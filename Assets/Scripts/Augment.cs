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
    public string[] Description;
    [HideInInspector] public Tier tier;
    [HideInInspector] public Sprite icon;
    [HideInInspector] public UnityAction[] actions;
    
    public string AugmentClass;
    bool baseStat;
    bool baseCard;
    bool baseCardUpgrade;
    public Augment(string augmentClass,string t, string[] d, string i, Tier ti, UnityAction[] a, bool baseStat = false, bool baseCard = false, bool baseCardUpgrade = false){
        Title = t;

        Description = d;
        actions = a;
        tier = ti;
        icon = Resources.Load<Sprite>("Icons/" + i);
        
        AugmentClass = augmentClass;
        this.baseStat = baseStat;
        this.baseCard = baseCard;
        this.baseCardUpgrade = baseCardUpgrade;
    }
    public void Activate(){
        if(Description.Length == 1){actions[0]();}else{
            actions[getLevel()]();
        }
    }
    
    public int getLevel(){
       return SkillTreeManager.Instance.getLevel(AugmentClass);
    }
    
    public string getDescription(){
        try{
            if(Description.Length == 1){return Description[0];}
            return Description[getLevel()];
        }catch{
            Debug.LogError("Error: " + AugmentClass + " at level " + getLevel() + " does not exist");
            return "";
        }
        
    }
    
    public string getNextDescription(){
        try{
            return Description[getLevel()+1];
        }catch{
            return "Max Level";
        }
        
    }
   

    public bool playable(){
        return baseStat || (baseCard && getLevel() > 0) || (baseCardUpgrade && getLevel() >= 0);
    }
    public string getAugmentClass(){return AugmentClass;}

    public bool isUnlockableMidGame(){
        return baseCard;
    }

    public SerializedAugment Serialize(){
        return new SerializedAugment(Title, getLevel());
    }
    public string getDescription(int lvl){
        try{
            return Description[lvl];
        }catch{
            return Description[0];
        }
        
    }
    public void Activate(int lvl){
        if(actions.Length == 1){
            actions[0]();
        }else{
            actions[lvl]();
        }
        
    }
}


