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
        FillSilver();
        FillGold();
        FillPrismatic();
    }
    void RefillDeck(Tier t){
        switch(t){
            case Tier.Silver:
                FillSilver();
                break;
            case Tier.Gold:
                FillGold();
                break;
            case Tier.Prismatic:
                FillPrismatic();
                break;
        }
    }
    void FillSilver(){   
        augments.Add(new Augment("Target Practice", "Increase your accuracy by +15% (capped at 100%)", "acc", Tier.Silver, new UnityAction(()=> Flamey.Instance.addAccuracy(15))));
        augments.Add(new Augment("Swifty Flames", "Increase your attack speed by +30", "atkspeed", Tier.Silver, new UnityAction(()=> Flamey.Instance.addAttackSpeed(.3f))));
        augments.Add(new Augment("Quick Shot", "Gain +1.25 Bullet Speed", "speed", Tier.Silver, new UnityAction(()=> Flamey.Instance.addBulletSpeed(1.25f))));
        augments.Add(new Augment("Warm Soup", "Heal 30% of your Max Health", "health", Tier.Silver, new UnityAction(()=> Flamey.Instance.addHealth(0,0.3f))));
        augments.Add(new Augment("Critical Thinking", "Gain +5% critical strike chance (capped at 80%)", "critchance", Tier.Silver, new UnityAction(()=> Flamey.Instance.addCritChance(7))));
        augments.Add(new Augment("Lucky Shots", "Gain +15% critical strike damage", "critdmg", Tier.Silver, new UnityAction(()=> Flamey.Instance.addCritDmg(0.15f))));
        augments.Add(new Augment("Not enough refreshes", "Gain a random Silver augment", "dice", Tier.Silver, new UnityAction(()=> ActivateAugment(randomPicking(Tier.Silver)))));
        augments.Add(new Augment("The more the better", "When you fire a shot, gain a 5% chance to fire an extra shot", "multishot", Tier.Silver, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new SecondShot(0.05f)))));
        augments.Add(new Augment("Hard Work", "Gain +10 Base Damage", "weight", Tier.Silver, new UnityAction(()=> Flamey.Instance.addDmg(10))));
    }
    void FillGold(){
        augments.Add(new Augment("Steady Aim", "Increase your accuracy by +30", "acc", Tier.Gold, new UnityAction(()=> Flamey.Instance.addAccuracy(30))));
        augments.Add(new Augment("Fire Dance", "Increase your attack speed by +60", "atkspeed", Tier.Gold, new UnityAction(()=> Flamey.Instance.addAttackSpeed(0.6f))));
        augments.Add(new Augment("Fire-Express", "Gain +2 Bullet Speed", "speed", Tier.Gold, new UnityAction(()=> Flamey.Instance.addBulletSpeed(2f))));
        augments.Add(new Augment("Sunfire Cape", "Heal 50% and gain +250 Max HP", "health", Tier.Gold, new UnityAction(()=> Flamey.Instance.addHealth(250, 0.75f))));
        augments.Add(new Augment("Fate's Favor", "Gain +10% critical strike chance (capped at 80%)", "critchance", Tier.Gold, new UnityAction(()=> Flamey.Instance.addCritChance(10))));
        augments.Add(new Augment("Critical Thinking", "Gain +30% critical strike damage", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addCritDmg(0.3f))));
        augments.Add(new Augment("Feelin' Blessed", "Gain 2 random Silver augments", "dice", Tier.Gold, new UnityAction(()=> {ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));})));
        augments.Add(new Augment("Double trouble", "When you fire a shot, gain a 15% chance to fire an extra shot", "multishot", Tier.Gold, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new SecondShot(0.15f)))));
        augments.Add(new Augment("Heavy Hitter", "Gain +25 Base Damage", "weight", Tier.Gold, new UnityAction(()=> Flamey.Instance.addDmg(25))));
    }
    void FillPrismatic(){
        augments.Add(new Augment("Eagle Eye", "Double your current Accuracy (capped at 100)", "acc", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multAccuracy(2))));
        augments.Add(new Augment("Flamethrower", "Increase your attack speed by x0.5", "atkspeed", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multAttackSpeed(1.5f))));
        augments.Add(new Augment("HiperDrive", "Gain x2 Bullet Speed (capped at 20)", "speed", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multBulletSpeed(2f))));
        augments.Add(new Augment("Absolute Unit", "Heal 75% and gain +500 Max HP", "health", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addHealth(500,1f))));
        augments.Add(new Augment("Critical Inferno", "Gain +15% critical strike chance (capped at 80%) and +170% critical strike damage", "critchance", Tier.Prismatic, new UnityAction(()=> {Flamey.Instance.addCritChance(15);Flamey.Instance.multCritDmg(1.7f);})));
        augments.Add(new Augment("VampFire", "Heal 2% of the damage you deal per hit", "vampfire", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0.02f)))));
        augments.Add(new Augment("Multicaster", "When you fire a shot, gain a 50% chance to fire an extra shot", "multishot", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnShootEffect(new SecondShot(0.5f)))));
        augments.Add(new Augment("Roll the Dices", "Gain 4 random Silver augments", "dice", Tier.Prismatic, new UnityAction(()=> {ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));ActivateAugment(randomPicking(Tier.Silver));})));
        augments.Add(new Augment("Hephaestus", "Gain +50 Base Damage", "weight", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addDmg(50))));
    }

    Augment pickFromDeck(){
        if(filteredAugments.Count == 0){
            RefillDeck(currentTier);
        }

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
        if(tempAugments.Count == 0){
            RefillDeck(tier);
        }
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




}
