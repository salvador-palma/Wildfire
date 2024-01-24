using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

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
            en.HittedArmorless((int)(Flamey.Instance.Armor * perc), 10);
        }

    }
    public void Stack(ThornsOnHitted thornsOnHitted){
        prob += thornsOnHitted.prob;
        perc += thornsOnHitted.perc;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 1){
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Prickly Fire");
            deck.removeFromDeck("Thorns everywhere!");
            deck.removeFromDeck("Cactus on Fire");
        }      
    }
    public string getDescription()
    {
        return "Everytime you get hit by an enemy you have a " + prob*100f + "% chance of returning " + perc*100f+ "% of your Armor as damage back. This effect ignores enemy armor completely";
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
}
