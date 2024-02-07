using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        return "OrbitalUnlock";
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
            deck.removeFromDeck("Savings Account");
            deck.removeFromDeck("Tax Payment");
            deck.removeFromDeck("Robbery");
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
        return "Whenever you kill an enemy, there's a " + Mathf.Round(p*100) + "% of getting more embers. This value is then multiplied by " + Mathf.Round(mult*100) + "%. You can check the Bestiary for more info on enemy specific drop rates.";
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



public class CandleTurrets : NotEspecificEffect
{
    public int dmg;
    public float atkSpeed;
    public int amount;

    public static CandleTurrets Instance;
    public static GameObject CandleCircle;
    
    public CandleTurrets(int dmg, float atkSpeed, int amount){
       
        this.amount = amount;
        this.dmg = dmg;
        this.atkSpeed = atkSpeed;
        if(Instance == null){
            Instance = this;     
            StartCandleCircle();
            UpdateAmount();
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        UpdateAmount();
    }

    public void StartCandleCircle(){
        CandleCircle = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/CandleCircle"));
        
    }
    public void UpdateAmount(){
        
        for(int i = 0; i < amount; i++){
           
            CandleCircle.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return amount + " candles stand by your side shooting at random targets with " + dmg + " damage and " + Mathf.Round(atkSpeed) + " attack speed";
    }

    public string getIcon()
    {
        return "CandleUnlock";
    }

    public string getText()
    {
        return "Candle Turrets";
    }

    public string getType()
    {
        return "Especial Effect";
    }

    public void Stack(CandleTurrets candleTurrets){
        
        dmg += candleTurrets.dmg;
        atkSpeed += candleTurrets.atkSpeed;
        amount += candleTurrets.amount;
        RemoveUselessAugments();
    }
    bool checkedForAmount = false;
    private void RemoveUselessAugments(){
       
        if(amount >= 6 && !checkedForAmount){
            checkedForAmount = true;
            amount = 6;
            CandleCircle.transform.GetChild(6).gameObject.SetActive(true);
            GameObject.Find("logs").SetActive(false);
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Philosopher's Stone");
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Alembic Artistry");
            deck.removeFromDeck("Ancient Wizard");
            deck.removeFromDeck("Begin the Ritual");

        } 
          
    }

    
}

public class Summoner : NotEspecificEffect
{
    public int dmg;
    public float atkSpeed;
    public float speed;
    public int amount;

    public List<Bee> bees;
    public static Summoner Instance;
    public static GameObject Bee;
    
    public Summoner(int dmg, float atkSpeed, float speed, int amount){
       
        this.amount = amount;
        this.dmg = dmg;
        this.atkSpeed = atkSpeed;
        this.speed = speed;
        if(Instance == null){
            Instance = this;     
            bees = new List<Bee>();
            Bee = Resources.Load<GameObject>("Prefab/Bee");
            ApplyEffect();
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        for(int i = bees.Count; i < amount; i++){
            bees.Add(Flamey.Instance.SpawnObject(Bee).GetComponent<Bee>());
        }

        foreach(Bee b in bees){
            b.UpdateStats();
        }
    }

    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return amount + "Bees will fight by your side, targeting the closest enemy and applying On-Hit effects. Each Bee deals " + dmg + " damage, has " + Mathf.Round(atkSpeed) + " attack speed and " + Mathf.Round(speed) + " speed";
    }

    public string getIcon()
    {
        return "SummonUnlock";
    }

    public string getText()
    {
        return "Bee Summoner";
    }

    public string getType()
    {
        return "Especial Effect";
    }

    public void Stack(Summoner summoner){
        
        dmg += summoner.dmg;
        atkSpeed += summoner.atkSpeed;
        amount += summoner.amount;
        speed += summoner.speed;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(amount > 10){
            amount = 10;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Bee Hive");
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Rapid Shooters");
            deck.removeFromDeck("Bee-autiful Pets");
            deck.removeFromDeck("Bee Swarm");
        } 
        if(speed >= 4f){
            speed = 4f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Speeding Up");
            deck.removeFromDeck("Agility");
            deck.removeFromDeck("Bee Acrobacy League");
        } 
          
    }

    
}
