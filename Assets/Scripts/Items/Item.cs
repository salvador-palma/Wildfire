using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Item : MonoBehaviour
{

    public static Dictionary<Item, int> Items;

    [SerializeField] public string Name;
    [SerializeField] public string[] Unlocks;
    [SerializeField] bool initial;
    
    public int level;

    public int MinimumPrice;
    [SerializeField] public Dialogue[] presentation;

    private void Start() {
        if(Items == null){Items = new Dictionary<Item, int>();}

        level = GameVariables.GetVariable(Name + " Item");
        if(level == -1 && initial){
            level = 0;
            GameVariables.SetVariable(Name + " Item" , level);
        }
        Items[this] = level;
        if(level<=-1){gameObject.SetActive(false);}
    }
    
    public void Purchase(){

        gameObject.SetActive(false);
        Unlock();
    }
    public void Unlock(){
        level = 1;
        GameVariables.SetVariable(Name + " Item" , level);
    }

    static int getLevel(string name){
        return Items.Where(k => k.Key.Name == name).FirstOrDefault().Value;
    }

}
