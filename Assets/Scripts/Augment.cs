using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Augment
{
    public string Title;
    public string Description;
    public UnityAction action;

    public Augment(string t, string d, UnityAction a){
        Title = t;
        Description = d;
        action = a;
    }
    public void Activate(){
        action();
    }
}
