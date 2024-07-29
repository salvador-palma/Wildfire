using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck Instance {get; set;}
    [SerializeField] List<Augment> augments;
    [SerializeField] GameObject[] Slots;
    [SerializeField] GameObject SlotsParent;
    private Augment[] currentAugments;
    [SerializeField] private Button[] RefreshButtons;
    [SerializeField] private Sprite[] Stars;
    private List<Augment> refreshedAugments;

    [SerializeField] private Sprite[] tierSprites;
    private List<Augment> filteredAugments;
    private Tier currentTier;

    private bool[] PhaseTiers = new bool[4];
    private int currPhase = 0;


    public static event EventHandler RoundOver;
    public static event EventHandler RoundStart;
    public GameState gameState;

    
    private void Awake(){
        Debug.Log("Done here Deck");
        
        Instance = this;
        RoundOver += ClearRemainderObjects;
        // Console.Log("<color=#00ff00> Deck Initialized! </color>");
        
    }

    

    private void Start() {
        Console.Log("<color=#00ff00> Deck Started! </color>");
        if(PlayerPrefs.GetInt("PlayerLoad", 0) == 0){
            
            gameState = new GameState();
        }

        currentAugments = new Augment[3];
        
        refreshedAugments = new List<Augment>();
        PhaseTiers = Distribuitons.sillyGoose(4, Distribuitons.RandomBinomial(4, 0.33f));


    }

    void FillDeck(){
        if(DeckBuilder.Instance == null){
                Console.Log("<color=#0000ff> FillDeck: DeckBuilder was not initialized </color>");
        }
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
        slot.transform.Find("Stars").GetComponent<Image>().sprite = Stars[augment.isUnlockableMidGame() ? 5:augment.getLevel()];
        currentAugments[i] = augment;
        
    }
    

    public void PickedAugment(int i){
        if(currentAugments[i]==null){return;}
        SlotsParent.GetComponent<Animator>().Play("OutroSlots");

        currPhase++;
        if(currentTier == Tier.Prismatic){GameUI.Instance.PrismaticPicked(); resetPhaseAugmentTier();}
        
        RoundStart?.Invoke(this, new EventArgs());

        ActivateAugment(currentAugments[i]);
        
        refreshedAugments.Clear();
        currentAugments = new Augment[]{null,null,null};
        
        if(!EnemySpawner.Instance.Paused){
            EnemySpawner.Instance.newRound();
        }
        

    }

    public void StartAugments(bool isPrismaticRound, bool OnlyUnlockables = false){
        if(Flamey.Instance.GameEnd){return;}


        RoundOver?.Invoke(this, new EventArgs());
        //if(FlameCircle.Instance != null){FlameCircle.Instance.SetSpin(false);}

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

    public void removeClassFromDeck(string augmentClass){
        augments.RemoveAll(a => a.AugmentClass == augmentClass);
    }
    public void removeCardFromDeck(string name){
        augments.RemoveAll(a => a.Title == name);
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
        gameState.MaxHP = Flamey.Instance.MaxHealth;
        gameState.CurrentRound = EnemySpawner.Instance.current_round;
        gameState.NextTiers = PhaseTiers;
        GameState.SaveGameState(gameState);
    }
    public void LoadGame(bool withLoad){
        try{ 
            FillDeck();

            if(!withLoad){return;}
            Console.Log("<color=#0000ff> Passed Loading </color>");
            gameState = GameState.LoadGameState();
            if(Flamey.Instance == null){
                Console.Log("<color=#0000ff> Flamey was not initialized </color>");
            }
            Flamey.Instance.addEmbers(gameState.CollectedEmbers);
            
            PhaseTiers = gameState.NextTiers;
            if(GameUI.Instance == null){
                Console.Log("<color=#0000ff> GameUI was not initialized </color>");
            }
            if(DeckBuilder.Instance == null){
                Console.Log("<color=#0000ff> DeckBuilder was not initialized </color>");
            }
            if(LocalBestiary.INSTANCE == null){
                Console.Log("<color=#0000ff> LocalBestiary was not initialized </color>");
            }
            if(EnemySpawner.Instance == null){
                Console.Log("<color=#0000ff> EnemySpawner was not initialized </color>");
            }
            foreach(SerializedAugment a in gameState.augments){
                GameUI.Instance.AddAugment(a);
                DeckBuilder.Instance.getAugmentByName(a.title).Activate(a.level);
            }
            Flamey.Instance.Health = gameState.Health;
            Flamey.Instance.MaxHealth = gameState.MaxHP;

            EnemySpawner.Instance.current_round = gameState.CurrentRound;
            EnemySpawner.Instance.PickedEnemies = LocalBestiary.INSTANCE.getEnemiesFromIDs(gameState.EnemyIDs);

        }catch(Exception e){
            Console.Log("LoadGame: " + e.Message);
        }
        
    }
    private void ClearRemainderObjects(object sender, EventArgs e)
    {
        int i = 0;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Flare")){
            if(!g.activeInHierarchy && g.GetComponent<Flare>().FlareSpot != null){
                Destroy(g.GetComponent<Flare>().FlareSpot);
                Debug.Log("Destroyed " + i);
                i++;
            }
             
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Prop")){
            Destroy(g);
             
        }
    }


}



[Serializable]
public class GameState{
    public GameState(){
        augments = new List<SerializedAugment>();
    }

    public List<SerializedAugment> augments;
    public int CollectedEmbers;
    public float Health;
    public int MaxHP;
    public int[] EnemyIDs;
    public int CurrentRound;
    public bool[] NextTiers;


    public static GameState LoadGameState(){
        GameState result = null;
        if(File.Exists(Application.persistentDataPath +"/gameState.json")){
            string json = File.ReadAllText(Application.persistentDataPath +"/gameState.json");
            result = JsonUtility.FromJson<GameState>(json);
        }
        return result;
    }
    public static void SaveGameState(GameState gameState){
        string json = JsonUtility.ToJson(gameState);
        File.WriteAllText(Application.persistentDataPath + "/gameState.json", json);
    }

    public static void Delete(){
        if(File.Exists(Application.persistentDataPath +"/gameState.json")){
            File.Delete(Application.persistentDataPath +"/gameState.json");
            
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
