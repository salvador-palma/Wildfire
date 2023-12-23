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

    [SerializeField] private Sprite[] tierSprites;
    private List<Augment> filteredAugments;
    private Tier currentTier;

    private bool[] PhaseTiers = new bool[4];
    private int currPhase = 0;
    private void Awake(){
        Instance = this;
    }
    private void Start() {
        
        currentAugments = new Augment[3];
        FillDeck();
        refreshedAugments = new List<Augment>();
        PhaseTiers = Distribuitons.sillyGoose(4, Distribuitons.RandomBinomial(4, 0.33f));
    }

    void FillDeck(){
        augments = DeckBuilder.Instance.getAllCards();
        
       
        

        

        

        


        // //SHRED ON HIT
        // augments.Add(new Augment("Shredding Flames", "Unlock the ability to shred enemy armor", "shred", Tier.Prismatic, new UnityAction(()=> {
        //     removeFromDeck("Shredding Flames");
        //     Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0.1f));
        //     augments.Add(new Augment("Weaken", "Gain +10% chance to proc your Shredding Flames effect", "shred", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0.1f, 0f)))));
        //     augments.Add(new Augment("Armor Corruptor", "Gain +20% chance to proc your Shredding Flames effect", "shred", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0.2f, 0f)))));
        //     augments.Add(new Augment("Disintegration Field", "Gain +40% chance to proc your Shredding Flames effect", "shred", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0.4f, 0f)))));
        //     augments.Add(new Augment("Cheese Shredder", "Your Shredding Flames effect reduces +5% more enemy armor per proc", "shred", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0f, 0.05f)))));
        //     augments.Add(new Augment("Black Cleaver", "Your Shredding Flames effect reduces +15% more enemy armor per proc", "shred", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0f, 0.15f)))));
        //     augments.Add(new Augment("Molecular Decomposition", "Your Shredding Flames effect reduces +35% more enemy armor per proc", "shred", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ShredOnHit(0f, 0.35f)))));
        // })));

        // //ASSASSIN'S PATHS
        // augments.Add(new Augment("Assassin's Path", "Unlock the ability to pierce armor and execute enemies", "assassin", Tier.Prismatic, new UnityAction(()=> {
        //     removeFromDeck("Assassin's Path");
        //     Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f));
        //     Flamey.Instance.addArmorPen(0.05f);
        //     augments.Add(new Augment("Execution Enforcer", "You can execute enemies for +2% of their Max Health (capped at 50%)", "assassin", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.02f)))));
        //     augments.Add(new Augment("Soul Collector", "You can execute enemies for +4% of their Max Health (capped at 50%)", "assassin", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.04f)))));
        //     augments.Add(new Augment("La Guillotine", "You can execute enemies for +10% of their Max Health (capped at 50%)", "assassin", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new ExecuteOnHit(0.1f)))));
        //     augments.Add(new Augment("Shell Breaker", "Gain +5% Armor Penetration", "assassin", Tier.Silver, new UnityAction(()=> Flamey.Instance.addArmorPen(0.05f))));
        //     augments.Add(new Augment("Quantum Piercing", "Gain +12% Armor Penetration", "assassin", Tier.Gold, new UnityAction(()=> Flamey.Instance.addArmorPen(0.12f))));
        //     augments.Add(new Augment("Lance of Aether", "Gain +25% Armor Penetration", "assassin", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addArmorPen(0.25f))));
        // })));


        // //KRAKEN SLAYER
        // augments.Add(new Augment("Blue Flame", "Unlock the ability to shoot blue flames that inflict extra damage", "blueflame", Tier.Prismatic, new UnityAction(()=> {
        //     removeFromDeck("Blue Flame");
        //     Flamey.Instance.addOnShootEffect(new KrakenSlayer(20, 50));

        //     augments.Add(new Augment("The Bluer The Better", "You will need 1 shot less to proc Blue Flame", "blueflame", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(1, 0)))));
        //     augments.Add(new Augment("Propane Combustion", "You will need 3 shots less to proc Blue Flame", "blueflame", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(3, 0)))));
        //     augments.Add(new Augment("Never ending Blue", "You will need 7 shots less to proc Blue Flame", "blueflame", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(7, 0)))));
        //     augments.Add(new Augment("Propane Leakage", "Your Blue Flame deals +25 extra damage", "blueflame", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 25)))));
        //     augments.Add(new Augment("Powerfull Blue", "Your Blue Flame deals +50 extra damage", "blueflame", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 50)))));
        //     augments.Add(new Augment("Blue Inferno", "Your Blue Flame deals +100 extra damage", "blueflame", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new KrakenSlayer(0, 100)))));
        // })));


        // //ORBITAL FLAMES
        // augments.Add(new Augment("Orbital Flames", "A tiny Flame will orbit around you damaging the foes it collides with", "orbital", Tier.Prismatic, new UnityAction(()=> {
        //     removeFromDeck("Orbital Flames");
        //     Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 25));

        //     augments.Add(new Augment("Tame the Flames", "Gain +1 tiny Flame in your Orbital Field", "orbital", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(1, 0)))));
        //     augments.Add(new Augment("Tiny Flames Win", "Your Orbital Flames deal +10 damage", "orbital", Tier.Silver, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 10)))));
        //     augments.Add(new Augment("Relliable Damage", "Your Orbital Flames deal +25 damage", "orbital", Tier.Gold, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 25)))));
        //     augments.Add(new Augment("Saturn", "Your Orbital Flames deal +50 damage", "orbital", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addNotEspecificEffect(new FlameCircle(0, 50)))));
        // })));

        // //LAVA POOL
        // augments.Add(new Augment("Lava Pool", "Unlock the ability to create Lava Pools that ignore enemy armor", "lava", Tier.Prismatic, new UnityAction(()=> {
        //     removeFromDeck("Lava Pool");
        //     Flamey.Instance.addOnLandEffect(new BurnOnLand(1f, 25, 0.1f, 2f));

        //     augments.Add(new Augment("Hot Tub", "Your Lava Pool will inflict +10 damage per second", "lava", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,10,0,0)))));
        //     augments.Add(new Augment("Magical Scorch", "Your Lava Pool will inflict +25 damage per second", "lava", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,25,0,0)))));
        //     augments.Add(new Augment("Conflagration", "Your Lava Pool will inflict +50 damage per second", "lava", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,50,0,0)))));


        //     augments.Add(new Augment("Hot Steps", "Gain +5% probability of spawning a Lava Pool when your shot lands (capped at 50%)", "lava", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.05f,0)))));
        //     augments.Add(new Augment("Lava here, Lava there", "Gain +15% probability of spawning a Lava Pool when your shot lands (capped at 50%)", "lava", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.15f,0)))));
        //     augments.Add(new Augment("The Apocalypse", "Gain +25% probability of spawning a Lava Pool when your shot lands (capped at 50%)", "lava", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0.25f,0)))));
        
        //     augments.Add(new Augment("Heat Area", "Your Lava Pool grows by +0.25 (capped at 2.5)", "lava", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0.25f,0,0,0)))));
        //     augments.Add(new Augment("Lava Lakes", "Your Lava Pool grows by +0.5 (capped at 2.5)", "lava", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0.5f,0,0,0)))));
        //     augments.Add(new Augment("Inside the volcano", "Your Lava Pool grows by +1 (capped at 2.5)", "lava", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(1f,0,0,0)))));
        
        //     augments.Add(new Augment("Sear the ground", "Your Lava Pool lasts for +0.75 seconds", "lava", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,0.75f)))));
        //     augments.Add(new Augment("Eternally Hot", "Your Lava Pool lasts for +1.5 seconds", "lava", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,1.5f)))));
        //     augments.Add(new Augment("Unsettling Magma", "Your Lava Pool lasts for +3 seconds", "lava", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnLandEffect(new BurnOnLand(0,0,0,3f)))));
        // })));


        // //STATIK SHOT
        // augments.Add(new Augment("Static Energy", "Unlock the ability to send static energy to enemies nearby of your target", "statik", Tier.Prismatic, new UnityAction(()=> {
        //     removeFromDeck("Static Energy");
        //     Flamey.Instance.addOnHitEffect(new StatikOnHit(0.1f,25,3));

        //     augments.Add(new Augment("Watts Up", "Gain +5% probability to proc your Static Energy effect", "statik", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0.05f,0,0)))));
        //     augments.Add(new Augment("Electrifying Possibilities", "Gain +15% probability to proc your Static Energy effect", "statik", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0.15f,0,0)))));
        //     augments.Add(new Augment("The Sparkster", "Gain +30% probability to proc your Static Energy effect", "statik", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0.30f,0,0)))));
        //     augments.Add(new Augment("Shock Dart", "Your Statik Energy deals +10 extra damage", "statik", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0,10,0)))));
        //     augments.Add(new Augment("Shocking Advancement", "Your Statik Energy deals +25 extra damage", "statik", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0,25,0)))));
        //     augments.Add(new Augment("Zeus", "Your Statik Energy deals +50 extra damage", "statik", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0,50,0)))));
        //     augments.Add(new Augment("Conductive materials", "Your Statik Energy will be able to cross through 1 more enemy", "statik", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,1)))));
        //     augments.Add(new Augment("Feel the Flow", "Your Statik Energy will be able to cross through 2 more enemies", "statik", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,2)))));
        //     augments.Add(new Augment("Amping Up!", "Your Statik Energy will be able to cross through 5 more enemies", "statik", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new StatikOnHit(0,0,5)))));
        // })));
    //30 silver 30 gold 41 prismatic

    }   



    Augment pickFromDeck(){
        Augment aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        //MAKES UNIQUES REMOVE AFTER
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
    public void ChangeSingular(Augment augment, GameObject slot, int i){
        slot.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = augment.Title;
        slot.transform.Find("Icon").GetComponent<Image>().sprite = augment.icon;
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = augment.getDescription();
        currentAugments[i] = augment;
        
    }

    public void PickedAugment(int i){
        SlotsParent.GetComponent<Animator>().Play("OutroSlots");

        currPhase++;
        if(currentTier == Tier.Prismatic){GameUI.Instance.PrismaticPicked(); resetPhaseAugmentTier();}
        
        if(FlameCircle.Instance != null){FlameCircle.Instance.SetSpin(true);}
        ActivateAugment(currentAugments[i]);
        refreshedAugments.Clear();
        EnemySpawner.Instance.newRound();
        currentAugments = new Augment[]{null,null,null};
        
    }

    public void StartAugments(bool isPrismaticRound){
        if(Flamey.Instance.GameEnd){return;}

        if(FlameCircle.Instance != null){FlameCircle.Instance.SetSpin(false);}

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
            ChangeColors(tierSprites[4],tierSprites[5]);
            return augments.FindAll( a => a.tier == Tier.Prismatic);
        }
        if(PhaseTiers[currPhase]){
            currentTier = Tier.Gold;
            ChangeColors(tierSprites[2],tierSprites[3]);
            return augments.FindAll( a => a.tier == Tier.Gold);
        }else{
            currentTier = Tier.Silver;
            ChangeColors(tierSprites[0],tierSprites[1]);
            return augments.FindAll( a => a.tier == Tier.Silver);
        }
    }
    private void ChangeColors(Sprite sprite, Sprite back){
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].GetComponent<Image>().sprite = back;
            Slots[i].transform.Find("Shadow").GetComponent<Image>().sprite = sprite;
        }
    }
    public Sprite getTierSprite(Tier t){
         switch(t){
            case Tier.Silver:
                return tierSprites[1];
            case Tier.Gold:
                return tierSprites[3];
            case Tier.Prismatic:
                return tierSprites[5];
                
        }
        return null;
    }

    public Augment randomPicking(Tier tier){
        List<Augment> tempAugments = augments.FindAll( a => a.tier == tier);
        
        return tempAugments[UnityEngine.Random.Range(0, tempAugments.Count)];
    }

    public void ActivateAugment(Augment a){
        a.Activate();
        if(!a.getDescription().Contains("random")){GameUI.Instance.AddAugment(a);}
        
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

    public void resetPhaseAugmentTier(){
        PhaseTiers = Distribuitons.sillyGoose(4, Distribuitons.RandomBinomial(4, 0.33f));
        currPhase = 0;
    }
    public void AddAugmentClass(List<string> str){
        augments.AddRange(DeckBuilder.Instance.GetAugmentsFromClasses(str));
    }


}
