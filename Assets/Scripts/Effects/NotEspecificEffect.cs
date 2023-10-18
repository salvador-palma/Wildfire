using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NotEspecificEffect : Effect{
    public bool addList();
    public void ApplyEffect();
}

public class FlameCircle : NotEspecificEffect
{
    public int amount;
    int prevamount;
    public int damage;
    public static FlameCircle Instance;
    public Spinner SpinnerInstance;
    
    public FlameCircle(int amount, int damage){
       
        this.amount = amount;
        prevamount = amount;
        this.damage = damage;
        if(Instance == null){
            Instance = this;
            GameObject g = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle "+amount));
            SpinnerInstance = g.GetComponent<Spinner>();
            
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        SpinnerInstance.speed = Flamey.Instance.BulletSpeed;
        
        //Flamey.Instance.addNotEspecificEffect(new FlameCircle(1,20)); //REMOVE AFTER
    }
    public void UpdateAmount(){
        if(amount != prevamount){
            prevamount = amount;
            Spinner next = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle "+amount)).GetComponent<Spinner>();
            SpinnerInstance.kill();
            SpinnerInstance = next;
        }
    }
    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return amount + " Flames orbit around you in a circle. Upon collision with an enemy, it deals " + damage + " damage to them. This effect scales with Bullet Speed.";
    }

    public string getIcon()
    {
        return "orbital";
    }

    public string getText()
    {
        return "Orbital Flames";
    }

    public string getType()
    {
        return "Especial Effect";
    }

    public void Stack(FlameCircle flameCircle){
        amount = Mathf.Min(flameCircle.amount + amount, 4);
        damage += flameCircle.damage;
        UpdateAmount();
    }

    
}

public class CritUnlock : NotEspecificEffect
{
    public CritUnlock(){}
    public bool addList()
    {
        return true;
    }

    public void ApplyEffect()
    {
        return;
    }

    public string getDescription()
    {
        return "Your shots have a " + Flamey.Instance.CritChance+ "% critical chance and x" + Flamey.Instance.CritMultiplier + " critical damage multiplier.";
    }

    public string getIcon()
    {
        return "critchance";
    }

    public string getText()
    {
        return "Critical Inferno";
    }

    public string getType()
    {
        return "Especial Effect";
    }
}
