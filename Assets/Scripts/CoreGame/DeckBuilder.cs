using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeckBuilder : MonoBehaviour
{
    public static DeckBuilder Instance;
    public List<Augment> AllAugments;
    int[] basePrice;
    int[] upgradePrice;
    int[] unlockPrice;
    void Awake(){
        if(Instance==null){Instance = this;}
        if(Instance==this){DefineAugmentClasses();}
        basePrice = new int[4]{300,900,2500,10000};
        upgradePrice = new int[2]{1500,7500};
        unlockPrice = new int[1]{500};
    }
    
    public int getPrice(string skill, int level){
        List<string> firstUnlocks = new List<string>(){"OrbitalUnlock","Assassins", "CritUnlock", "RegenUnlock", "ShredUnlock", "BurstUnlock"};
        int[] prices = new int[0];
        switch(SkillTreeManager.Instance.GetSkills(skill).max_value){
            case 1: prices =  unlockPrice;
                break;
            case 2: prices = upgradePrice;
                break;
            case 4: prices = basePrice;
                break;  
        }
        if(level>=prices.Length || level < 0){return -1;}
        return firstUnlocks.Contains(skill) ? 300 : prices[level];
    }
    public void DefineAugmentClasses(){
        AllAugments = new List<Augment>
        {
            new Augment("Dmg","Hard Work", new string[5]{"Gain +5 Base Damage", 
                                                    "Gain +10 Base Damage", 
                                                    "Gain +15 Base Damage", 
                                                    "Gain +20 Base Damage", 
                                                    "Gain +30 Base Damage"}, "Dmg", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addDmg(5)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(30)),},baseStat: true),
            new Augment("Dmg","Heavy Hitter", new string[5]{"Gain +15 Base Damage", 
                                                    "Gain +20 Base Damage", 
                                                    "Gain +25 Base Damage", 
                                                    "Gain +35 Base Damage", 
                                                    "Gain +50 Base Damage"}, "Dmg", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addDmg(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(25)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(50)),},baseStat: true),
            new Augment("Dmg","Hephaestus", new string[5]{"Gain +30 Base Damage", 
                                                    "Gain +40 Base Damage", 
                                                    "Gain +50 Base Damage", 
                                                    "Gain +65 Base Damage", 
                                                    "Gain +80 Base Damage"}, "Dmg", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addDmg(30)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(40)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(50)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(65)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addDmg(80)),},baseStat: true),
            new Augment("Acc","Target Practice", new string[5]{"Increase your accuracy by +5%", 
                                                    "Increase your accuracy by +10%", 
                                                    "Increase your accuracy by +15%", 
                                                    "Increase your accuracy by +20%", 
                                                    "Increase your accuracy by +25%"}, "Acc", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAccuracy(5)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(25)),},baseStat: true),
            new Augment("Acc","Steady Aim", new string[5]{"Increase your accuracy by +10%", 
                                                    "Increase your accuracy by +15%", 
                                                    "Increase your accuracy by +25%", 
                                                    "Increase your accuracy by +35%", 
                                                    "Increase your accuracy by +50%"}, "Acc", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAccuracy(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(25)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(50)),},baseStat: true),
            new Augment("Acc","Eagle Eye", new string[5]{"Increase your accuracy by +20%", 
                                                    "Increase your accuracy by +30%", 
                                                    "Increase your accuracy by +50%", 
                                                    "Increase your accuracy by +75%", 
                                                    "Max your accuracy"}, "Acc", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAccuracy(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(30)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(50)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(75)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAccuracy(100)),},baseStat: true),
            new Augment("AtkSpeed","Swifty Flames", new string[5]{
                                                    "Increase your attack speed by +10", 
                                                    "Increase your attack speed by +20", 
                                                    "Increase your attack speed by +30", 
                                                    "Increase your attack speed by +50", 
                                                    "Increase your attack speed by +70"}, "AtkSpeed", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAttackSpeed(.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.3f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.7f)),},baseStat: true),
            new Augment("AtkSpeed","Fire Dance", new string[5]{
                                                    "Increase your attack speed by +20", 
                                                    "Increase your attack speed by +40", 
                                                    "Increase your attack speed by +60", 
                                                    "Increase your attack speed by +100", 
                                                    "Increase your attack speed by +150"}, "AtkSpeed", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAttackSpeed(.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(.6f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(1.5f)),},baseStat: true),
            new Augment("AtkSpeed","Flamethrower", new string[5]{
                                                    "Increase your attack speed by +50", 
                                                    "Increase your attack speed by +80", 
                                                    "Multiply your attack speed by x1.5", 
                                                    "Multiply your attack speed by x2", 
                                                    "Multiply your attack speed by x2.5"}, "AtkSpeed", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addAttackSpeed(0.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addAttackSpeed(0.8f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(1.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.multAttackSpeed(2.5f)),},baseStat: true),
            new Augment("BltSpeed","Quick Shot", new string[5]{
                                                    "Gain +0.25 Bullet Speed", 
                                                    "Gain +0.5 Bullet Speed", 
                                                    "Gain +0.8 Bullet Speed", 
                                                    "Gain +1.2 Bullet Speed", 
                                                    "Gain +1.8 Bullet Speed"}, "BltSpeed", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addBulletSpeed(.25f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(.8f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1.8f)),},baseStat: true),
            new Augment("BltSpeed","Fire-Express", new string[5]{
                                                    "Gain +0.5 Bullet Speed", 
                                                    "Gain +1 Bullet Speed", 
                                                    "Gain +1.75 Bullet Speed", 
                                                    "Gain +2.2 Bullet Speed", 
                                                    "Gain +3 Bullet Speed"}, "BltSpeed", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addBulletSpeed(.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(1.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(2.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(3f)),},baseStat: true),
            new Augment("BltSpeed","HiperDrive", new string[5]{
                                                    "Gain +1 Bullet Speed", 
                                                    "Gain +2 Bullet Speed", 
                                                    "Gain +4 Bullet Speed", 
                                                    "Gain +6 Bullet Speed", 
                                                    "Gain +8 Bullet Speed"}, "BltSpeed", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addBulletSpeed(1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(6f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addBulletSpeed(8f)),},baseStat: true),
            new Augment("Health","Warm Soup", new string[5]{
                                                    "Heal 10% of your Max Health", 
                                                    "Heal 25% of your Max Health", 
                                                    "Heal 50% of your Max Health", 
                                                    "Heal 75% and gain +250 Max HP", 
                                                    "Heal 100% and gain +500 Max HP"}, "Health", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addHealth(0,.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(0,.25f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(0,.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(250,.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(500,1f)),},baseStat: true),
            new Augment("Health","Sunfire Cape", new string[5]{"Heal 30% of your Max Health", 
                                                    "Heal 40% and gain +100 Max HP", 
                                                    "Heal 50% and gain +250 Max HP", 
                                                    "Heal 75% and gain +550 Max HP", 
                                                    "Heal 100% and gain +1100 Max HP"}, "Health", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addHealth(0,.3f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(100,.4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(250,.5f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(550,.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(1100,1f)),},baseStat: true),
            new Augment("Health","Absolute Unit", new string[5]{
                                                    "Heal 40% and gain +100 Max HP", 
                                                    "Heal 60% and gain +250 Max HP", 
                                                    "Heal 75% and gain +500 Max HP", 
                                                    "Heal 100% and gain +1000 Max HP", 
                                                    "Heal 100% and gain +2000 Max HP"}, "Health", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addHealth(100,.4f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(250,.6f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(500,.75f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(1000,1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addHealth(2000,1f)),},baseStat: true),  
            new Augment("Armor","Mesh Armor", new string[5]{
                                                    "Increase your armor by +5", 
                                                    "Increase your armor by +7", 
                                                    "Increase your armor by +10", 
                                                    "Increase your armor by +20", 
                                                    "Increase your armor by +25"}, "Armor", Tier.Silver, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addArmor(5)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(25)),},baseStat: true),  
            new Augment("Armor","Long Lasting Fire", new string[5]{
                                                    "Increase your armor by +10", 
                                                    "Increase your armor by +15", 
                                                    "Increase your armor by +20", 
                                                    "Increase your armor by +35", 
                                                    "Increase your armor by +50"}, "Armor", Tier.Gold, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addArmor(10)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(15)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(20)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(55)),},baseStat: true),                      
            new Augment("Armor","The Armor of God", new string[5]{
                                                    "Increase your armor by +25", 
                                                    "Increase your armor by +35", 
                                                    "Increase your armor by +50", 
                                                    "Increase your armor by +70", 
                                                    "Increase your armor by +100"}, "Armor", Tier.Prismatic, new UnityAction[5]{new UnityAction(() => Flamey.Instance.addArmor(25)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(35)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(50)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(70)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmor(100)),},baseStat: true), 
            
        


            new Augment("MulticasterUnlock" ,"Multicaster", new string[1]{"Unlock the ability to multicast"}, "MulticasterUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("MulticasterUnlock");
                Flamey.Instance.addOnShootEffect(new SecondShot(0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"MulticasterProb"});            
            })}, baseCard: true),   
            new Augment("MulticasterProb","The more the better", new string[3]{"When you fire a shot, gain a 3% chance to fire an extra shot", 
                                                                            "When you fire a shot, gain a 7% chance to fire an extra shot", 
                                                                            "When you fire a shot, gain a 12% chance to fire an extra shot"}, "MulticasterProb", Tier.Silver, new UnityAction[3]{
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.03f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.07f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.12f)))}), 
            new Augment("MulticasterProb","Double trouble", new string[3]{"When you fire a shot, gain a 10% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 15% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 20% chance to fire an extra shot"}, "MulticasterProb", Tier.Gold, new UnityAction[3]{
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.1f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.15f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.20f)))}), 
            new Augment("MulticasterProb","Casting Cascade", new string[3]{"When you fire a shot, gain a 20% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 30% chance to fire an extra shot", 
                                                                        "When you fire a shot, gain a 40% chance to fire an extra shot"}, "MulticasterProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.2f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.3f))),
                                                                                                                                                new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.4f)))}), 

            new Augment("CritUnlock" ,"Critical Inferno", new string[1]{"Unlock the ability to critical strike"}, "CritUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("CritUnlock");
                Flamey.Instance.addOnShootEffect(new CritUnlock(0.1f, 1.5f));
                Deck.Instance.AddAugmentClass(new List<string>{"CritMult","CritChance"});            
            })}, baseCard: true),  
            new Augment("CritMult","Lucky Shots", new string[3]{"Gain +5% critical strike damage", 
                                                            "Gain +15% critical strike damage", 
                                                            "Gain +30% critical strike damage"}, "CritMult", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.3f)))}), 
            new Augment("CritMult","Critical Thinking", new string[3]{"Gain +15% critical strike damage", 
                                                            "Gain +35% critical strike damage", 
                                                            "Gain +70% critical strike damage"}, "CritMult", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.35f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.7f)))}),
            new Augment("CritChance","Critical Miracle", new string[3]{"Gain +3% critical strike chance", 
                                                            "Gain +7% critical strike chance", 
                                                            "Gain +15% critical strike chance"}, "CritChance", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.03f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.07f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.15f, 0f)))}),
            new Augment("CritChance","Fate's Favor", new string[3]{"Gain +7% critical strike chance", 
                                                            "Gain +15% critical strike chance", 
                                                            "Gain +30% critical strike chance"}, "CritChance", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.07f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.15f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.3f, 0f)))}),
            new Augment("CritChance","Overheat", new string[3]{"Gain +10% critical strike chance and +30% critical strike damage", 
                                                            "Gain +15% critical strike chance and +60% critical strike damage", 
                                                            "Gain +30% critical strike chance and +120% critical strike damage"}, "CritChance", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.1f, 0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.15f, 0.6f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.3f, 1.2f)))}),
            new Augment("VampUnlock" ,"The Blood Mage", new string[1]{"Unlock the ability to life-steal"}, "VampUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("VampUnlock");
                Flamey.Instance.addOnHitEffect(new VampOnHit(0.05f,0.05f));
                Deck.Instance.AddAugmentClass(new List<string>{"VampProb","VampPerc"});            
            })}, baseCard: true),  
            new Augment("VampProb","Steal to Heal", new string[3]{"Gain +2% chance to proc your Blood Mage effect", 
                                                            "Gain +5% chance to proc your Blood Mage effect", 
                                                            "Gain +8% chance to proc your Blood Mage effect"}, "VampProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.08f)))}), 
            new Augment("VampProb","Eternal Hunger", new string[3]{"Gain +4% chance to proc your Blood Mage effect", 
                                                            "Gain +10% chance to proc your Blood Mage effect", 
                                                            "Gain +16% chance to proc your Blood Mage effect"}, "VampProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.04f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.16f)))}),
            new Augment("VampProb","Soul Harvester", new string[3]{"Gain +10% chance to proc your Blood Mage effect", 
                                                            "Gain +20% chance to proc your Blood Mage effect", 
                                                            "Gain +30% chance to proc your Blood Mage effect"}, "VampProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.3f)))}),  
            new Augment("VampPerc","Sustenance", new string[3]{"Gain +2% Heal on your Blood Mage effect", 
                                                            "Gain +3% Heal on your Blood Mage effect", 
                                                            "Gain +5% Heal on your Blood Mage effect"}, "VampPerc", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.02f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.03f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.05f,0f)))}),  
            new Augment("VampPerc","Vampire Survivor", new string[3]{"Gain +4% Heal on your Blood Mage effect", 
                                                            "Gain +7% Heal on your Blood Mage effect", 
                                                            "Gain +12% Heal on your Blood Mage effect"}, "VampPerc", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.04f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.07f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.12f,0f)))}),
            new Augment("VampPerc","Blood Pact", new string[3]{"Gain +8% Heal on your Blood Mage effect", 
                                                            "Gain +15% Heal on your Blood Mage effect", 
                                                            "Gain +25% Heal on your Blood Mage effect"}, "VampPerc", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.08f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.15f,0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.25f,0f)))}),    
            
            new Augment("BurstUnlock" ,"Burst Shot", new string[1]{"Unlock the ability to send burst shots"}, "BurstUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("BurstUnlock");
                Flamey.Instance.addOnShootEffect(new BurstShot(50, 5));
                Deck.Instance.AddAugmentClass(new List<string>{"BurstInterval","BurstAmount"});            
            })}, baseCard: true),  
            new Augment("BurstInterval","Happy Trigger", new string[3]{"You will need 2 shots less to proc Burst Shot", 
                                                                        "You will need 4 shots less to proc Burst Shot", 
                                                                        "You will need 7 shots less to proc Burst Shot"}, "BurstInterval", Tier.Silver, new UnityAction[3]{
                                                                                                                    new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(2,0))),
                                                                                                                    new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(4,0))),
                                                                                                                    new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(7,0)))}), 
            new Augment("BurstInterval","Bullet Symphony", new string[3]{"You will need 5 shots less to proc Burst Shot", 
                                                            "You will need 8 shots less to proc Burst Shot", 
                                                            "You will need 14 shots less to proc Burst Shot"}, "BurstInterval", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(5,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(8,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(14,0)))}), 
            new Augment("BurstInterval","Make It Rain", new string[3]{"You will need 10 shots less to proc Burst Shot", 
                                                            "You will need 15 shots less to proc Burst Shot", 
                                                            "You will need 25 shots less to proc Burst Shot"}, "BurstInterval", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(10,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(15,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(25,0)))}), 
            new Augment("BurstAmount","Burst Barricade", new string[3]{"Your Burst Shot will shoot an extra flame", 
                                                            "Your Burst Shot will +2 flames", 
                                                            "Your Burst Shot will +3 flames"}, "BurstAmount", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,3)))}), 
            new Augment("BurstAmount","Burst Unleashed", new string[3]{"Your Burst Shot will +2 flames", 
                                                            "Your Burst Shot will +3 flames", 
                                                            "Your Burst Shot will +5 flames"}, "BurstAmount", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,3))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,6)))}), 
            new Augment("BurstAmount","Burst to Victory", new string[3]{"Your Burst Shot will +3 flames", 
                                                            "Your Burst Shot will +5 flames", 
                                                            "Your Burst Shot will +10 flames"}, "BurstAmount", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,3))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,5))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,10)))}), 
            
            new Augment("IceUnlock" ,"Frost Fire", new string[1]{"Unlock the ability to Slow enemies using ice(?)"}, "IceUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("IceUnlock");
                Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"IceDuration","IceProb"});            
            })}, baseCard: true),  
            new Augment("IceProb","IcyHot", new string[3]{"Gain +3% chance to proc your Frost Fire effect", 
                                                            "Gain +5% chance to proc your Frost Fire effect", 
                                                            "Gain +8% chance to proc your Frost Fire effect"}, "IceProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.03f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.08f)))}), 
            new Augment("IceProb","Glacial Energy", new string[3]{"Gain +7% chance to proc your Frost Fire effect", 
                                                            "Gain +15% chance to proc your Frost Fire effect", 
                                                            "Gain +20% chance to proc your Frost Fire effect"}, "IceProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.07f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.20f)))}), 
            new Augment("IceProb","A Dance of Fire and Ice", new string[3]{"Gain +15% chance to proc your Frost Fire effect", 
                                                            "Gain +25% chance to proc your Frost Fire effect", 
                                                            "Gain +40% chance to proc your Frost Fire effect"}, "IceProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.40f)))}), 
            new Augment("IceDuration","Slowly but Surely", new string[3]{"Your Frost Fire effect lasts for 0.2 seconds more", 
                                                            "Your Frost Fire effect lasts for 0.4 seconds more", 
                                                            "Your Frost Fire effect lasts for 0.6 seconds more"}, "IceDuration", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0.2f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(.4f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(.6f, 0)))}),
            new Augment("IceDuration","Frost Bite", new string[3]{"Your Frost Fire effect lasts for 0.5 seconds more", 
                                                            "Your Frost Fire effect lasts for 1 seconds more", 
                                                            "Your Frost Fire effect lasts for 1.5 seconds more"}, "IceDuration", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(.5f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1.5f, 0)))}), 
            new Augment("IceDuration","Absolute Zero", new string[3]{"Your Frost Fire effect lasts for 1 seconds more", 
                                                            "Your Frost Fire effect lasts for 2 seconds more", 
                                                            "Your Frost Fire effect lasts for 3 seconds more"}, "IceDuration", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(3, 0)))}),   
            
            new Augment("ShredUnlock" ,"Shredding Flames", new string[1]{"Unlock the ability to shred enemy armor"}, "ShredUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("ShredUnlock");
                Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"ShredProb","ShredPerc"});            
            })}, baseCard: true),   
            new Augment("ShredProb","Weaken", new string[3]{"Gain +5% chance to proc your Shredding Flames effect", 
                                                            "Gain +10% chance to proc your Shredding Flames effect", 
                                                            "Gain +15% chance to proc your Shredding Flames effect"}, "ShredProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.05f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.15f, 0f)))}),
            new Augment("ShredProb","Armor Corruptor", new string[3]{"Gain +10% chance to proc your Shredding Flames effect", 
                                                            "Gain +20% chance to proc your Shredding Flames effect", 
                                                            "Gain +30% chance to proc your Shredding Flames effect"}, "ShredProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.2f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.3f, 0f)))}),
            new Augment("ShredProb","Disintegration Field", new string[3]{"Gain +20% chance to proc your Shredding Flames effect", 
                                                            "Gain +40% chance to proc your Shredding Flames effect", 
                                                            "Gain +60% chance to proc your Shredding Flames effect"}, "ShredProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.2f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.4f, 0f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.6f, 0f)))}),  
            new Augment("ShredPerc","Cheese Shredder", new string[3]{"Your Shredding Flames effect reduces +3% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +5% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +10% more enemy armor per proc"}, "ShredPerc", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.03f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.1f)))}),  
            new Augment("ShredPerc","Black Cleaver", new string[3]{"Your Shredding Flames effect reduces +7% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +15% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +20% more enemy armor per proc"}, "ShredPerc", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.2f)))}),   
            new Augment("ShredPerc","Molecular Decomposition", new string[3]{"Your Shredding Flames effect reduces +15% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +30% more enemy armor per proc", 
                                                            "Your Shredding Flames effect reduces +40% more enemy armor per proc"}, "ShredPerc", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.4f)))}),                         
            new Augment("Assassins" ,"Assassin's Path", new string[1]{"Unlock the ability to pierce armor and execute enemies"}, "Assassins", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Assassins");
                Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f));
                Flamey.Instance.addArmorPen(0.05f);
                Deck.Instance.AddAugmentClass(new List<string>{"ArmorPen","Execute"});            
            })}, baseCard: true),  
            new Augment("Execute","Execution Enforcer", new string[3]{"You can execute enemies for +1% of their Max Health", 
                                                            "You can execute enemies for +2% of their Max Health", 
                                                            "You can execute enemies for +5% of their Max Health"}, "Execute", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.01f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f)))}),
            new Augment("Execute","Soul Collector", new string[3]{"You can execute enemies for +2% of their Max Health", 
                                                            "You can execute enemies for +5% of their Max Health", 
                                                            "You can execute enemies for +10% of their Max Health"}, "Execute", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f)))}),
            new Augment("Execute","La Guillotine", new string[3]{"You can execute enemies for +5% of their Max Health", 
                                                            "You can execute enemies for +10% of their Max Health", 
                                                            "You can execute enemies for +25% of their Max Health"}, "Execute", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.25f)))}),
            new Augment("ArmorPen","Shell Breaker", new string[3]{"Gain +3% Armor Penetration", 
                                                            "Gain +5% Armor Penetration", 
                                                            "Gain +7% Armor Penetration"}, "ArmorPen", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.03f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.05f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.07f))}),
            new Augment("ArmorPen","Quantum Piercing", new string[3]{"Gain +5% Armor Penetration", 
                                                            "Gain +10% Armor Penetration", 
                                                            "Gain +15% Armor Penetration"}, "ArmorPen", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.05f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.15f))}),
            new Augment("ArmorPen","Lance of Aether", new string[3]{"Gain +10% Armor Penetration", 
                                                            "Gain +20% Armor Penetration", 
                                                            "Gain +30% Armor Penetration"}, "ArmorPen", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.1f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.2f)),
                                                                                                                        new UnityAction(() => Flamey.Instance.addArmorPen(0.3f))}),
            new Augment("BlueFlameUnlock" ,"Blue Flame", new string[1]{"Unlock the ability to shoot blue flames that inflict extra damage"}, "BlueFlameUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("BlueFlameUnlock");
                Flamey.Instance.addOnShootEffect(new KrakenSlayer(20, 100));
                Deck.Instance.AddAugmentClass(new List<string>{"BlueFlameInterval","BlueFlameDmg"});            
            })}, baseCard: true),  
            new Augment("BlueFlameInterval","The Bluer The Better", new string[3]{"You will need 1 shot less to proc Blue Flame", 
                                                            "You will need 2 shots less to proc Blue Flame", 
                                                            "You will need 3 shots less to proc Blue Flame"}, "BlueFlameInterval", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(1, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(3, 0)))}),
            new Augment("BlueFlameInterval","Propane Combustion", new string[3]{"You will need 2 shot less to proc Blue Flame", 
                                                            "You will need 4 shots less to proc Blue Flame", 
                                                            "You will need 6 shots less to proc Blue Flame"}, "BlueFlameInterval", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(4, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(6, 0)))}),
            new Augment("BlueFlameInterval","Never ending Blue", new string[3]{"You will need 4 shot less to proc Blue Flame", 
                                                            "You will need 8 shots less to proc Blue Flame", 
                                                            "You will need 12 shots less to proc Blue Flame"}, "BlueFlameInterval", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(4, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(8, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(12, 0)))}),
            new Augment("BlueFlameDmg","Propane Leakage", new string[3]{"Your Blue Flame deals +15 extra damage", 
                                                            "Your Blue Flame deals +25 extra damage", 
                                                            "Your Blue Flame deals +40 extra damage"}, "BlueFlameDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 15))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50)))}),
            new Augment("BlueFlameDmg","Powerfull Blue", new string[3]{"Your Blue Flame deals +30 extra damage", 
                                                            "Your Blue Flame deals +50 extra damage", 
                                                            "Your Blue Flame deals +80 extra damage"}, "BlueFlameDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100)))}),
            new Augment("BlueFlameDmg","Blue Inferno", new string[3]{"Your Blue Flame deals +50 extra damage", 
                                                            "Your Blue Flame deals +100 extra damage", 
                                                            "Your Blue Flame deals +200 extra damage"}, "BlueFlameDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 200)))}),
            
            new Augment("OrbitalUnlock" ,"Orbital Flames", new string[1]{"A tiny Flame will orbit around you damaging the foes it collides with"}, "OrbitalUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("OrbitalUnlock");
                Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 25));
                Deck.Instance.AddAugmentClass(new List<string>{"OrbitalDmg","OrbitalAmount"});            
            })}, baseCard: true), 
            new Augment("OrbitalAmount","Tame the Flames", new string[3]{"Gain +1 tiny Flame in your Orbital Field", 
                                                            "Gain +2 tiny Flame in your Orbital Field", 
                                                            "Gain +3 tiny Flame in your Orbital Field"}, "OrbitalAmount", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(2, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(3, 0)))}),
            new Augment("OrbitalDmg","Tiny Flames Win", new string[3]{"Your Orbital Flames deal +10 damage", 
                                                            "Your Orbital Flames deal +15 damage", 
                                                            "Your Orbital Flames deal +20 damage"}, "OrbitalDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 10))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 15))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 20)))}),
            new Augment("OrbitalDmg","Reliable Damage", new string[3]{"Your Orbital Flames deal +20 damage", 
                                                            "Your Orbital Flames deal +30 damage", 
                                                            "Your Orbital Flames deal +40 damage"}, "OrbitalDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 20))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 40)))}),
            new Augment("OrbitalDmg","Saturn", new string[3]{"Your Orbital Flames deal +40 damage", 
                                                            "Your Orbital Flames deal +60 damage", 
                                                            "Your Orbital Flames deal +80 damage"}, "OrbitalDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 40))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0,60))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 80)))}),

            new Augment("LavaPoolUnlock" ,"Lava Pool", new string[1]{"Unlock the ability to create Lava Pools that ignore enemy armor"}, "LavaPoolUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("LavaPoolUnlock");
                Flamey.Instance.addOnLandEffect(new BurnOnLand(1f, 25, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"LavaPoolDmg","LavaPoolSize","LavaPoolProb","LavaPoolDuration"});            
            })}, baseCard: true), 
            new Augment("LavaPoolDmg","Hot Tub", new string[3]{"Your Lava Pool will inflict +5 damage per second", 
                                                            "Your Lava Pool will inflict +10 damage per second", 
                                                            "Your Lava Pool will inflict +15 damage per second"}, "LavaPoolDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,5,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,10,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,15,0,0)))}),
            new Augment("LavaPoolDmg","Magical Scorch", new string[3]{"Your Lava Pool will inflict +10 damage per second", 
                                                            "Your Lava Pool will inflict +20 damage per second", 
                                                            "Your Lava Pool will inflict +30 damage per second"}, "LavaPoolDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,10,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,20,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,30,0,0)))}),
            new Augment("LavaPoolDmg","Conflagration", new string[3]{"Your Lava Pool will inflict +20 damage per second", 
                                                            "Your Lava Pool will inflict +40 damage per second", 
                                                            "Your Lava Pool will inflict +60 damage per second"}, "LavaPoolDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,20,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,40,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,60,0,0)))}),
            new Augment("LavaPoolProb","Hot Steps", new string[3]{"Gain +3% probability of spawning a Lava Pool when your shot lands", 
                                                            "Gain +5% probability of spawning a Lava Pool when your shot lands", 
                                                            "Gain +7% probability of spawning a Lava Pool when your shot lands"}, "LavaPoolProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.03f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.05f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.07f,0)))}),
            new Augment("LavaPoolProb","Lava here, Lava there", new string[3]{"Gain +7% probability of spawning a Lava Pool when your shot lands", 
                                                            "Gain +10% probability of spawning a Lava Pool when your shot lands", 
                                                            "Gain +15% probability of spawning a Lava Pool when your shot lands"}, "LavaPoolProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.07f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.15f,0)))}),
            new Augment("LavaPoolProb","The Apocalypse", new string[3]{"Gain +15% probability of spawning a Lava Pool when your shot lands", 
                                                            "Gain +20% probability of spawning a Lava Pool when your shot lands", 
                                                            "Gain +30% probability of spawning a Lava Pool when your shot lands"}, "LavaPoolProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.2f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.3f,0)))}),
            new Augment("LavaPoolSize","Heat Area", new string[3]{"Your Lava Pool grows by +2 units", 
                                                            "Your Lava Pool grows by +2.5 units", 
                                                            "Your Lava Pool grows by +3 units"}, "LavaPoolSize", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.2f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.25f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.3f,0,0,0)))}),
            new Augment("LavaPoolSize","Lava Lakes", new string[3]{"Your Lava Pool grows by +4 units", 
                                                            "Your Lava Pool grows by +5 units", 
                                                            "Your Lava Pool grows by +6 units"}, "LavaPoolSize", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.4f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.5f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.6f,0,0,0)))}),
            new Augment("LavaPoolSize","Inside the volcano", new string[3]{"Your Lava Pool grows by +8 units", 
                                                            "Your Lava Pool grows by +10 units", 
                                                            "Your Lava Pool grows by +12 units"}, "LavaPoolSize", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.8f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(1f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(1.2f,0,0,0)))}),
            new Augment("LavaPoolDuration","Sear the ground", new string[3]{"Your Lava Pool lasts for +0.3 seconds", 
                                                            "Your Lava Pool lasts for +0.5 seconds", 
                                                            "Your Lava Pool lasts for +0.8 seconds"}, "LavaPoolDuration", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.5f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.8f)))}),
            new Augment("LavaPoolDuration","Eternally Hot", new string[3]{"Your Lava Pool lasts for +0.8 seconds", 
                                                            "Your Lava Pool lasts for +1.2 seconds", 
                                                            "Your Lava Pool lasts for +1.7 seconds"}, "LavaPoolDuration", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.7f)))}),
            new Augment("LavaPoolDuration","Unsettling Magma", new string[3]{"Your Lava Pool lasts for +1.8 seconds", 
                                                            "Your Lava Pool lasts for +2.4 seconds", 
                                                            "Your Lava Pool lasts for +3.5 seconds"}, "LavaPoolDuration", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,2.4f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,3.5f)))}),
            new Augment("StatikUnlock" ,"Static Energy", new string[1]{"Unlock the ability to send static energy to enemies nearby of your target"}, "StatikUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("StatikUnlock");
                Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,25,3));
                Deck.Instance.AddAugmentClass(new List<string>{"StatikProb","StatikDmg","StatikTTL"});            
            })}, baseCard: true), 
            new Augment("StatikProb","Watts Up", new string[3]{"Gain +3% probability to proc your Static Energy effect", 
                                                            "Gain +5% probability to proc your Static Energy effect", 
                                                            "Gain +7% probability to proc your Static Energy effect"}, "StatikProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.03f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.05f,0,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.07f,0,0)))}),
            new Augment("StatikProb","Electrifying Possibilities", new string[3]{"Gain +7% probability to proc your Static Energy effect", 
                                                            "Gain +10% probability to proc your Static Energy effect", 
                                                            "Gain +15% probability to proc your Static Energy effect"}, "StatikProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.07f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.15f,0,0)))}),
            new Augment("StatikProb","The Sparkster", new string[3]{"Gain +20% probability to proc your Static Energy effect", 
                                                            "Gain +25% probability to proc your Static Energy effect", 
                                                            "Gain +35% probability to proc your Static Energy effect"}, "StatikProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.2f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.25f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.35f,0,0)))}),
            new Augment("StatikDmg","Shock Dart", new string[3]{"Your Statik Energy deals +5 extra damage", 
                                                            "Your Statik Energy deals +10 extra damage", 
                                                            "Your Statik Energy deals +15 extra damage"}, "StatikDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,5,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,10,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,15,0)))}),
            new Augment("StatikDmg","Shocking Advancement", new string[3]{"Your Statik Energy deals +10 extra damage", 
                                                            "Your Statik Energy deals +20 extra damage", 
                                                            "Your Statik Energy deals +40 extra damage"}, "StatikDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,10,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,20,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,30,0)))}),
            new Augment("StatikDmg","Zeus", new string[3]{"Your Statik Energy deals +20 extra damage", 
                                                            "Your Statik Energy deals +50 extra damage", 
                                                            "Your Statik Energy deals +100 extra damage"}, "StatikDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,20,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,50,0))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,100,0)))}),
            new Augment("StatikTTL","Conductive materials", new string[3]{"Your Statik Energy will be able to cross through 1 more enemy", 
                                                            "Your Statik Energy will be able to cross through 1 more enemies", 
                                                            "Your Statik Energy will be able to cross through 2 more enemies"}, "StatikTTL", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2)))}),
            new Augment("StatikTTL","Feel the Flow", new string[3]{"Your Statik Energy will be able to cross through 1 more enemies", 
                                                            "Your Statik Energy will be able to cross through 2 more enemies", 
                                                            "Your Statik Energy will be able to cross through 4 more enemies"}, "StatikTTL", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,4)))}),
            new Augment("StatikTTL","Amping Up!", new string[3]{"Your Statik Energy will be able to cross through 2 more enemies", 
                                                            "Your Statik Energy will be able to cross through 3 more enemies", 
                                                            "Your Statik Energy will be able to cross through 6 more enemies"}, "StatikTTL", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,3))),
                                                                                                                           new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,6)))}),
            new Augment("IcePoolUnlock" ,"Ice Pool", new string[1]{"Unlock the ability to create Snow Pools that slow down enemies"}, "IcePoolUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("IcePoolUnlock");
                Flamey.Instance.addOnLandEffect(new IceOnLand(1f, 0.1f, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"IcePoolDuration","IcePoolProb","IcePoolSlow","IcePoolSize"});            
            })}, baseCard: true), 
            new Augment("IcePoolSlow","Cold Bath", new string[3]{"Your Snow Pool will slow down enemies for 2% more", 
                                                            "Your Snow Pool will slow down enemies for 3% more", 
                                                            "Your Snow Pool will slow down enemies for 4% more"}, "IcePoolSlow", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.02f,0,0))),  
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.03f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.04f,0,0)))}),
            new Augment("IcePoolSlow","Glacial Grip", new string[3]{"Your Snow Pool will slow down enemies for 4% more", 
                                                            "Your Snow Pool will slow down enemies for 6% more", 
                                                            "Your Snow Pool will slow down enemies for 8% more"}, "IcePoolSlow", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.04f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.06f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.08f,0,0)))}),
            new Augment("IcePoolSlow","Frozen Stasis", new string[3]{"Your Snow Pool will slow down enemies for 8% more", 
                                                            "Your Snow Pool will slow down enemies for 12% more", 
                                                            "Your Snow Pool will slow down enemies for 16% more"}, "IcePoolSlow", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.08f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.12f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.16f,0,0)))}),
            new Augment("IcePoolProb","Cold Steps", new string[3]{"Gain +3% probability of spawning an Snow Pool when your shot lands", 
                                                            "Gain +5% probability of spawning an Snow Pool when your shot lands", 
                                                            "Gain +7% probability of spawning an Snow Pool when your shot lands"}, "IcePoolProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.03f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.05f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.07f,0)))}),
            new Augment("IcePoolProb","Ice here, Ice there", new string[3]{"Gain +7% probability of spawning an Snow Pool when your shot lands", 
                                                            "Gain +10% probability of spawning an Snow Pool when your shot lands", 
                                                            "Gain +15% probability of spawning an Snow Pool when your shot lands"}, "IcePoolProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.07f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.15f,0)))}),
            new Augment("IcePoolProb","The North Pole", new string[3]{"Gain +15% probability of spawning an Snow Pool when your shot lands", 
                                                            "Gain +20% probability of spawning an Snow Pool when your shot lands", 
                                                            "Gain +30% probability of spawning an Snow Pool when your shot lands"}, "IcePoolProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.2f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.3f,0)))}),
            new Augment("IcePoolSize","Cold breeze", new string[3]{"Your Snow Pool grows by +2 units", 
                                                            "Your Snow Pool grows by +2.5 units", 
                                                            "Your Snow Pool grows by +3 units"}, "IcePoolSize", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.2f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.25f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.3f,0,0,0)))}),
            new Augment("IcePoolSize","Frozen Lakes", new string[3]{"Your Snow Pool grows by +4 units", 
                                                            "Your Snow Pool grows by +5 units", 
                                                            "Your Snow Pool grows by +6 units"}, "IcePoolSize", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.4f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.5f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.6f,0,0,0)))}),
            new Augment("IcePoolSize","Inside the iceberg", new string[3]{"Your Snow Pool grows by +8 units", 
                                                            "Your Snow Pool grows by +10 units", 
                                                            "Your Snow Pool grows by +12 units"}, "IcePoolSize", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.8f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(1f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(1.2f,0,0,0)))}),
            new Augment("IcePoolDuration","Cooling down", new string[3]{"Your Snow Pool lasts for +0.3 seconds", 
                                                            "Your Snow Pool lasts for +0.5 seconds", 
                                                            "Your Snow Pool lasts for +0.8 seconds"}, "IcePoolDuration", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,0.5f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,0.8f)))}),
            new Augment("IcePoolDuration","Eternally Cold", new string[3]{"Your Snow Pool lasts for +0.8 seconds", 
                                                            "Your Snow Pool lasts for +1.2 seconds", 
                                                            "Your Snow Pool lasts for +1.7 seconds"}, "IcePoolDuration", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,0.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,1.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,1.7f)))}),
            new Augment("IcePoolDuration","Absolute Frost", new string[3]{"Your Ice Pool lasts for +1.8 seconds", 
                                                            "Your Ice Pool lasts for +2.4 seconds", 
                                                            "Your Ice Pool lasts for +3.5 seconds"}, "IcePoolDuration", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,1.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,2.4f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,3.5f)))}),
            new Augment("ThornsUnlock" ,"Thorns", new string[1]{"Unlock the ability to deal damage back when hitted"}, "ThornsUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("ThornsUnlock");
                Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0.1f, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"ThornsPerc","ThornsProb"});            
            })}, baseCard: true),  
            new Augment("ThornsPerc","Innocent Spikes", new string[3]{"Your Thorn effect will reflect +2% of your Armor", 
                                                            "Your Thorn effect will reflect +5% of your Armor", 
                                                            "Your Thorn effect will reflect +7% of your Armor"}, "ThornsPerc", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .02f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .07f)))}),
            new Augment("ThornsPerc","Spiky Vengeance", new string[3]{"Your Thorn effect will reflect +5% of your Armor", 
                                                            "Your Thorn effect will reflect +10% of your Armor", 
                                                            "Your Thorn effect will reflect +15% of your Armor"}, "ThornsPerc", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .15f)))}),
            new Augment("ThornsPerc","PufferFish", new string[3]{"Your Thorn effect will reflect +15% of your Armor", 
                                                            "Your Thorn effect will reflect +25% of your Armor", 
                                                            "Your Thorn effect will reflect +35% of your Armor"}, "ThornsPerc", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .35f)))}),
            new Augment("ThornsProb","Prickly Fire", new string[3]{"Gain +3% chance to proc your Thorns effect", 
                                                            "Gain +5% chance to proc your Thorns effect", 
                                                            "Gain +7% chance to proc your Thorns effect"}, "ThornsProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.03f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.05f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.07f, 0)))}),
            new Augment("ThornsProb","Thorns everywhere!", new string[3]{"Gain +7% chance to proc your Thorns effect", 
                                                            "Gain +10% chance to proc your Thorns effect", 
                                                            "Gain +15% chance to proc your Thorns effect"}, "ThornsProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.07f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.1f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.15f, 0)))}),
            new Augment("ThornsProb","Cactus on Fire", new string[3]{"Gain +20% chance to proc your Thorns effect", 
                                                            "Gain +25% chance to proc your Thorns effect", 
                                                            "Gain +35% chance to proc your Thorns effect"}, "ThornsProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.2f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.25f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.35f, 0)))}),
            

            new Augment("MoneyMult","Little by little", new string[3]{"Your ember multiplier will improve by +5%", 
                                                            "Your ember multiplier will improve by +10%", 
                                                            "Your ember multiplier will improve by 20%"}, "MoneyMult", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .2f)))}, baseCardUpgrade:true),
            new Augment("MoneyMult","Stock Trading", new string[3]{"Your ember multiplier will improve by +10%", 
                                                            "Your ember multiplier will improve by +20%", 
                                                            "Your ember multiplier will improve by +35%"}, "MoneyMult", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .35f)))}, baseCardUpgrade:true),
            new Augment("MoneyMult","Billionaire", new string[3]{"Your ember multiplier will improve by +15%", 
                                                            "Your ember multiplier will improve by +35%", 
                                                            "Your ember multiplier will improve by +50%"}, "MoneyMult", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .35f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .5f)))}, baseCardUpgrade:true),
            new Augment("MoneyProb","Savings Account", new string[3]{"Gain +3% probability to drop more embers from enemies", 
                                                            "Gain +5% probability to drop more embers from enemies", 
                                                            "Gain +7% probability to drop more embers from enemies"}, "MoneyProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.03f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.05f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.07f, 0)))}, baseCardUpgrade:true),
            new Augment("MoneyProb","Tax Payment", new string[3]{"Gain +7% probability to drop more embers from enemies", 
                                                            "Gain +10% probability to drop more embers from enemies", 
                                                            "Gain +15% probability to drop more embers from enemies"}, "MoneyProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.07f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.1f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.15f, 0)))}, baseCardUpgrade:true),
            new Augment("MoneyProb","Robbery", new string[3]{"Gain +15% probability to drop more embers from enemies", 
                                                            "Gain +20% probability to drop more embers from enemies", 
                                                            "Gain +30% probability to drop more embers from enemies"}, "MoneyProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.15f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0.2f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(.3f, 0)))}, baseCardUpgrade:true),
            new Augment("GambleImprove","Not enough refreshes", new string[3]{"Gain a random silver augment", 
                                                            "Gain 2 random silver augments", 
                                                            "Gain 1 random gold augment"}, "GambleImprove", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Silver))),
                                                                                                                        new UnityAction(() => {for (int i = 0; i < 2; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Silver));}}),
                                                                                                                        new UnityAction(() => Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Gold)))}, baseCardUpgrade:true),
            new Augment("GambleImprove","Feelin' Blessed", new string[3]{"Gain a random gold augment", 
                                                            "Gain 2 random silver augments", 
                                                            "Gain 2 random gold augments"}, "GambleImprove", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Gold))),
                                                                                                                        new UnityAction(() => {for (int i = 0; i < 2; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Silver));}}),
                                                                                                                        new UnityAction(() => {for (int i = 0; i < 2; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Gold));}})}, baseCardUpgrade:true),
            new Augment("GambleImprove","Roll the Dice", new string[3]{"Gain a random prismatic augment", 
                                                            "Gain 2 random gold augments", 
                                                            "Gain 2 random prismatic augments"}, "GambleImprove", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Prismatic))),
                                                                                                                        new UnityAction(() => {for (int i = 0; i < 2; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Gold));}}),
                                                                                                                        new UnityAction(() => {for (int i = 0; i < 2; i++){Deck.Instance.ActivateAugment(Deck.Instance.randomPicking(Tier.Prismatic));}})}, baseCardUpgrade:true),

            
            new Augment("DrainPoolUnlock" ,"Drain Pool", new string[1]{"Unlock the ability to create Draining Pools"}, "DrainPoolUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("DrainPoolUnlock");
                Flamey.Instance.addOnLandEffect(new DrainOnLand(1f, 0.01f, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"DrainPoolPerc","DrainPoolProb","DrainPoolSize","DrainPoolDuration"});            
            })}, baseCard: true),  

            new Augment("DrainPoolPerc","Harvesting", new string[3]{"Your Drain Pool will heal you for +0.25% Enemy Max HP per tick", 
                                                            "Your Drain Pool will heal you for +0.5% Enemy Max HP per tick", 
                                                            "Your Drain Pool will heal you for +1% Enemy Max HP per tick"}, "DrainPoolPerc", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.0025f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.005f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.01f,0,0)))}),
            new Augment("DrainPoolPerc","Carnivorous Plant", new string[3]{"Your Drain Pool will heal you for +0.75% Enemy Max HP per tick", 
                                                            "Your Drain Pool will heal you for +1% Enemy Max HP per tick", 
                                                            "Your Drain Pool will heal you for +2.5% Enemy Max HP per tick"}, "DrainPoolPerc", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.0075f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.01f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.025f,0,0)))}),
            new Augment("DrainPoolPerc","Photosynthesis", new string[3]{"Your Drain Pool will heal you for +2% Enemy Max HP per tick", 
                                                            "Your Drain Pool will heal you for +3% Enemy Max HP per tick", 
                                                            "Your Drain Pool will heal you for +5% Enemy Max HP per tick"}, "DrainPoolPerc", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.02f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.03f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.05f,0,0)))}),
            new Augment("DrainPoolProb","Sowing the Field", new string[3]{"Gain +3% probability of spawning a Drain Pool when your shot lands", 
                                                            "Gain +5% probability of spawning a Drain Pool when your shot lands", 
                                                            "Gain +7% probability of spawning a Drain Pool when your shot lands"}, "DrainPoolProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.03f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.05f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.07f,0)))}),
            new Augment("DrainPoolProb","Garden's Embrace", new string[3]{"Gain +7% probability of spawning a Drain Pool when your shot lands", 
                                                            "Gain +10% probability of spawning a Drain Pool when your shot lands", 
                                                            "Gain +15% probability of spawning a Drain Pool when your shot lands"}, "DrainPoolProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.07f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.15f,0)))}),
            new Augment("DrainPoolProb","Lifebloom Aura", new string[3]{"Gain +15% probability of spawning a Drain Pool when your shot lands", 
                                                            "Gain +20% probability of spawning a Drain Pool when your shot lands", 
                                                            "Gain +30% probability of spawning a Drain Pool when your shot lands"}, "DrainPoolProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.2f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.3f,0)))}),
            new Augment("DrainPoolSize","Sprouting", new string[3]{"Your Drain Pool grows by +2 units", 
                                                            "Your Drain Pool grows by +2.5 units", 
                                                            "Your Drain Pool grows by +3 units"}, "DrainPoolSize", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.2f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.25f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.3f,0,0,0)))}),
            new Augment("DrainPoolSize","Flowering Surge", new string[3]{"Your Drain Pool grows by +4 units", 
                                                            "Your Drain Pool grows by +5 units", 
                                                            "Your Drain Pool grows by +6 units"}, "DrainPoolSize", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.4f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.5f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.6f,0,0,0)))}),
            new Augment("DrainPoolSize","Botanical Boom", new string[3]{"Your Drain Pool grows by +8 units", 
                                                            "Your Drain Pool grows by +10 units", 
                                                            "Your Drain Pool grows by +12 units"}, "DrainPoolSize", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.8f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(1f,0,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(1.2f,0,0,0)))}),
            new Augment("DrainPoolDuration","Lasting Petals", new string[3]{"Your Drain Pool lasts for +0.3 seconds", 
                                                            "Your Drain Pool lasts for +0.5 seconds", 
                                                            "Your Drain Pool lasts for +0.8 seconds"}, "DrainPoolDuration", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,0.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,0.5f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,0.8f)))}),
            new Augment("DrainPoolDuration","Everlasting Blossom", new string[3]{"Your Drain Pool lasts for +0.8 seconds", 
                                                            "Your Drain Pool lasts for +1.2 seconds", 
                                                            "Your Drain Pool lasts for +1.7 seconds"}, "DrainPoolDuration", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,0.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,1.2f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,1.7f)))}),
            new Augment("DrainPoolDuration","Garden of Eden", new string[3]{"Your Drain Pool lasts for +1.8 seconds", 
                                                            "Your Drain Pool lasts for +2.4 seconds", 
                                                            "Your Drain Pool lasts for +3.5 seconds"}, "DrainPoolDuration", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,1.8f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,2.4f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,3.5f)))}),
            
            
            new Augment("ExplodeUnlock" ,"Explosion", new string[1]{"Unlock the ability to generate explosions whenever enemies die"}, "ExplodeUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("ExplodeUnlock");
                Flamey.Instance.addOnKillEffect(new Explosion(0.1f,50));
                Deck.Instance.AddAugmentClass(new List<string>{"ExplodeProb","ExplodeDmg"});            
            })}, baseCard: true),  
            new Augment("ExplodeProb","Bomb Rush", new string[3]{"Gain +5% chance to generate an explosion whenever you kill an enemy", 
                                                            "Gain +7% chance to generate an explosion whenever you kill an enemy", 
                                                            "Gain +10% chance to generate an explosion whenever you kill an enemy"}, "ExplodeProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.05f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.07f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.1f,0)))}), 
            new Augment("ExplodeProb","Grenade Launcher", new string[3]{"Gain +10% chance to generate an explosion whenever you kill an enemy", 
                                                            "Gain +15% chance to generate an explosion whenever you kill an enemy", 
                                                            "Gain +20% chance to generate an explosion whenever you kill an enemy"}, "ExplodeProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.2f,0)))}),
            new Augment("ExplodeProb","Bombardment", new string[3]{"Gain +20% chance to generate an explosion whenever you kill an enemy", 
                                                            "Gain +30% chance to generate an explosion whenever you kill an enemy", 
                                                            "Gain +40% chance to generate an explosion whenever you kill an enemy"}, "ExplodeProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.2f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.3f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.4f,0)))}),  
            new Augment("ExplodeDmg","Cherry Bomb", new string[3]{"Generated explosions will deal +10 damage", 
                                                            "Generated explosions will deal +20 damage", 
                                                            "Generated explosions will deal +30 damage"}, "ExplodeDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,10))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,20))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,30)))}),  
            new Augment("ExplodeDmg","Dynamite Blast", new string[3]{"Generated explosions will deal +25 damage", 
                                                            "Generated explosions will deal +50 damage", 
                                                            "Generated explosions will deal +75 damage"}, "ExplodeDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,25))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,50))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,75)))}),
            new Augment("ExplodeDmg","Nuke Blast", new string[3]{"Generated explosions will deal +50 damage", 
                                                            "Generated explosions will deal +100 damage", 
                                                            "Generated explosions will deal +150 damage"}, "ExplodeDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,50))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,100))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,150)))}),
            
            
            new Augment("NecroUnlock" ,"Necromancer", new string[1]{"Unlock the ability to summon ghouls whenever enemies die"}, "NecroUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("NecroUnlock");
                Flamey.Instance.addOnKillEffect(new Necromancer(0.1f, 0.25f));
                Deck.Instance.AddAugmentClass(new List<string>{"NecroProb","NecroStats"});            
            })}, baseCard: true),  
            new Augment("NecroProb","Wraith Walkers", new string[3]{"Gain +5% chance to summon a ghoul whenever you kill an enemy", 
                                                            "Gain +7% chance to summon a ghoul whenever you kill an enemy", 
                                                            "Gain +10% chance to summon a ghoul whenever you kill an enemy"}, "NecroProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0.05f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.07f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.1f,0)))}), 
            new Augment("NecroProb","Soul Shepard", new string[3]{"Gain +10% chance to summon a ghoul whenever you kill an enemy", 
                                                            "Gain +15% chance to summon a ghoul whenever you kill an enemy", 
                                                            "Gain +20% chance to summon a ghoul whenever you kill an enemy"}, "NecroProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.2f,0)))}),
            new Augment("NecroProb","Crypt of the Necromancer", new string[3]{"Gain +20% chance to summon a ghoul whenever you kill an enemy", 
                                                            "Gain +30% chance to summon a ghoul whenever you kill an enemy", 
                                                            "Gain +40% chance to summon a ghoul whenever you kill an enemy"}, "NecroProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.2f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.3f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.4f,0)))}),  
            new Augment("NecroStats","Phantom Infusion", new string[3]{"Ghouls' damage will increase by 5% of your damage", 
                                                            "Ghouls' damage will increase by 7% of your damage", 
                                                            "Ghouls' damage will increase by 15% of your damage"}, "NecroStats", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.07f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.15f)))}),  
            new Augment("NecroStats","Death's Embrace", new string[3]{"Ghouls' damage will increase by 12% of your damage", 
                                                            "Ghouls' damage will increase by 15% of your damage", 
                                                            "Ghouls' damage will increase by 25% of your damage"}, "NecroStats", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.12f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.25f)))}),
            new Augment("NecroStats","Hero's Spirit", new string[3]{"Ghouls' damage will increase by 25% of your damage", 
                                                            "Ghouls' damage will increase by 30% of your damage", 
                                                            "Ghouls' damage will increase by 50% of your damage"}, "NecroStats", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.3f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.5f)))}),
            new Augment("BulletsUnlock" ,"Pirate", new string[1]{"Unlock the ability to shoot bullets around dead enemies and loot their bodies"}, "BulletsUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("BulletsUnlock");
                Flamey.Instance.addOnKillEffect(new Bullets(0.1f, 50, 2));
                Deck.Instance.AddAugmentClass(new List<string>{"BulletsProb","BulletsDmg","BulletsAmount"});            
            })}, baseCard: true),  
            new Augment("BulletsProb","Pirate Wannabe", new string[3]{"Gain +5% chance to release bullets whenever you kill an enemy", 
                                                            "Gain +7% chance to release bullets whenever you kill an enemy", 
                                                            "Gain +10% chance to release bullets whenever you kill an enemy"}, "BulletsProb", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0.05f, 0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.07f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.1f,0,0)))}), 
            new Augment("BulletsProb","Yes, Captain!", new string[3]{"Gain +10% chance to release bullets whenever you kill an enemy", 
                                                            "Gain +15% chance to release bullets whenever you kill an enemy", 
                                                            "Gain +20% chance to release bullets whenever you kill an enemy"}, "BulletsProb", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.1f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.15f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.2f,0,0)))}),
            new Augment("BulletsProb","Shoot it, Loot it", new string[3]{"Gain +20% chance to release bullets whenever you kill an enemy", 
                                                            "Gain +30% chance to release bullets whenever you kill an enemy", 
                                                            "Gain +40% chance to release bullets whenever you kill an enemy"}, "BulletsProb", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.2f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.3f,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.4f,0,0)))}),  
            new Augment("BulletsDmg","Round Shot", new string[3]{"Bullets will deal +10 damage", 
                                                            "Bullets will deal +15 damage", 
                                                            "Bullets will deal +20 damage"}, "BulletsDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,10,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,15,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,20,0)))}),  
            new Augment("BulletsDmg","Arggh!", new string[3]{"Bullets will deal +20 damage", 
                                                            "Bullets will deal +30 damage", 
                                                            "Bullets will deal +40 damage"}, "BulletsDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,20,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,30,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,40,0)))}),
            new Augment("BulletsDmg","Fire of Thieves", new string[3]{"Bullets will deal +40 damage", 
                                                            "Bullets will deal +60 damage", 
                                                            "Bullets will deal +100 damage"}, "BulletsDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,40,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,60,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,100,0)))}),
            new Augment("BulletsAmount","Cannonball Pile", new string[3]{"Your Bullet Shooter effect will shoot +1 bullet", 
                                                            "Your Bullet Shooter effect will shoot +2 bullets", 
                                                            "Your Bullet Shooter effect will shoot +3 bullets"}, "BulletsAmount", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,0,1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,0,2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,0,3)))}),
            new Augment("RegenUnlock" ,"Regeneration", new string[1]{"Unlock the ability to regenerate Health"}, "Regen", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("RegenUnlock");
                Flamey.Instance.addTimeBasedEffect(new HealthRegen(10f, 10f));
                Deck.Instance.AddAugmentClass(new List<string>{"RegenPerSecond","RegenPerRound"});            
            })}, baseCard: true),  
            new Augment("RegenPerSecond","Self-Healing Fire", new string[3]{"Each second you will regen +10 Health", 
                                                            "Each second you will regen +15 Health", 
                                                            "Each second you will regen +25 Health"}, "RegenPerSecond", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(10f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(15f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(25f, 0)))}), 
            new Augment("RegenPerSecond","Perseverance", new string[3]{"Each second you will regen +20 Health", 
                                                            "Each second you will regen +35 Health", 
                                                            "Each second you will regen +50 Health"}, "RegenPerSecond", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(20f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(35f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(50f, 0)))}),
            new Augment("RegenPerSecond","Heart of Fire", new string[3]{"Each second you will gain +50 Max HP", 
                                                            "Each second you will gain +75 Max HP", 
                                                            "Each second you will gain +100 Max HP"}, "RegenPerSecond", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(50f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(75f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(100f, 0)))}),  
            new Augment("RegenPerRound","Emergency Bandage", new string[3]{"At the end of each round you gain regen +10 Max HP", 
                                                            "At the end of each round you gain regen +15 Max HP", 
                                                            "At the end of each round you gain regen +25 Max HP"}, "RegenPerRound", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 10))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 15))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 25)))}),  
            new Augment("RegenPerRound","Leftovers", new string[3]{"At the end of each round you gain regen +20 Max HP", 
                                                            "At the end of each round you gain regen +30 Max HP", 
                                                            "At the end of each round you gain regen +50 Max HP"}, "RegenPerRound", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 20))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 30))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 50)))}),
            new Augment("RegenPerRound","Free Healthcare", new string[3]{"At the end of each round you gain regen +40 Max HP", 
                                                            "At the end of each round you gain regen +60 Max HP", 
                                                            "At the end of each round you gain regen +100 Max HP"}, "RegenPerRound", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 40))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 60))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 100)))}),
            new Augment("ThunderUnlock" ,"Thunder", new string[1]{"Unlock the ability to control Thunder"}, "ThunderUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("ThunderUnlock");
                Flamey.Instance.addTimeBasedEffect(new LightningEffect(40, 25, 0));
                Deck.Instance.AddAugmentClass(new List<string>{"ThunderDmg","ThunderInterval"});            
            })}, baseCard: true),  
            new Augment("ThunderInterval","Charge it up!", new string[3]{"Thunder cooldown decreases by 7%", 
                                                            "Thunder cooldown decreases by 15%", 
                                                            "Thunder cooldown decreases by 25%"}, "ThunderInterval", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.07f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.25f)))}), 
            new Augment("ThunderInterval","Eletric Discharge", new string[3]{"Thunder cooldown decreases by 15%", 
                                                            "Thunder cooldown decreases by 25%", 
                                                            "Thunder cooldown decreases by 35%"}, "ThunderInterval", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.35f)))}),
            new Augment("ThunderInterval","Thunderstorm", new string[3]{"Thunder cooldown decreases by 25%", 
                                                            "Thunder cooldown decreases by 35%", 
                                                            "Thunder cooldown decreases by 50%"}, "ThunderInterval", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.35f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.5f)))}),  
            new Augment("ThunderDmg","Volt Tackle", new string[3]{"Thunder will deal +10 damage on impact", 
                                                            "Thunder will deal +20 damage on impact", 
                                                            "Thunder will deal +30 damage on impact"}, "ThunderDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,10,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,20,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,30,0)))}),  
            new Augment("ThunderDmg","Zap Cannon", new string[3]{"Thunder will deal +20 damage on impact", 
                                                            "Thunder will deal +40 damage on impact", 
                                                            "Thunder will deal +60 damage on impact"}, "ThunderDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,20,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,40,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,60,0)))}),
            new Augment("ThunderDmg","Lightning Synergy", new string[3]{"Thunder will deal +50 damage on impact", 
                                                            "Thunder will deal +100 damage on impact", 
                                                            "Thunder will deal +200 damage on impact"}, "ThunderDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,50,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,100,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,200,0)))}),
            new Augment("ImmolateUnlock" ,"Immolate", new string[1]{"Unlock the ability to release Heat Waves that ignore Armor"}, "ImmolateUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("ImmolateUnlock");
                Flamey.Instance.addTimeBasedEffect(new Immolate(100, 25, 0.3f, 0));
                Deck.Instance.AddAugmentClass(new List<string>{"ImmolateInterval","ImmolateDmg","ImmolateRadius"});            
            })}, baseCard: true),  
            new Augment("ImmolateInterval","Heat Discharge", new string[3]{"Immolate cooldown decreases by 5%", 
                                                            "Immolate cooldown decreases by 10%", 
                                                            "Immolate cooldown decreases by 15%"}, "ImmolateInterval", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0, 0, 0.05f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.1f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.15f)))}), 
            new Augment("ImmolateInterval","Accumulated Heat", new string[3]{"Immolate cooldown decreases by 15%", 
                                                            "Immolate cooldown decreases by 25%", 
                                                            "Immolate cooldown decreases by 35%"}, "ImmolateInterval", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.15f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.25f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.35f)))}),
            new Augment("ImmolateInterval","Releasing Everything", new string[3]{"Immolate cooldown decreases by 35%", 
                                                            "Immolate cooldown decreases by 50%", 
                                                            "Immolate cooldown decreases by 75%"}, "ImmolateInterval", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.35f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.5f))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.75f)))}),  
            new Augment("ImmolateDmg","Summer Heat", new string[3]{"Immolate will deal +15 damage", 
                                                            "Immolate will deal +25 damage", 
                                                            "Immolate will deal +35 damage"}, "ImmolateDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,10,0 ,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,15,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,25,0,0)))}),  
            new Augment("ImmolateDmg","Universal Damage", new string[3]{"Immolate will deal +30 damage", 
                                                            "Immolate will deal +50 damage", 
                                                            "Immolate will deal +75 damage"}, "ImmolateDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,20,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,30,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,50,0,0)))}),
            new Augment("ImmolateDmg","Nuclear Shockwave", new string[3]{"Immolate will deal +75 damage", 
                                                            "Immolate will deal +100 damage", 
                                                            "Immolate will deal +200 damage"}, "ImmolateDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,50,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,75,0,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,150,0,0)))}),
            new Augment("ImmolateRadius","Waving Flames", new string[3]{"Immolating waves will travel for +10 radius", 
                                                            "Immolating waves will travel for +15 radius", 
                                                            "Immolating waves will travel for +25 radius"}, "ImmolateRadius", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.1f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.15f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.25f,0)))}),  
            new Augment("ImmolateRadius","Spread the Fire", new string[3]{"Immolating waves will travel for +25 radius", 
                                                            "Immolating waves will travel for +35 radius", 
                                                            "Immolating waves will travel for +50 radius"}, "ImmolateRadius", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.25f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.35f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.5f,0)))}),
            new Augment("ImmolateRadius","Across the Globe", new string[3]{"Immolating waves will travel for +50 radius", 
                                                            "Immolating waves will travel for +75 radius", 
                                                            "Immolating waves will travel for +100 radius"}, "ImmolateRadius", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.5f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.75f,0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,1f,0)))}),
            new Augment("CandleUnlock" ,"Arcanist", new string[1]{"Unlock the ability to start a ritual"}, "CandleUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("CandleUnlock");
                Flamey.Instance.addNotEspecificEffect(new CandleTurrets(5, 0.2f, 2));
                Deck.Instance.AddAugmentClass(new List<string>{"CandleDmg","CandleAmount","CandleAtkSpeed"});            
            })}, baseCard: true),  
            new Augment("CandleAtkSpeed","Alembic Artistry", new string[3]{"Candles will increase their attack speed by +10", 
                                                            "Candles will increase their attack speed by +15", 
                                                            "Candles will increase their attack speed by +20"}, "CandleAtkSpeed", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.1f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.15f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.2f, 0))),}), 
            new Augment("CandleAtkSpeed","Ancient Wizard", new string[3]{"Candles will increase their attack speed by +20", 
                                                            "Candles will increase their attack speed by +30", 
                                                            "Candles will increase their attack speed by +40"}, "CandleAtkSpeed", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.2f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.3f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.4f, 0))),}), 
            new Augment("CandleAtkSpeed","Begin the Ritual", new string[3]{"Candles will increase their attack speed by +40", 
                                                            "Candles will increase their attack speed by +60", 
                                                            "Candles will increase their attack speed by +80"}, "CandleAtkSpeed", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.4f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.6f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.8f, 0))),}), 
            new Augment("CandleDmg","Apprentice", new string[3]{"Candles will deal +5 damage", 
                                                            "Candles will deal +10 damage", 
                                                            "Candles will deal +15 damage"}, "CandleDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(5, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(10, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(15, 0, 0))),}),  
            new Augment("CandleDmg","Aether Infusion", new string[3]{"Candles will deal +10 damage", 
                                                            "Candles will deal +20 damage", 
                                                            "Candles will deal +30 damage"}, "CandleDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(10, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(20, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(30, 0, 0))),}), 
            new Augment("CandleDmg","Witchcraft Master", new string[3]{"Candles will deal +20 damage", 
                                                            "Candles will deal +40 damage", 
                                                            "Candles will deal +60 damage"}, "CandleDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(20, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(40, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(60, 0, 0))),}), 
            new Augment("CandleAmount","Philosopher's Stone", new string[3]{"Gain +1 Candle", 
                                                            "Gain +2 Candles", 
                                                            "Gain +3 Candles"}, "CandleAmount", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0, 1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0, 2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0, 3))),}), 

            new Augment("SummonUnlock" ,"Beekeeper", new string[1]{"Unlock the ability to own a Bee Swarm"}, "SummonUnlock", Tier.Prismatic, new UnityAction[1]{new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("SummonUnlock");
                Flamey.Instance.addNotEspecificEffect(new Summoner(5, 0.25f, 0.5f, 1));
                Deck.Instance.AddAugmentClass(new List<string>{"SummonAtkSpeed","SummonDmg", "SummonSpeed","SummonAmount"});            
            })}, baseCard: true),  
            new Augment("SummonAtkSpeed","Rapid Shooters", new string[3]{"Bees will increase their attack speed by +10", 
                                                            "Bees will increase their attack speed by +20", 
                                                            "Bees will increase their attack speed by +35"}, "SummonAtkSpeed", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.1f, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.2f, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.35f, 0, 0))),}), 
            new Augment("SummonAtkSpeed","Bee-autiful Pets", new string[3]{"Bees will increase their attack speed by +20", 
                                                            "Bees will increase their attack speed by +40", 
                                                            "Bees will increase their attack speed by +75"}, "SummonAtkSpeed", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.2f, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.4f, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.75f, 0, 0))),}), 
            new Augment("SummonAtkSpeed","Bee Swarm", new string[3]{"Bees will increase their attack speed by +50", 
                                                            "Bees will increase their attack speed by +100", 
                                                            "Bees will increase their attack speed by +150"}, "SummonAtkSpeed", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.5f, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 1f, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 1.5f, 0, 0))),}), 
            new Augment("SummonDmg","Baby Bee", new string[3]{"Bees will deal +5 damage", 
                                                            "Bees will deal +10 damage", 
                                                            "Bees will deal +15 damage"}, "SummonDmg", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(5, 0, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(10, 0, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(15, 0, 0, 0))),}),  
            new Augment("SummonDmg","Bee Stronger", new string[3]{"Bees will deal +10 damage", 
                                                            "Bees will deal +20 damage", 
                                                            "Bees will deal +35 damage"}, "SummonDmg", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(10, 0, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(20, 0, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(35, 0, 0, 0))),}), 
            new Augment("SummonDmg","The Queen", new string[3]{"Bees will deal +25 damage", 
                                                            "Bees will deal +50 damage", 
                                                            "Bees will deal +75 damage"}, "SummonDmg", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(25, 0, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(50, 0, 0, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(75, 0, 0, 0))),}), 
            new Augment("SummonSpeed","Speeding Up", new string[3]{"Increase Bee speed by +0.25", 
                                                            "Increase Bee speed by +0.5", 
                                                            "Increase Bee speed by +0.75"}, "SummonSpeed", Tier.Silver, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0.25f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0.5f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0.75f, 0))),}),  
            new Augment("SummonSpeed","Agility", new string[3]{"Increase Bee speed by +0.5", 
                                                            "Increase Bee speed by +1", 
                                                            "Increase Bee speed by +1.5"}, "SummonSpeed", Tier.Gold, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, .5f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 1f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 1.5f, 0))),}), 
            new Augment("SummonSpeed","Bee Acrobacy League", new string[3]{"Increase Bee speed by +1", 
                                                            "Increase Bee speed by +2", 
                                                            "Increase Bee speed by +3"}, "SummonSpeed", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 1f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 2f, 0))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 3f, 0))),}), 
            
            new Augment("SummonAmount","Bee Hive", new string[3]{"Your Bee Swarm gains an extra Bee", 
                                                            "Your Bee Swarm gains an extra 2 Bees", 
                                                            "Your Bee Swarm gains an extra 4 Bees"}, "SummonAmount", Tier.Prismatic, new UnityAction[3]{
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0, 1))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0, 2))),
                                                                                                                        new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0, 4))),}), 
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

    public Augment getAugmentByName(string title){
        foreach (Augment item in AllAugments)
        {
            if(item.Title == title){
                return item;
            }
        }
        return null;

    }

    





}
