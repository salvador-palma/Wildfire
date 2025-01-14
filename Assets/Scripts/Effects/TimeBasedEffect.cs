
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface TimeBasedEffect : Effect
{
    public bool addList();
    public void ApplyEffect();
    public void ApplyRound();
}


public class HealthRegen : TimeBasedEffect
{
    public static HealthRegen Instance;

    public float perSec;
    public float perRound;

    public int tickAmount;
    private Image cooldownImage;
    private int activeRoundsLeft;
    private int activeRoundsCooldown = 5;
    private GameObject pheonix;
    private GameObject pheonixPrefab;
    public HealthRegen(float perSec, float perRound){
        this.perSec = perSec;
        this.perRound = perRound;
        if(Instance == null){
            Instance = this;
            pheonixPrefab = Resources.Load<GameObject>("Prefab/AbilityCharacter/Pheonix");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        if(tickAmount <= 0){
            tickAmount = 4;
            Flamey.Instance.addHealth(perSec);
            
            
        }else{
            tickAmount--;
        }
        
    }
    public void ApplyRound()
    {
        Flamey.Instance.addHealth((int)perRound, 0);
    }

    public void Stack(HealthRegen healthRegen){
        perSec += healthRegen.perSec;
        perRound += healthRegen.perRound;
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    

    private void CheckMaxed(){
        if(perSec >= 100 && perRound >= 100 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Regeneration");
            maxed = true;
        }

    }
    public void SpawnExtraAssets(){
        pheonix = Flamey.Instance.SpawnObject(pheonixPrefab);
        cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/Regen"));
        activeRoundsLeft=activeRoundsCooldown;
        cooldownImage.fillAmount = ((float)activeRoundsLeft)/activeRoundsCooldown;

        Deck.RoundOver += UpdateActive;
    }
    public bool PassiveAvailable(){
        return Character.Instance.isCharacter("Regeneration") && activeRoundsLeft==activeRoundsCooldown;
    }
    public void ReleasePheonix(){

        cooldownImage.fillAmount = 0;
        activeRoundsLeft= 0;
        foreach (Enemy en in EnemySpawner.Instance.PresentEnemies)
        {
            en.extraAtkSpeedDelay = 10;
            en.Stun(10);
        }
        pheonix.GetComponent<Animator>().Play("Flight");
        
    }
    
    private void UpdateActive(object sender, EventArgs e){
        if(activeRoundsLeft<activeRoundsCooldown){
            activeRoundsLeft++;
            cooldownImage.fillAmount = ((float)activeRoundsLeft)/activeRoundsCooldown;
        }
    }
    
    
    public string getDescription()
    {
        return "You can regenerate <color=#0CD405>health</color> per second and gain <color=#0CD405>Max HP</color> at the end of each round";
    }
    public string getCaps()
    {
        return string.Format("Regen/s: {0}/s <br>Max HP/round: {1}/round", Mathf.Round(perSec * 100.0f) * 0.01f, Mathf.Round(perRound * 100.0f) * 0.01f);
    }

    public string getIcon()
    {
        return "Regen";
    }

    public string getText()
    {
        return "Regeneration";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class LightningEffect : TimeBasedEffect

{
    public static LightningEffect Instance;
    public static IPoolable lightning;
    public int interval;
    int current_interval;
    public int dmg;
    Image cooldownImage;

    public LightningEffect(int interval, int dmg, float percRed){
        this.dmg = dmg;
        this.interval = interval;
        if(Instance == null){
            Instance = this;
            cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/ThunderUnlock"));
            lightning = Resources.Load<GameObject>("Prefab/Lightning").GetComponent<IPoolable>();
        }else{
            Instance.Stack(this, percRed);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        current_interval--;
        
        if(current_interval <=0 ){
            current_interval = interval;
            int amount = 2;
            AudioManager.PlayOneShot(FMODEvents.Instance.Thunder, Vector2.zero);
            if(Character.Instance.isCharacter("Thunder")){amount*=2;}
            for(int i = 0; i < amount; i++)
            {
                Vector2 v = Flamey.Instance.getRandomHomingPosition();
                ObjectPooling.Spawn(lightning, new float[]{v.x, v.y + 10.91f});
                
            }
            

        }
        cooldownImage.fillAmount = 1 - ((float)current_interval)/interval;
    }
    public void ApplyRound(){}

    public void Stack(LightningEffect lightningEffect, float percRed){
        if(percRed > 0){
            if((int)Math.Floor(interval * (1-percRed)) == interval){
                interval-=1;
            }else{
                interval = (int)Math.Floor(interval * (1-percRed));
            }
            
        }
        
        dmg += lightningEffect.dmg;
        RemoveUselessAugments();
    }
    bool maxed;
    void RemoveUselessAugments(){
        if(interval <= 1){
            interval = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ThunderInterval");
        }
        if(!maxed){CheckMaxed();}
    }
    private void CheckMaxed(){
        if(interval <= 1 && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Thunder");
        }
    }
    
    public string getDescription()
    {
        return "Each few amount of seconds, <color=#FFCC7C>3 thunders</color> will spawn at a convenient location dealing <color=#FF5858>damage</color> to enemies struck by it. This ability applies <color=#FF99F3>On-Land Effects";
    }
    public string getCaps()
    {
        return string.Format("Interval: {0}s (Min 0.25s)<br>Damage: +{1}", Mathf.Round((float)interval/4 * 100.0f) * 0.01f, dmg);
    }

    public string getIcon()
    {
        return "ThunderUnlock";
    }

    public string getText()
    {
        return "Thunder";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class Immolate : TimeBasedEffect
{
    public static Immolate Instance;
    public int ImmolateType = -1; //0-Fire, 1-Water, 2-Earth, 3-Air 
    /*
        Fire - True Damage
        Water - Healing and Cleansing
        Earth - Shield
        Air - Small Stun
    */
    public GameObject[] ImmolateRings;
   
    public static GameObject ring;
    public int interval;
    int current_interval;
    public int dmg;
    public float radius;
    private Image cooldownImage;
    private GameObject elementsPanelPrefab;
    private GameObject[] spiritPrefab;
    private GameObject spirit;
    GameObject elementsPanel; 
    bool isCharacter;
    public Immolate(int interval, int dmg, float radius, float percRed){
        this.dmg = dmg;
        this.interval = interval;
        this.radius = radius;
        if(Instance == null){
            Instance = this;
            cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/ImmolateUnlock"));
            ring = Resources.Load<GameObject>("Prefab/Ring");
            ImmolateRings = new GameObject[4];
            ImmolateRings[0] = Resources.Load<GameObject>("Prefab/AbilityCharacter/FireRing");
            ImmolateRings[1] = Resources.Load<GameObject>("Prefab/AbilityCharacter/WaterRing");
            ImmolateRings[2] = Resources.Load<GameObject>("Prefab/AbilityCharacter/EarthRing");
            ImmolateRings[3] = Resources.Load<GameObject>("Prefab/AbilityCharacter/AirRing");
            spiritPrefab = new GameObject[4];
            spiritPrefab[0] = Resources.Load<GameObject>("Prefab/AbilityCharacter/SpiritFire");
            spiritPrefab[1] = Resources.Load<GameObject>("Prefab/AbilityCharacter/SpiritWater");
            spiritPrefab[2] = Resources.Load<GameObject>("Prefab/AbilityCharacter/SpiritEarth");
            spiritPrefab[3] = Resources.Load<GameObject>("Prefab/AbilityCharacter/SpiritAir");

            elementsPanelPrefab = Resources.Load<GameObject>("Prefab/AbilityCharacter/ElementSelectionPanel");
            
            isCharacter = Character.Instance.isCharacter("Immolate");


        }else{
            Instance.Stack(this, percRed);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect()
    {
        current_interval--;
        
        if(current_interval <=0 ){
            
            if(isCharacter){
                spirit.GetComponent<Animator>().Play("SpiritShoot");
                current_interval = interval;  
            }else{
                current_interval = interval;  
                ShootImmolate();
            }
            

        }
        cooldownImage.fillAmount = 1 - ((float)current_interval)/interval;
    }
    public void ShootImmolate(){
        AudioManager.PlayOneShot(FMODEvents.Instance.Immolate, Vector2.zero);
        Flamey.Instance.SpawnObject(ImmolateType == -1 ? ring : ImmolateRings[ImmolateType]);    
    }
    public void ApplyRound(){}

    public void Stack(Immolate immolate, float percRed){
        if(percRed > 0){
            if((int)Math.Floor(interval * (1-percRed)) == interval){
                interval-=1;
            }else{
                interval = (int)Math.Floor(interval * (1-percRed));
            }
            
        }
        
        dmg += immolate.dmg;
        radius += immolate.radius;
        RemoveUselessAugments();
    }

    void RemoveUselessAugments(){
        if(interval <= 16 && SkillTreeManager.Instance.getLevel("Immolate") < 2){
            interval = 16;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ImmolateInterval");
        }else if(interval <= 8 && SkillTreeManager.Instance.getLevel("Immolate") >= 2) {
            interval = 8;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ImmolateInterval");
        }
        if(radius >= 2f){
            radius = 2f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ImmolateRadius");
        }
        if(!maxed){CheckMaxed();}
    }
    public bool maxed;
    private void CheckMaxed(){
        if(interval <= 8 && SkillTreeManager.Instance.getLevel("Immolate") >= 2 && radius >= 2f && !Character.Instance.isACharacter()){
            StartSelectScreen();
            maxed = true;
        }else if(interval <= 16 && SkillTreeManager.Instance.getLevel("Immolate") < 2 && radius >= 2f && !Character.Instance.isACharacter()){
            StartSelectScreen();
            maxed = true;
        }
    }
    public void StartSelectScreen(){
        EnemySpawner.Instance.Paused = true;
        elementsPanel = GameUI.Instance.SpawnUI(elementsPanelPrefab);
        Transform elementContainer = elementsPanel.transform.Find("Elements");
        elementContainer.Find("Fire").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(0));
        elementContainer.Find("Water").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(1));
        elementContainer.Find("Earth").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(2));
        elementContainer.Find("Air").GetComponent<Button>().onClick.AddListener(()=>TransformIntoCharacter(3));
    }
    public void TransformIntoCharacter(int n){

        elementsPanel.GetComponent<Animator>().Play("ExitOptions");
        ImmolateType = n;
        switch(n){
            case 0: Character.Instance.SetupCharacter("ImmolateFire"); break;
            case 1: Character.Instance.SetupCharacter("ImmolateWater"); break;
            case 2: Character.Instance.SetupCharacter("ImmolateEarth"); break;
            case 3: Character.Instance.SetupCharacter("ImmolateAir"); break;
        }
        
    }
    public void SpawnExtraAssets(int n = -1){
        if(n != -1){ImmolateType = n;}
        isCharacter=true;
        if(elementsPanel!=null){elementsPanel.SetActive(false);}
        spirit = Flamey.Instance.SpawnObject(spiritPrefab[ImmolateType]);
    }

    public string getDescription()
    {
        return "Each few amount of seconds, you will release a <color=#FFCC7C>wave of energy</color> that travels through the campsite dealing <color=#FF5858>damage</color> to enemies caught by it and ignoring <color=#919191>Armor</color> completely";
    }
    public string getCaps()
    {
        return string.Format("Interval: {0}s (Min. 2s)<br>Travel Radius: {1} units (Max 200 units)<br>Damage: +{2}", Mathf.Round((float)interval/4 * 100.0f) * 0.01f, Mathf.Round(radius*100), dmg);
    }

    public string getIcon()
    {
        return "ImmolateUnlock";
    }

    public string getText()
    {
        return "Immolate";
    }

    public string getType()
    {
        return "Time-based Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

