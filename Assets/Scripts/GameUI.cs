using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    private float[] TimeScaleTime = new float[]{2.5f,3f,3.5f,4f}; //2.5f, 3f, 3.5f, 4f
    private int[] TimeScaleValue = new int[]{2,3,4,5};
    private int TimeScaleIndex;
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
    [SerializeField] DynamicText[] EffectTexts;
    [SerializeField] GameObject PassiveContainer;


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
    [SerializeField] GameObject CornerPopUpAnim;
    
    [Header("Spawnable UI")]
    public GameObject AbilityOptionContainer;
    public GameObject SpawnableUIPanel;
    public event EventHandler SpawnExtrasEvent;
    public GameObject UICooldownsContainer;
    public GameObject UICooldownsTemplate;
    public GameObject UIActiveCooldownsTemplate;

    [Header("QuickStart")]
    public LocalBestiary localBestiary;

    private void Awake() {
        Instance = this;
        FastForwardButtons[0].interactable = false;
        FinalStatsButton.onClick.AddListener(()=>{FinalStatsPanel.SetActive(!FinalStatsPanel.activeInHierarchy);});
        
        if(GameVariables.GetVariable("BestiaryReady") <= 0){MenuTabs[2].SetActive(false);ButtonTabs[2].gameObject.SetActive(false);}
        if(!Character.Instance.HasAtLeastOneCharacter()){MenuTabs[3].SetActive(false);ButtonTabs[3].gameObject.SetActive(false);}
        if(!SkillTreeManager.Instance.HasAtLeastOneSkill()){MenuTabs[1].SetActive(false);ButtonTabs[1].gameObject.SetActive(false);}

        if(Item.has("Hourglass")){TimeScaleIndex++;}
        if(Item.has("Magical Hourglass")){TimeScaleIndex++;}
        if(Item.has("Celestial Hourglass")){TimeScaleIndex++;}
        FastForwardButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + TimeScaleValue[TimeScaleIndex];

        localBestiary.Awake();
        InteractiveCursor.ChangeCursor(0);
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
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", PausePanel.activeInHierarchy ? 0 : 1);

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
            if(current_Tab == 1){Canvas.ForceUpdateCanvases();}
        }
    }

    public void AddAugment(Augment a){
        if(!ownsAugment){ownsAugment = true; NoAugmentsText.SetActive(false);}

        Deck.Instance.inBuildAugments.Add(a);
        GameObject go = Instantiate(AugmentTemplate, AugmentContainer.transform);
        
        go.transform.GetChild(0).GetComponent<Image>().sprite = Deck.Instance.getTierSprite(a.tier);
        go.transform.GetChild(1).GetComponent<DynamicText>().SetText(a.Title);
        go.transform.GetChild(2).GetComponent<DynamicText>().SetText(a.getDescription());
        go.transform.GetChild(3).GetComponent<Image>().sprite = a.icon;
        go.SetActive(true);
    }
    public void AddAugment(SerializedAugment serA){
        if(!ownsAugment){ownsAugment = true; NoAugmentsText.SetActive(false);}
        GameObject go = Instantiate(AugmentTemplate, AugmentContainer.transform);

        Augment a = DeckBuilder.Instance.getAugmentByName(serA.title);

        go.transform.GetChild(0).GetComponent<Image>().sprite = Deck.Instance.getTierSprite(a.tier);
        go.transform.GetChild(1).GetComponent<DynamicText>().SetText(a.Title);
        go.transform.GetChild(2).GetComponent<DynamicText>().SetText(a.getDescription());
        go.transform.GetChild(3).GetComponent<Image>().sprite = a.icon;
        go.SetActive(true);
    }

    public void defineStats(){
        Flamey f = Flamey.Instance;
        StatsTexts[0].text = "" + f.Dmg;
        StatsTexts[1].text = f.atkSpeed.ToString("F2") + "/s";
        StatsTexts[2].text = "x"+ f.BulletSpeed.ToString("F1");
        StatsTexts[3].text = f.accuracy.ToString("F0") + "%";
        StatsTexts[4].text = f.Armor.ToString();
        StatsTexts[5].text = f.Health.ToString("F0")+"/"+f.MaxHealth.ToString("F0");
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
        if(latestInfoEffect==null){PassiveContainer.SetActive(true); PassiveContainer.transform.parent.Find("Caps Label").gameObject.SetActive(true);}
        
        foreach(Transform abilityOption in AbilityOptionContainer.transform){abilityOption.gameObject.SetActive(false);}

        Ability ability = SkillTreeManager.Instance.getAbility(e.getText());
        int level = SkillTreeManager.Instance.getLevel(ability.Name);


        EffectTexts[3].SetText("<size=100%><style=\"Yellow\">- Level 1 -</style><br><size=80%>{0}", new string[]{ability.AbilityDescription1});
        EffectTexts[4].SetText("<size=100%><style=\"Yellow\">- Level 2 -</style><br><size=80%>{0}", new string[]{ability.AbilityDescription2});
        EffectTexts[5].SetText("<size=100%><style=\"Yellow\">- Level 3 -</style><br><size=80%>{0}", new string[]{ability.AbilityDescription3});
        EffectTexts[3].setColor(Color.white);
        EffectTexts[4].setColor(Color.white);
        EffectTexts[5].setColor(Color.white);
        switch (level+1)
        {    
            case 0:
                EffectTexts[3].setColor(new Color(1,1,1,0.3f));
                EffectTexts[4].SetText("<size=100%><style=\"Yellow\">- Level 2 -</style><br><size=80%>{0}", new string[]{"???"});
                EffectTexts[4].setColor(new Color(1,1,1,0.3f));
                EffectTexts[5].SetText("<size=100%><style=\"Yellow\">- Level 3 -</style><br><size=80%>{0}", new string[]{"???"});
                EffectTexts[5].setColor(new Color(1,1,1,0.3f));
            break;
            case 1:
                EffectTexts[4].setColor(new Color(1,1,1,0.3f));
                EffectTexts[5].SetText("<size=100%><style=\"Yellow\">- Level 3 -</style><br><size=80%>{0}", new string[]{"???"});
                EffectTexts[5].setColor(new Color(1,1,1,0.3f));
            break;
            case 2:
                EffectTexts[5].setColor(new Color(1,1,1,0.3f));
            break;
        }

        latestInfoEffect = e;
        EffectTexts[0].SetText(e.getText());
        EffectTexts[1].SetText(e.getType());
        string[] caps = e.getCaps();
        EffectTexts[2].SetText(caps[0], caps.Skip(1).ToArray());

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
            FastForwardButtons[1].GetComponent<Animator>().Play("FastForwardDetail");
            
            SpeedUp(TimeScaleTime[TimeScaleIndex]);

        }else{
            FastForwardButtons[0].interactable = false;
            FastForwardButtons[1].interactable = true;
            SpeedUp(1f);
        }
    }

    public void GameOverEffect(){
         AudioManager.Instance.SetAmbienceParameter("OST_Volume", 1);
        int n = EnemySpawner.Instance.current_round; 
        RoundsLastedText.GetComponent<DynamicText>().SetText("YOU'VE SURVIVED UNTIL {0}h{1}", new string[]{(n/10).ToString(), ((n%10)*6).ToString("00")});
        foreach (Transform item in  Flamey.Instance.transform)
        {
            item.GetComponent<SpriteRenderer>().sortingOrder += 2;
        }
        // Flamey.Instance.GetComponent<SpriteRenderer>().sortingOrder = 4;
        GetComponent<Animator>().Play("GameOver");
        if(EnemySpawner.Instance.isOnAugments){Deck.Instance.VisualOutroSlots();}
        setUpFinalStats();
        AudioManager.Instance.SetAmbienceParameter("Dead", 1f);
    }
    public void PlayButtonSound(int n){
        AudioManager.Instance.PlayButtonSound(n);
    }
    public void loadScene(string str){
        SkillTreeManager.AddEmbersToJSON(Flamey.Instance.Embers);
        LocalBestiary.INSTANCE.UpdateBestiaryValues();

        
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 1);
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
        template.transform.GetChild(0).GetComponent<DynamicText>().SetText(simpleStat.Title);
        template.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = simpleStat.value + "";
    }

    public void StartGameEvent(){
        EnemySpawner.Instance.StartGame();
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
            EnemySpawner.Instance.Paused = false;
            EnemySpawner.Instance.newRound();
        }else{
            //Character.Instance.Unlock();
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
        CharacterNameDescriptionTxt.SetText(string.Format("{0}<br><color=#999999><size=10>{1}</size></color>",Character.Instance.getName().ToUpper(), Character.Instance.getDescription()));
        Character.Instance.TransformVesselToCharacter(CharacterImage);
    }
    public void UpdateProfileCharacter(){
        Character.Instance.TransformVesselToCharacter(ProfileVessel);
        Debug.Log("UPDATED PROFILE CHARACTERS");
    }


    public void CornerPopUp(string title, string description, Sprite icon){
        CornerPopUpAnim.transform.GetChild(1).GetComponent<DynamicText>().SetText(title);
        CornerPopUpAnim.transform.GetChild(2).GetComponent<DynamicText>().SetText(description);
        CornerPopUpAnim.transform.GetChild(3).GetComponent<Image>().sprite = icon;
        CornerPopUpAnim.GetComponent<Animator>().Play("CornerPopUp");

    }
    public void CompleteQuestIfHasAndQueueDialogue(int questID, string npcname, int dialogueID){
        if(GameVariables.hasQuest(questID)){
            GameVariables.CompleteQuest(questID);
            NPC.QueueDialogue(npcname, dialogueID);
            Quest q = QuestBoard.Instance.Quests[questID]; 
            CornerPopUp("Quest Complete", q.Title, q.Avatar);
        }

        
        
    }


    /* ===== EXTRA UI ===== */
    public GameObject SpawnUI(GameObject prefab){
        return Instantiate(prefab, SpawnableUIPanel.transform);
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
