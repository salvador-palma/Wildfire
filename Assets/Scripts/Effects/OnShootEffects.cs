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
        if(Random.Range(0f,1f) < perc){
            
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
        return "Secondary Fire";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Whenever you fire a shot, there's a " + perc + "% chance that you fire a second shot.";
    }
}
