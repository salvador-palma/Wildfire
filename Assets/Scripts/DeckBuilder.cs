using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeckBuilder : MonoBehaviour
{
    public static DeckBuilder Instance;
    public List<Augment> AllAugments;
    public Dictionary<string, int[]> AugmentPrices;
    
    void Awake(){
        if(Instance==null){Instance = this;}
        if(Instance == this){DefineAugmentClasses();DefineAugmentClassesPrice();}
        
    }
    
  
    public void DefineAugmentClassesPrice(){
        AugmentPrices = new Dictionary<string, int[]>();
        AugmentPrices["Damage"] = new int[4]{100,300,900,2700};
        AugmentPrices["Accuracy"] = new int[4]{100,300,900,2700};
        AugmentPrices["Atk Speed"] = new int[4]{100,300,900,2700};
        AugmentPrices["Bullet Speed"] = new int[4]{100,300,900,2700};
        AugmentPrices["Armor"] = new int[4]{100,300,900,2700};
        AugmentPrices["Health"] = new int[4]{100,300,900,2700};

        AugmentPrices["MulticasterUnlock"] = new int[1]{800};
        AugmentPrices["Multicaster"] = new int[2]{500, 1000};

        AugmentPrices["CriticUnlock"] = new int[1]{800};
        AugmentPrices["CriticChance"] = new int[2]{500, 1000};
        AugmentPrices["CriticDmg"] = new int[2]{500, 1000};

        AugmentPrices["VampUnlock"] = new int[1]{800};
        AugmentPrices["VampProb"] = new int[2]{500, 1000};
        AugmentPrices["VampPerc"] = new int[2]{500, 1000};

        AugmentPrices["BurstUnlock"] = new int[1]{800};
        AugmentPrices["BurstAmount"] = new int[2]{500, 1000};
        AugmentPrices["BurstInterval"] = new int[2]{500, 1000};

        AugmentPrices["IceUnlock"] = new int[1]{800};
        AugmentPrices["IceDuration"] = new int[2]{500, 1000};
        AugmentPrices["IceProb"] = new int[2]{500, 1000};
        
    }
    public void DefineAugmentClasses(){
        AllAugments = new List<Augment>
        {
            new Augment("Damage","Hard Work", new string[5]{"Gain +5 Base Damage", 
                                                    "Gain +10 Base Damage", 
                                                    "Gain +15 Base Damage", 
                                                    "Gain +20 Base Damage", 
                                                    "Gain +30 Base Damage"}, "weight", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addDmg(5)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(30)),},baseStat: true),
            new Augment("Damage","Heavy Hitter", new string[5]{"Gain +15 Base Damage", 
                                                    "Gain +20 Base Damage", 
                                                    "Gain +25 Base Damage", 
                                                    "Gain +35 Base Damage", 
                                                    "Gain +50 Base Damage"}, "weight", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addDmg(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(25)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(50)),},baseStat: true),
            new Augment("Damage","Hephaestus", new string[5]{"Gain +40 Base Damage", 
                                                    "Gain +50 Base Damage", 
                                                    "Gain +65 Base Damage", 
                                                    "Gain +80 Base Damage", 
                                                    "Gain +100 Base Damage"}, "weight", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addDmg(40)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(50)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(65)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(80)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(100)),},baseStat: true),
            new Augment("Accuracy","Target Practice", new string[5]{"Increase your accuracy by +5%", 
                                                    "Increase your accuracy by +10%", 
                                                    "Increase your accuracy by +15%", 
                                                    "Increase your accuracy by +20%", 
                                                    "Increase your accuracy by +25%"}, "acc", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAccuracy(5)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(25)),},baseStat: true),
            new Augment("Accuracy","Steady Aim", new string[5]{"Increase your accuracy by +10%", 
                                                    "Increase your accuracy by +15%", 
                                                    "Increase your accuracy by +25%", 
                                                    "Increase your accuracy by +35%", 
                                                    "Increase your accuracy by +50%"}, "acc", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAccuracy(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(25)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(50)),},baseStat: true),
            new Augment("Accuracy","Eagle Eye", new string[5]{"Increase your accuracy by +20%", 
                                                    "Increase your accuracy by +30%", 
                                                    "Increase your accuracy by +50%", 
                                                    "Increase your accuracy by +75%", 
                                                    "Max your accuracy"}, "acc", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAccuracy(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(30)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(50)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(75)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(100)),},baseStat: true),
            new Augment("Atk Speed","Swifty Flames", new string[5]{
                                                    "Increase your attack speed by +15", 
                                                    "Increase your attack speed by +25", 
                                                    "Increase your attack speed by +40", 
                                                    "Increase your attack speed by +60", 
                                                    "Increase your attack speed by +80"}, "atkspeed", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAttackSpeed(.15f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.25f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.6f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(1f)),},baseStat: true),
            new Augment("Atk Speed","Fire Dance", new string[5]{
                                                    "Increase your attack speed by +30", 
                                                    "Increase your attack speed by +50", 
                                                    "Increase your attack speed by +70", 
                                                    "Increase your attack speed by +100", 
                                                    "Increase your attack speed by +160"}, "atkspeed", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAttackSpeed(.3f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.7f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(1.6f)),},baseStat: true),
            new Augment("Atk Speed","Flamethrower", new string[5]{
                                                    "Gain 25% attack speed", 
                                                    "Gain 50% attack speed", 
                                                    "Gain 100% attack speed", 
                                                    "Gain 150% attack speed", 
                                                    "Gain 250% attack speed"}, "atkspeed", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.multAttackSpeed(1.25f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(1.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(2.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(3.5f)),},baseStat: true),
            new Augment("Bullet Speed","Quick Shot", new string[5]{
                                                    "Gain +0.25 Bullet Speed", 
                                                    "Gain +0.5 Bullet Speed", 
                                                    "Gain +0.8 Bullet Speed", 
                                                    "Gain +1.2 Bullet Speed", 
                                                    "Gain +1.8 Bullet Speed"}, "speed", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addBulletSpeed(.25f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(.8f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1.8f)),},baseStat: true),
            new Augment("Bullet Speed","Fire-Express", new string[5]{
                                                    "Gain +0.5 Bullet Speed", 
                                                    "Gain +1 Bullet Speed", 
                                                    "Gain +1.75 Bullet Speed", 
                                                    "Gain +2.2 Bullet Speed", 
                                                    "Gain +3 Bullet Speed"}, "speed", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addBulletSpeed(.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(2.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(3f)),},baseStat: true),
            new Augment("Bullet Speed","HiperDrive", new string[5]{
                                                    "Gain +1 Bullet Speed", 
                                                    "Gain +2 Bullet Speed", 
                                                    "Gain +4 Bullet Speed", 
                                                    "Gain +6 Bullet Speed", 
                                                    "Gain +8 Bullet Speed"}, "speed", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addBulletSpeed(1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(6f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(8f)),},baseStat: true),
            new Augment("Health","Warm Soup", new string[5]{
                                                    "Heal 10% of your Max Health", 
                                                    "Heal 25% of your Max Health", 
                                                    "Heal 50% of your Max Health", 
                                                    "Heal 75% and gain +250 Max HP", 
                                                    "Heal 100% and gain +500 Max HP"}, "health", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addHealth(0,.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(0,.25f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(0,.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(250,.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(500,1f)),},baseStat: true),
            new Augment("Health","Sunfire Cape", new string[5]{"Heal 30% of your Max Health", 
                                                    "Heal 40% and gain +100 Max HP", 
                                                    "Heal 50% and gain +250 Max HP", 
                                                    "Heal 75% and gain +550 Max HP", 
                                                    "Heal 100% and gain +1100 Max HP"}, "health", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addHealth(0,.3f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(100,.4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(250,.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(550,.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(1100,1f)),},baseStat: true),
            new Augment("Health","Absolute Unit", new string[5]{
                                                    "Heal 40% and gain +100 Max HP", 
                                                    "Heal 60% and gain +250 Max HP", 
                                                    "Heal 75% and gain +500 Max HP", 
                                                    "Heal 100% and gain +1000 Max HP", 
                                                    "Heal 100% and gain +2000 Max HP"}, "health", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addHealth(100,.4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(250,.6f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(500,.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(1000,1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(2000,1f)),},baseStat: true),  
            new Augment("Armor","Mesh Armor", new string[5]{
                                                    "Increase your armor by +5", 
                                                    "Increase your armor by +7", 
                                                    "Increase your armor by +10", 
                                                    "Increase your armor by +20", 
                                                    "Increase your armor by +25"}, "armor", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addArmor(5)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(25)),},baseStat: true),  
            new Augment("Armor","Long Lasting Fire", new string[5]{
                                                    "Increase your armor by +10", 
                                                    "Increase your armor by +15", 
                                                    "Increase your armor by +20", 
                                                    "Increase your armor by +35", 
                                                    "Increase your armor by +50"}, "armor", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addArmor(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(55)),},baseStat: true),                      
            new Augment("Armor","The Armor of God", new string[5]{
                                                    "Increase your armor by +25", 
                                                    "Increase your armor by +35", 
                                                    "Increase your armor by +50", 
                                                    "Increase your armor by +70", 
                                                    "Increase your armor by +100"}, "armor", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addArmor(25)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(50)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(70)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(100)),},baseStat: true), 
            new Augment("Random","Not enough refreshes", new string[1]{
                                                    "Gain 2 random Silver augments"}, "dice", Tier.Silver, new UnityAction[1]{new UnityAction(() => {for (int i = 0; i < 2; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Silver));}})},baseStat: true), 
            new Augment("Random","Feelin' Blessed", new string[1]{
                                                    "Gain 3 random Silver augments"}, "dice", Tier.Gold, new UnityAction[1]{new UnityAction(() => {for (int i = 0; i < 3; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Silver));}})},baseStat: true),      
            new Augment("Random","Roll the Dices", new string[1]{
                                                    "Gain 3 random Gold augments"}, "dice", Tier.Prismatic, new UnityAction[1]{new UnityAction(() => {for (int i = 0; i < 3; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Gold));}})},baseStat: true),  
        


            new Augment("MulticasterUnlock" ,"Multicaster", new string[1]{"Unlock the ability to multicast"}, "multishot", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Multicaster");
                Flamey.Instance.addOnShootEffect(new SecondShot(0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"Multicaster"});            
            })}, baseCard: true),   
            new Augment("Multicaster","The more the better", new string[3]{"When you fire a shot, gain a 3% chance to fire an extra shot", 
                                                                            "When you fire a shot, gain a 7% chance to fire an extra shot", 
                                                                            "When you fire a shot, gain a 12% chance to fire an extra shot"}, "multishot", Tier.Silver, new UnityAction[3]{
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.03f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.07f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.12f)))}), 
            new Augment("Multicaster","Double trouble", new string[3]{"When you fire a shot, gain a 10% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 15% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 20% chance to fire an extra shot"}, "multishot", Tier.Gold, new UnityAction[3]{
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.1f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.15f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.20f)))}), 
            new Augment("Multicaster","Casting Cascade", new string[3]{"When you fire a shot, gain a 20% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 30% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 40% chance to fire an extra shot"}, "multishot", Tier.Prismatic, new UnityAction[3]{
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.2f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.3f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.4f)))}), 

            new Augment("CriticUnlock" ,"Critical Inferno", new string[1]{"Unlock the ability to critical strike"}, "critchance", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Critical Inferno");
                Flamey.Instance.addOnShootEffect(new CritUnlock(0.1f, 1.5f));
                Deck.Instance.AddAugmentClass(new List<string>{"CriticDmg","CriticChance"});            
            })}, baseCard: true),  
            new Augment("CriticDmg","Lucky Shots", new string[3]{"Gain +5% critical strike damage", 
                                                            "Gain +15% critical strike damage", 
                                                            "Gain +30% critical strike damage"}, "critdmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.3f)))}), 
            new Augment("CriticDmg","Critical Thinking", new string[3]{"Gain +15% critical strike damage", 
                                                            "Gain +35% critical strike damage", 
                                                            "Gain +70% critical strike damage"}, "critdmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.35f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.7f)))}),
            new Augment("CriticChance","Critical Miracle", new string[3]{"Gain +3% critical strike chance (capped at 80%)", 
                                                            "Gain +7% critical strike chance (capped at 80%)", 
                                                            "Gain +15% critical strike chance (capped at 80%)"}, "critchance", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.03f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.07f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.15f, 0f)))}),
            new Augment("CriticChance","Fate's Favor", new string[3]{"Gain +7% critical strike chance (capped at 80%)", 
                                                            "Gain +15% critical strike chance (capped at 80%)", 
                                                            "Gain +30% critical strike chance (capped at 80%)"}, "critchance", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.07f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.15f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.3f, 0f)))}),
            new Augment("CriticChance","Overheat", new string[3]{"Gain +10% critical strike chance (capped at 80%) and +30% critical strike damage", 
                                                            "Gain +15% critical strike chance (capped at 80%) and +60% critical strike damage", 
                                                            "Gain +30% critical strike chance (capped at 80%) and +120% critical strike damage"}, "critchance", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.1f, 0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.15f, 0.6f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.3f, 1.2f)))}),
            new Augment("VampUnlock" ,"The Blood Mage", new string[1]{"Unlock the ability to life-steal"}, "vampfire", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("The Blood Mage");
                Flamey.Instance.addOnHitEffect(new VampOnHit(0.05f,0.05f));
                Deck.Instance.AddAugmentClass(new List<string>{"VampProb","VampPerc"});            
            })}, baseCard: true),  
            new Augment("VampProb","Steal to Heal", new string[3]{"Gain +2% chance to proc your Blood Mage effect", 
                                                            "Gain +5% chance to proc your Blood Mage effect", 
                                                            "Gain +8% chance to proc your Blood Mage effect"}, "vampfire", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.08f)))}), 
            new Augment("VampProb","Eternal Hunger", new string[3]{"Gain +4% chance to proc your Blood Mage effect", 
                                                            "Gain +10% chance to proc your Blood Mage effect", 
                                                            "Gain +16% chance to proc your Blood Mage effect"}, "vampfire", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.04f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.16f)))}),
            new Augment("VampProb","Soul Harvester", new string[3]{"Gain +10% chance to proc your Blood Mage effect", 
                                                            "Gain +20% chance to proc your Blood Mage effect", 
                                                            "Gain +30% chance to proc your Blood Mage effect"}, "vampfire", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.3f)))}),  
            new Augment("VampPerc","Sustenance", new string[3]{"Gain +2% Heal on your Blood Mage effect", 
                                                            "Gain +3% Heal on your Blood Mage effect", 
                                                            "Gain +5% Heal on your Blood Mage effect"}, "vampfire", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.02f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.03f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.05f,0f)))}),  
            new Augment("VampPerc","Vampire Survivor", new string[3]{"Gain +4% Heal on your Blood Mage effect", 
                                                            "Gain +7% Heal on your Blood Mage effect", 
                                                            "Gain +12% Heal on your Blood Mage effect"}, "vampfire", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.04f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.07f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.12f,0f)))}),
            new Augment("VampPerc","Blood Pact", new string[3]{"Gain +8% Heal on your Blood Mage effect", 
                                                            "Gain +15% Heal on your Blood Mage effect", 
                                                            "Gain +25% Heal on your Blood Mage effect"}, "vampfire", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.08f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.15f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.25f,0f)))}),    
            
            new Augment("BurstUnlock" ,"Burst Shot", new string[1]{"Unlock the ability to send burst shots"}, "burst", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Burst Shot");
                Flamey.Instance.addOnShootEffect(new BurstShot(50, 5));
                Deck.Instance.AddAugmentClass(new List<string>{"BurstInterval","BurstAmount"});            
            })}, baseCard: true),  
            new Augment("BurstInterval","Happy Trigger", new string[3]{"You will need 2 shots less to proc Burst Shot", 
                                                                        "You will need 4 shots less to proc Burst Shot", 
                                                                        "You will need 7 shots less to proc Burst Shot"}, "burst", Tier.Silver, new UnityAction[3]{
                                                                                                                    new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(2,0))),
                                                                                                                    new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(4,0))),
                                                                                                                    new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(7,0)))}), 
            new Augment("BurstInterval","Bullet Symphony", new string[3]{"You will need 5 shots less to proc Burst Shot", 
                                                            "You will need 8 shots less to proc Burst Shot", 
                                                            "You will need 14 shots less to proc Burst Shot"}, "burst", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(5,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(8,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(14,0)))}), 
            new Augment("BurstInterval","Make It Rain", new string[3]{"You will need 10 shots less to proc Burst Shot", 
                                                            "You will need 15 shots less to proc Burst Shot", 
                                                            "You will need 25 shots less to proc Burst Shot"}, "burst", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(10,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(15,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(25,0)))}), 
            new Augment("BurstAmount","Burst Barricade", new string[3]{"Your Burst Shot will shoot an extra flame", 
                                                            "Your Burst Shot will +2 flames", 
                                                            "Your Burst Shot will +3 flames"}, "burst", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,3)))}), 
            new Augment("BurstAmount","Burst Unleashed", new string[3]{"Your Burst Shot will +2 flames", 
                                                            "Your Burst Shot will +3 flames", 
                                                            "Your Burst Shot will +5 flames"}, "burst", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,3))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,6)))}), 
            new Augment("BurstAmount","Burst to Victory", new string[3]{"Your Burst Shot will +3 flames", 
                                                            "Your Burst Shot will +5 flames", 
                                                            "Your Burst Shot will +10 flames"}, "burst", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,3))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,5))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,10)))}), 
            
            new Augment("IceUnlock" ,"Frost Fire", new string[1]{"Unlock the ability to Slow enemies using ice(?)"}, "ice", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Frost Fire");
                Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"IceDuration","IceProb"});            
            })}, baseCard: true),  
            new Augment("IceProb","IcyHot", new string[3]{"Gain +3% chance to proc your Frost Fire effect", 
                                                            "Gain +5% chance to proc your Frost Fire effect", 
                                                            "Gain +8% chance to proc your Frost Fire effect"}, "ice", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.03f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.08f)))}), 
            new Augment("IceProb","Glacial Energy", new string[3]{"Gain +7% chance to proc your Frost Fire effect", 
                                                            "Gain +15% chance to proc your Frost Fire effect", 
                                                            "Gain +20% chance to proc your Frost Fire effect"}, "ice", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.07f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.20f)))}), 
            new Augment("IceProb","A Dance of Fire and Ice", new string[3]{"Gain +15% chance to proc your Frost Fire effect", 
                                                            "Gain +25% chance to proc your Frost Fire effect", 
                                                            "Gain +40% chance to proc your Frost Fire effect"}, "ice", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.40f)))}), 
            new Augment("IceDuration","Slowly but Surely", new string[3]{"Your Frost Fire effect lasts for 0.2 seconds more", 
                                                            "Your Frost Fire effect lasts for 0.4 seconds more", 
                                                            "Your Frost Fire effect lasts for 0.6 seconds more"}, "ice", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(200, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(400, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(600, 0)))}),
            new Augment("IceDuration","Frost Bite", new string[3]{"Your Frost Fire effect lasts for 0.5 seconds more", 
                                                            "Your Frost Fire effect lasts for 1 seconds more", 
                                                            "Your Frost Fire effect lasts for 1.5 seconds more"}, "ice", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(500, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1500, 0)))}), 
            new Augment("IceDuration","Absolute Zero", new string[3]{"Your Frost Fire effect lasts for 1 seconds more", 
                                                            "Your Frost Fire effect lasts for 2 seconds more", 
                                                            "Your Frost Fire effect lasts for 3 seconds more"}, "ice", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(2000, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(3000, 0)))}),   
        };

    }

    public List<Augment> getAllCards(){
        List<Augment> result = new List<Augment>();
        foreach (Augment item in AllAugments)
        {
            if(item.playable()){result.Add(item);}
        }
        return result;
    }

    public List<Augment> GetAugmentsFromClasses(List<string> augmentClasses){
        List<Augment> result = new List<Augment>();
        foreach (Augment item in AllAugments)
        {
            if(augmentClasses.Contains(item.getAugmentClass())){
                result.Add(item);
            }
        }
        return result;
    }





}
