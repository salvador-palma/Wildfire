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
    [SerializeField] GameObject PausePanel;
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
    
    [Header("Effects")]
    
    [SerializeField] GameObject EffectContainer;
    [SerializeField] GameObject EffectTemplate;
    [SerializeField] TextMeshProUGUI[] EffectTexts;


    [SerializeField] Animator BlackScreen;
    [SerializeField] Animator BlackScreenOver;
    [SerializeField] private TextMeshProUGUI RoundsLastedText;

    [SerializeField] private Animator LimitRoundAnimator;
    


    [Header("Final Stats")]
    
    [SerializeField] GameObject FinalStatsPanel;
    [SerializeField] Button FinalStatsButton;
    List<SimpleStat> FinalStats;
    [SerializeField] GameObject[] StatTemplates;
    

    private void Awake() {
        Instance = this;
        FastForwardButtons[0].interactable = false;
        FinalStatsButton.onClick.AddListener(()=>{FinalStatsPanel.SetActive(!FinalStatsPanel.activeInHierarchy);});
        
    }
    

    public void UpdateProgressBar(int round){
        AugmentTemplate.SetActive(false);
        EffectTemplate.SetActive(false);
        ProgressBar.fillAmount = Progresses[round%5];
        
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
        EraseEffectInfo();
    }
    public void UpdateMenuInfo(int current_round){
        defineStats();
        defineEffectList();
        setRoundCounter(current_round);
        
    }
    
    public void changeTab(int index){
        // if(index==1){defineStats();}
        // if(index==2){defineEffectList();}
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

    public void defineStats(){
        Flamey f = Flamey.Instance;
        StatsTexts[0].text = "Base Damage: " + f.Dmg;
        StatsTexts[1].text = "Attack Speed: " + f.atkSpeed + " flames/s";
        StatsTexts[2].text = "Bullet Speed: x"+f.BulletSpeed;
        StatsTexts[3].text = "Accuracy: " + f.accuracy + "%";
        StatsTexts[4].text = "Armor: " + f.Armor;
        StatsTexts[5].text = f.Health+"/"+f.MaxHealth;
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
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(e.getIcon());
            go.SetActive(true);
        }
    }
    public void DisplayEffectInfo(Effect e){
        
        EffectTexts[0].text = e.getText();
        EffectTexts[1].text = e.getType();
        EffectTexts[2].text = e.getDescription();
    }
    public void EraseEffectInfo(){
        
        EffectTexts[0].text = "";
        EffectTexts[1].text = "";
        EffectTexts[2].text = "";
    }

    public void SpeedUp(float f){
        Time.timeScale = f;
    }

    public void ToggleSpeedUp(){
        if(Time.timeScale == 1f){
            FastForwardButtons[0].interactable = true;
            FastForwardButtons[1].interactable = false;
            SpeedUp(3f);

        }else{
            FastForwardButtons[0].interactable = false;
            FastForwardButtons[1].interactable = true;
            SpeedUp(1f);
        }
        //Debug.Log(Time.timeScale);
    }

    public void GameOverEffect(){
        RoundsLastedText.text = "YOU'VE SURVIVED " + EnemySpawner.Instance.current_round + " ROUNDS";
        Flamey.Instance.GetComponent<SpriteRenderer>().sortingOrder = 4;
        GetComponent<Animator>().Play("GameOver");
        setUpFinalStats();
    }
    public void loadScene(string str){
        SceneManager.LoadScene(str);
    }
    public void StartOver(){
        GetComponent<Animator>().Play("StartOver");
    }
    public void GameToMenu(){
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
    public void BlackScreenOverOn(){
        BlackScreenOver.Play("BlackScreen");
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
        roundCounter.text = "ROUND " + n;
    }
    
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
        EnemySpawner.Instance.StartGame();
    }
    
}

public class SimpleStat{
    public string Title;
    public int value;
    public SimpleStat(string t, int v){Title = t; value = v;}
}
