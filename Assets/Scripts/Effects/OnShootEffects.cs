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
        Flamey.Instance.InstantiateShot(new List<string>(){"Multicaster"});
    }

    public void Stack(SecondShot secondShot){
        perc += secondShot.perc;
        RemoveUselessAugments();
    }

    private void RemoveUselessAugments(){
        if(perc > 1){
            perc = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("MulticasterProb");
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
        return "Whenever you fire a shot, there's a chance to fire an extra one. Extra shots will not count towards Multicaster or Burst Shot Effects";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 100%)<br>", Mathf.Round(perc*100));
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
                Flare f = Flamey.Instance.InstantiateShot(new List<string>(){"Burst Shot", "Multicaster"});
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
        if(amount >= 20){
            amount = 20;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BurstAmount");
        }
        if(interval >= 10){
            interval = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BurstInterval");
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
        return "Shoot extra Burst Shots everytime you shoot a certain amount of flames. Extra shots will not count towards Multicaster or Burst Shot Effects";
    }
    public string getCaps()
    {
        return string.Format("Burst Shots: {0} Flames (Max. 20)<br>Burst Interval: {1} Flames (Min. 10)", amount, interval);
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
            deck.removeClassFromDeck("BlueFlameInterval");
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
        return "Shoot a powerfull Blue Flame that deals Extra Damage everytime you shoot a certain amount of flames.";
    }
    public string getCaps()
    {
        return string.Format("Extra Damage: +{0} Damage <br>Blue Flame Interval: {1} Flames (Min. 0)", extraDmg, interval);
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
            deck.removeClassFromDeck("CritChance");
        }
        if(mult >= 5f){
            mult = 5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("CritMult");
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getDescription()
    {
        return "Your shots have a chance of critically striking, multiplying your damage by a certain amount.";
    }
    public string getCaps()
    {
        return string.Format("Critic Chance: +{0}% (Max. 80%)<br>Damage Multiplier: x{1} (Max. x5)", Mathf.Round(perc*100f), Mathf.Round(mult * 100f) * 0.01f);
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