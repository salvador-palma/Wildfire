using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public interface NotEspecificEffect : Effect{
    public bool addList();
    public void ApplyEffect();
}

public class FlameCircle : NotEspecificEffect
{
    public int amount;
    int prevamount;
    public int damage;
    public static FlameCircle Instance;
    public Spinner SpinnerInstance;
    GameObject planetsPanelPrefab;
    GameObject planetsPanel;
    public int PlanetType = -1;
    
    public FlameCircle(int amount, int damage){
       
        this.amount = amount;
        prevamount = amount;
        this.damage = damage;
        if(Instance == null){
            Instance = this;
            GameObject g = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle "+amount));
            planetsPanelPrefab = Resources.Load<GameObject>("Prefab/AbilityCharacter/PlanetSelectionPanel");
            SpinnerInstance = g.GetComponent<Spinner>();
            Deck.RoundOver += SetSpinFalse;
            Deck.RoundStart += SetSpinTrue;
            
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        SpinnerInstance.speed = Flamey.Instance.BulletSpeed;
        
        
    }
    public void SetSpinFalse(object sender ,EventArgs e){
        SetSpin(false);
    }
    public void SetSpinTrue(object sender ,EventArgs e){
        SetSpin(true);
    }
    public void SetSpin(bool b){
        SpinnerInstance.canSpin = b;
    }
    public void UpdateAmount(){
        if(amount != prevamount){
            prevamount = amount;
            Spinner next = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle "+amount)).GetComponent<Spinner>();
            SpinnerInstance.kill();
            SpinnerInstance = next;
        }
    }
    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return "Flames orbit around you in a circle. Colliding with an enemy deals damage and applies <color=#FF99F3>On-Hit Effects</color>. <color=#AFEDFF>Angular speed</color> scales with <color=#AFEDFF>Bullet Speed";
    }
    public string getCaps()
    {
        return string.Format("Orbits: {0} (Max. 4)<br>Damage: {1}", amount, damage);
    }

    public string getIcon()
    {
        return "OrbitalUnlock";
    }

    public string getText()
    {
        return "Orbital Flames";
    }

    public string getType()
    {
        return "Especial Effect";
    }

