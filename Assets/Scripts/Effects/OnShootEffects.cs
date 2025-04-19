using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface OnShootEffects : Effect
{
    public bool addList();
    public int ApplyEffect();

}

public class SecondShot : OnShootEffects{


    public static SecondShot Instance;
    
    public float perc;

    public int currentTargetingOption;

    public GameObject optionMenu;
    Enemy secondaryTarget;
    public SecondShot(float p){
        perc = p;
        if(Instance == null){
            Instance = this;
            currentTargetingOption = PlayerPrefs.GetInt("MulticasterTargetingOption", 0);
            optionMenu = GameUI.Instance.AbilityOptionContainer.transform.Find("Multicaster").gameObject;
            
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        if(UnityEngine.Random.Range(0f,1f) < perc){
            
            ShootWithDelay();
        }
        return 0;
    }
    private async void ShootWithDelay(){
        await Task.Delay(100);
        
        Flare f = Flamey.Instance.InstantiateShot(new List<string>(){"Multicaster"});
        try{
            switch(currentTargetingOption)
            {
                default:
                case 0://Same
                    f.setTarget(Flamey.Instance.current_homing.HitCenter.position);
                    break;
                case 1://Random
                    f.setTarget(Flamey.Instance.getRandomHomingPosition());
                    break;
                case 2://Second Closest
                    if(secondaryTarget==null){getHoming();}
                    if(secondaryTarget==null){f.setTarget(Flamey.Instance.current_homing.HitCenter.position);}
                    else{f.setTarget(secondaryTarget.HitCenter.position);}
                    break;
                case 3://Mouse
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    f.setTarget(worldPosition);
                    break;
            }
        }catch{
            Debug.LogWarning("Prevented error: Multicaster");
        }
    }

    public void Stack(SecondShot secondShot){
        perc += secondShot.perc;
        RemoveUselessAugments();
    }

    private void getHoming(){
        secondaryTarget = Flamey.Instance.getRandomHomingEnemy();
    }
    private void RemoveUselessAugments(){
        if(perc >= 1f){
            perc = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("MulticasterProb");
        }

        if(GameVariables.hasQuest(16) && perc>=1f && BurstShot.Instance != null && BurstShot.Instance.interval <=10){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(16, "Rowl", 16);
        }

        
        if(GameVariables.hasQuest(25) && perc>=1 && Flamey.Instance.atkSpeed>=12f){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(25, "Rowl", 15);
        }
        
    }
   
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Multicaster";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Whenever you fire a shot, there's a chance to fire an <color=#FFCC7C>extra one</color>. Extra shots will <color=#FF5858>not</color> count towards <color=#FFFF00>Multicaster</color> or <color=#FFFF00>Burst Shot Effects</color>";
    }
    public string[] getCaps()
    {
        return new string[]{"Chance: {0}% (Max. 100%)<br>", Mathf.Round(perc*100).ToString()};
    }
    public string getIcon()
    {
        return "MulticasterUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return SkillTreeManager.Instance.getLevel("Multicaster") >= 1 ? optionMenu : null;
    }
   
}

public class BurstShot : OnShootEffects{
    public static BurstShot Instance;
    public int interval;
    public int amount;
    private int leftToShoot;
    private Image cooldownImage;
    private Button activeCooldownImage;
    private int activeRoundsLeft;
    private int activeRoundsCooldown = 3;
    
    public int currentTargetingOption;

    private int ExtraBurstShots = 0;
    public GameObject optionMenu;
    public BurstShot(int interval, int amount){
        this.interval = interval;
        this.amount = amount;
        if(Instance == null){
            Instance = this;
            cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/BurstUnlock"));
            currentTargetingOption = Math.Max(0,PlayerPrefs.GetInt("BurstShotTargetingOption", -1));
            optionMenu = GameUI.Instance.AbilityOptionContainer.transform.Find("BurstShot").gameObject;

        }else{
            Instance.Stack(this);
        }
        Deck.RoundOver += ResetExtraShots;
    }

    private void ResetExtraShots(object sender, EventArgs e)
    {
        ExtraBurstShots = 0;
    }

