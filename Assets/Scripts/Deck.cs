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