    public void Stack(FlameCircle flameCircle){
        amount = Mathf.Min(flameCircle.amount + amount, 4);
        damage += flameCircle.damage;
        UpdateAmount();
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(amount == 4){
        
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("OrbitalAmount");
        }  
          
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(amount >= 4f && !Character.Instance.isACharacter()){
            StartSelectScreen();
            maxed=true;
        }
    }
    public void StartSelectScreen(){
        EnemySpawner.Instance.Paused = true;
        planetsPanel = GameUI.Instance.SpawnUI(planetsPanelPrefab);
        Transform elementContainer = planetsPanelPrefab.transform.Find("Elements");
        elementContainer.Find("Mercury").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(0));
        elementContainer.Find("Venus").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(1));
        elementContainer.Find("Earth").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(2));
        elementContainer.Find("Mars").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(3));
        elementContainer.Find("Jupiter").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(4));
        elementContainer.Find("Saturn").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(5));
        elementContainer.Find("Uranus").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(6));
        elementContainer.Find("Neptune").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(7));
    }
    public void TransformIntoCharacter(int n){

        planetsPanel.GetComponent<Animator>().Play("ExitOptions");
        PlanetType = n;
        switch(n){
            case 0: Character.Instance.SetupCharacter("OrbitalMercury"); break;
            case 1: Character.Instance.SetupCharacter("OrbitalVenus"); break;
            case 2: Character.Instance.SetupCharacter("OrbitalEarth"); break;
            case 3: Character.Instance.SetupCharacter("OrbitalMars"); break;
            case 4: Character.Instance.SetupCharacter("OrbitalJupiter"); break;
            case 5: Character.Instance.SetupCharacter("OrbitalSaturn"); break;
            case 6: Character.Instance.SetupCharacter("OrbitalUranus"); break;
            case 7: Character.Instance.SetupCharacter("OrbitalNeptune"); break;
        }
        
    }
    public void SpawnExtraAssets(int n){
        switch(n){
            case 0:SpinnerInstance.multiplier *= 1.5f; break;
            case 1:break;
            case 2:Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle Earth")).GetComponent<Spinner>();break;
            case 3:SpinnerInstance.GetComponent<Animator>().enabled=true;break;
            case 4:
                Spinner next = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle Jupiter")).GetComponent<Spinner>();
                SpinnerInstance.kill();
                SpinnerInstance = next;
            break;
            case 5:
                Spinner next2 = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle Saturn")).GetComponent<Spinner>();
                SpinnerInstance.kill();
                SpinnerInstance = next2;
            break;
            case 6:break;
            case 7: SpinnerInstance.multiplier *= .25f;break;
        }
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class MoneyMultipliers : NotEspecificEffect
{
    public static MoneyMultipliers Instance;

    public float mult;
    public int perRound;
    public float ShinyChance;
    public float ShinyMultiplier;
    public MoneyMultipliers(int perRound, float mult){
        this.mult = mult;
        this.perRound = perRound;
        
        if(Instance == null){
            Instance = this;
            mult = Math.Max(1, mult);
            Deck.RoundOver += GiveEmbersRound;
            ReloadShinyStats();
            return;

        }else{
            Instance.Stack(this);
        }  
    }
    public void ReloadShinyStats(){
        switch(SkillTreeManager.Instance.getLevel("Ember Generation")){
            case 1:
                ShinyChance = 0.0001f;
                ShinyMultiplier = 10;
                break;
            case 2:
                ShinyChance = 0.001f;
                ShinyMultiplier=100;
                break;
        }
        if(Character.Instance.isCharacter("Ember Generation")){
            ShinyChance = 0.01f;
            ShinyMultiplier=100;
        }
    }
    public void GiveEmbersRound(object sender ,EventArgs e){
        Flamey.Instance.addEmbers(perRound);
    }
    public void Stack(MoneyMultipliers moneyMultipliers){
        perRound+=moneyMultipliers.perRound;
        mult+=moneyMultipliers.mult;
        RemoveUselessAugments();
        
    }
    public bool maxed;
    private void CheckMaxed(){
        if(mult >= 2f && perRound >= 250 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Ember Generation");
        }
    }
    private void RemoveUselessAugments(){ 
        if(!maxed){CheckMaxed();}
    }
    public bool addList()
    {
        return Instance == this;
    }

    public void ApplyEffect()
    {
        return;
    }

    public string getDescription()
    {
        
        return "Multiply your <color=#FFCC7C>ember</color> gains and passively win some each round. You can check the <color=#FFFF00>Bestiary</color> for more info on enemy specific drop rates.";
    }
    public string getCaps()
    {
        if(ShinyChance > 0){
            return string.Format("Embers /Round: +{0}<br>Multiplier: x{1}<br>Shiny Spawn Chance: {2}%<br>Shiny Ember Multiplier: x{3}", perRound, Mathf.Round(mult*100)*0.01f, (ShinyChance*100).ToString("F2"), ShinyMultiplier);
        }
        return string.Format("Embers /Round: +{0}<br>Multiplier: x{1}", perRound, Mathf.Round(mult*100)*0.01f);
    }

    public string getIcon()
    {
        return "MoneyUnlock";
    }

    public string getText()
    {
        return "Ember Generation";
    }

    public string getType()
    {
        return "Especial Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class CandleTurrets : NotEspecificEffect
{
    public int dmg;
    public float atkSpeed;
    public int amount;

    public static CandleTurrets Instance;
    public static GameObject CandleCircle;

    UnityEngine.UI.Image cooldownImage;
    
    public CandleTurrets(int dmg, float atkSpeed, int amount){
       
        this.amount = amount;
        this.dmg = dmg;
        this.atkSpeed = atkSpeed;
        if(Instance == null){
            Instance = this;
            this.amount *= SkillTreeManager.Instance.getLevel("Ritual") >= 2 ? 2 : 1;     
            StartCandleCircle();
            UpdateAmount();
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        UpdateAmount();
    }

    public void StartCandleCircle(){
        CandleCircle = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/CandleCircle"));
        
    }
    public void UpdateAmount(){
        
        for(int i = 0; i < amount; i++){
           
            CandleCircle.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return "Lit <color=#FFCC7C>candles</color> stand around you shooting at random targets. Their hits will <color=#FF5858>not</color> apply <color=#FF99F3>On-Hit</color> nor <color=#FF99F3>On-Land Effects";
    }
    public string getCaps()
    {
        return string.Format("Candle Amount: {0} (Max. 6)<br>Damage: +{1} <br>Attack Speed: {2}/s (Max. 3/s)", amount, dmg, Mathf.Round(atkSpeed * 100f) * 0.01f);
    }

    public string getIcon()
    {
        return "CandleUnlock";
    }

    public string getText()
    {
        return "Candle Turrets";
    }

    public string getType()
    {
        return "Especial Effect";
    }

    public void Stack(CandleTurrets candleTurrets){
        
        dmg += candleTurrets.dmg;
        atkSpeed += candleTurrets.atkSpeed;
        amount += candleTurrets.amount * (SkillTreeManager.Instance.getLevel("Ritual") >= 2 ? 2 : 1);
        RemoveUselessAugments();
    }


    private void RemoveUselessAugments(){
       
        if(amount >= 6){
            amount = 6;
            Deck.Instance.removeClassFromDeck("CandleAmount");
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("CandleAtkSpeed");
        } 
          
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(amount >= 6f && atkSpeed >= 3f && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Ritual");
            maxed = true;
        }
    }
    public void SpawnExtraAssets(){
        CandleCircle.transform.GetChild(6).gameObject.SetActive(true);
        cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/CandleDmg"));
    }


    private int damageTicks;
    public void AddDamageTick(){
        damageTicks++;

        if(damageTicks > 100){
            damageTicks = 0;
            dmg+=5;
        }
        cooldownImage.fillAmount = ((float)damageTicks)/100;
    }

    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class Summoner : NotEspecificEffect
{
    public int dmg;
    public float atkSpeed;
    public float speed;
    public int amount;

    public List<Bee> bees;
    public static Summoner Instance;
    public static GameObject Bee;
    
    public Summoner(int dmg, float atkSpeed, float speed, int amount){
       
        this.amount = amount;
        this.dmg = dmg;
        this.atkSpeed = atkSpeed;
        this.speed = speed;
        if(Instance == null){
            Instance = this;     
            bees = new List<Bee>();
            Bee = Resources.Load<GameObject>("Prefab/Bee");
            ApplyEffect();
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        for(int i = bees.Count; i < amount; i++){
            bees.Add(Flamey.Instance.SpawnObject(Bee).GetComponent<Bee>());
        }

        foreach(Bee b in bees){
            b.UpdateStats();
        }
    }

    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return "<color=#FFCC7C>Bees</color> will fight by your side, targeting random enemies and applying <color=#FF99F3>On-Hit effects.";
    }
    public string getCaps()
    {
        return string.Format("Bee Amount: {0} (Max. 10)<br>Bee Damage: +{1} <br>Bee Attack Speed: {2}/s (Max. 4/s) <br>Bee Speed: {3} (Max. 4)", amount, dmg, Mathf.Round(atkSpeed *  100)/100, Mathf.Round(speed*  100)/100);
    }

    public string getIcon()
    {
        return "SummonUnlock";
    }

    public string getText()
    {
        return "Bee Summoner";
    }

    public string getType()
    {
        return "Especial Effect";
    }

    public void Stack(Summoner summoner){
        
        dmg += summoner.dmg;
        atkSpeed += summoner.atkSpeed;
        amount += summoner.amount;
        speed += summoner.speed;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(amount > 10){
            amount = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonAmount");
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonAtkSpeed");
        } 
        if(speed >= 4f){
            speed = 4f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonSpeed");
        } 
          
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(amount >= 10 && atkSpeed >= 3f && speed >= 4f && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Bee Summoner");
            maxed = true;
        }
    }

    public GameObject getAbilityOptionMenu(){
        return null;
    }
}