    public int ApplyEffect()
    {
        leftToShoot--;
        
        cooldownImage.fillAmount = 1 - ((float)leftToShoot)/interval;
        if(leftToShoot <= 0){
            leftToShoot = interval;
            Burst();
        }
        return 0;
    }
    public void Burst(int a = -1){
        int acutal_amount = a == - 1 ? amount + ExtraBurstShots : 250;
        Debug.Log("Burst: " + acutal_amount);
        for(int i =0; i < acutal_amount; i++){
                Flare f = Flamey.Instance.InstantiateShot(new List<string>(){"Burst Shot", "Multicaster"});

                switch(currentTargetingOption)
                {
                    case 1:
                        f.setTarget(Flamey.Instance.current_homing.HitCenter.position);
                        break;
                    case 2:
                        f.setTarget(Enemy.getPredicatedEnemyPosition((e1,e2)=> e2.MaxHealth - e1.MaxHealth));
                        break;
                    case 0:
                    default:
                        f.setTarget(Flamey.Instance.getRandomHomingPosition());
                        break;
                }
                
        }

        if(SkillTreeManager.Instance.getLevel("Burst Shot") >= 2){
            
            ExtraBurstShots = Math.Min(ExtraBurstShots+1, amount);
        }
    }
   

    public void Stack(BurstShot secondShot){
        amount = Mathf.Min(20, amount + secondShot.amount);
        interval = Mathf.Max(10, interval - secondShot.interval);
        RemoveUselessAugments();    
    }

    private void RemoveUselessAugments(){
        if(amount >= 20){
            amount = 20;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BurstAmount");
        }
        if(interval <= 10){
            interval = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BurstInterval");
        }
        if(GameVariables.hasQuest(16) && interval<=10 && SecondShot.Instance != null && SecondShot.Instance.perc >= 1f){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(16, "Rowl", 16);
        }
       
    }
   
    public void SpawnExtraAssets(){
        // activeCooldownImage = GameUI.Instance.SpawnUIActiveMetric(Resources.Load<Sprite>("Icons/BurstAmount"));
        // activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        // Deck.RoundOver += UpdateActive;
        // activeCooldownImage.onClick.AddListener(() => {
        //     Burst(250);
        //     activeCooldownImage.interactable = false;
        //     activeRoundsLeft = 0;
        //     activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;

        // });
    }
    
    private void UpdateActive(object sender, EventArgs e){
        if(activeRoundsLeft<activeRoundsCooldown){
            activeRoundsLeft++;
            activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = ((float)activeRoundsLeft)/activeRoundsCooldown;
        }
        if(activeRoundsLeft>=activeRoundsCooldown){
             activeCooldownImage.interactable = true;
        }
    }

    public bool addList(){
        return Instance == this;
    }

     
    public string getText()
    {
        return "Burst Shot";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription()
    {
        return "Shoot extra <color=#FFCC7C>Burst Shots</color> everytime you shoot a certain amount of flames. <color=#FFCC7C>Extra shots</color> will <color=#FF5858>not</color> count towards <color=#FFFF00>Multicaster</color> or <color=#FFFF00>Burst Shot Effects</color>";
    }
    public string[] getCaps()
    {
        return new string[]{"Burst Shots: {0} Flames (Max. 20)<br>Burst Interval: {1} Flames (Min. 10)", amount.ToString(), interval.ToString()};
    }
    public string getIcon()
    {
        return "BurstUnlock";
    }
    public GameObject getAbilityOptionMenu(){

        return SkillTreeManager.Instance.getLevel("Burst Shot") >= 1 ? optionMenu : null;
    }
}

public class KrakenSlayer : OnShootEffects{


    public static KrakenSlayer Instance;
    public int interval;
    public int extraDmg;
    int curr;

    int FlamesPurpleCooldown = 20;
    int FlamesUntilPurple = 20;
    Button activeCooldownImage;
    private int activeRoundsLeft;
    private int activeRoundsCooldown = 1;
    public bool purpleON;

    public KrakenSlayer(int interval, int extraDmg){
        this.interval = interval;
        this.extraDmg = extraDmg;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        if(purpleON){return 6;}
        

        if(curr <= 0){
            curr = interval;
            if(SkillTreeManager.Instance.getLevel("Magical Shot") >= 2){
                FlamesUntilPurple--;
                if(FlamesUntilPurple <= 0){
                    FlamesUntilPurple = FlamesPurpleCooldown;
                    return 6;
                }
            }
            return 3;
        }else{
            curr--;
        }
        return 0;
    }
  

