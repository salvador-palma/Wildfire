using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public interface OnHittedEffects : Effect
{
    public bool addList();
    public void ApplyEffect(Enemy en = null);
}

public class ThornsOnHitted : OnHittedEffects
{
    public static ThornsOnHitted Instance;

    public float perc;
    public float prob;

    public ThornsOnHitted(float prob, float perc){
        
        this.prob = prob;
        this.perc = perc;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public bool addList()
    {
        return Instance == this;
    }

    public void ApplyEffect(Enemy en = null)
    {
        if(en==null){return;}

        if(Random.Range(0f,1f) < prob){
            if(Flamey.Instance.Armor == 0){return;}
            en.Hitted((int)(Flamey.Instance.Armor * perc), 10, ignoreArmor: false, onHit: true);
        }

    }
    public void Stack(ThornsOnHitted thornsOnHitted){
        
        prob += thornsOnHitted.prob;
        perc += thornsOnHitted.perc;
        RemoveUselessAugments();
    }
    public bool maxed;
    private void RemoveUselessAugments(){
        if(prob >= 1){
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ThornsProb");
        }      
        if(!maxed){CheckMaxed();}
    }

    private void CheckMaxed(){
        if(prob >= 1f){
            Character.Instance.SetupCharacter("Thorns");
        }
    }
    public string getDescription()
    {
        return "Everytime you get hit by an enemy you have a chance of returning a percentage of your <color=#919191>Armor</color> as <color=#FF5858>damage</color> back and applying <color=#FF99F3>On-Hit Effects";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 100%) <br>Armor to Damage Percentage: {1}%", Mathf.Round(prob*100f), Mathf.Round(perc*100f));
    }

    public string getIcon()
    {
        return "ThornsUnlock";
    }

    public string getText()
    {
        return "Thorns";
    }

    public string getType()
    {
        return "On-Hitted Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}
