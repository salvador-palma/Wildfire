using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck Instance {get; private set;}
    [SerializeField] List<Augment> augments;
    [SerializeField] GameObject[] Slots;
    [SerializeField] GameObject SlotsParent;
    private Augment[] currentAugments;
    [SerializeField] private Button[] RefreshButtons;
    private List<Augment> refreshedAugments;

    [SerializeField] private Color[] tierColors;
    private List<Augment> filteredAugments;
    private Tier currentTier;
    private void Start() {
        Instance = this;
        currentAugments = new Augment[3];
        FillDeck();
        refreshedAugments = new List<Augment>();
    }

    void FillDeck(){
        //ACCURACY
        augments.Add(new Augment("Target Practice", "Increase your accuracy by +15% (capped at 100%)", "acc", Tier.Silver, new UnityAction(()=> Flamey.Instance.addAccuracy(15))));
        augments.Add(new Augment("Steady Aim", "Increase your accuracy by +30", "acc", Tier.Gold, new UnityAction(()=> Flamey.Instance.addAccuracy(30))));
        augments.Add(new Augment("Eagle Eye", "Double your current Accuracy (capped at 100)", "acc", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multAccuracy(2))));
        
        //ATTACK SPEED
        augments.Add(new Augment("Swifty Flames", "Increase your attack speed by +30", "atkspeed", Tier.Silver, new UnityAction(()=> Flamey.Instance.addAttackSpeed(.3f))));
        augments.Add(new Augment("Fire Dance", "Increase your attack speed by +60", "atkspeed", Tier.Gold, new UnityAction(()=> Flamey.Instance.addAttackSpeed(0.6f))));
        augments.Add(new Augment("Flamethrower", "Increase your attack speed by x0.5", "atkspeed", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multAttackSpeed(1.5f))));

        //BULLET SPEED
        augments.Add(new Augment("Quick Shot", "Gain +0.75 Bullet Speed", "speed", Tier.Silver, new UnityAction(()=> Flamey.Instance.addBulletSpeed(0.75f))));
        augments.Add(new Augment("Fire-Express", "Gain +1.25 Bullet Speed", "speed", Tier.Gold, new UnityAction(()=> Flamey.Instance.addBulletSpeed(1.25f))));
        augments.Add(new Augment("HiperDrive", "Gain +2 Bullet Speed (capped at 20)", "speed", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addBulletSpeed(2f))));

        //MAX HEALTH 
        augments.Add(new Augment("Warm Soup", "Heal 30% of your Max Health", "health", Tier.Silver, new UnityAction(()=> Flamey.Instance.addHealth(0,0.3f))));
        augments.Add(new Augment("Sunfire Cape", "Heal 50% and gain +250 Max HP", "health", Tier.Gold, new UnityAction(()=> Flamey.Instance.addHealth(250, 0.75f))));
        augments.Add(new Augment("Absolute Unit", "Heal 75% and gain +500 Max HP", "health", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addHealth(500,1f))));
       
        //RANDOM AUGMENTS
        augments.Add(new Augment("Feelin' Blessed", "Gain 2 random Silver augments", "dice", Tier.Gold, new UnityAction(()=> {ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));})));
        augments.Add(new Augment("Not enough refreshes", "Gain a random Silver augment", "dice", Tier.Silver, new UnityAction(()=> ActivateAugment(randomPicking(Tier.Silver)))));
        augments.Add(new Augment("Roll the Dices", "Gain 4 random Silver augments", "dice", Tier.Prismatic, new UnityAction(()=> {ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));})));

        //DAMAGE
        augments.Add(new Augment("Hard Work", "Gain +10 Base Damage", "weight", Tier.Silver, new UnityAction(()=> Flamey.Instance.addDmg(10))));
        augments.Add(new Augment("Heavy Hitter", "Gain +25 Base Damage", "weight", Tier.Gold, new UnityAction(()=> Flamey.Instance.addDmg(25))));
        augments.Add(new Augment("Hephaestus", "Gain +50 Base Damage", "weight", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addDmg(50))));

        //ARMOR
        augments.Add(new Augment("Mesh Armor", "Increase your armor by +10", "acc", Tier.Silver, new UnityAction(()=> Flamey.Instance.addArmor(10))));
        augments.Add(new Augment("Long Lasting Fire", "Increase your armor by +20", "acc", Tier.Gold, new UnityAction(()=> Flamey.Instance.addArmor(20))));
        augments.Add(new Augment("The Armor of God", "Increase your armor by +35", "acc", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addArmor(35))));



        //SKILL TREES

        //CRITIC 
        augments.Add(new Augment("Critical Inferno", "Unlock the ability to critical strike", "critchance", Tier.Prismatic, new UnityAction(()=> {
            Flamey.Instance.addNotEspecificEffect(new CritUnlock());
            Flamey.Instance.addCritDmg(1.1f);
            Flamey.Instance.addCritChance(0.1f);
            removeFromDeck("Critical Inferno");
            augments.Add(new Augment("Lucky Shots", "Gain +15% critical strike damage", "critdmg", Tier.Silver, new UnityAction(()=> Flamey.Instance.addCritDmg(0.15f))));
            augments.Add(new Augment("Critical Thinking", "Gain +30% critical strike damage", "critdmg", Tier.Gold, new UnityAction(()=> Flamey.Instance.addCritDmg(0.3f))));
            augments.Add(new Augment("Critical Thinking", "Gain +5% critical strike chance (capped at 80%)", "critchance", Tier.Silver, new UnityAction(()=> Flamey.Instance.addCritChance(7))));
            augments.Add(new Augment("Fate's Favor", "Gain +10% critical strike chance (capped at 80%)", "critchance", Tier.Gold, new UnityAction(()=> Flamey.Instance.addCritChance(10))));
            augments.Add(new Augment("Overheat", "Gain +15% critical strike chance (capped at 80%) and +60% critical strike damage", "critchance", Tier.Prismatic, new UnityAction(()=> {Flamey.Instance.addCritChance(15);Flamey.Instance.addCritDmg(0.6f);})));
        })));

        //VAMPIRE ONHIT 
        augments.Add(new Augment("The Blood Mage", "Unlock the ability to life-steal", "vampfire", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("The Blood Mage");
            Flamey.Instance.addOnHitEffect(new VampOnHit(0.1f,0.1f));
            augments.Add(new Augment("Steal to Heal", "Gain +5% chance to proc your Blood Mage effect", "vampfire", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.05f)))));
            augments.Add(new Augment("Eternal Hunger", "Gain +10% chance to proc your Blood Mage effect", "vampfire", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.10f)))));
            augments.Add(new Augment("Soul Harvester", "Gain +20% chance to proc your Blood Mage effect", "vampfire", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.2f)))));
            augments.Add(new Augment("Sustenance", "Gain +3% Heal on your Blood Mage effect", "vampfire", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0.03f,0f)))));
            augments.Add(new Augment("Vampire Survivor", "Gain +7% Heal on your Blood Mage effect", "vampfire", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0.07f,0f)))));
            augments.Add(new Augment("Blood Pact", "Gain +15% Heal on your Blood Mage effect", "vampfire", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0.15f,0f)))));
        })));

        //MULTICASTER
        augments.Add(new Augment("Multicaster", "Unlock the ability to multicast", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Multicaster");
            Flamey.Instance.addOnShootEffect(new SecondShot(0.05f));
            augments.Add(new Augment("The more the better", "When you fire a shot, gain a 5% chance to fire an extra shot", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new SecondShot(0.05f)))));
            augments.Add(new Augment("Double trouble", "When you fire a shot, gain a 15% chance to fire an extra shot", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new SecondShot(0.15f)))));
            augments.Add(new Augment("Casting Cascade", "When you fire a shot, gain a 35% chance to fire an extra shot", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new SecondShot(0.35f)))));
        
        })));

        //BURST SHOT
        augments.Add(new Augment("Burst Shot", "Unlock the ability to burst shot", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Burst Shot");
            Flamey.Instance.addOnShootEffect(new BurstShot(50, 3));
            augments.Add(new Augment("Happy Trigger", "You will need 5 shots less to proc Burst Shot", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new BurstShot(5,0)))));
            augments.Add(new Augment("Bullet Symphony", "You will need 10 shots less to proc Burst Shot", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new BurstShot(10,0)))));
            augments.Add(new Augment("Make It Rain", "You will need 20 shots less to proc Burst Shot", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new BurstShot(20,0)))));
            augments.Add(new Augment("Burst Barricade", "Your Burst Shot will shoot an extra flame", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new BurstShot(0,1)))));
            augments.Add(new Augment("Burst Unleashed", "Your Burst Shot will shoot two extra flames", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new BurstShot(0,2)))));
            augments.Add(new Augment("Burst to Victory", "Your Burst Shot will shoot four extra flames", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new BurstShot(0,4)))));
        
        })));

        //ICE SOUL
        augments.Add(new Augment("Frost Fire", "Unlock the ability to Slow Enemies using ice(?)", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Frost Fire");
            Flamey.Instance.addOnHitEffect(new IceOnHit(500, 0.1f));
            augments.Add(new Augment("IcyHot", "Gain +5% chance to proc your Frost Fire effect", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.05f)))));
            augments.Add(new Augment("Glacial Energy", "Gain +15% chance to proc your Frost Fire effect", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.15f)))));
            augments.Add(new Augment("A Dance of Fire and Ice", "Gain +30% chance to proc your Frost Fire effect", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.3f)))));
            augments.Add(new Augment("Slowly but Surely", "Your Frost Fire effect lasts for 0.2 seconds more", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new IceOnHit(200, 0)))));
            augments.Add(new Augment("Frost Bite", "Your Frost Fire effect lasts for 0.5 seconds more", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new IceOnHit(500, 0)))));
            augments.Add(new Augment("Absolute Zero", "Your Frost Fire effect lasts for 1 second more", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0)))));
        })));


        //SHRED ON HIT
        augments.Add(new Augment("Shredding Flames", "Unlock the ability to shred enemy armor", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Shredding Flames");
            Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0.05f));
            augments.Add(new Augment("Weaken", "Gain +10% chance to proc your Shredding Flames effect", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0f)))));
            augments.Add(new Augment("Armor", "Gain +20% chance to proc your Shredding Flames effect", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0.2f, 0f)))));
            augments.Add(new Augment("Disintegration Field", "Gain +35% chance to proc your Frost Fire effect", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0.35f, 0f)))));
            augments.Add(new Augment("Cheese Shredder", "Your Shredding Flames effect reduces +5% more enemy armor per proc", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0f, 0.05f)))));
            augments.Add(new Augment("Black Cleaver", "Your Shredding Flames effect reduces +15% more enemy armor per proc", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0f, 0.15f)))));
            augments.Add(new Augment("Molecular Decomposition", "Your Shredding Flames effect reduces +30% more enemy armor per proc", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0f, 0.30f)))));
        })));

        //ASSASSIN'S PATHS
        augments.Add(new Augment("Assassin's Path", "Unlock the ability to pierce armor and execute enemies", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Assassin's Path");
            Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f));
            Flamey.Instance.addArmorPen(0.05f);
            augments.Add(new Augment("Execution Enforcer", "You can execute enemies for +2% of their Max Health (capped at 50%)", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f)))));
            augments.Add(new Augment("Soul Collector", "You can execute enemies for +4% of their Max Health (capped at 50%)", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.04f)))));
            augments.Add(new Augment("La Guillotine", "You can execute enemies for +10% of their Max Health (capped at 50%)", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f)))));
            augments.Add(new Augment("Shell Breaker", "Gain +5% Armor Penetration", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addArmorPen(0.05f))));
            augments.Add(new Augment("Quantum Piercing", "Gain +13% Armor Penetration", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addArmorPen(0.13f))));
            augments.Add(new Augment("Lance of Aether", "Gain +25% Armor Penetration", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addArmorPen(0.25f))));
        })));


        //KRAKEN SLAYER
        augments.Add(new Augment("Blue Flame", "Each 20 shots send a blue flame that causes +50 extra damage", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Blue Flame");
            Flamey.Instance.addOnShootEffect(new KrakenSlayer(20, 50));

            augments.Add(new Augment("The Bluer The Better", "You will need 1 shot less to proc Blue Flame (capped at 5 shots interval)", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(1, 0)))));
            augments.Add(new Augment("Propane Combustion", "You will need 2 shots less to proc Blue Flame (capped at 5 shots interval)", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(2, 0)))));
            augments.Add(new Augment("The never ending Blue", "You will need 5 shots less to proc Blue Flame (capped at 5 shots interval)", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(5, 0)))));
            augments.Add(new Augment("Propane Leakage", "Your Blue Flame deals +25 extra damage", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 25)))));
            augments.Add(new Augment("Powerfull Blue", "Your Blue Flame deals +50 extra damage", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50)))));
            augments.Add(new Augment("Blue Inferno", "Your Blue Flame deals +100 extra damage", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100)))));
        })));


        //ORBITAL FLAMES
        augments.Add(new Augment("Orbital Flames", "A tiny Flame will orbit around you, damaging the foes it collides with", "multishot", Tier.Prismatic, new UnityAction(()=> {
            removeFromDeck("Orbital Flames");
            Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 25));

            augments.Add(new Augment("Tame the Flames", "Gain +1 tiny Flame in your Orbital Field", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 0)))));
            augments.Add(new Augment("Tiny Flames Win", "Your Orbital Flames deal +10 damage", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 10)))));
            augments.Add(new Augment("Relliable Damage", "Your Orbital Flames deal +25 damage", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 25)))));
            augments.Add(new Augment("Saturn", "Your Orbital Flames deal +50 damage", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 50)))));
        })));
    }   



    Augment pickFromDeck(){
        Augment aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];; 
        while(currentAugments.Contains(aug)){
            aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        }
        return aug;
    }
    void ChangeSlots(){
        for (int i = 0; i < 3; i++)
        {
            ChangeSingular(pickFromDeck(), Slots[i], i);
        }
    }
    void ChangeSingular(Augment augment, GameObject slot, int i){
        slot.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = augment.Title;
        slot.transform.Find("Icon").GetComponent<Image>().sprite = augment.icon;
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = augment.Description;
        currentAugments[i] = augment;
        
    }

    public void PickedAugment(int i){
        if(currentTier == Tier.Prismatic){GameUI.Instance.PrismaticPicked();}
        SlotsParent.GetComponent<Animator>().Play("OutroSlots");
        
        ActivateAugment(currentAugments[i]);
        refreshedAugments.Clear();
        EnemySpawner.Instance.newRound();
        currentAugments = new Augment[]{null,null,null};
        GameUI.Instance.UpdateMenuInfo();
    }

    public void StartAugments(bool isPrismaticRound){
        if(Flamey.Instance.GameEnd){return;}
        filteredAugments = FilterAugments(isPrismaticRound);
        SlotsParent.GetComponent<Animator>().Play("EnterSlots");
        if(isPrismaticRound){GameUI.Instance.FillAll();}
        ChangeSlots();
        EnableRefreshes();
    }
    private void EnableRefreshes(){
        for (int i = 0; i < 3; i++)
        {
            RefreshButtons[i].interactable = true;
        }
    }

    private List<Augment> FilterAugments(bool isPrismaticRound){
        if(isPrismaticRound){
            currentTier = Tier.Prismatic;
            ChangeColors(tierColors[0],tierColors[1]);
            return augments.FindAll( a => a.tier == Tier.Prismatic);
        }
        if(UnityEngine.Random.Range(0f,1f) < 0.3f){
            currentTier = Tier.Gold;
            ChangeColors(tierColors[2],tierColors[3]);
            return augments.FindAll( a => a.tier == Tier.Gold);
        }else{
            currentTier = Tier.Silver;
            ChangeColors(tierColors[4],tierColors[5]);
            return augments.FindAll( a => a.tier == Tier.Silver);
        }
    }
    private void ChangeColors(Color basic, Color shade){
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].GetComponent<Image>().color = shade;
            Slots[i].transform.Find("Shadow").GetComponent<Image>().color = basic;
        }
    }
    public Tuple<Color,Color> getTierColors(Tier t){
         switch(t){
            case Tier.Silver:
                return new Tuple<Color, Color>(tierColors[4],tierColors[5]);
            case Tier.Gold:
                return new Tuple<Color, Color>(tierColors[2],tierColors[3]);
            case Tier.Prismatic:
                return new Tuple<Color, Color>(tierColors[0],tierColors[1]);
                
        }
        return null;
    }

    private Augment randomPicking(Tier tier){
        List<Augment> tempAugments = augments.FindAll( a => a.tier == tier);
        
        return tempAugments[UnityEngine.Random.Range(0, tempAugments.Count)];
    }

    private void ActivateAugment(Augment a){
        a.action();
        if(!a.Description.Contains("random Silver")){GameUI.Instance.AddAugment(a);}
        
    }

    public void RefreshAugment(int index){
        RefreshButtons[index].interactable = false;
        Augment aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        while(currentAugments.Contains(aug) || refreshedAugments.Contains(aug)){
            aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        }
        refreshedAugments.Add(currentAugments[index]);
        ChangeSingular(aug,Slots[index], index);
    }

    public void removeFromDeck(string title){
        augments.RemoveAll(a => a.Title == title);
    }



}
