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
    [SerializeField] GameObject CanvasDeck;
    [SerializeField] private Augment[] currentAugments;

    [SerializeField] private Color[] tierColors;
    private List<Augment> filteredAugments;
    private Tier currentTier;
    private void Start() {
        Instance = this;
        currentAugments = new Augment[3];
        FillDeck();
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
        augments.Add(new Augment("Target Practice", "Increase your accuracy by +15% (capped at 100%)", Tier.Silver, new UnityAction(()=> Flamey.Instance.addAccuracy(15))));
        augments.Add(new Augment("Swifty Flames", "Increase your attack speed by +30", Tier.Silver, new UnityAction(()=> Flamey.Instance.addAttackSpeed(.3f))));
        augments.Add(new Augment("Quick Shot", "Gain +1.25 Bullet Speed", Tier.Silver, new UnityAction(()=> Flamey.Instance.addBulletSpeed(1.25f))));
        augments.Add(new Augment("Warm Soup", "Heal 50% and gain +100 Max HP", Tier.Silver, new UnityAction(()=> Flamey.Instance.addHealth(100,0.5f))));
        augments.Add(new Augment("Critical Thinking", "Gain +5% critical strike chance (capped at 80%)", Tier.Silver, new UnityAction(()=> Flamey.Instance.addCritChance(7))));
        augments.Add(new Augment("Lucky Shots", "Gain +15% critical strike damage", Tier.Silver, new UnityAction(()=> Flamey.Instance.addCritDmg(0.15f))));
        augments.Add(new Augment("Not enough refreshes", "Gain a random Silver augment", Tier.Silver, new UnityAction(()=> randomPicking(Tier.Silver).action())));
    }
    void FillGold(){
        augments.Add(new Augment("Steady Aim", "Increase your accuracy by +30", Tier.Gold, new UnityAction(()=> Flamey.Instance.addAccuracy(30))));
        augments.Add(new Augment("Fire Dance", "Increase your attack speed by 125%", Tier.Gold, new UnityAction(()=> Flamey.Instance.multAttackSpeed(2.25f))));
        augments.Add(new Augment("Fire-Express", "Gain x1.5 Bullet Speed", Tier.Gold, new UnityAction(()=> Flamey.Instance.multBulletSpeed(1.5f))));
        augments.Add(new Augment("Sunfire Cape", "Heal 75% and gain +250 Max HP", Tier.Gold, new UnityAction(()=> Flamey.Instance.addHealth(250, 0.75f))));
        augments.Add(new Augment("Fate's Favor", "Gain +10% critical strike chance (capped at 80%)", Tier.Gold, new UnityAction(()=> Flamey.Instance.addCritChance(10))));
        augments.Add(new Augment("Critical Thinking", "Gain +30% critical strike damage", Tier.Gold, new UnityAction(()=> Flamey.Instance.addCritDmg(0.3f))));
        augments.Add(new Augment("Blessed", "Gain 2 random Silver augments", Tier.Gold, new UnityAction(()=> {randomPicking(Tier.Silver).action();randomPicking(Tier.Silver).action();})));
    }
    void FillPrismatic(){
        augments.Add(new Augment("Eagle Eye", "Double your current Accuracy (capped at 100)", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multAccuracy(2))));
        augments.Add(new Augment("Flamethrower", "Increase your attack speed by 200%", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multAttackSpeed(3f))));
        augments.Add(new Augment("HiperDrive", "Gain x2 Bullet Speed (capped at 20)", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.multBulletSpeed(2f))));
        augments.Add(new Augment("Absolute Unit", "Heal to full HP and gain +500 Max HP", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addHealth(500,1f))));
        augments.Add(new Augment("Critical Inferno", "Gain +15% critical strike chance (capped at 80%) and x1.7 critical strike damage", Tier.Prismatic, new UnityAction(()=> {Flamey.Instance.addCritChance(15);Flamey.Instance.multCritDmg(1.7f);})));
        augments.Add(new Augment("VampFire", "Heal 2% of the damage you deal per hit", Tier.Prismatic, new UnityAction(()=> Flamey.Instance.addOnHitEffect(new VampOnHit(0.02f)))));
    }

    Augment pickFromDeck(){
        if(filteredAugments.Count == 0){
            RefillDeck(currentTier);
        }
        Augment aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        if(currentTier == Tier.Prismatic){
            filteredAugments.Remove(aug);
            augments.Remove(aug);
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
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = augment.Description;
        currentAugments[i] = augment;
        
    }

    public void PickedAugment(int i){
        CanvasDeck.SetActive(false);
        currentAugments[i].action();
        EnemySpawner.Instance.newRound();
    }

    public void StartAugments(bool isPrismaticRound){
        filteredAugments = FilterAugments(isPrismaticRound);
        CanvasDeck.SetActive(true);
        ChangeSlots();
    }

    private List<Augment> FilterAugments(bool isPrismaticRound){
        if(isPrismaticRound){
            currentTier = Tier.Prismatic;
            ChangeColors(tierColors[0],tierColors[1]);
            return augments.FindAll( a => a.tier == Tier.Prismatic);
        }
        if(UnityEngine.Random.Range(0f,1f) < 0.33f){
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

    private Augment randomPicking(Tier tier){
        List<Augment> tempAugments = augments.FindAll( a => a.tier == tier);
        if(tempAugments.Count == 0){
            RefillDeck(tier);
        }
        return tempAugments[UnityEngine.Random.Range(0, tempAugments.Count)];
    }




}
