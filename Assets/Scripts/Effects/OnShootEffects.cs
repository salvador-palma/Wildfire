using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface OnShootEffects : Effect
{
    public bool addList();
    public int ApplyEffect();

}

public class SecondShot : OnShootEffects{


    public static SecondShot Instance;
    public float perc;
    public SecondShot(float p){
        perc = p;
        Debug.Log(p);
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        if(UnityEngine.Random.Range(0f,1f) < perc){
            
            ShootWithDelay();
        }
        return 0;
    }
    private async void ShootWithDelay(){
        await Task.Delay(100);
        Flamey.Instance.InstantiateShot(new List<string>(){"MultiCaster"});
    }

    public void Stack(SecondShot secondShot){
        perc += secondShot.perc;
        RemoveUselessAugments();
    }

    private void RemoveUselessAugments(){
        if(perc > 1){
            perc = 1;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("The more the better");
            deck.removeFromDeck("Double trouble");
            deck.removeFromDeck("Casting Cascade");
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Multicaster";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Whenever you fire a shot, there's a " + Mathf.Round(perc*100) + "% chance that you fire a second shot.";
    }
    public string getIcon()
    {
        return "MulticasterUnlock";
    }
}

public class BurstShot : OnShootEffects{
    public static BurstShot Instance;
    public int interval;
    public int amount;
    private int leftToShoot;
    public BurstShot(int interval, int amount){
        this.interval = interval;
        this.amount = amount;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        leftToShoot--;
        if(leftToShoot <= 0){
            leftToShoot = interval;
            for(int i =0; i< amount; i++){
                Flare f = Flamey.Instance.InstantiateShot(new List<string>(){"Burst Shot", "MultiCaster"});
                f.setTarget(Flamey.Instance.getRandomHomingPosition());
            }
        }
        return 0;
    }
   

    public void Stack(BurstShot secondShot){
        amount = Mathf.Min(20, amount + secondShot.amount);
        interval = Mathf.Max(10, interval - secondShot.interval);
        RemoveUselessAugments();    
    }

    private void RemoveUselessAugments(){
        if(amount == 20){
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Burst Barricade");
            deck.removeFromDeck("Burst Unleashed");
            deck.removeFromDeck("Burst to Victory");
        }
        if(interval == 10){
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Happy Trigger");
            deck.removeFromDeck("Bullet Symphony");
            deck.removeFromDeck("Make It Rain");
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Burst Shot";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Each " + interval + " shots, you will shoot a burst of " + amount + " extra shots";
    }
    public string getIcon()
    {
        return "BurstUnlock";
    }
}

public class KrakenSlayer : OnShootEffects{


    public static KrakenSlayer Instance;
    public int interval;
    public int extraDmg;
    int curr;
    public KrakenSlayer(int interval, int extraDmg){
        this.interval = interval;
        this.extraDmg = extraDmg;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        curr--;
        if(curr <= 0){
            curr = interval;
            return 2;
        }
        return 0;
    }
  

    public void Stack(KrakenSlayer krakenSlayer){
        interval = Mathf.Max(0, interval - krakenSlayer.interval);
        extraDmg += krakenSlayer.extraDmg;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(interval == 0){
            Deck deck = Deck.Instance;
            deck.removeFromDeck("The Bluer The Better");
            deck.removeFromDeck("Propane Combustion");
            deck.removeFromDeck("Never ending Blue");
            Flamey.Instance.gameObject.GetComponent<Animator>().SetTrigger("GoBlue");
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Blue Flame";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Each " + interval + " shots, you will shoot an extra powerfull shot that deals +" + extraDmg + " extra damage";
    }
    public string getIcon()
    {
        return "BlueFlameUnlock";
    }
}

public class CritUnlock : OnShootEffects{


    public static CritUnlock Instance;
    public float perc;
    public float mult;
    public CritUnlock(float perc, float mult){
        this.perc = perc;
        this.mult = mult;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        if(Distribuitons.RandomUniform(0f,1f) <= perc){
           
            return 1;
        }
        return 0;
    }
    

    public void Stack(CritUnlock critUnlock){
        perc += critUnlock.perc;
        mult += critUnlock.mult;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(perc >= 0.8f){
            perc = 0.8f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Critical Miracle");
            deck.removeFromDeck("Fate's Favor");
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getDescription()
    {
        return "Your shots have a " + Mathf.Round(perc*100f*100f) * 0.01f + "% critical chance and x" + Mathf.Round(mult) + " critical damage multiplier.";
    }

    public string getIcon()
    {
        return "CritUnlock";
    }

    public string getText()
    {
        return "Critical Strike";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }
}