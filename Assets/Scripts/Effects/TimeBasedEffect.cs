
using UnityEngine;

public interface TimeBasedEffect : Effect
{
    public bool addList();
    public void ApplyEffect();
    public void ApplyRound();
}


public class HealthRegen : TimeBasedEffect
{
    public static HealthRegen Instance;

    public float perSec;
    public float perRound;

    public int tickAmount;
    public HealthRegen(float perSec, float perRound){
        this.perSec = perSec;
        this.perRound = perRound;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        if(tickAmount <= 0){
            tickAmount = 4;
            Flamey.Instance.addHealth(perSec);
        }else{
            tickAmount--;
        }
        
    }
    public void ApplyRound()
    {
        Flamey.Instance.addHealth(perRound);
    }

    public void Stack(HealthRegen healthRegen){
        perSec += healthRegen.perSec;
        perRound += healthRegen.perRound;
        
    }
    
    public string getDescription()
    {
        return "You regen " + Mathf.Round(perSec * 100.0f) * 0.01f + " health per second and " + Mathf.Round(perRound * 100.0f) * 0.01f + " at the end of each round";
    }

    public string getIcon()
    {
        return "Regen";
    }

    public string getText()
    {
        return "Health Regen";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
}

public class LightningEffect : TimeBasedEffect

{
    public static LightningEffect Instance;
    public static GameObject lightning;
    public int interval;
    int current_interval;
    public int dmg;

    public LightningEffect(int interval, int dmg){
        this.dmg = dmg;
        this.interval = interval;
        if(Instance == null){
            Instance = this;
            lightning = Resources.Load<GameObject>("Prefab/Lightning");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        if(current_interval <=0 ){
            current_interval = interval;
            Vector2 v = Flamey.Instance.getRandomHomingPosition();
            GameObject go =  Flamey.Instance.SpawnObject(lightning);
            go.transform.position = new Vector2(v.x, v.y + 10.91f);

        }else{
            current_interval--;
        }
    }
    public void ApplyRound(){}

    public void Stack(LightningEffect lightningEffect){
        interval -= lightningEffect.interval;
        dmg += lightningEffect.dmg;
        RemoveUselessAugments();
    }

    void RemoveUselessAugments(){
        if(interval <= 1){
            interval = 1;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Charge it up!");
            deck.removeFromDeck("Eletric Discharge");
            deck.removeFromDeck("Thunderstorm");
        } 
    }
    
    public string getDescription()
    {
        return "Each " + Mathf.Round((float)interval/4 * 100.0f) * 0.01f + " seconds, a thunder will spawn at a convenient location dealing " + dmg + " to enemies struck by it. When it lands, the thunder applies On-Land Effects";
    }

    public string getIcon()
    {
        return "ThunderUnlock";
    }

    public string getText()
    {
        return "Thunder";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
}

public class Immolate : TimeBasedEffect
{
    public static Immolate Instance;
    public static GameObject ring;
    public int interval;
    int current_interval;
    public int dmg;
    public float radius;

    public Immolate(int interval, int dmg, float radius){
        this.dmg = dmg;
        this.interval = interval;
        this.radius = radius;
        if(Instance == null){
            Instance = this;
            ring = Resources.Load<GameObject>("Prefab/Ring");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        if(current_interval <=0 ){
            current_interval = interval;
            
            Flamey.Instance.SpawnObject(ring);
            

        }else{
            current_interval--;
        }
    }
    public void ApplyRound(){}

    public void Stack(Immolate immolate){
        interval -= immolate.interval;
        dmg += immolate.dmg;
        radius += immolate.radius;
        RemoveUselessAugments();
    }

    void RemoveUselessAugments(){
        if(interval <= 16){
            interval = 16;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Heat Discharge");
            deck.removeFromDeck("Accumulated Heat");
            deck.removeFromDeck("Releasing Everything");
        } 
        if(radius >= 2f){
            radius = 2f;
            Deck deck = Deck.Instance;
            deck.removeFromDeck("Waving Flames");
            deck.removeFromDeck("Spread the Fire");
            deck.removeFromDeck("Across the Globe");
        }
    }
    
    public string getDescription()
    {
        return "Each " + Mathf.Round((float)interval/4 * 100.0f) * 0.01f + " seconds, you will release energy that travels " + Mathf.Round(radius*100) + " units and deals " + dmg + " damage";
    }

    public string getIcon()
    {
        return "ImmolateUnlock";
    }

    public string getText()
    {
        return "Immolate";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
}

