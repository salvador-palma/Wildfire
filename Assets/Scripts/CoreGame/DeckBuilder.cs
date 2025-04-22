using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DeckBuilder : MonoBehaviour
{
    public static DeckBuilder Instance;
    public List<Augment> AllAugments;

    int[] firstLayerPrices;
    int[] secondLayerPrices;
    int[] thirdLayerPrices;
    int[] fourthLayerPrices;

    void Awake(){
        if(Instance==null){Instance = this;}
        if(Instance==this){DefineAugmentClasses();}

        firstLayerPrices = new int[]{1000, 5000, 20000};
        secondLayerPrices = new int[]{2500, 10000, 50000};
        thirdLayerPrices = new int[]{5000, 20000, 100000};
        fourthLayerPrices = new int[]{10000, 40000, 200000};
    }
    
    public int getPrice(string skill, int level){
        if(level < 0 ){return -2;}
        if(level > 2){return -1;}

        List<string> firstLayer = new List<string>(){"Assassin","Orbits", "Critical Strike", "Regeneration", "Multicaster"};
        List<string> secondLayer = new List<string>(){"Vampire","Pirate", "Thorns", "Resonance", "Burst Shot", "Freeze", "Magical Shot"};
        List<string> thirdLayer = new List<string>(){"Flower Field","Explosion", "Immolate", "Lava Pool", "Snow Pool", "Static Energy", "Thunder"};
        List<string> fourthLayer = new List<string>(){"Necromancer","Ritual", "Bee Summoner", "Ember Generation", "Gambling"};


        if(firstLayer.Contains(skill)){
            return firstLayerPrices[level];
        }else if(secondLayer.Contains(skill)){
            return secondLayerPrices[level];
        }else if(thirdLayer.Contains(skill)){
            return thirdLayerPrices[level];
        }else if(fourthLayer.Contains(skill)){
            return fourthLayerPrices[level];
        }else{
            Debug.LogWarning("Couldn't find price for " + skill + " at level " + level);
            return -1;
        }
    }
    public void DefineAugmentClasses(){
        AllAugments = new List<Augment>
        {
            new Augment("Dmg","Hard Work", "Gain +10 Base Damage", "Dmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addDmg(10)),baseStat: true, immoType:IMMOLATE.FIRE),
            new Augment("Dmg","Heavy Hitter","Gain +25 Base Damage", "Dmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addDmg(25)), baseStat: true, immoType:IMMOLATE.FIRE),
            new Augment("Dmg","Hephaestus","Gain +50 Base Damage", "Dmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addDmg(50)), baseStat: true, immoType:IMMOLATE.FIRE),
            
            new Augment("Acc","Target Practice", "Increase your accuracy by +15%", "Acc", Tier.Silver, new UnityAction(() => Flamey.Instance.addAccuracy(15)),baseStat: true),
            new Augment("Acc","Steady Aim", "Increase your accuracy by +30%", "Acc", Tier.Gold, new UnityAction(() => Flamey.Instance.addAccuracy(30)),baseStat: true),
            new Augment("Acc","Eagle Eye", "Double your current accuracy", "Acc", Tier.Prismatic, new UnityAction(() => Flamey.Instance.multAccuracy(2f)),baseStat: true),
            
            new Augment("AtkSpeed","Swifty Flames", "Increase your attack speed by +30", "AtkSpeed", Tier.Silver, new UnityAction(() => Flamey.Instance.addAttackSpeed(.3f)), baseStat: true),
            new Augment("AtkSpeed","Fire Dance", "Increase your attack speed by +60", "AtkSpeed", Tier.Gold, new UnityAction(() => Flamey.Instance.addAttackSpeed(.6f)), baseStat: true),
            new Augment("AtkSpeed","Flamethrower", "Mutliply your attack speed by x1.5", "AtkSpeed", Tier.Prismatic, new UnityAction(() => Flamey.Instance.multAttackSpeed(1.5f)), baseStat: true),

            new Augment("BltSpeed","Quick Shot", "Gain +0.8 Bullet Speed", "BltSpeed", Tier.Silver, new UnityAction(() => Flamey.Instance.addBulletSpeed(.8f)), baseStat: true),
            new Augment("BltSpeed","Fire-Express", "Gain +2 Bullet Speed", "BltSpeed", Tier.Gold, new UnityAction(() => Flamey.Instance.addBulletSpeed(2f)), baseStat: true),
            new Augment("BltSpeed","HiperDrive", "Gain +4 Bullet Speed", "BltSpeed", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addBulletSpeed(4f)), baseStat: true),
            
            new Augment("Health","Warm Soup", "Heal 50% of your Max Health and gain +100 HP", "Health", Tier.Silver, new UnityAction(() => Flamey.Instance.addHealth(100,.5f)),baseStat: true, immoType:IMMOLATE.EARTH),
            new Augment("Health","Sunfire Cape", "Heal 75% of your Max Health and gain +250 HP", "Health", Tier.Gold, new UnityAction(() => Flamey.Instance.addHealth(250,.75f)),baseStat: true, immoType:IMMOLATE.EARTH),
            new Augment("Health","Absolute Unit", "Heal 100% of your Max Health and gain +1000 HP", "Health", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addHealth(1000,1f)),baseStat: true, immoType:IMMOLATE.EARTH),

            new Augment("Armor","Mesh Armor", "Increase your armor by +10", "Armor", Tier.Silver, new UnityAction(() => Flamey.Instance.addArmor(10)), baseStat: true, immoType:IMMOLATE.EARTH),
            new Augment("Armor","Long Lasting Fire", "Increase your armor by +25", "Armor", Tier.Gold, new UnityAction(() => Flamey.Instance.addArmor(25)), baseStat: true, immoType:IMMOLATE.EARTH),
            new Augment("Armor","The Armor of God", "Increase your armor by +50", "Armor", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addArmor(50)), baseStat: true, immoType:IMMOLATE.EARTH),    

    

            new Augment("Multicaster" ,"Multicaster", "Unlock the ability to multicast", "MulticasterUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Multicaster");
                Flamey.Instance.addOnShootEffect(new SecondShot(0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"MulticasterProb"});            
            }), baseCard: true),   

            new Augment("MulticasterProb","The more the better", "When you fire a shot, gain a 5% chance to fire an extra shot", "MulticasterProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.05f)))), 
            new Augment("MulticasterProb","Double Trouble", "When you fire a shot, gain a 15% chance to fire an extra shot", "MulticasterProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.15f)))),
            new Augment("MulticasterProb","Casting Cascade", "When you fire a shot, gain a 35% chance to fire an extra shot", "MulticasterProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnShootEffect(new SecondShot(0.35f)))),



            new Augment("Critical Strike" ,"Critical Strike", "Unlock the ability to critical strike", "CritUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Critical Strike");
                Flamey.Instance.addOnShootEffect(new CritUnlock(0.1f, 1.5f));
                Deck.Instance.AddAugmentClass(new List<string>{"CritMult","CritChance"});            
            }), baseCard: true, immoType:IMMOLATE.FIRE),  

            new Augment("CritMult","Lucky Shots", "Gain +15% critical strike damage", "CritMult", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.15f))), immoType:IMMOLATE.FIRE),
            new Augment("CritMult","Critical Thinking", "Gain +30% critical strike damage", "CritMult", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0f, 0.3f))), immoType:IMMOLATE.FIRE), 
            
            new Augment("CritChance","Critical Miracle", "Gain +5% critical strike chance", "CritChance", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.05f, 0f))), immoType:IMMOLATE.FIRE),
            new Augment("CritChance","Fate's Favor", "Gain +10% critical strike chance", "CritChance", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.1f, 0f))), immoType:IMMOLATE.FIRE),  
            new Augment("CritChance","Overheat", "Gain +25% critical strike chance and +60% critical strike damage", "CritChance", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnShootEffect(new CritUnlock(0.25f, 0.6f))), immoType:IMMOLATE.FIRE),    
            
            

            new Augment("Vampire" ,"Vampire", "Unlock the ability to life-steal", "VampUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Vampire");
                Flamey.Instance.addOnHitEffect(new VampOnHit(0.05f,0.05f));
                Deck.Instance.AddAugmentClass(new List<string>{"VampProb","VampPerc"});            
            }), baseCard: true, immoType:IMMOLATE.WATER),  

            new Augment("VampProb","Steal to Heal", "Gain +5% chance to proc your Vampire effect", "VampProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.05f))), immoType:IMMOLATE.WATER),
            new Augment("VampProb","Eternal Hunger", "Gain +10% chance to proc your Vampire effect", "VampProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.1f))), immoType:IMMOLATE.WATER), 
            new Augment("VampProb","Soul Harvester", "Gain +25% chance to proc your Vampire effect", "VampProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0f,0.25f))), immoType:IMMOLATE.WATER),       
            new Augment("VampPerc","Sustenance", "Gain +5% Heal on your Vampire effect", "VampPerc", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.05f,0f))), immoType:IMMOLATE.WATER),
            new Augment("VampPerc","Vampire Survivor", "Gain +10% Heal on your Vampire effect", "VampPerc", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.1f,0f))), immoType:IMMOLATE.WATER),  
            new Augment("VampPerc","Blood Pact", "Gain +20% Heal on your Vampire effect", "VampPerc", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new VampOnHit(0.2f,0f))), immoType:IMMOLATE.WATER),  
   
            
            new Augment("Burst Shot" ,"Burst Shot", "Unlock the ability to send burst shots", "BurstUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Burst Shot");
                Flamey.Instance.addOnShootEffect(new BurstShot(50, 5));
                Deck.Instance.AddAugmentClass(new List<string>{"BurstInterval","BurstAmount"});            
            }), baseCard: true),  

            new Augment("BurstInterval","Happy Trigger", "You will need 5 shots less to proc Burst Shot", "BurstInterval", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(5,0)))), 
            new Augment("BurstInterval","Bullet Symphony", "You will need 10 shots less to proc Burst Shot", "BurstInterval", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(10,0)))), 
            new Augment("BurstInterval","Make It Rain", "You will need 20 shots less to proc Burst Shot", "BurstInterval", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(20,0)))), 
            new Augment("BurstAmount","Burst Barricade", "Your Burst Shot will +1 flames", "BurstAmount", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,1)))), 
            new Augment("BurstAmount","Burst Unleashed", "Your Burst Shot will +2 flames", "BurstAmount", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,2)))), 
            new Augment("BurstAmount","Burst to Victory", "Your Burst Shot will +5 flames", "BurstAmount", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnShootEffect(new BurstShot(0,5)))), 
            


            new Augment("Freeze" ,"Freeze!", "Unlock the ability to Slow enemies using ice(?)", "IceUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Freeze");
                Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"IceDuration","IceProb"});            
            }), baseCard: true, immoType:IMMOLATE.AIR),  
            new Augment("IceProb","IcyHot","Gain +5% chance to proc your Frost Fire effect", "IceProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.05f))), immoType:IMMOLATE.AIR),
            new Augment("IceProb","Glacial Energy","Gain +15% chance to proc your Frost Fire effect", "IceProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.15f))), immoType:IMMOLATE.AIR), 
            new Augment("IceProb","A Dance of Fire and Ice","Gain +35% chance to proc your Frost Fire effect", "IceProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(0, 0.35f))), immoType:IMMOLATE.AIR),  
            new Augment("IceDuration","Slowly but Surely", "Your Frost Fire effect lasts for 0.5 seconds more", "IceDuration", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(500, 0))), immoType:IMMOLATE.AIR),
            new Augment("IceDuration","Frost Bite", "Your Frost Fire effect lasts for 1 seconds more", "IceDuration", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(1000, 0))), immoType:IMMOLATE.AIR),
            new Augment("IceDuration","Absolute Zero", "Your Frost Fire effect lasts for 2.5 seconds more", "IceDuration", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new IceOnHit(2500, 0))), immoType:IMMOLATE.AIR),


            
            new Augment("Resonance" ,"Resonance", "Unlock the ability to shred enemy armor through Sound", "ShredUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Resonance");
                Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"ShredProb","ShredPerc"});            
            }), baseCard: true),   
            new Augment("ShredProb","Reverb", "Gain +5% chance to proc Resonance", "ShredProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.05f, 0f)))),
            new Augment("ShredProb","Turn it Up!", "Gain +15% chance to proc Resonance", "ShredProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.15f, 0f)))),
            new Augment("ShredProb","Echoing Fracture", "Gain +35% chance to proc Resonance", "ShredProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0.35f, 0f)))),
            new Augment("ShredPerc","Bass Boost", "Resonance reduces +5% more enemy armor per proc", "ShredPerc", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.05f)))),  
            new Augment("ShredPerc","Harmonic Shredding", "Resonance effect reduces +10% more enemy armor per proc", "ShredPerc", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.1f)))),  
            new Augment("ShredPerc","Sonic Rupture", "Resonance effect reduces +25% more enemy armor per proc", "ShredPerc", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ShredOnHit(0, 0.25f)))),  


            new Augment("Assassin" ,"Assassin's Path", "Unlock the ability to pierce armor and execute enemies", "Assassins", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Assassin");
                
                Flamey.Instance.addArmorPen(0.05f);
                if(SkillTreeManager.Instance.getLevel("Assassin")>=1){
                    Deck.Instance.AddAugmentClass(new List<string>{"ArmorPen","Execute"});   
                    Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.025f));         
                }else{
                    Deck.Instance.AddAugmentClass(new List<string>{"ArmorPen"});   
                    Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0));           
                }
                
            }), baseCard: true, immoType:IMMOLATE.FIRE),  
            new Augment("Execute","Execution Enforcer", "You can execute enemies for +2.5% of their Max Health", "Execute", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.025f))), immoType:IMMOLATE.FIRE),
            new Augment("Execute","Soul Collector", "You can execute enemies for +5% of their Max Health", "Execute", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.05f))), immoType:IMMOLATE.FIRE),
            new Augment("Execute","La Guillotine", "You can execute enemies for +10% of their Max Health", "Execute", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f))), immoType:IMMOLATE.FIRE),
            new Augment("ArmorPen","Shell Breaker", "Gain +5% Armor Penetration", "ArmorPen", Tier.Silver, new UnityAction(() => Flamey.Instance.addArmorPen(0.05f)), immoType:IMMOLATE.FIRE),
            new Augment("ArmorPen","Quantum Piercing", "Gain +10% Armor Penetration", "ArmorPen", Tier.Gold, new UnityAction(() => Flamey.Instance.addArmorPen(0.10f)), immoType:IMMOLATE.FIRE),
            new Augment("ArmorPen","Lance of Aether", "Gain +25% Armor Penetration", "ArmorPen", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addArmorPen(0.25f)), immoType:IMMOLATE.FIRE),

            
            new Augment("Magical Shot" ,"Magical Shot", "Unlock the ability to shoot magical flames that inflict extra damage", "BlueFlameUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Magical Shot");
                
                if(SkillTreeManager.Instance.getLevel("Magical Shot") >= 1){
                    Flamey.Instance.addOnShootEffect(new KrakenSlayer(10, 100));
                }else{
                    Flamey.Instance.addOnShootEffect(new KrakenSlayer(20, 100));
                }
                
                
                
                
                Deck.Instance.AddAugmentClass(new List<string>{"BlueFlameInterval","BlueFlameDmg"});   

            }), baseCard: true, immoType:IMMOLATE.FIRE),  
            new Augment("BlueFlameInterval","The Bluer The Better", "You will need 1 shots less to proc Blue Flame", "BlueFlameInterval", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(1, 0)))),
            new Augment("BlueFlameInterval","Propane Combustion", "You will need 3 shots less to proc Blue Flame", "BlueFlameInterval", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(3, 0)))),
            new Augment("BlueFlameInterval","Never ending Blue", "You will need 7 shots less to proc Blue Flame", "BlueFlameInterval", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(7, 0)))),
            new Augment("BlueFlameDmg","Propane Leakage", "Your Blue Flame deals +25 extra damage", "BlueFlameDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 25))), immoType:IMMOLATE.FIRE),
            new Augment("BlueFlameDmg","Powerfull Blue", "Your Blue Flame deals +50 extra damage", "BlueFlameDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50))), immoType:IMMOLATE.FIRE),
            new Augment("BlueFlameDmg","Blue Inferno", "Your Blue Flame deals +100 extra damage", "BlueFlameDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100))), immoType:IMMOLATE.FIRE),

            

            new Augment("Orbits" ,"Orbital Flames", "A tiny Flame will orbit around you damaging the foes it collides with", "OrbitalUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Orbits");
                Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 50));
                Deck.Instance.AddAugmentClass(new List<string>{"OrbitalDmg","OrbitalAmount"});            
            }), baseCard: true, immoType:IMMOLATE.EARTH), 

            new Augment("OrbitalAmount","Tame the Flames", "Gain +1 Flame in your Orbital Field", "OrbitalAmount", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 0))), immoType:IMMOLATE.EARTH),
            new Augment("OrbitalDmg","Tiny Flames Win", "Your Orbital Flames deal +10 damage", "OrbitalDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 10))), immoType:IMMOLATE.FIRE),
            new Augment("OrbitalDmg","Reliable Damage", "Your Orbital Flames deal +25 damage", "OrbitalDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 25))), immoType:IMMOLATE.FIRE),
            new Augment("OrbitalDmg","Saturn", "Your Orbital Flames deal +50 damage", "OrbitalDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 50))), immoType:IMMOLATE.FIRE),


            new Augment("Lava Pool" ,"Lava Pool", "Unlock the ability to create Lava Pools ", "LavaPoolUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Lava Pool");
                Flamey.Instance.addOnLandEffect(new BurnOnLand(1f, 25, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"LavaPoolDmg","LavaPoolSize","LavaPoolProb","LavaPoolDuration"});            
            }), baseCard: true, immoType:IMMOLATE.FIRE), 
            new Augment("LavaPoolDmg","Hot Tub", "Your Lava Pool will inflict +10 damage per second", "LavaPoolDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,10,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolDmg","Magical Scorch", "Your Lava Pool will inflict +25 damage per second", "LavaPoolDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,25,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolDmg","Conflagration", "Your Lava Pool will inflict +50 damage per second", "LavaPoolDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,50,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolProb","Hot Steps", "Gain +5% probability of spawning a Lava Pool when your shot lands", "LavaPoolProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.05f,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolProb","Lava here, Lava there", "Gain +10% probability of spawning a Lava Pool when your shot lands", "LavaPoolProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.10f,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolProb","The Apocalypse", "Gain +25% probability of spawning a Lava Pool when your shot lands", "LavaPoolProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.25f,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolSize","Heat Area", "Your Lava Pool grows by +2.5 units", "LavaPoolSize", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.25f,0,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolSize","Lava Lakes", "Your Lava Pool grows by +5 units", "LavaPoolSize", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0.5f,0,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolSize","Inside the volcano", "Your Lava Pool grows by +10 units", "LavaPoolSize", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(1f,0,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolDuration","Sear the ground", "Your Lava Pool lasts for +0.75 seconds", "LavaPoolDuration", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.75f))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolDuration","Eternally Hot", "Your Lava Pool lasts for +1.5 seconds", "LavaPoolDuration", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.5f))), immoType:IMMOLATE.FIRE),
            new Augment("LavaPoolDuration","Unsettling Magma", "Your Lava Pool lasts for +3 seconds", "LavaPoolDuration", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,3f))), immoType:IMMOLATE.FIRE),
            


            new Augment("Static Energy" ,"Static Energy", "Unlock the ability to release static energy", "StatikUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Static Energy");
                Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,50,3));
                Deck.Instance.AddAugmentClass(new List<string>{"StatikProb","StatikDmg","StatikTTL"});            
            }), baseCard: true), 
            new Augment("StatikProb","Watts Up", "Gain +5% probability to proc your Static Energy effect", "StatikProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.05f,0,0)))),
            new Augment("StatikProb","Electrifying Possibilities", "Gain +10% probability to proc your Static Energy effect", "StatikProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,0,0)))),
            new Augment("StatikProb","The Sparkster", "Gain +25% probability to proc your Static Energy effect", "StatikProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0.25f,0,0)))),
            new Augment("StatikDmg","Shock Dart", "Your Static Energy deals +10 extra damage", "StatikDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,10,0))), immoType:IMMOLATE.FIRE),
            new Augment("StatikDmg","Shocking Advancement", "Your Static Energy deals +20 extra damage", "StatikDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,20,0))), immoType:IMMOLATE.FIRE),
            new Augment("StatikDmg","Thor", "Your Static Energy deals +50 extra damage", "StatikDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,50,0))), immoType:IMMOLATE.FIRE),
            new Augment("StatikTTL","Conductive materials", "Your Static Energy will be able to cross through 1 more enemy", "StatikTTL", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1)))),
            new Augment("StatikTTL","Feel the Flow", "Your Static Energy will be able to cross through 2 more enemies", "StatikTTL", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2)))),
            new Augment("StatikTTL","Amping Up!", "Your Static Energy will be able to cross through 5 more enemies", "StatikTTL", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,5)))),
            

            new Augment("Snow Pool" ,"Snow Pool", "Unlock the ability to create Snow Pools that slow down enemies", "IcePoolUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Snow Pool");
                Flamey.Instance.addOnLandEffect(new IceOnLand(1f, 0.1f, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"IcePoolDuration","IcePoolProb","IcePoolSlow","IcePoolSize"});            
            }), baseCard: true, immoType:IMMOLATE.AIR), 
            new Augment("IcePoolSlow","Cold Bath", "Your Snow Pool will slow down enemies for 5% more", "IcePoolSlow", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.05f,0,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolSlow","Glacial Grip", "Your Snow Pool will slow down enemies for 10% more", "IcePoolSlow", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.1f,0,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolSlow","Frozen Stasis", "Your Snow Pool will slow down enemies for 20% more", "IcePoolSlow", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0.2f,0,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolProb","Cold Steps", "Gain +5% probability of spawning an Snow Pool when your shot lands", "IcePoolProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.05f,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolProb","Ice here, Ice there", "Gain +10% probability of spawning an Snow Pool when your shot lands", "IcePoolProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.1f,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolProb","The North Pole", "Gain +25% probability of spawning an Snow Pool when your shot lands", "IcePoolProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0.25f,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolSize","Cold breeze", "Your Snow Pool grows by +2.5 units", "IcePoolSize", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.25f,0,0,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolSize","Frozen Lakes", "Your Snow Pool grows by +5 units", "IcePoolSize", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0.5f,0,0,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolSize","Inside the Iceberg", "Your Snow Pool grows by +10 units", "IcePoolSize", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(1f,0,0,0))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolDuration","Cooling Down", "Your Snow Pool lasts for +0.75 seconds", "IcePoolDuration", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,0.75f))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolDuration","Eternally Cold", "Your Snow Pool lasts for +1.5 seconds", "IcePoolDuration", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,1.5f))), immoType:IMMOLATE.AIR),
            new Augment("IcePoolDuration","Absolute Frost", "Your Snow Pool lasts for +3 seconds", "IcePoolDuration", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new IceOnLand(0,0,0,3f))), immoType:IMMOLATE.AIR),
            


            new Augment("Thorns" ,"Thorns", "Unlock the ability to deal damage back when hitted", "ThornsUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Thorns");
                Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0.1f, 0.1f));
                Deck.Instance.AddAugmentClass(new List<string>{"ThornsPerc","ThornsProb"});            
            }), baseCard: true, immoType:IMMOLATE.EARTH),  
            new Augment("ThornsPerc","Innocent Spikes", "Your Thorn effect will reflect +10% of your Armor", "ThornsPerc", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .1f))), immoType:IMMOLATE.EARTH),
            new Augment("ThornsPerc","Spiky Vengeance", "Your Thorn effect will reflect +20% of your Armor", "ThornsPerc", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .2f))), immoType:IMMOLATE.EARTH),
            new Augment("ThornsPerc","PufferFish", "Your Thorn effect will reflect +50% of your Armor", "ThornsPerc", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(0, .5f))), immoType:IMMOLATE.EARTH),
            new Augment("ThornsProb","Prickly Fire", "Gain +5% chance to proc your Thorns effect", "ThornsProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.05f, 0))), immoType:IMMOLATE.EARTH),
            new Augment("ThornsProb","Thorns everywhere!", "Gain +10% chance to proc your Thorns effect", "ThornsProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.1f, 0))), immoType:IMMOLATE.EARTH),
            new Augment("ThornsProb","Cactus on Fire", "Gain +25% chance to proc your Thorns effect", "ThornsProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnHittedEffect(new ThornsOnHitted(.25f, 0))), immoType:IMMOLATE.EARTH),
            

            

            new Augment("Ember Generation","Little by little", "Your ember multiplier will improve by +10%", "MoneyMult", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .1f))), baseCardUpgrade:true),
            new Augment("Ember Generation","Stock Trading", "Your ember multiplier will improve by +25%", "MoneyMult", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .25f))), baseCardUpgrade:true),
            new Augment("Ember Generation","Capitalist Adventure", "Your ember multiplier will improve by +50%", "MoneyMult", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, .5f))), baseCardUpgrade:true),
            new Augment("Ember Generation","Savings Account", "Gain +25 embers per round", "MoneyProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(25, 0))), baseCardUpgrade:true),
            new Augment("Ember Generation","Tax Payment", "Gain +50 embers per round", "MoneyProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(50, 0))), baseCardUpgrade:true),
            new Augment("Ember Generation","Robbery", "Gain +100 embers per round", "MoneyProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(100, 0))), baseCardUpgrade:true),

            
            new Augment("Gambling","Not enough refreshes", "Gain 2 random silver augments", "GambleImprove", Tier.Silver, new UnityAction(() => Deck.Instance.Gamble(2, Tier.Silver, "Not enough refreshes")), baseCardUpgrade:true),
            new Augment("Gambling","Feelin' Blessed", "Gain 4 random silver augments", "GambleImprove", Tier.Gold, new UnityAction(() => Deck.Instance.Gamble(4, Tier.Silver, "Feelin' Blessed")), baseCardUpgrade:true),
            new Augment("Gambling","Roll the Dice", "Gain 4 random gold augments", "GambleImprove", Tier.Prismatic, new UnityAction(() => Deck.Instance.Gamble(4, Tier.Gold, "Roll the Dice")), baseCardUpgrade:true),


            
            new Augment("Flower Field" ,"Flower Field", "Unlock the ability to scatter Flowers", "DrainPoolUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Flower Field");
                Flamey.Instance.addOnLandEffect(new DrainOnLand(1f, 0.01f, 0.05f, 1f));
                Deck.Instance.AddAugmentClass(new List<string>{"DrainPoolPerc","DrainPoolProb","DrainPoolSize","DrainPoolDuration"});            
            }), baseCard: true, immoType:IMMOLATE.WATER),  

            new Augment("DrainPoolPerc","Harvesting", "Your Flowers will heal you for +0.25% Enemy Max HP per tick", "DrainPoolPerc", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.0025f,0,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolPerc","Carnivorous Plant", "Your Flowers will heal you for +0.5% Enemy Max HP per tick", "DrainPoolPerc", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.005f,0,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolPerc","Photosynthesis", "Your Flowers will heal you for +1% Enemy Max HP per tick", "DrainPoolPerc", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,.01f,0,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolProb","Sowing the Field", "Gain +5% probability of spawning a Flower when your shot lands", "DrainPoolProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.05f,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolProb","Garden's Embrace", "Gain +10% probability of spawning a Flower when your shot lands", "DrainPoolProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.1f,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolProb","Lifebloom Aura", "Gain +20% probability of spawning a Flower when your shot lands", "DrainPoolProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0.2f,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolSize","Sprouting", "Your Flowers grow by +2.5 units", "DrainPoolSize", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.25f,0,0,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolSize","Flowering Surge", "Your Flowers grow by +5 units", "DrainPoolSize", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0.5f,0,0,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolSize","Botanical Boom", "Your Flowers grow by +10 units", "DrainPoolSize", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(1f,0,0,0))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolDuration","Lasting Petals", "Your Flowers last for +0.75 seconds", "DrainPoolDuration", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,0.75f))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolDuration","Everlasting Blossom", "Your Flowers last for +1.5 seconds", "DrainPoolDuration", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,1.5f))), immoType:IMMOLATE.WATER),
            new Augment("DrainPoolDuration","Garden of Eden", "Your Flowers last for +3 seconds", "DrainPoolDuration", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnLandEffect(new DrainOnLand(0,0,0,3f))), immoType:IMMOLATE.WATER),
            
            
            
            new Augment("Explosion" ,"Explosion", "Unlock the ability to generate explosions whenever enemies die", "ExplodeUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Explosion");
                Flamey.Instance.addOnKillEffect(new Explosion(0.1f,50));
                Deck.Instance.AddAugmentClass(new List<string>{"ExplodeProb","ExplodeDmg"});            
            }), baseCard: true, immoType:IMMOLATE.FIRE),  
            new Augment("ExplodeProb","Bomb Rush", "Gain +5% chance to generate an explosion whenever you kill an enemy", "ExplodeProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.05f,0))), immoType:IMMOLATE.FIRE), 
            new Augment("ExplodeProb","Grenade Launcher", "Gain +10% chance to generate an explosion whenever you kill an enemy", "ExplodeProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.1f,0))), immoType:IMMOLATE.FIRE), 
            new Augment("ExplodeProb","Bombardment", "Gain +25% chance to generate an explosion whenever you kill an enemy", "ExplodeProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(.25f,0))), immoType:IMMOLATE.FIRE), 
            new Augment("ExplodeDmg","Cherry Bomb", "Generated explosions will deal +25 damage", "ExplodeDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,25))), immoType:IMMOLATE.FIRE),  
            new Augment("ExplodeDmg","Dynamite Blast", "Generated explosions will deal +50 damage", "ExplodeDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,50))), immoType:IMMOLATE.FIRE),  
            new Augment("ExplodeDmg","Nuke Blast", "Generated explosions will deal +100 damage", "ExplodeDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Explosion(0,100))), immoType:IMMOLATE.FIRE),  
            
            
            
            new Augment("Necromancer" ,"Necromancer", "Unlock the ability to summon ghouls whenever enemies die", "NecroUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Necromancer");
                Flamey.Instance.addOnKillEffect(new Necromancer(0.1f, 0.25f));
                Deck.Instance.AddAugmentClass(new List<string>{"NecroProb","NecroStats"});            
            }), baseCard: true),  
            new Augment("NecroProb","Wraith Walkers", "Gain +5% chance to summon a ghoul whenever you kill an enemy", "NecroProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.05f,0)))), 
            new Augment("NecroProb","Soul Shepard", "Gain +10% chance to summon a ghoul whenever you kill an enemy", "NecroProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.1f,0)))), 
            new Augment("NecroProb","Crypt of the Necromancer", "Gain +25% chance to summon a ghoul whenever you kill an enemy", "NecroProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(.25f,0)))), 
            new Augment("NecroStats","Phantom Infusion", "Ghouls' damage will increase by 5% of your damage", "NecroStats", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.05f))), immoType:IMMOLATE.FIRE),  
            new Augment("NecroStats","Death's Embrace", "Ghouls' damage will increase by 15% of your damage", "NecroStats", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.15f))), immoType:IMMOLATE.FIRE),  
            new Augment("NecroStats","Hero's Spirit", "Ghouls' damage will increase by 35% of your damage", "NecroStats", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Necromancer(0,.35f))), immoType:IMMOLATE.FIRE),  
            

            new Augment("Pirate" ,"Pirate", "Unlock the ability to shoot bullets around dead enemies and loot their bodies", "BulletsUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Pirate");
                Flamey.Instance.addOnKillEffect(new Bullets(0.1f, 50, 2));
                Deck.Instance.AddAugmentClass(new List<string>{"BulletsProb","BulletsDmg","BulletsAmount"});            
            }), baseCard: true),  
            new Augment("BulletsProb","Pirate Wannabe", "Gain +5% chance to release roundshots whenever you kill an enemy", "BulletsProb", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.05f,0,0)))), 
            new Augment("BulletsProb","Yes, Captain!", "Gain +10% chance to release roundshots whenever you kill an enemy", "BulletsProb", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.1f,0,0)))), 
            new Augment("BulletsProb","Shoot it, Loot it", "Gain +25% chance to release roundshots whenever you kill an enemy", "BulletsProb", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(.25f,0,0)))), 
            new Augment("BulletsDmg","Round Shot", "Roundshots will deal +15 damage", "BulletsDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,15,0))), immoType:IMMOLATE.FIRE), 
            new Augment("BulletsDmg","Arggh!", "Roundshots will deal +30 damage", "BulletsDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,30,0))), immoType:IMMOLATE.FIRE),  
            new Augment("BulletsDmg","Fire of Thieves", "Roundshots will deal +75 damage", "BulletsDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,75,0))), immoType:IMMOLATE.FIRE),   
            new Augment("BulletsAmount","Cannonball Pile", "Your Pirate effect will shoot 1 more roundshot", "BulletsAmount", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addOnKillEffect(new Bullets(0,0,1)))),





            new Augment("Regeneration" ,"Regeneration", "Unlock the ability to regenerate Health", "Regen", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Regeneration");
                Flamey.Instance.addTimeBasedEffect(new HealthRegen(5f, 10f));
                Deck.Instance.AddAugmentClass(new List<string>{"RegenPerSecond","RegenPerRound"});            
            }), baseCard: true, immoType:IMMOLATE.WATER),  
            new Augment("RegenPerSecond","Self-Healing Fire", "Each second you will regen +5 Health", "RegenPerSecond", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(5f, 0))), immoType:IMMOLATE.WATER), 
            new Augment("RegenPerSecond","Perseverance", "Each second you will regen +10 Health", "RegenPerSecond", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(10f, 0))), immoType:IMMOLATE.WATER), 
            new Augment("RegenPerSecond","Heart of Fire", "Each second you will regen +25 Health", "RegenPerSecond", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(25f, 0))), immoType:IMMOLATE.WATER), 
            new Augment("RegenPerRound","Emergency Bandage", "At the end of each round you gain +10 Max HP", "RegenPerRound", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 10))), immoType:IMMOLATE.WATER),  
            new Augment("RegenPerRound","Leftovers", "At the end of each round you gain +25 Max HP", "RegenPerRound", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 25))), immoType:IMMOLATE.WATER), 
            new Augment("RegenPerRound","Free Healthcare", "At the end of each round you gain +50 Max HP", "RegenPerRound", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new HealthRegen(0, 50))), immoType:IMMOLATE.WATER), 
            


            new Augment("Thunder" ,"Thunder", "Unlock the ability to control Thunder", "ThunderUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Thunder");
                Flamey.Instance.addTimeBasedEffect(new LightningEffect(40, 100, 0));
                Deck.Instance.AddAugmentClass(new List<string>{"ThunderDmg","ThunderInterval"});            
            }), baseCard: true),  
            new Augment("ThunderInterval","Charge it up!", "Thunder cooldown decreases by 10%", "ThunderInterval", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.1f)))), 
            new Augment("ThunderInterval","Eletric Discharge", "Thunder cooldown decreases by 25%", "ThunderInterval", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.25f)))), 
            new Augment("ThunderInterval","Thunderstorm", "Thunder cooldown decreases by 50%", "ThunderInterval", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0, 0, 0.5f)))), 
            new Augment("ThunderDmg","Volt Tackle", "Thunder will deal +25 damage on impact", "ThunderDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,25,0))), immoType:IMMOLATE.FIRE),  
            new Augment("ThunderDmg","Zap Cannon", "Thunder will deal +50 damage on impact", "ThunderDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,50,0))), immoType:IMMOLATE.FIRE), 
            new Augment("ThunderDmg","Lightning Synergy", "Thunder will deal +150 damage on impact", "ThunderDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new LightningEffect(0,150,0))), immoType:IMMOLATE.FIRE), 
            


            new Augment("Immolate" ,"Immolate", "Unlock the ability to release Heat Waves", "ImmolateUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Immolate");
                if(SkillTreeManager.Instance.getLevel("Immolate") >= 1){
                    Flamey.Instance.addTimeBasedEffect(new Immolate(50, 50, 0.3f, 0));
                }else{
                    Flamey.Instance.addTimeBasedEffect(new Immolate(100, 50, 0.3f, 0));
                }

                
                Deck.Instance.AddAugmentClass(new List<string>{"ImmolateInterval","ImmolateDmg","ImmolateRadius"});       
                     
            }), baseCard: true),  
            new Augment("ImmolateInterval","Heat Discharge", "Immolate cooldown decreases by 10%", "ImmolateInterval", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.1f)))), 
            new Augment("ImmolateInterval","Accumulated Heat", "Immolate cooldown decreases by 25%", "ImmolateInterval", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.25f)))),  
            new Augment("ImmolateInterval","Releasing Everything", "Immolate cooldown decreases by 50%", "ImmolateInterval", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0, 0,0, 0.5f)))), 
            new Augment("ImmolateDmg","Summer Heat", "Immolate will deal +20 damage", "ImmolateDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,25,0,0))), immoType:IMMOLATE.FIRE),
            new Augment("ImmolateDmg","Universal Damage", "Immolate will deal +40 damage", "ImmolateDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,40,0,0))), immoType:IMMOLATE.FIRE), 
            new Augment("ImmolateDmg","Nuclear Shockwave", "Immolate will deal +80 damage", "ImmolateDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,80,0,0))), immoType:IMMOLATE.FIRE),   
            new Augment("ImmolateRadius","Waving Flames", "Immolating waves will travel for +15 radius", "ImmolateRadius", Tier.Silver, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.15f,0)))),  
            new Augment("ImmolateRadius","Spread the Fire", "Immolating waves will travel for +30 radius", "ImmolateRadius", Tier.Gold, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.30f,0)))), 
            new Augment("ImmolateRadius","Across the Globe", "Immolating waves will travel for +75 radius", "ImmolateRadius", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addTimeBasedEffect(new Immolate(0,0,0.75f,0)))), 
            



            new Augment("Ritual" ,"Ritual", "Start the Ritual", "CandleUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Ritual");
                Flamey.Instance.addNotEspecificEffect(new CandleTurrets(5, 0.2f, 1));
                Deck.Instance.AddAugmentClass(new List<string>{"CandleDmg","CandleAmount","CandleAtkSpeed"});            
            }), baseCard: true),  
            new Augment("CandleAtkSpeed","Alembic Artistry", "Candles will increase their attack speed by +20", "CandleAtkSpeed", Tier.Silver,  new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.2f, 0)))), 
            new Augment("CandleAtkSpeed","Ancient Wizard", "Candles will increase their attack speed by +40", "CandleAtkSpeed", Tier.Gold,  new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.4f, 0)))), 
            new Augment("CandleAtkSpeed","Begin the Ritual", "Candles will increase their attack speed by +80", "CandleAtkSpeed", Tier.Prismatic,  new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0.8f, 0)))), 
            new Augment("CandleDmg","Apprentice", "Candles will deal +10 damage", "CandleDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(10, 0, 0))), immoType:IMMOLATE.FIRE), 
            new Augment("CandleDmg","Aether Infusion", "Candles will deal +20 damage", "CandleDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(25, 0, 0))), immoType:IMMOLATE.FIRE), 
            new Augment("CandleDmg","Witchcraft Master", "Candles will deal +50 damage", "CandleDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(50, 0, 0))), immoType:IMMOLATE.FIRE), 
            new Augment("CandleAmount","Philosopher's Stone", "Gain +1 Candles", "CandleAmount", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new CandleTurrets(0, 0, 1)))), 



            new Augment("Bee Summoner" ,"Beekeeper", "Unlock the ability to own a Bee Swarm", "SummonUnlock", Tier.Prismatic, new UnityAction(()=> {
                Deck.Instance.removeClassFromDeck("Bee Summoner");
                Flamey.Instance.addNotEspecificEffect(new Summoner(25, 0.5f, 0.5f, 1));
                if(SkillTreeManager.Instance.getLevel("Bee Summoner") >= 1){
                    Deck.Instance.AddAugmentClass(new List<string>{"SummonAtkSpeed","SummonDmg", "SummonSpeed","SummonAmount","SummonAmountExtra"});      
                }else{
                    Deck.Instance.AddAugmentClass(new List<string>{"SummonAtkSpeed","SummonDmg", "SummonSpeed","SummonAmount"});      
                }
                      
            }), baseCard: true),  
            new Augment("SummonAtkSpeed","Rapid Shooters", "Bees will increase their attack speed by +20", "SummonAtkSpeed", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.20f, 0, 0))), immoType:IMMOLATE.FIRE), 
            new Augment("SummonAtkSpeed","Bee-autiful Pets", "Bees will increase their attack speed by +40", "SummonAtkSpeed", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.40f, 0, 0))), immoType:IMMOLATE.FIRE), 
            new Augment("SummonAtkSpeed","Bee Swarm", "Bees will increase their attack speed by +80", "SummonAtkSpeed", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0.80f, 0, 0))), immoType:IMMOLATE.FIRE), 
            new Augment("SummonDmg","Baby Bee", "Bees will deal +10 damage", "SummonDmg", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(10, 0, 0, 0)))),
            new Augment("SummonDmg","Bee Stronger", "Bees will deal +25 damage", "SummonDmg", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(25, 0, 0, 0)))),
            new Augment("SummonDmg","Queen-like Power", "Bees will deal +50 damage", "SummonDmg", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(50, 0, 0, 0)))),  
            new Augment("SummonSpeed","Speeding Up", "Increase Bee speed by +0.25", "SummonSpeed", Tier.Silver, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 0.25f, 0)))),  
            new Augment("SummonSpeed","Agility", "Increase Bee speed by +0.5", "SummonSpeed", Tier.Gold, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, .5f, 0)))),
            new Augment("SummonSpeed","Bee Acrobatic League", "Increase Bee speed by +1", "SummonSpeed", Tier.Prismatic, new UnityAction(() => Flamey.Instance.addNotEspecificEffect(new Summoner(0, 0, 1f, 0)))),
            
            new Augment("SummonAmount","Worker Bee", "Your Bee Swarm gains an extra Worker Bee", "SummonAmount", Tier.Silver, new UnityAction(() => Summoner.Instance.addBee(1, 0))), 
            new Augment("SummonAmount","More Workers", "Your Bee Swarm gains 2 extra Worker Bees", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(2, 0))), 

            new Augment("SummonAmountExtra","Puncher Bee", "Your Bee Swarm gains an extra Puncher Bee", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(1, 1)), immoType:IMMOLATE.FIRE),
            new Augment("SummonAmountExtra","Assassin Bee", "Your Bee Swarm gains an extra Assassin Bee", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(1, 2)), immoType:IMMOLATE.FIRE),
            new Augment("SummonAmountExtra","Agile Bee", "Your Bee Swarm gains an extra Agile Bee", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(1, 3))),
            new Augment("SummonAmountExtra","Warrior Bee", "Your Bee Swarm gains an extra Warrior Bee", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(1, 4)), immoType:IMMOLATE.FIRE),
            new Augment("SummonAmountExtra","Pollinator Bee", "Your Bee Swarm gains an extra Pollinator Bee", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(1, 5)), immoType:IMMOLATE.WATER),
            new Augment("SummonAmountExtra","Chemical Bee", "Your Bee Swarm gains an extra Chemical Bee", "SummonAmount", Tier.Gold, new UnityAction(() => Summoner.Instance.addBee(1, 6))),
        
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

    public List<Augment> GetAugmentsFromClasses(List<string> augmentClasses, bool inPool=false){
        List<Augment> result = new List<Augment>();
        List<Augment> iteratingList = inPool  ? Deck.Instance.augments : AllAugments;
        foreach (Augment item in iteratingList)
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
