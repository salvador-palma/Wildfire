using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    public static GameUI Instance {get; private set;}
    [SerializeField] private Image ProgressBar;
    private float[] Progresses = new float[]{0f,.15f,.36f,.56f,.77f};
    [SerializeField] private Animator PrismaticLightAnimator;

    
    [Header("Menu")]
    [SerializeField] Color ActiveTab;
    [SerializeField] Color InactiveTab;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject[] MenuTabs;
    [SerializeField] Image[] ButtonTabs;
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

    
    private void Awake() {
        Instance = this;
        
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
        PausePanel.SetActive(!PausePanel.activeInHierarchy);
        
    }
    
    public void changeTab(int index){
        if(index==1){defineStats();}
        if(index==2){defineEffectList();}
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
        Tuple<Color,Color> t = Deck.Instance.getTierColors(a.tier);
        go.GetComponent<Image>().color = t.Item1;
        go.transform.GetChild(0).GetComponent<Image>().color = t.Item1;
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = a.Title;
        go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = a.Description;
        go.SetActive(true);
    }

    public void defineStats(){
        Flamey f = Flamey.Instance;
        StatsTexts[0].text = "Base Damage: " + f.Dmg;
        StatsTexts[1].text = "Attack Speed: " + f.atkSpeed;
        StatsTexts[2].text = "Critical Strike Chance: " + f.CritChance;
        StatsTexts[3].text = "Critical Damage Multiplier: x" + f.CritMultiplier;
        StatsTexts[4].text = "Bullet Speed: " + f.BulletSpeed;
        StatsTexts[5].text = "Accuracy: " + f.accuracy;
        StatsTexts[6].text = f.Health+"/"+f.MaxHealth;
        healthSlider.maxValue = f.MaxHealth;
        healthSlider.value = f.Health;
    }

    public void defineEffectList(){
        List<Effect> el = new List<Effect>();
        el.AddRange(Flamey.Instance.onHitEffects);
        el.AddRange(Flamey.Instance.onShootEffects);
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
            go.SetActive(true);
        }
    }
    public void DisplayEffectInfo(Effect e){
        
        EffectTexts[0].text = e.getText();
        EffectTexts[1].text = e.getType();
        EffectTexts[2].text = e.getDescription();
    }
    
   
    


    
}
