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
    public void SetSpin(bool b){
        SpinnerInstance.canSpin = b;
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
        return amount + " Flames orbit around you in a circle. Upon collision with an enemy, it deals " + damage + " damage to them. This effect scales with Bullet Speed and applies On-Hit effects";
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
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(amount == 4){
        
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Tame the Flames");
        }  
          
    }

    
}

public class MoneyMultipliers : NotEspecificEffect
{
    public static MoneyMultipliers Instance;

    public float mult;
    public float p;
    public MoneyMultipliers(float p, float mult, bool init = false){
        this.mult = mult;
        this.p = p;
        if(init){
            Instance = this;
            return;
        }

        if(Instance == null){
            Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0.1f, 1f, true));

        }    
        Instance.Stack(this);
        
    }
    public void Stack(MoneyMultipliers moneyMultipliers){
        p+=moneyMultipliers.p;
        mult+=moneyMultipliers.mult;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(p >= 1){
            p = 1;
            Deck deck = Deck.Instance;
            // deck.removeFromDeck("Tame the Flames");
        }  
          
    }
    public bool addList()
    {
        return Instance == this;
    }

    public void ApplyEffect()
    {
        return;
    }

    public string getDescription()
    {
        return "Whenever you kill an enemy, there's a " + p*100 + "% of getting more embers. This value is then multiplied by " + mult*100 + "%. You can check the Bestiary for more info on enemy specific drop rates.";
    }

    public string getIcon()
    {
        return "MoneyUnlock";
    }

    public string getText()
    {
        return "Ember Generation";
    }

    public string getType()
    {
        return "Especial Effect";
    }
}