    public void Stack(KrakenSlayer krakenSlayer){
        interval = Mathf.Max(0, interval - krakenSlayer.interval);
        extraDmg += krakenSlayer.extraDmg;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(interval == 0){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BlueFlameInterval");
            
        }
        
    }
    
    public void SpawnExtraAssets(){
        activeCooldownImage = GameUI.Instance.SpawnUIActiveMetric(Resources.Load<Sprite>("Icons/BlueFlameInterval"));
        activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        Deck.RoundOver += UpdateActive;
        activeCooldownImage.onClick.AddListener(() => {
            purpleON = true;
            Flamey.Instance.GetComponent<Animator>().SetBool("BluePink",true);
            AudioManager.PlayOneShot(FMODEvents.Instance.EvilLaugh, Vector2.zero);
            Flamey.Instance.callFunctionAfter(() =>{ purpleON = false; Flamey.Instance.GetComponent<Animator>().SetBool("BluePink",false);}, 5f);
            activeCooldownImage.interactable = false;
            activeRoundsLeft = 0;
            activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;

        });
    }
    public void TurnPurple(){
        purpleON = false;
    }
    private void UpdateActive(object sender, EventArgs e){
        if(activeRoundsLeft<activeRoundsCooldown){
            activeRoundsLeft++;
            activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = ((float)activeRoundsLeft)/activeRoundsCooldown;
        }
        if(activeRoundsLeft>=activeRoundsCooldown){
             activeCooldownImage.interactable = true;
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Magical Shot";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }

    public string getDescription() 
    {
        return "Shoot a powerful <color=#53D1FF>Blue Flame</color> that deals <color=#FF5858>Extra Damage</color> everytime you shoot a certain amount of flames.";
    }
    public string[] getCaps()
    {
        return new string[]{"Extra Damage: +{0} Damage <br>Blue Flame Interval: {1} Flames (Min. 0)", extraDmg.ToString(), interval.ToString()};
    }
    public string getIcon()
    {
        return "BlueFlameUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class CritUnlock : OnShootEffects{


    public static CritUnlock Instance;
    public float perc;
    public float mult;
    public CritUnlock(float perc, float mult){
        this.perc = perc;
        this.mult = mult;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }

    public int ApplyEffect()
    {
        if(Distribuitons.RandomUniform(0f,1f) <= perc){
            if(SkillTreeManager.Instance.getLevel("Critical Strike") >= 2 && UnityEngine.Random.Range(0f,1f) < 0.1f){
                return 2;
            }
            return 1;
        }
        return 0;
    }
    

    public void Stack(CritUnlock critUnlock){
        perc += critUnlock.perc;
        mult += critUnlock.mult;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(perc >= 0.8f){
            perc = 0.8f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("CritChance");
        }
        if(mult >= 5f && SkillTreeManager.Instance.getLevel("Critical Strike") < 2){
            mult = 5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("CritMult");
        }
        
    }
  
    public bool addList(){
        return Instance == this;
    }

    public string getDescription()
    {
        return "Your shots have a chance of <color=#FF5858>critically striking</color>, multiplying your <color=#FF5858>damage</color> by a certain amount.";
    }
    public string[] getCaps()
    {
        
        if(SkillTreeManager.Instance.getLevel("Critical Strike") < 1){
            return new string[]{"Critic Chance: +{0}% (Max. 80%)<br>Damage Multiplier: x{1} (Max. x5)", Mathf.Round(perc*100f).ToString(), (Mathf.Round(mult * 100f) * 0.01f).ToString()};
        }
        return new string[]{"Critic Chance: +{0}% (Max. 80%)<br>Damage Multiplier: x{1} (Max. Infinite)", Mathf.Round(perc*100f).ToString(), (Mathf.Round(mult * 100f) * 0.01f).ToString()};
    }

    public string getIcon()
    {
        return "CritUnlock";
    }

    public string getText()
    {
        return "Critical Strike";
    }

    public string getType()
    {
        return "On-Shoot Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

