using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

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
            Spinner.multiplier=9f;
            if(SkillTreeManager.Instance.getLevel("Orbits")>=2){this.amount=2;}
            GameObject g = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle "+this.amount));
            planetsPanelPrefab = Resources.Load<GameObject>("Prefab/AbilityCharacter/PlanetSelectionPanel");
            SpinnerInstance = g.GetComponent<Spinner>();

            if(SkillTreeManager.Instance.getLevel("Orbits")>=1){
                Spinner.multiplier*=2f;
            }
            
        }else{
            Instance.Stack(this);
        }
    }

    public void ApplyEffect()
    {
        SpinnerInstance.speed = Flamey.Instance.BulletSpeed * Gambling.getGambleMultiplier(1);
        
        
    }
  
    public void UpdateAmount(){
         
        
        Spinner next = Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle "+amount)).GetComponent<Spinner>();
        SpinnerInstance.kill();
        SpinnerInstance = next;
        
        
            
        
    }
    public bool addList()
    {
        return Instance == this;
    }

    public string getDescription()
    {
        return "Flames orbit around you in a circle. Colliding with an enemy deals damage and applies <color=#FF99F3>On-Hit Effects</color>. <color=#AFEDFF>Angular speed</color> scales with <color=#AFEDFF>Bullet Speed";
    }
    public string[] getCaps()
    {
        return new string[]{"Orbits: {0} (Max. 4)<br>Damage: {1}", amount.ToString(), damage.ToString()};
    }

    public string getIcon()
    {
        return "OrbitalUnlock";
    }

    public string getText()
    {
        return "Orbits";
    }

    public string getType()
    {
        return "Special Effect";
    }

    public void Stack(FlameCircle flameCircle){

        if(flameCircle.amount>0){
            if(SkillTreeManager.Instance.getLevel("Orbits")>=2){
                amount = Mathf.Min(flameCircle.amount + 1 + amount, 4);
            }else{
                amount = Mathf.Min(flameCircle.amount + amount, 4);
            }
            UpdateAmount();
        }
        
        
        damage += flameCircle.damage;
        
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(amount == 4){
        
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("OrbitalAmount");
        
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(41,"Betsy",14);
        }  
          
        
    }
   
    public void SpawnExtraAssets(int n){
        PlanetType = n;
        Debug.Log("SpawningExtra");
        switch(n){
            case 0:Spinner.multiplier *= 1.5f; break;
            case 1:break;
            case 2:Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle Earth")).GetComponent<Spinner>();break;
            case 3:break;
            case 4:
                UpdateAmount();
            break;
            case 5:
                Flamey.Instance.SpawnObject(Resources.Load<GameObject>("Prefab/Flame Circle Saturn")).GetComponent<Spinner>();break;
            case 6:
                SpinnerInstance.GetComponent<Animator>().enabled=true;
            break;
            case 7: Spinner.multiplier *= .25f;break;
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
        EnemySpawner.Instance.ShinyChance = 0.00001f;
        switch(SkillTreeManager.Instance.getLevel("Ember Generation")){
            case 1:
                EnemySpawner.Instance.ShinyChance *= 10;
                EnemySpawner.Instance.ShinyMultiplier=10;
                break;
            case 2:
                EnemySpawner.Instance.ShinyChance *= 100;
                EnemySpawner.Instance.ShinyMultiplier=100;
                break;
        }
        if(Character.Instance.isCharacter("Ember Generation")){
            EnemySpawner.Instance.ShinyChance *= 10;
            EnemySpawner.Instance.ShinyMultiplier*=10;
        }
    }
    public void GiveEmbersRound(object sender ,EventArgs e){
        Flamey.Instance.addEmbers(perRound);
    }
    public void Stack(MoneyMultipliers moneyMultipliers){
        perRound+=moneyMultipliers.perRound;
        mult+=moneyMultipliers.mult;
        
        
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
    public string[] getCaps()
    {
        if(EnemySpawner.Instance.ShinyChance > 0){
            return new string[]{"Embers /Round: +{0}<br>Multiplier: x{1}<br>Shiny Spawn Chance: {2}%<br>Shiny Ember Multiplier: x{3}", perRound.ToString(), (Mathf.Round(mult*100)*0.01f).ToString(), (EnemySpawner.Instance.ShinyChance*100).ToString("F2"), EnemySpawner.Instance.ShinyMultiplier.ToString()};
        }
        return new string[]{"Embers /Round: +{0}<br>Multiplier: x{1}", perRound.ToString(), (Mathf.Round(mult*100)*0.01f).ToString()};
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
        return "Special Effect";
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
    public string[] getCaps()
    {
        return new string[]{"Candle Amount: {0} (Max. 6)<br>Damage: +{1} <br>Attack Speed: {2}/s (Max. 3/s)", amount.ToString(), dmg.ToString(), (Mathf.Round(atkSpeed * 100f) * 0.01f).ToString()};
    }

    public string getIcon()
    {
        return "CandleUnlock";
    }

    public string getText()
    {
        return "Ritual";
    }

    public string getType()
    {
        return "Special Effect";
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
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(10, "Naal", 11);
        } 
        if(atkSpeed >= 3f){
            atkSpeed = 3f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("CandleAtkSpeed");
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
    public GameObject[] BeeTypes;

    private int activeRoundsLeft;
    private int activeRoundsCooldown = 5;
    private UnityEngine.UI.Image cooldownImage;
    
    public Summoner(int dmg, float atkSpeed, float speed, int amount){
        
        this.amount = amount;
        this.dmg = dmg;
        this.atkSpeed = atkSpeed;
        this.speed = speed;
        if(Instance == null){
            if(SkillTreeManager.Instance.getLevel("Bee Summoner") >= 2){this.amount*=2;}
            Instance = this;     
            bees = new List<Bee>();
            BeeTypes = new GameObject[]{
                Resources.Load<GameObject>("Prefab/WorkerBee"),
                Resources.Load<GameObject>("Prefab/PuncherBee"), //
                Resources.Load<GameObject>("Prefab/AssassinBee"), //
                Resources.Load<GameObject>("Prefab/AgileBee"), //
                Resources.Load<GameObject>("Prefab/WarriorBee"), //
                Resources.Load<GameObject>("Prefab/PollinatorBee"), //
                Resources.Load<GameObject>("Prefab/ChemicalBee"), //
            };
            for(int i =0; i!=this.amount; i++){
                 Bee b = Flamey.Instance.SpawnObject(BeeTypes[0]).GetComponent<Bee>();
                b.UpdateStats();
                bees.Add(b);
            }
           
            ApplyEffect();
        }else{
            Instance.Stack(this);
        }
    }
    public void addBee(int amount, int type){
        int max = 14;
        if(SkillTreeManager.Instance.getLevel("Bee Summoner") >= 2){amount*=2;}
        if(amount + this.amount > 14){
            if(Character.Instance.isCharacter("Bee Summoner")){
                this.amount += amount;
                bees.Add(Flamey.Instance.SpawnObject(BeeTypes[type]).GetComponent<Bee>());
                RemoveUselessAugments();
                return;
            }else{
                amount = max - this.amount;
            }
            
        }
        for(int i = 0;i<amount;i++){
            Bee b = Flamey.Instance.SpawnObject(BeeTypes[type]).GetComponent<Bee>();
            b.UpdateStats();
            bees.Add(b);
        }
        this.amount += amount; 

        //CHECK IF HAS EVERY BEE
        HashSet<string> allTypes = new HashSet<string>();
        foreach(GameObject go in BeeTypes){
            allTypes.Add(go.GetComponent<Bee>().Type);
        }
        foreach (Bee b in bees)
        {
            string t = b.Type;
            if(allTypes.Contains(t)){
                allTypes.Remove(t);
            }
        }
        Debug.Log("Left Types: ");
        allTypes.ToList().ForEach(E => Debug.Log(E));
        if(allTypes.Count == 0){
           GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(44, "Betsy", 26);
        }
        RemoveUselessAugments();
    }
    public void ApplyEffect()
    {
        RoundUpdate();
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
    public string[] getCaps()
    {
        return new string[]{"Bee Amount: {0} (Max. 14)<br>Bee Damage: +{1} <br>Bee Attack Speed: {2}/s (Max. 4/s) <br>Bee Speed: {3} (Max. 4)", amount.ToString(), dmg.ToString(), (Mathf.Round(atkSpeed *  100)/100).ToString(), (Mathf.Round(speed*  100)/100).ToString()};
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
        return "Special Effect";
    }

    public void Stack(Summoner summoner){
        
        dmg += summoner.dmg;
        atkSpeed += summoner.atkSpeed;
        speed += summoner.speed;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if( amount >= 14){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonAmount");
            deck.removeClassFromDeck("SummonAmountExtra");
        } 
        if(atkSpeed >= 4f){
            atkSpeed = 4f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonAtkSpeed");
        } 
        if(speed >= 4f){
            speed = 4f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SummonSpeed");
        } 
          
       
    }
    
    public void SpawnExtraAssets(){
        cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/SummonAmount"));
    }
    public void RoundUpdate(){
        if(cooldownImage !=null){

        
            if(activeRoundsLeft<activeRoundsCooldown){
                activeRoundsLeft++;
                if(activeRoundsLeft==activeRoundsCooldown){
                    activeRoundsLeft=0;
                    addBee(1,0);
                }
                cooldownImage.fillAmount = ((float)activeRoundsLeft)/activeRoundsCooldown;
            }
        }
    }

    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class Gambling : NotEspecificEffect{
    public static Gambling Instance;

    public bool WithLuck = false;
    public int LuckType = -1;
    //LUCK TYPES
    //0 - Damage
    //1 - Blt Sped
    //2 - Atk Spd
    //3 - En Spawn
    //4 - En Health
    //5 - En Speed
    //6 - En Dmg
    public float[,] LuckMultipliers = new float[,]{
        {0.5f, 2f},
        {0.5f, 2f},
        {0.5f, 2f},
        {2f, 0.5f},
        {2f, 0.5f},
        {2f, 0.5f},
        {2f, 0.5f}
    };
    public string[] LucksTypesText = new string[]{
        "Damage",
        "Bullet Speed",
        "Attack Speed",
        "Enemy Spawn Rate",
        "Enemy Health",
        "Enemy Speed",
        "Enemy Damage"
    };
    public float LuckCombo;
    bool Gambled;

    Animator RoulleteAnim;
    GameObject LuckMeter;
    Slider LuckMeterSlider;
    DynamicText BuffType;
    DynamicText BuffTitle;
    int RoundsWithoutPick;
    public Gambling(){
        if(Instance == null){
            Instance = this;
            Deck.RoundStart += ReduceGambling;
            RoulleteAnim = GameObject.Find("RouletteWheel")?.transform.parent.parent.GetComponent<Animator>();
            LuckMeter = Resources.Load<GameObject>("Prefab/AbilityCharacter/Luck Meter UI");
            return;
        }
    }
    public void ResetInstance(){
        Instance = null;
        Deck.RoundStart -= ReduceGambling;
    }
    public void ReduceGambling(object sender, EventArgs e)
    {
        try{
            if(Character.Instance==null || !Character.Instance.isCharacter("Gambling")){return;}
            if(RoundsWithoutPick == 5){
                //EXPIRE BUFF
                
                BuffTitle.SetText("WARNING");
                BuffType.SetText("Previous Effect Expired");
                LuckType = -1;
                LuckMeterSlider.GetComponent<Animator>().Play(!WithLuck ? "Buff" : "Debuff");
                Flamey.Instance?.GetComponent<Animator>().SetInteger("ClownType", -1);
            }
            if(!Gambled){LuckCombo = Math.Clamp(LuckCombo - 0.1f, 0.25f, 0.75f); LuckMeterSlider.value = LuckCombo; RoundsWithoutPick++;}
            Gambled = false;
            Debug.Log("Luck: " + LuckCombo);
        }catch{
            Debug.Log("Error ReduceGambling: " + e.ToString());
        }
        
    }

    public static float getGambleMultiplier(int type){
        if(Instance==null || Instance.LuckType != type){return 1;}
        
        return Instance.LuckMultipliers[type, Instance.WithLuck ? 1 : 0];
    }
    
    public bool addList()
    {
        return Instance == this;
    }

    public void ApplyEffect()
    {
        return;
    }

    
    public void SpinTheWheel(){
        if(!Character.Instance.isCharacter("Gambling")){return;}
        if(RoulleteAnim==null){RoulleteAnim = GameObject.Find("RouletteWheel")?.transform.parent.parent.GetComponent<Animator>();}
        RoulleteAnim.Play("SpinTheWheel");
        RoundsWithoutPick=0;
        Gambled = true;
        LuckCombo = Math.Clamp(LuckCombo + 0.05f, 0.25f, 0.75f);
        LuckMeterSlider.value = LuckCombo;
        LuckType = Random.Range(0,7);
        
        WithLuck = Random.Range(0f, 1f) < LuckCombo;

        //Debug.Log((WithLuck ? "Buff " : "Debuff ") + LucksTypesText[LuckType] + " x" + LuckMultipliers[LuckType, WithLuck ? 1 : 0].ToString());
        BuffTitle.SetText(WithLuck ? "BUFF" : "DEBUFF");
        BuffType.SetText(LucksTypesText[LuckType] + " x{0}", new string[]{LuckMultipliers[LuckType, WithLuck ? 1 : 0].ToString()});
        LuckMeterSlider.GetComponent<Animator>().Play(WithLuck ? "Buff" : "Debuff");

        Flamey.Instance?.GetComponent<Animator>().SetInteger("ClownType", WithLuck ? 1 : -1);
    }

   

    public string getDescription()
    { 
        return "You can gamble for augments in the augment picking phase";
    }
    public string[] getCaps()
    {
        return new string[]{"No upgradable stats"};
    }

    public string getIcon()
    {
        return "GambleImprove";
    }

    public string getText()
    {
        return "Gambling";
    }

    public string getType()
    {
        return "Special Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }

    public void SpawnExtraAssets(){
        GameObject g = GameUI.Instance.SpawnUI(LuckMeter);
        LuckMeterSlider = g.GetComponent<Slider>();
        BuffType = LuckMeterSlider.transform.Find("BuffPanel").Find("Description").GetComponent<DynamicText>();
        BuffTitle = LuckMeterSlider.transform.Find("BuffPanel").Find("Type").GetComponent<DynamicText>();
    }
}