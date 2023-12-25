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
    
    string AugmentClass;
    bool baseStat;
    bool baseCard;
    public Augment(string augmentClass,string t, string[] d, string i, Tier ti, UnityAction[] a, bool baseStat = false, bool baseCard = false){
        Title = t;

        Description = d;
        actions = a;
        tier = ti;
        icon = Resources.Load<Sprite>(i);
        
        AugmentClass = augmentClass;
        this.baseStat = baseStat;
        this.baseCard = baseCard;
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
        return baseStat || (baseCard && getLevel() > 0);
    }
    public string getAugmentClass(){return AugmentClass;}
}


