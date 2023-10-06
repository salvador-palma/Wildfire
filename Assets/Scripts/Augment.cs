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
    public string Description;
    public Tier tier;
    public Sprite icon;
    public UnityAction action;

    public Augment(string t, string d, string i, Tier ti, UnityAction a){
        Title = t;
        Description = d;
        action = a;
        tier = ti;
        icon = Resources.Load<Sprite>(i);
    }
    public void Activate(){
        action();
    }
}
