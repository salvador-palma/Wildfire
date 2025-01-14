using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck Instance {get; set;}
    [SerializeField] public List<Augment> augments;
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

    [SerializeField] float GoldAugmentProbability = 0.1f;
    
    private void Awake(){
       
        
        Instance = this;
        RoundOver += ClearRemainderObjects;
      
        
    }

    

    private void Start() {
        
        if(PlayerPrefs.GetInt("PlayerLoad", 0) == 0){
            
            gameState = new GameState();
        }
        GamblingStack=new List<Augment>();
        currentAugments = new Augment[3];
        
        refreshedAugments = new List<Augment>();
        PhaseTiers = Distribuitons.sillyGoose(4, Distribuitons.RandomBinomial(4, 0.33f));

        CheckBlackMarketItems();

    }

    public void FillDeck(){
        
        augments = DeckBuilder.Instance.getAllCards();

        if(SkillTreeManager.Instance.getLevel("Gambling") >= 1){
            augments.AddRange(new List<Augment>(){
                new Augment("Gambling","Not enough refreshes", "Gain 2 random silver augments", "GambleImprove", Tier.Silver, new UnityAction(() => Deck.Instance.Gamble(2, Tier.Silver, "Not enough refreshes")), baseCardUpgrade:true),
                new Augment("Gambling","Feelin' Blessed", "Gain 4 random silver augments", "GambleImprove", Tier.Gold, new UnityAction(() => Deck.Instance.Gamble(4, Tier.Silver, "Feelin' Blessed")), baseCardUpgrade:true),
                new Augment("Gambling","Roll the Dice", "Gain 4 random gold augments", "GambleImprove", Tier.Prismatic, new UnityAction(() => Deck.Instance.Gamble(4, Tier.Gold, "Roll the Dice")), baseCardUpgrade:true)
            });
        }
        
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
    public void ChangeSingular(Augment augment, GameObject slot, int i, bool forGamble = false){

        slot.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = augment == null ? "" : augment.Title;
        slot.transform.Find("Icon").GetComponent<Image>().sprite = augment == null ? null : augment.icon;
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = augment == null ? "" : augment.getDescription();
        if(!forGamble){currentAugments[i] = augment;}
        
    }
    

    public void PickedAugment(int i){
        if(currentAugments[i]==null){return;}
        SlotsParent.GetComponent<Animator>().Play("OutroSlots");

        currPhase++;
        if(currentTier == Tier.Prismatic){GameUI.Instance.PrismaticPicked(); resetPhaseAugmentTier();}
        
        

        ActivateAugment(currentAugments[i]);
        
        refreshedAugments.Clear();
        currentAugments = new Augment[]{null,null,null};
        
        if(!EnemySpawner.Instance.Paused){
            RoundStart?.Invoke(this, new EventArgs());
            EnemySpawner.Instance.newRound();
        }
        

    }

    public void VisualOutroSlots(){
        SlotsParent.GetComponent<Animator>().Play("OutroSlots");
    }

    public void StartAugments(bool isPrismaticRound, bool OnlyUnlockables = false){
        if(Flamey.Instance.GameEnd){return;}


        RoundOver?.Invoke(this, new EventArgs());
        //if(FlameCircle.Instance != null){FlameCircle.Instance.SetSpin(false);}

        filteredAugments = FilterAugments(isPrismaticRound, OnlyUnlockables);
        SlotsParent.GetComponent<Animator>().Play("EnterSlots");
        AudioManager.PlayOneShot(isPrismaticRound ? FMODEvents.Instance.PrismaticAugment : FMODEvents.Instance.DefaultAugment, transform.position);
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

        if(SkillTreeManager.Instance.getLevel("Gambling") < 2){
            tempAugments.RemoveAll(a => a.AugmentClass=="Gambling");
        }
        
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
        PhaseTiers = Distribuitons.sillyGoose(4, Distribuitons.RandomBinomial(4, GoldAugmentProbability));
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
            // FillDeck();

            if(!withLoad){return;}
            

            gameState = GameState.LoadGameState();
            
            Flamey.Instance.addEmbers(gameState.CollectedEmbers);
            
            PhaseTiers = gameState.NextTiers;
            
            foreach(SerializedAugment a in gameState.augments){
                GameUI.Instance.AddAugment(a);
                DeckBuilder.Instance.getAugmentByName(a.title).Activate();
            }
            Flamey.Instance.Health = gameState.Health;
            Flamey.Instance.MaxHealth = gameState.MaxHP;

            EnemySpawner.Instance.current_round = gameState.CurrentRound;
            EnemySpawner.Instance.PickedEnemies = LocalBestiary.INSTANCE.getEnemiesFromIDs(gameState.EnemyIDs);

        }catch(Exception e){
            Debug.Log(e.ToString());
        }
        
    }
    private void ClearRemainderObjects(object sender, EventArgs e)
    {
        
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Prop")){
            IPoolable p = g.GetComponent<IPoolable>();
            if(p!=null){
                p.UnPool();
            }else{
                Destroy(g);
            }
             
        }
    }


    /*=========== GAMBLING SECTION ============= */
    [Header("Gambling Section")]
    [SerializeField] Transform ExtraSlotContainer;
    [SerializeField] GameObject ExtraOriginalVessel;
    public List<Augment> GamblingStack;
    public void PlayOutroExtraSlotsGambling(){
        SlotsParent.GetComponent<Animator>().Play("ExtraOutroSlots");
        StartCoroutine(ExtraOutroSlotsAfter());
        

    }
    public IEnumerator ExtraOutroSlotsAfter(){
        yield return new WaitForSeconds(2);
        if(GamblingStack.Count() == 0){
            EnemySpawner.Instance.Paused = false;
            RoundStart?.Invoke(this, new EventArgs());
            EnemySpawner.Instance.newRound();
        }else{
            Debug.Log("Getting Stack");
            
            Augment a = GamblingStack.First();
            GamblingStack.Remove(a);
            ActivateAugment(a);
            Debug.Log("Over Stack");
        }
    }

    
    public void Gamble(int amount, Tier tier, string original_name){
        
        EnemySpawner.Instance.Paused = true;
        Augment[] result = new Augment[amount];
        for(int i = 0; i < amount; i++){
            result[i] = randomPicking(tier);
            if(result[i].AugmentClass=="Gambling"){
                GamblingStack.Add(result[i]);
            }else{
                ActivateAugment(result[i]);
            }
        }
        switch(tier){
            case Tier.Silver:
                SetupGamblingResult(DeckBuilder.Instance.getAugmentByName(original_name), result, tierSprites[0], tierSprites[1]);break;
            case Tier.Gold:
                SetupGamblingResult(DeckBuilder.Instance.getAugmentByName(original_name), result, tierSprites[2], tierSprites[3]);break;
            case Tier.Prismatic:
                SetupGamblingResult(DeckBuilder.Instance.getAugmentByName(original_name), result, tierSprites[4], tierSprites[5]);break;
        }
    }
    public void SetupGamblingResult(Augment og, Augment[] results, Sprite sprite, Sprite back){
        ChangeColorsExtra(ExtraOriginalVessel);
        ChangeSingular(og, ExtraOriginalVessel, -1, forGamble:true);
        foreach(Transform child in ExtraSlotContainer.transform){
            if(child.name.Contains("Clone")){Destroy(child.gameObject);}
        }
        Transform template = ExtraSlotContainer.transform.GetChild(0).transform;
        foreach (Augment item in results)
        {
            Transform tr = Instantiate(template, ExtraSlotContainer);
            ChangeColorsExtra(tr.gameObject);
            ChangeSingular(item, tr.gameObject, -1, forGamble:true);
            tr.gameObject.SetActive(true);
            
        }
        
        SlotsParent.GetComponent<Animator>().Play("ExtraSlots");

        void ChangeColorsExtra(GameObject vessel){
            vessel.GetComponent<Image>().sprite = back;
            vessel.transform.Find("Shadow").GetComponent<Image>().sprite = sprite;
        }
    }


    private void CheckBlackMarketItems(){
        string[] AnvilTypes = new string[]{"Wood Anvil","Stone Anvil","Bronze Anvil","Sturdy Anvil","Deluxe Anvil","Royal Anvil","Star Forger"};
        foreach(string Anvil in AnvilTypes){if(Item.has(Anvil)){GoldAugmentProbability+=0.05f;}}
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
    public float AtkSpeed;
    public string Character;
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
    public SerializedAugment(string title){
        this.title = title;
    }
}
