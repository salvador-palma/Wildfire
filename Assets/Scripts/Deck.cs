using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Loading;
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



    GameState gameState;

    private void Awake(){
        Instance = this;
        
        
    }
    private void Start() {

        if(PlayerPrefs.GetInt("PlayerLoad", 0) == 1){
            LoadGame();
        }else{
            gameState = new GameState();
        }

        currentAugments = new Augment[3];
        FillDeck();
        refreshedAugments = new List<Augment>();
        PhaseTiers = Distribuitons.sillyGoose(4, Distribuitons.RandomBinomial(4, 0.33f));
    }

    void FillDeck(){
        augments = DeckBuilder.Instance.getAllCards();
    }   



    Augment pickFromDeck(){
        Augment aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        //MAKES UNIQUES REMOVE AFTER
        if(filteredAugments.Count <= 3){
            int i = 0;
            while(currentAugments.Contains(filteredAugments[i])){
                i++;
                if(i >= filteredAugments.Count){return filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];}
            }
            return filteredAugments[i];
        }
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

        slot.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = augment == null ? "" : augment.Title;
        slot.transform.Find("Icon").GetComponent<Image>().sprite = augment == null ? null : augment.icon;
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = augment == null ? "" : augment.getDescription();
        currentAugments[i] = augment;
        
    }
    

    public void PickedAugment(int i){
        if(currentAugments[i]==null){return;}
        SlotsParent.GetComponent<Animator>().Play("OutroSlots");

        currPhase++;
        if(currentTier == Tier.Prismatic){GameUI.Instance.PrismaticPicked(); resetPhaseAugmentTier();}
        
        if(FlameCircle.Instance != null){FlameCircle.Instance.SetSpin(true);}
        ActivateAugment(currentAugments[i]);
        refreshedAugments.Clear();

        
        EnemySpawner.Instance.newRound();
        
        

        currentAugments = new Augment[]{null,null,null};
        
    }

    public void StartAugments(bool isPrismaticRound, bool OnlyUnlockables = false){
        if(Flamey.Instance.GameEnd){return;}

        if(FlameCircle.Instance != null){FlameCircle.Instance.SetSpin(false);}

        filteredAugments = FilterAugments(isPrismaticRound, OnlyUnlockables);
        SlotsParent.GetComponent<Animator>().Play("EnterSlots");
        if(isPrismaticRound){GameUI.Instance.FillAll();}
        ChangeSlots();
        EnableRefreshes(!OnlyUnlockables);
    }
    private void EnableRefreshes(bool enable){
        for (int i = 0; i < 3; i++)
        {
            RefreshButtons[i].gameObject.SetActive(enable);
            RefreshButtons[i].interactable = true;
        }
    }

    private List<Augment> FilterAugments(bool isPrismaticRound, bool OnlyUnlockables){

        if(isPrismaticRound || OnlyUnlockables){
            currentTier = Tier.Prismatic;
            ChangeColors(tierSprites[4],tierSprites[5]);
            return OnlyUnlockables ? augments.FindAll( a => a.isUnlockableMidGame()) : augments.FindAll( a => a.tier == Tier.Prismatic);
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
        if(!a.getDescription().Contains("random")){
            GameUI.Instance.AddAugment(a);
            updateGameState(a);
        }
        
    }
    

    public void RefreshAugment(int index){
        RefreshButtons[index].interactable = false;
        Augment aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
        int tries = 200;
        while(tries > 0 && (currentAugments.Contains(aug) || refreshedAugments.Contains(aug))){
            aug = filteredAugments[UnityEngine.Random.Range(0, filteredAugments.Count)];
            tries--;
        }
        if(tries <= 0){Debug.LogWarning("Reroll Limit Exceeded!");}
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


    public bool hasAtLeastOneUnlockable(){
        bool result = false;
        foreach(Augment a in augments){
            result = result || a.isUnlockableMidGame();
        }
        return result;
    }

    private void updateGameState(Augment augmentToAdd){
        gameState.augments.Add(augmentToAdd.Serialize());
        gameState.CollectedEmbers = Flamey.Instance.Embers;
        gameState.Health = Flamey.Instance.Health;
        gameState.CurrentRound = EnemySpawner.Instance.current_round;
        gameState.NextTiers = PhaseTiers;
        GameState.SaveGameState(gameState);
    }
    private void LoadGame(){
        gameState = GameState.LoadGameState();
        Flamey.Instance.addEmbers(gameState.CollectedEmbers);
        Flamey.Instance.Health = gameState.Health;
        
        PhaseTiers = gameState.NextTiers;

        foreach(SerializedAugment a in gameState.augments){
            GameUI.Instance.AddAugment(a);
            DeckBuilder.Instance.getAugmentByName(a.title).Activate(a.level);
        }

        EnemySpawner.Instance.current_round = gameState.CurrentRound;
       
        
    }


}



[Serializable]
public class GameState{
    public GameState(){
        augments = new List<SerializedAugment>();
    }

    public List<SerializedAugment> augments;
    public int CollectedEmbers;
    public int Health;
    public int CurrentRound;
    public bool[] NextTiers;


    public static GameState LoadGameState(){
        GameState result = null;
        if(File.Exists(Application.dataPath +"/gameState.json")){
            string json = File.ReadAllText(Application.dataPath +"/gameState.json");
            result = JsonUtility.FromJson<GameState>(json);
        }
        return result;
    }
    public static void SaveGameState(GameState gameState){
        string json = JsonUtility.ToJson(gameState);
        File.WriteAllText(Application.dataPath + "/gameState.json", json);
    }

    public static void Delete(){
        if(File.Exists(Application.dataPath +"/gameState.json")){
            File.Delete(Application.dataPath +"/gameState.json");
            
        }
    }

}

[Serializable]
public class SerializedAugment{
    public string title;
    public int level;
    public SerializedAugment(string title, int level){
        this.title = title;
        this.level = level;
    }
}
