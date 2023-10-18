using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface OnShootEffects : Effect
{
    public bool addList();
    public void ApplyEffect();

}

public class SecondShot : OnShootEffects{


    public static SecondShot Instance;
    public float perc;
    public SecondShot(float p){
        perc = p;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        if(UnityEngine.Random.Range(0f,1f) < perc){
            
            ShootWithDelay();
        }
    }
    private async void ShootWithDelay(){
        await Task.Delay(100);
        Flamey.Instance.InstantiateShot();
    }

    public void Stack(SecondShot secondShot){
        perc += secondShot.perc;
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "MultiCaster";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Whenever you fire a shot, there's a " + perc*100 + "% chance that you fire a second shot.";
    }
    public string getIcon()
    {
        return "multishot";
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

    public void ApplyEffect()
    {
        leftToShoot--;
        if(leftToShoot <= 0){
            leftToShoot = interval;
            for(int i =0; i< amount; i++){
                Flare f = Flamey.Instance.InstantiateShot();
                f.setTarget(Flamey.Instance.getRandomHomingPosition());
            }
        }
    }
   

    public void Stack(BurstShot secondShot){
        amount = Mathf.Min(20, amount + secondShot.amount);
        interval = Mathf.Max(10, interval - secondShot.interval);
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
        return "burst";
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

    public void ApplyEffect()
    {
        curr--;
        if(curr <= 0){
            curr = interval;
            ShootWithDelay();
        }
    }
    private void ShootWithDelay(){
        //await Task.Delay(100);
        Flamey.Instance.InstantiateShot(extraDmg, 2);
        
    }

    public void Stack(KrakenSlayer krakenSlayer){
        interval = Mathf.Max(5, interval - krakenSlayer.interval);
        extraDmg += krakenSlayer.extraDmg;
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
        return "blueflame";
    }
}