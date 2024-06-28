using System;
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
            Deck.RoundOver += SetSpinFalse;
            Deck.RoundStart += SetSpinTrue;
            
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        SpinnerInstance.speed = Flamey.Instance.BulletSpeed;
        
        
    }
    public void SetSpinFalse(object sender ,EventArgs e){
        SetSpin(false);
    }
    public void SetSpinTrue(object sender ,EventArgs e){
        SetSpin(true);
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
        return "Flames orbit around you in a circle. Colliding with an enemy deals damage and applies <color=#FF99F3>On-Hit Effects</color>. <color=#AFEDFF>Angular speed</color> scales with <color=#AFEDFF>Bullet Speed";
    }
    public string getCaps()
    {
        return string.Format("Orbits: {0} (Max. 4)<br>Damage: {1}", amount, damage);
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
            deck.removeClassFromDeck("OrbitalAmount");
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
            deck.removeClassFromDeck("MoneyProb");
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
        return "Whenever you kill an enemy, there's a chance to loot more <color=#FFCC7C>embers</color> and multiply them. You can check the <color=#FFFF00>Bestiary</color> for more info on enemy specific drop rates.";
    }
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 100%)<br>Multiplier: x{1}", Mathf.Round(p*100), Mathf.Round(mult*100)*0.01f);
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
        return "Lit <color=#FFCC7C>candles</color> stand around you shooting at random targets. Their hits will <color=#FF5858>not</color> apply <color=#FF99F3>On-Hit</color> nor <color=#FF99F3>On-Land Effects";
    }
    public string getCaps()
    {
        return string.Format("Candle Amount: {0} (Max. 6)<br>Damage: +{1} <br>Attack Speed: {2}/s (Max. 3/s)", amount, dmg, Mathf.Round(atkSpeed * 100f) * 0.01f);
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
            deck.removeClassFromDeck("CandleAmount");
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("CandleAtkSpeed");
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
        return "<color=#FFCC7C>Bees</color> will fight by your side, targeting random enemies and applying <color=#FF99F3>On-Hit effects.";
    }
    public string getCaps()
    {
        return string.Format("Bee Amount: {0} (Max. 10)<br>Bee Damage: +{1} <br>Bee Attack Speed: {2}/s (Max. 4/s) <br>Bee Speed: {3} (Max. 4)", amount, dmg, Mathf.Round(atkSpeed *  100)/100, Mathf.Round(speed*  100)/100);
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
            deck.removeClassFromDeck("SummonAmount");
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonAtkSpeed");
        } 
        if(speed >= 4f){
            speed = 4f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonSpeed");
        } 
          
    }

    
}
