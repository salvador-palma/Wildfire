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
    
    public int getPrice(string skill, int level){
        int[] prices = AugmentPrices[skill];
        if(level>=prices.Length || level < 0){return -1;}
        return prices[level];
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

        AugmentPrices["ShredUnlock"] = new int[1]{800};
        AugmentPrices["ShredProb"] = new int[2]{500, 1000};
        AugmentPrices["ShredPerc"] = new int[2]{500, 1000};

        AugmentPrices["AssassinUnlock"] = new int[1]{800};
        AugmentPrices["ArmorPen"] = new int[2]{500, 1000};
        AugmentPrices["Execute"] = new int[2]{500, 1000};

        AugmentPrices["BlueUnlock"] = new int[1]{800};
        AugmentPrices["BlueInterval"] = new int[2]{500, 1000};
        AugmentPrices["BlueDmg"] = new int[2]{500, 1000};

        AugmentPrices["OrbitalUnlock"] = new int[1]{800};
        AugmentPrices["OrbitalAmount"] = new int[2]{500, 1000};
        AugmentPrices["OrbitalDmg"] = new int[2]{500, 1000};

        AugmentPrices["LavaPoolUnlock"] = new int[1]{800};
        AugmentPrices["LavaProb"] = new int[2]{500, 1000};
        AugmentPrices["LavaDuration"] = new int[2]{500, 1000};
        AugmentPrices["LavaSize"] = new int[2]{500, 1000};
        AugmentPrices["LavaDmg"] = new int[2]{500, 1000};

        AugmentPrices["StaticUnlock"] = new int[1]{800};
        AugmentPrices["StatikDmg"] = new int[2]{500, 1000};
        AugmentPrices["StatikTTL"] = new int[2]{500, 1000};
        AugmentPrices["StatikProb"] = new int[2]{500, 1000};
        
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
            
            new Augment("ShredUnlock" ,"Shredding Flames", new string[1]{"Unlock the ability to shred enemy armor"}, "shred", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Shredding Flames");
                Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"ShredProb","ShredPerc"});            
            })}, baseCard: true),   
            new Augment("ShredProb","Weaken", new string[3]{"Gain +5% chance to proc your Shredding Flames effect", 
                                                            "Gain +10% chance to proc your Shredding Flames effect", 
                                                            "Gain +15% chance to proc your Shredding Flames effect"}, "shred", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.05f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.15f, 0f)))}),
            new Augment("ShredProb","Armor Corruptor", new string[3]{"Gain +10% chance to proc your Shredding Flames effect", 
                                                            "Gain +20% chance to proc your Shredding Flames effect", 
                                                            "Gain +30% chance to proc your Shredding Flames effect"}, "shred", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.2f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.3f, 0f)))}),
            new Augment("ShredProb","Disintegration Field", new string[3]{"Gain +20% chance to proc your Shredding Flames effect", 
                                                            "Gain +40% chance to proc your Shredding Flames effect", 
                                                            "Gain +60% chance to proc your Shredding Flames effect"}, "shred", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.2f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.4f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.6f, 0f)))}),  
            new Augment("ShredPerc","Cheese Shredder", new string[3]{"Your Shredding Flames effect reduces +3% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +5% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +10% more enemy armor per proc"}, "shred", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.03f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.1f)))}),  
            new Augment("ShredPerc","Black Cleaver", new string[3]{"Your Shredding Flames effect reduces +7% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +15% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +20% more enemy armor per proc"}, "shred", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.2f)))}),   
            new Augment("ShredPerc","Molecular Decomposition", new string[3]{"Your Shredding Flames effect reduces +15% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +30% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +50% more enemy armor per proc"}, "shred", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.5f)))}),                         
            new Augment("AssassinUnlock" ,"Assassin's Path", new string[1]{"Unlock the ability to pierce armor and execute enemies"}, "assassin", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Shredding Flames");
                Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f));
                Flamey.Instance.addArmorPen(0.05f);
                Deck.Instance.AddAugmentClass(new List<string>{"ArmorPen","Execute"});            
            })}, baseCard: true),  
            new Augment("Execute","Execution Enforcer", new string[3]{"You can execute enemies for +1% of their Max Health (capped at 50%)", 
                                                            "You can execute enemies for +2% of their Max Health (capped at 50%)", 
                                                            "You can execute enemies for +5% of their Max Health (capped at 50%)"}, "assassin", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.01f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f)))}),
            new Augment("Execute","Soul Collector", new string[3]{"You can execute enemies for +2% of their Max Health (capped at 50%)", 
                                                            "You can execute enemies for +5% of their Max Health (capped at 50%)", 
                                                            "You can execute enemies for +10% of their Max Health (capped at 50%)"}, "assassin", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f)))}),
            new Augment("Execute","La Guillotine", new string[3]{"You can execute enemies for +5% of their Max Health (capped at 50%)", 
                                                            "You can execute enemies for +10% of their Max Health (capped at 50%)", 
                                                            "You can execute enemies for +25% of their Max Health (capped at 50%)"}, "assassin", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.25f)))}),
            new Augment("ArmorPen","Shell Breaker", new string[3]{"Gain +3% Armor Penetration", 
                                                            "Gain +5% Armor Penetration", 
                                                            "Gain +7% Armor Penetration"}, "assassin", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.03f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.05f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.07f))}),
            new Augment("ArmorPen","Quantum Piercing", new string[3]{"Gain +5% Armor Penetration", 
                                                            "Gain +10% Armor Penetration", 
                                                            "Gain +15% Armor Penetration"}, "assassin", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.05f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.15f))}),
            new Augment("ArmorPen","Lance of Aether", new string[3]{"Gain +10% Armor Penetration", 
                                                            "Gain +20% Armor Penetration", 
                                                            "Gain +30% Armor Penetration"}, "assassin", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.3f))}),
            new Augment("BlueUnlock" ,"Blue Flame", new string[1]{"Unlock the ability to shoot blue flames that inflict extra damage"}, "blueflame", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Blue Flame");
                Flamey.Instance.addOnShootEffect(new KrakenSlayer(20, 100));
                Deck.Instance.AddAugmentClass(new List<string>{"BlueInterval","BlueDmg"});            
            })}, baseCard: true),  
            new Augment("BlueInterval","The Bluer The Better", new string[3]{"You will need 1 shot less to proc Blue Flame", 
                                                            "You will need 2 shots less to proc Blue Flame", 
                                                            "You will need 3 shots less to proc Blue Flame"}, "blueflame", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(1, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(3, 0)))}),
            new Augment("BlueInterval","Propane Combustion", new string[3]{"You will need 2 shot less to proc Blue Flame", 
                                                            "You will need 4 shots less to proc Blue Flame", 
                                                            "You will need 6 shots less to proc Blue Flame"}, "blueflame", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(4, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(6, 0)))}),
            new Augment("BlueInterval","Never ending Blue", new string[3]{"You will need 4 shot less to proc Blue Flame", 
                                                            "You will need 8 shots less to proc Blue Flame", 
                                                            "You will need 12 shots less to proc Blue Flame"}, "blueflame", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(4, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(8, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(12, 0)))}),
            new Augment("BlueDmg","Propane Leakage", new string[3]{"Your Blue Flame deals +15 extra damage", 
                                                            "Your Blue Flame deals +25 extra damage", 
                                                            "Your Blue Flame deals +40 extra damage"}, "blueflame", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 15))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50)))}),
            new Augment("BlueDmg","Powerfull Blue", new string[3]{"Your Blue Flame deals +30 extra damage", 
                                                            "Your Blue Flame deals +50 extra damage", 
                                                            "Your Blue Flame deals +80 extra damage"}, "blueflame", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100)))}),
            new Augment("BlueDmg","Blue Inferno", new string[3]{"Your Blue Flame deals +50 extra damage", 
                                                            "Your Blue Flame deals +100 extra damage", 
                                                            "Your Blue Flame deals +200 extra damage"}, "blueflame", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 200)))}),
            
            new Augment("OrbitalUnlock" ,"Orbital Flames", new string[1]{"A tiny Flame will orbit around you damaging the foes it collides with"}, "orbital", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Orbital Flames");
                Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 25));
                Deck.Instance.AddAugmentClass(new List<string>{"OrbitalDmg","OrbitalAmount"});            
            })}, baseCard: true), 
            new Augment("OrbitalAmount","Tame the Flames", new string[3]{"Gain +1 tiny Flame in your Orbital Field (max. 4)", 
                                                            "Gain +2 tiny Flame in your Orbital Field (max. 4)", 
                                                            "Gain +3 tiny Flame in your Orbital Field (max. 4)"}, "orbital", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(3, 0)))}),
            new Augment("OrbitalDmg","Tiny Flames Win", new string[3]{"Your Orbital Flames deal +10 damage", 
                                                            "Your Orbital Flames deal +15 damage", 
                                                            "Your Orbital Flames deal +20 damage"}, "orbital", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 10))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 15))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 20)))}),
            new Augment("OrbitalDmg","Reliable Damage", new string[3]{"Your Orbital Flames deal +20 damage", 
                                                            "Your Orbital Flames deal +30 damage", 
                                                            "Your Orbital Flames deal +40 damage"}, "orbital", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 20))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 40)))}),
            new Augment("OrbitalDmg","Saturn", new string[3]{"Your Orbital Flames deal +40 damage", 
                                                            "Your Orbital Flames deal +60 damage", 
                                                            "Your Orbital Flames deal +80 damage"}, "orbital", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 40))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0,60))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 80)))}),

            new Augment("LavaPoolUnlock" ,"Lava Pool", new string[1]{"Unlock the ability to create Lava Pools that ignore enemy armor"}, "lava", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Lava Pool");
                Flamey.Instance.addOnLandEffect(new BurnOnLand(1f, 25, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"LavaDmg","LavaSize","LavaProb","LavaDuration"});            
            })}, baseCard: true), 
            new Augment("LavaDmg","Hot Tub", new string[3]{"Your Lava Pool will inflict +5 damage per second", 
                                                            "Your Lava Pool will inflict +10 damage per second", 
                                                            "Your Lava Pool will inflict +15 damage per second"}, "lava", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,5,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,10,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,15,0,0)))}),
            new Augment("LavaDmg","Magical Scorch", new string[3]{"Your Lava Pool will inflict +10 damage per second", 
                                                            "Your Lava Pool will inflict +20 damage per second", 
                                                            "Your Lava Pool will inflict +30 damage per second"}, "lava", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,10,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,20,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,30,0,0)))}),
            new Augment("LavaDmg","Conflagration", new string[3]{"Your Lava Pool will inflict +20 damage per second", 
                                                            "Your Lava Pool will inflict +40 damage per second", 
                                                            "Your Lava Pool will inflict +60 damage per second"}, "lava", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,20,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,40,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,60,0,0)))}),
            new Augment("LavaProb","Hot Steps", new string[3]{"Gain +3% probability of spawning a Lava Pool when your shot lands (capped at 50%)", 
                                                            "Gain +5% probability of spawning a Lava Pool when your shot lands (capped at 50%)", 
                                                            "Gain +7% probability of spawning a Lava Pool when your shot lands (capped at 50%)"}, "lava", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.03f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.05f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.07f,0)))}),
            new Augment("LavaProb","Lava here, Lava there", new string[3]{"Gain +7% probability of spawning a Lava Pool when your shot lands (capped at 50%)", 
                                                            "Gain +10% probability of spawning a Lava Pool when your shot lands (capped at 50%)", 
                                                            "Gain +15% probability of spawning a Lava Pool when your shot lands (capped at 50%)"}, "lava", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.07f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.15f,0)))}),
            new Augment("LavaProb","The Apocalypse", new string[3]{"Gain +15% probability of spawning a Lava Pool when your shot lands (capped at 50%)", 
                                                            "Gain +20% probability of spawning a Lava Pool when your shot lands (capped at 50%)", 
                                                            "Gain +30% probability of spawning a Lava Pool when your shot lands (capped at 50%)"}, "lava", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.2f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.3f,0)))}),
            new Augment("LavaSize","Heat Area", new string[3]{"Your Lava Pool grows by +0.20 (capped at 2.5)", 
                                                            "Your Lava Pool grows by +0.25 (capped at 2.5)", 
                                                            "Your Lava Pool grows by +0.30 (capped at 2.5)"}, "lava", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.2f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.25f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.3f,0,0,0)))}),
            new Augment("LavaSize","Lava Lakes", new string[3]{"Your Lava Pool grows by +0.4 (capped at 2.5)", 
                                                            "Your Lava Pool grows by +0.5 (capped at 2.5)", 
                                                            "Your Lava Pool grows by +0.6 (capped at 2.5)"}, "lava", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.4f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.5f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.6f,0,0,0)))}),
            new Augment("LavaSize","Inside the volcano", new string[3]{"Your Lava Pool grows by +0.8 (capped at 2.5)", 
                                                            "Your Lava Pool grows by +1 (capped at 2.5)", 
                                                            "Your Lava Pool grows by +1.2 (capped at 2.5)"}, "lava", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.8f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(1f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(1.2f,0,0,0)))}),
            new Augment("LavaDuration","Sear the ground", new string[3]{"Your Lava Pool lasts for +0.3 seconds", 
                                                            "Your Lava Pool lasts for +0.5 seconds", 
                                                            "Your Lava Pool lasts for +0.8 seconds"}, "lava", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.5f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.8f)))}),
            new Augment("LavaDuration","Eternally Hot", new string[3]{"Your Lava Pool lasts for +0.8 seconds", 
                                                            "Your Lava Pool lasts for +1.2 seconds", 
                                                            "Your Lava Pool lasts for +1.7 seconds"}, "lava", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.7f)))}),
            new Augment("LavaDuration","Unsettling Magma", new string[3]{"Your Lava Pool lasts for +1.8 seconds", 
                                                            "Your Lava Pool lasts for +2.4 seconds", 
                                                            "Your Lava Pool lasts for +3.5 seconds"}, "lava", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,2.4f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,3.5f)))}),
            new Augment("StaticUnlock" ,"Static Energy", new string[1]{"Unlock the ability to send static energy to enemies nearby of your target"}, "statik", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeFromDeck("Static Energy");
                Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,25,3));
                Deck.Instance.AddAugmentClass(new List<string>{"StatikProb","StatikDmg","StatikTTL"});            
            })}, baseCard: true), 
            new Augment("StatikProb","Watts Up", new string[3]{"Gain +3% probability to proc your Static Energy effect", 
                                                            "Gain +5% probability to proc your Static Energy effect", 
                                                            "Gain +7% probability to proc your Static Energy effect"}, "statik", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.03f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.05f,0,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.07f,0,0)))}),
            new Augment("StatikProb","Electrifying Possibilities", new string[3]{"Gain +7% probability to proc your Static Energy effect", 
                                                            "Gain +10% probability to proc your Static Energy effect", 
                                                            "Gain +15% probability to proc your Static Energy effect"}, "statik", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.7f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.15f,0,0)))}),
            new Augment("StatikProb","The Sparkster", new string[3]{"Gain +20% probability to proc your Static Energy effect", 
                                                            "Gain +25% probability to proc your Static Energy effect", 
                                                            "Gain +35% probability to proc your Static Energy effect"}, "statik", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.2f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.25f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.35f,0,0)))}),
            new Augment("StatikDmg","Shock Dart", new string[3]{"Your Statik Energy deals +5 extra damage", 
                                                            "Your Statik Energy deals +10 extra damage", 
                                                            "Your Statik Energy deals +15 extra damage"}, "statik", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,5,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,10,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,15,0)))}),
            new Augment("StatikDmg","Shocking Advancement", new string[3]{"Your Statik Energy deals +10 extra damage", 
                                                            "Your Statik Energy deals +20 extra damage", 
                                                            "Your Statik Energy deals +40 extra damage"}, "statik", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,10,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,20,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,30,0)))}),
            new Augment("StatikDmg","Zeus", new string[3]{"Your Statik Energy deals +20 extra damage", 
                                                            "Your Statik Energy deals +50 extra damage", 
                                                            "Your Statik Energy deals +100 extra damage"}, "statik", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,20,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,50,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,100,0)))}),
            new Augment("StatikTTL","Conductive materials", new string[3]{"Your Statik Energy will be able to cross through 1 more enemy", 
                                                            "Your Statik Energy will be able to cross through 1 more enemies", 
                                                            "Your Statik Energy will be able to cross through 2 more enemies"}, "statik", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2)))}),
            new Augment("StatikTTL","Feel the Flow", new string[3]{"Your Statik Energy will be able to cross through 2 more enemies", 
                                                            "Your Statik Energy will be able to cross through 3 more enemies", 
                                                            "Your Statik Energy will be able to cross through 4 more enemies"}, "statik", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,3))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,4)))}),
            new Augment("StatikTTL","Amping Up!", new string[3]{"Your Statik Energy will be able to cross through 4 more enemies", 
                                                            "Your Statik Energy will be able to cross through 6 more enemies", 
                                                            "Your Statik Energy will be able to cross through 8 more enemies"}, "statik", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,4))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,6))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,8)))}),

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
