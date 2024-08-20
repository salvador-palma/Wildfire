using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    public static GameUI Instance {get; private set;}
    [SerializeField] private Image ProgressBar;
    private float[] Progresses = new float[]{0f,.15f,.36f,.56f,.77f};
    [SerializeField] private Animator PrismaticLightAnimator;

    [Header("FastForward")]
    [SerializeField]Button[] FastForwardButtons;
    [Header("Menu")]
    [SerializeField] Color ActiveTab;
    [SerializeField] Color InactiveTab;
    [SerializeField] public GameObject PausePanel;
    [SerializeField] GameObject[] MenuTabs;
    [SerializeField] Image[] ButtonTabs;
    [SerializeField] TextMeshProUGUI roundCounter;
    int current_Tab = 0;

    
    [Header("Augments")]
    [SerializeField] GameObject AugmentContainer;
    [SerializeField] GameObject AugmentTemplate;
    [SerializeField] GameObject NoAugmentsText;
    bool ownsAugment = false;
    
    [Header("Stats")]
    [SerializeField] TextMeshProUGUI[] StatsTexts;
    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject ProfileVessel;
    [SerializeField] TextMeshProUGUI EmberAmountTxt;
    
    [Header("Effects")]
    
    Effect latestInfoEffect;
    [SerializeField] GameObject EffectContainer;
    [SerializeField] GameObject EffectTemplate;
    [SerializeField] TextMeshProUGUI[] EffectTexts;
    [SerializeField] Image EffectIcon;


    [SerializeField] Animator BlackScreen;
    [SerializeField] Animator BlackScreenOver;
    [SerializeField] private TextMeshProUGUI RoundsLastedText;

    [SerializeField] private Animator LimitRoundAnimator;
    
    [Header("Pop Up End Night")]
    [SerializeField] private GameObject ConfirmEndNightPanel;

    [Header("Final Stats")]
    
    [SerializeField] GameObject FinalStatsPanel;
    [SerializeField] Button FinalStatsButton;
    List<SimpleStat> FinalStats;
    [SerializeField] GameObject[] StatTemplates;
    
    [Header("Character Pop Up")]
    [SerializeField] TextMeshProUGUI CharacterNameDescriptionTxt;
    [SerializeField] GameObject CharacterImage;
    
    [Header("Spawnable UI")]
    public GameObject AbilityOptionContainer;
    public GameObject SpawnableUIPanel;
    public event EventHandler SpawnExtrasEvent;
    public GameObject UICooldownsContainer;
    public GameObject UICooldownsTemplate;
    public GameObject UIActiveCooldownsTemplate;

    private void Awake() {
        Instance = this;
        FastForwardButtons[0].interactable = false;
        FinalStatsButton.onClick.AddListener(()=>{FinalStatsPanel.SetActive(!FinalStatsPanel.activeInHierarchy);});
        
        if(GameVariables.GetVariable("BestiaryReady") <= 0){MenuTabs[2].SetActive(false);ButtonTabs[2].gameObject.SetActive(false);}
    }
    

    public void UpdateProgressBar(int round){
        AugmentTemplate.SetActive(false);
        EffectTemplate.SetActive(false);
        
        ProgressBar.fillAmount = Progresses[round < 0 ? 0 : round%5];
        
    }
    public void FillAll(){
         ProgressBar.fillAmount = 1;
         PrismaticLightAnimator.Play("PrismaticLight");
    }

    public void PrismaticPicked(){
         
         PrismaticLightAnimator.Play("PrismaticLightOff");
    }

    
    public void TogglePausePanel(){
        Flamey f = Flamey.Instance;
        PausePanel.SetActive(!PausePanel.activeInHierarchy);
        StatsTexts[5].text = f.Health+"/"+f.MaxHealth;
        healthSlider.maxValue = f.MaxHealth;
        healthSlider.value = f.Health;
    }
    public void UpdateMenuInfo(int current_round){
        defineStats();
        defineEffectList();
        setRoundCounter(current_round);
        
    }
    
    public void changeTab(int index){
        if(index != current_Tab){
            MenuTabs[current_Tab].SetActive(false);
            ButtonTabs[current_Tab].color = InactiveTab;
            ButtonTabs[index].color = ActiveTab;
            MenuTabs[index].SetActive(true);
            current_Tab = index;
        }
    }

    public void AddAugment(Augment a){
        if(!ownsAugment){ownsAugment = true; NoAugmentsText.SetActive(false);}
        GameObject go = Instantiate(AugmentTemplate, AugmentContainer.transform);
        
        go.transform.GetChild(0).GetComponent<Image>().sprite = Deck.Instance.getTierSprite(a.tier);
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = a.Title;
        go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = a.getDescription();
        go.transform.GetChild(3).GetComponent<Image>().sprite = a.icon;
        go.SetActive(true);
    }
    public void AddAugment(SerializedAugment serA){
        if(!ownsAugment){ownsAugment = true; NoAugmentsText.SetActive(false);}
        GameObject go = Instantiate(AugmentTemplate, AugmentContainer.transform);

        Augment a = DeckBuilder.Instance.getAugmentByName(serA.title);

        go.transform.GetChild(0).GetComponent<Image>().sprite = Deck.Instance.getTierSprite(a.tier);
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = a.Title;
        go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = a.getDescription();
        go.transform.GetChild(3).GetComponent<Image>().sprite = a.icon;
        go.SetActive(true);
    }

    public void defineStats(){
        Flamey f = Flamey.Instance;
        StatsTexts[0].text = "" + f.Dmg;
        StatsTexts[1].text = f.atkSpeed.ToString("F2") + "/s";
        StatsTexts[2].text = "x"+ f.BulletSpeed;
        StatsTexts[3].text = f.accuracy + "%";
        StatsTexts[4].text = f.Armor.ToString();
        StatsTexts[5].text = f.Health+"/"+f.MaxHealth;
        StatsTexts[6].text = f.Embers.ToString();
        healthSlider.maxValue = f.MaxHealth;
        healthSlider.value = f.Health;
    }

    public void defineEffectList(){
        List<Effect> el = new List<Effect>();
        el.AddRange(Flamey.Instance.allEffects);
        UpdateEffects(el);
    }
    public void UpdateEffects(List<Effect> el){
        foreach(Transform t in EffectContainer.transform){
            if(t.gameObject == EffectTemplate){continue;}
            Destroy(t.gameObject);
        }
        foreach (Effect e in el)
        {
            GameObject go = Instantiate(EffectTemplate,EffectContainer.transform);
            go.GetComponent<Button>().onClick.AddListener(()=>DisplayEffectInfo(e));
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/"+e.getIcon());
            go.SetActive(true);
        }
        if(latestInfoEffect!=null){DisplayEffectInfo(latestInfoEffect);}
        
    }
    public void DisplayEffectInfo(Effect e){
        if(latestInfoEffect==null){EffectIcon.enabled = true; EffectIcon.transform.parent.Find("Caps Label").gameObject.SetActive(true);}
        
        foreach(Transform abilityOption in AbilityOptionContainer.transform){abilityOption.gameObject.SetActive(false);}

        latestInfoEffect = e;
        EffectTexts[0].text = e.getText();
        EffectTexts[1].text = e.getType();
        EffectTexts[2].text = e.getDescription();
        EffectTexts[3].text = e.getCaps();

        EffectIcon.sprite = Resources.Load<Sprite>("Icons/"+e.getIcon());

        GameObject optionMenu = e.getAbilityOptionMenu();
        if(optionMenu==null){return;}

        optionMenu.SetActive(true);

    }


    public void SpeedUp(float f){
        Time.timeScale = f;
    }

    public void ToggleSpeedUp(){
        if(Time.timeScale == 1f){
            FastForwardButtons[0].interactable = true;
            FastForwardButtons[1].interactable = false;
            SpeedUp(3.5f);

        }else{
            FastForwardButtons[0].interactable = false;
            FastForwardButtons[1].interactable = true;
            SpeedUp(1f);
        }
    }

    public void GameOverEffect(){
        int n = EnemySpawner.Instance.current_round; 
        RoundsLastedText.text = "YOU'VE SURVIVED UNTIL " + n/10 +"h"+ ((n%10)*6).ToString("00")  + " ";
        Flamey.Instance.GetComponent<SpriteRenderer>().sortingOrder = 4;
        GetComponent<Animator>().Play("GameOver");
        setUpFinalStats();
    }
    
    public void loadScene(string str){
        SkillTreeManager.AddEmbersToJSON(Flamey.Instance.Embers);
        LocalBestiary.INSTANCE.UpdateBestiaryValues();
        SceneManager.LoadScene(str);
    }

    private bool clickedAButton = false;
    public void StartOver(){
        if(clickedAButton){return;}
        clickedAButton = true;
        GetComponent<Animator>().Play("StartOver");
    }
    public void GameToMenu(){
        if(clickedAButton){return;}
        clickedAButton = true;
        GetComponent<Animator>().Play("GameToMenu");
    }
    public void DeleteAllEnemies(){
        GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in en){
            Destroy(e);
        }
    }

    public void BlackScreenOn(){
        BlackScreen.Play("BlackScreen");
    }
   
    public void BlackScreenOff(){
        BlackScreen.Play("BlackScreenOff");
    }
    public void ShowLimitRoundPanel(){
        LimitRoundAnimator.Play("LimitRoundOn");
    }
    public void ShowLimitRoundPanelOff(){
        LimitRoundAnimator.Play("LimitRoundOff");
        EnemySpawner.Instance.isOnAugments = true;
        Deck.Instance.StartAugments((59+1)%5 == 0);
    }
    public void setRoundCounter(int n){
        roundCounter.text = n/10 +"h"+ ((n%10)*6).ToString("00") ;
    }

    public void ForceEndGame(){
        Flamey.Instance.EndGame();
    }
    public void PopUpConfirmEndNightPanel(){
        ConfirmEndNightPanel.SetActive(!ConfirmEndNightPanel.activeInHierarchy);
    }
    

    /* ===== FINAL STATS ===== */
    private void setUpFinalStats(){
        FinalStats = new List<SimpleStat>();
        FinalStats.AddRange(Flamey.Instance.getBaseStats());
        displayFinalStatsPage(0);
        
    }
    private void displayFinalStatsPage(int page){
        for (int i = 0; i < 5; i++)
        {
            int currentIndex = i + page *5;
            if(currentIndex >= FinalStats.Count){StatTemplates[i].SetActive(false);}
            else{
                setFinalStatTemplate(FinalStats[currentIndex], StatTemplates[i]);
                StatTemplates[i].SetActive(true);
            }
        }
    }
    private void setFinalStatTemplate(SimpleStat simpleStat, GameObject template){
        template.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = simpleStat.Title;
        template.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = simpleStat.value + "";
    }

    public void StartGameEvent(){
        Console.Log("Start Game Event.");
        try{
            EnemySpawner.Instance.StartGame();
        }catch(Exception e){
            Console.Log("<color=#ff0000>"+e.ToString()+"</color>");
        }
        
    }

    public void SetEmberAmount(int n){
        EmberAmountTxt.text = n.ToString();
    }

    /* ===== CHARACTERS ===== */
    
    public void playCharacterTransition(){
        GetComponent<Animator>().SetTrigger("Character Transition");
    }
    public void CharacterTransitionLooksSetup(){
        Character.Instance.SetupActiveLooks();
    }
    public void CharacterUnlockedPopUp(){
        if(Character.Instance.isCharacterUnlocked()){
            EnemySpawner.Instance.Paused = true;
            EnemySpawner.Instance.newRound();
        }else{
            FillCharacterPopUpInfo();
            GetComponent<Animator>().Play("CharacterUnlockedPopUp");
        }
 
    }
    public void CharacterUnlockedPopDown(){
        GetComponent<Animator>().Play("CharacterUnlockedPopDown");
        
        EnemySpawner.Instance.Paused = false;
        
        EnemySpawner.Instance.newRound();
    }

    private void FillCharacterPopUpInfo(){
        CharacterNameDescriptionTxt.SetText(string.Format("{0}<br><color=#999999><size=10>{1}</size></color>",
                                            Character.Instance.getName().ToUpper(), Character.Instance.getDescription()));
        Character.Instance.TransformVesselToCharacter(CharacterImage);
    }
    public void UpdateProfileCharacter(){
        Character.Instance.TransformVesselToCharacter(ProfileVessel);
    }


    /* ===== EXTRA UI ===== */
    public GameObject SpawnUI(GameObject prefab){
        return Instantiate(prefab, SpawnableUIPanel.transform);
    }
    public void SpawnExtras(){
        SpawnExtrasEvent?.Invoke(this, new EventArgs());
    }

    public Image SpawnUIMetric(Sprite icon){
        GameObject go = Instantiate(UICooldownsTemplate,UICooldownsContainer.transform);
        go.SetActive(true);
        go.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        go.transform.GetChild(1).GetComponent<Image>().sprite = icon;
        return go.transform.GetChild(0).GetComponent<Image>();
    } 
    public Button SpawnUIActiveMetric(Sprite icon){
        GameObject go = Instantiate(UIActiveCooldownsTemplate,UICooldownsContainer.transform);
        go.SetActive(true);
        go.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        go.transform.GetChild(1).GetComponent<Image>().sprite = icon;
        return go.GetComponent<Button>();
    } 

    
   
}

public class SimpleStat{
    public string Title;
    public int value;
    public SimpleStat(string t, int v){Title = t; value = v;}
}
