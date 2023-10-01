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
    public UnityAction action;

    public Augment(string t, string d, Tier ti, UnityAction a){
        Title = t;
        Description = d;
        action = a;
        tier = ti;
    }
    public void Activate(){
        action();
    }
}
