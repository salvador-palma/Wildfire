using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FMOD.Studio;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class Flamey : MonoBehaviour
{
    public static Flamey Instance {get; private set;}

    [Header("Stats")]
    public int MaxHealth = 250;
    public float Health;
    public float Shield;
    public int Dmg= 50;

    public int Armor = 0;
    public float ArmorPen = 0;
    public int Embers = 0;
    public List<OnHitEffects> onHitEffects;
    public List<OnShootEffects> onShootEffects;
    public List<NotEspecificEffect> notEspecificEffects;
    public List<OnLandEffect> onLandEffects;
    public List<OnHittedEffects> onHittedEffects;
    public List<OnKillEffects> onKillEffects;
    public List<TimeBasedEffect> timedEffects;
    public List<Effect> allEffects;

    [Range(5f, 20f)] public float BulletSpeed;
    [Range(0f, 100f)] public float accuracy;
    [SerializeField]public float atkSpeed = 0.75f;
    float accUpdate;
    [HideInInspector] public float Accuracy;
    private float AtkSpeed;
    //===========

    //ATK SPEED TIMERS
    private float timerAS;
    private float timerASCounter;
    //===========
    [Header("Target")]
    [SerializeField] public Enemy current_homing;

    
    

    [Header("References")]
    private Animator anim;
    [SerializeField]private Slider HealthSlider;
    [SerializeField]private Slider ShieldSlider;
    

    public bool GameEnd;

    [Header("Status Effects")]
    public float stunTimeLeft;
    public int poisonsLeft;
    public bool Unhittable;

    //FINAL STATS COUNTERS
    [Header("Final Stats")]
    [HideInInspector] public int TotalKills;
    [HideInInspector] public int TotalShots;
    [HideInInspector] public ulong TotalDamage;
    [HideInInspector] public ulong TotalDamageTaken;
    [HideInInspector] public ulong TotalHealed;


    [SerializeField] public GameObject VisualDebug;

    [Header("Flare Settings")]
    [SerializeField] GameObject DefaultFlarePrefab;
    [SerializeField] GameObject DefaultFlareSpotPrefab;
    public static LayerMask EnemyMask;
    [SerializeField] public FlareType[] FlareTypes;


  
    private float secondTimer = 0.25f;
    private float tick = 0.25f;
    private int tickNumber;

    [Header("Global Settings")]
    public static float PoisonDrainPerc = 1/25f;

    [ContextMenu("Call Extra Function")]
    void ExtraFunction()
    {
        Hitted(50, 0.5f, null, onhitted:false);
    }

    private void Awake() {
        
        Health = MaxHealth;
       
        Instance = this;
        anim = GetComponent<Animator>();
        onHitEffects = new List<OnHitEffects>();
        onShootEffects = new List<OnShootEffects>();
        notEspecificEffects = new List<NotEspecificEffect>();
        allEffects = new List<Effect>();
        onLandEffects = new List<OnLandEffect>();
        onHittedEffects = new List<OnHittedEffects>();
        onKillEffects = new List<OnKillEffects>();
        timedEffects = new List<TimeBasedEffect>();

        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.SetAmbienceParameter("Dead", 0);
        Deck.Instance.FillDeck();
        Character.Instance.SetupActiveLooks();
        

        target(getHoming());
        AtkSpeed = atkSpeed;
        timerASCounter = AtkSpeedToSeconds(atkSpeed);;
        timerAS = timerASCounter;
        
        UpdateHealthUI();
        
        EnemyMask = LayerMask.GetMask("Enemy");

        CheckBlackMarketItems();   

        
    }
   
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            timerAS = 0;
        }
        if(GameEnd || Health <= 0){return;}
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameUI.Instance.TogglePausePanel();
        }
        
        
        

        if(current_homing == null){
            Console.Log("Searching enemies...");
            target(getHoming());
            if(current_homing == null){return;}
            
        }
        
        if(stunTimeLeft > 0f){
            stunTimeLeft -= Time.deltaTime;
            if(stunTimeLeft <= 0f){
                GetComponent<Animator>().Play("Happy");
            }
        }
        else if(timerAS > 0 ){
            
            timerAS -= Time.deltaTime;
        }else{
            if(atkSpeed != AtkSpeed){
                AtkSpeed = atkSpeed;
                updateTimerAS(atkSpeed);
            }
            if(accUpdate != accuracy){
                accUpdate = accuracy;
                updateAccuracy(accuracy);
            }
            timerAS = timerASCounter;
            shoot();
        }

        if(Health <= 0){
            if(HealthRegen.Instance != null && Character.Instance.isCharacter("Regeneration") && HealthRegen.Instance.PassiveAvailable()){
                HealthRegen.Instance.ReleasePheonix();
                Unhittable = true; 
                
            }else{
                EndGame();
            }
            
        }

        secondTimer-=Time.deltaTime;
        if(secondTimer <= 0){
            secondTimer = tick;
            tickNumber++;
            if(tickNumber >= 4){
                tickNumber = 0;
                EnemySpawner.Instance.ApplyPoisonEnemies();
                if(poisonsLeft > 0){ApplyPoison();}
            }
            
            ApplyTimed();
        }

        
    }

    public void shoot(){
        if(EnemySpawner.Instance.isOnAugments || current_homing == null){return;}
        TotalShots++;
        anim.Play("FlameShoot");
        
        
        int FlameType = ApplyOnShoot();

        ObjectPooling.Spawn(DefaultFlarePrefab.GetComponent<IPoolable>(), new float[]{FlameType});
    }
    public void target(Enemy e){
        
        if(e ==null){return;}
        if(current_homing!=null){current_homing.untarget();}
        
        if(e.canTarget()){
            e.target();
            current_homing = e;
        }
        
    }

    public Flare InstantiateShot(List<string> except = null){
        TotalShots++;
        int FlameType = ApplyOnShoot(except);
        GameObject go = ObjectPooling.Spawn(DefaultFlarePrefab.GetComponent<IPoolable>(), new float[]{FlameType});
        return go.GetComponent<Flare>();
    }
    public static float AtkSpeedToSeconds(float asp){
        return 1/asp;
    }

    public Enemy getHoming(int index = 0){
        
        List<Enemy> g =  new List<Enemy>();
        g.AddRange(GameObject.FindGameObjectsWithTag("Enemy").Select( item => item.GetComponent<Enemy>()).Where(x => x.canTarget()));
        
        if(g.Count <= 0){
            return null;
        }else{
            if(index == 0 || index > g.Count()-1){
                return g.Min();
            }
            g.Sort();
            return g[index];
            
        }
    }
    public UnityEngine.Vector2 getRandomHomingPosition(){
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        try{

            GameObject g = go[UnityEngine.Random.Range(0, go.Length)];
            while(!g.GetComponent<Enemy>().canTarget()){
                g = go[UnityEngine.Random.Range(0, go.Length)];
            }
            return g.GetComponent<Enemy>().HitCenter.position;
        }catch{
            Debug.Log("Covered Error! Flamey.getRandomHomingPosition()");
        }
        return UnityEngine.Vector2.zero;
    }
    public Enemy getRandomHomingEnemy(bool targetable = false, Predicate<Enemy> predicate = null){
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");

        if(targetable){
            go = predicate==null ? 
            go.Where(i=>i.GetComponent<Enemy>().canTarget()).ToArray() : 
            go.Where(i=>i.GetComponent<Enemy>().canTarget() && predicate(i.GetComponent<Enemy>())).ToArray();
        }
        if(go.Length == 0){return null;}
        try{
            GameObject g = go[UnityEngine.Random.Range(0, go.Length)];
            
            return g.GetComponent<Enemy>();
        }catch{
            Debug.Log("Covered Error! Flamey.getRandomHomingPosition()");
        }
        return null;
    }
   
    private void updateTimerAS(float asp){
        timerASCounter = AtkSpeedToSeconds(asp * Gambling.getGambleMultiplier(2));
    }
    private void updateAccuracy(float acc){
        Accuracy = PercentageToAccuracy(acc);
    }
    private float PercentageToAccuracy(float perc){
        return 0.8f - 0.008f * perc;
    }
    public void Hitted(int Dmg, float armPen, Enemy attacker, bool onhitted = true, bool isShake=true, int idHitTxt= 2){

        if(!Unhittable){
            //CHARACTER SPECIFIC
            if(Character.Instance.isCharacter("Burst") && BurstShot.Instance != null){
                BurstShot.Instance.Burst();
            }

            Dmg = (int)(Dmg * Gambling.getGambleMultiplier(6));
            
            if(Character.Instance.isCharacter("Crit")){
                if(UnityEngine.Random.Range(0f,1f) < CritUnlock.Instance.perc){
                    Dmg = (int)Math.Max(Dmg*0.1f, -4.5f * (CritUnlock.Instance.mult -5) + Dmg);
                }
            }
            //===================

            int dmgeff = (int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-armPen))) * Dmg);
            TotalDamageTaken+=(ulong)dmgeff;

            float shieldDamage = Shield - dmgeff;
            Dmg = (int)Math.Abs(shieldDamage);
            if(shieldDamage < 0){
                Health += shieldDamage;
                DamageUI.InstantiateTxtDmg(transform.position, "-"+ Math.Abs(shieldDamage), idHitTxt);
            }
            if(Shield > 0){
                if(shieldDamage<0){
                    DamageUI.InstantiateTxtDmg(transform.position, "-"+Math.Abs(Shield), 24);
                }else{
                    DamageUI.InstantiateTxtDmg(transform.position, "-"+Math.Abs(Shield - shieldDamage), 24);
                }
                

                Shield = Math.Max(shieldDamage, 0);
            }
            
            //ACHIEVMENT/QUEST
            EnemySpawner.Instance.RoundsWithoutDamage = 0;
            if(Health < MaxHealth/2f){
                EnemySpawner.Instance.RoundsBelow50PercMaxHP = 0;
            }
            //===================
            
            if(Health <= 0){
                if(HealthRegen.Instance != null && Character.Instance.isCharacter("Regeneration") && HealthRegen.Instance.PassiveAvailable()){
                    HealthRegen.Instance.ReleasePheonix();
                    Unhittable = true; 
                }else{
                    EndGame();
                }
            }

            UpdateHealthUI();
            
            if(isShake){CameraShake.Shake(0.5f,0.20f);} 
        }
        if(onhitted){ApplyOnHitted(attacker, Dmg);}
        
    }
    private void UpdateHealthUI(){
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;

        ShieldSlider.maxValue = MaxHealth;
        ShieldSlider.value = Shield;


    }

    bool called;
    public void EndGame(){
        if(called){return;}
        called=true;
        GameEnd = true;
        EnemySpawner.Instance.GameEnd = true;
        GameUI.Instance.PausePanel.SetActive(false);
        GameState.Delete();
        GameUI.Instance.SpeedUp(1f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies){
            if(e != null){
                e.GetComponent<Enemy>().EndEnemy();
            }
        }
        int SkillTreeState = Math.Max(0,GameVariables.GetVariable("SkillTreeReady"));
        if(SkillTreeState<=2){
            GameVariables.SetVariable("SkillTreeReady",SkillTreeState+1);
        }

        if(GameVariables.hasQuest(45) && Embers >= 30000){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(45, "Gyomyo", 10);
        }
        
        
        GameUI.Instance.GameOverEffect();
    }
    

    public void addAccuracy(float amount){
        accuracy = Math.Min(accuracy + amount, 100f);
        if(accuracy == 100f){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("Acc");
        }
    }
    public void addAttackSpeed(float amount){
        atkSpeed += amount;
        if(atkSpeed >= 12f){
            if(!(Character.Instance.isCharacter("Multicaster") && amount == 0.2f)){
                atkSpeed = 12f;
            }
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("AtkSpeed");

            if(GameVariables.hasQuest(25) && SecondShot.Instance != null && SecondShot.Instance.perc>=1f){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(25, "Rowl", 15);
            }
        }
    }
    public void multAttackSpeed(float amount){
        atkSpeed = Math.Min(atkSpeed * amount, 12f);
        if(atkSpeed == 12f){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("AtkSpeed");
        }
    }

    public void addBulletSpeed(float amount){
        BulletSpeed = Math.Min(BulletSpeed + amount, 20f);
        if(BulletSpeed == 20f){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BltSpeed");
            
            if(allEffects.Find(x => x.getText() == "Orbits") != null){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(36,"Betsy",15);
            }
        }
    }

    public void multAccuracy(float amount){
        accuracy = Math.Min(accuracy * amount, 100f);
        if(accuracy >= 100f){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("Acc");
        }
    }

    public void addDmg(int amount){Dmg += amount;}
    public void multDmg(int amount){Dmg *= amount;}
  
    public void addDmg(float amount){Dmg += (int)amount;}
    public void multDmg(float amount){Dmg += (int)amount;}

    public void addArmor(int amount){Armor += (int)amount;}
    public void addArmorPen(float amount){  
        ArmorPen += amount;      
        if(ArmorPen >= 0.8){
            ArmorPen = 0.8f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ArmorPen");
        }      
    
    }

    public void addHealth(int max_increase, float healperc){
        MaxHealth += max_increase;
        if(HealthRegen.Instance != null && SkillTreeManager.Instance.getLevel("Regeneration") >= 1 && Health > 0){
            healperc *= 2;
        } 
        if(IceOnHit.Instance != null && IceOnHit.Instance.getSlowPerc() >= .75f && GameVariables.hasQuest(28)){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(28, "Cloris", 10);
        }

        Health = (int)Math.Min(Health + MaxHealth * healperc,MaxHealth);
        TotalHealed+=(ulong)(MaxHealth * healperc);
        UpdateHealthUI();
        DamageUI.InstantiateTxtDmg(transform.position,""+ MaxHealth * healperc, 3);
    }

    private EventInstance healthInstance;
    public void addHealth(float HealAmount){
        if(HealthRegen.Instance != null && Health > 0){
            if(SkillTreeManager.Instance.getLevel("Regeneration") >= 1){
                HealAmount *= 2;
            }
            if(SkillTreeManager.Instance.getLevel("Regeneration") >= 2){
                float factor = (float)Math.Clamp(Math.Pow(Health-MaxHealth,2)/Math.Pow(MaxHealth,2) + 1, 1, 2);
                HealAmount *= factor;
            }
        } 

        if(Character.Instance.isCharacter("Vampire") && Health + HealAmount > MaxHealth){
            float AmountOverhealed = Health + HealAmount - MaxHealth;
            HealAmount -= AmountOverhealed;
            VampOnHit.Instance.OverHeal(AmountOverhealed);
        }
        
        if(MaxHealth==Health){return;}

        TotalHealed+=(ulong)HealAmount;
        Health = Math.Min(Health + HealAmount, MaxHealth);
        UpdateHealthUI();

        AudioManager.Instance.PlayHealingSound();
        

        DamageUI.InstantiateTxtDmg(transform.position, ""+ Mathf.Round(HealAmount * 10.0f) * 0.1f, 3);


        //VON VAN PYRE QUEST
        if(VampOnHit.Instance != null && GameVariables.hasQuest(21)){
            if(HealingTimer == -1){
                HealingTimer = Time.time;
            }else if(Time.time - HealingTimer <= 1f){
                HealedHealth+=HealAmount;
                   
                if(HealedHealth>=1000){
                    GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(21,"Betsy",24);
                }
            }else{
                HealingTimer = Time.time;
                if(HealedHealth!=0){Debug.Log("Avg Healed: " + HealedHealth);}
                HealedHealth = 0;
            }
        }
    }
    private float HealingTimer = -1;
    private float HealedHealth;

    public void addShield(float amount){
        Shield = Math.Min(MaxHealth * 0.3f , Shield + amount);
        UpdateHealthUI();
        if((int)Shield < (int)(MaxHealth * 0.3f)){DamageUI.InstantiateTxtDmg(transform.position, ""+ Mathf.Round(amount * 10.0f) * 0.1f, 25);}
        
    }

    public void Stun(float t){

        if(Character.Instance.isCharacter("Snow Pool")){
            Debug.Log("Blocked Stun");
            return;
        }

        if(stunTimeLeft > 0){return;}
        GetComponent<Animator>().Play("Stunned");
        stunTimeLeft = t;
    }
    public void Poison(int amount){
        poisonsLeft += amount;
    }
    public void ApplyPoison(){
        poisonsLeft--;
        AudioManager.PlayOneShot(FMODEvents.Instance.PoisonPop, Vector2.zero);
        Hitted((int)Math.Max(1,Health*PoisonDrainPerc), 1, null, onhitted:false, isShake:false, idHitTxt:14);
    }
    public void addOnHitEffect(OnHitEffects onhit){
        if(onhit.addList()){
            onHitEffects.Add(onhit);
            allEffects.Add(onhit);
        } 
    }
    public void addOnShootEffect(OnShootEffects onhit){
        if(onhit.addList()){
            onShootEffects.Add(onhit);
            allEffects.Add(onhit);
        }
    }
    public void addNotEspecificEffect(NotEspecificEffect onhit){
        if(onhit.addList()){
            notEspecificEffects.Add(onhit);
            allEffects.Add(onhit);
        }
    }
    public void addOnLandEffect(OnLandEffect onhit){
        if(onhit.addList()){
            onLandEffects.Add(onhit);
            allEffects.Add(onhit);
        }
    }
    public void addOnHittedEffect(OnHittedEffects onhit){
        if(onhit.addList()){
            onHittedEffects.Add(onhit);
            allEffects.Add(onhit);
        }
    }
    public void addOnKillEffect(OnKillEffects onhit){
        if(onhit.addList()){
            onKillEffects.Add(onhit);
            allEffects.Add(onhit);
        }
    }
    public void addTimeBasedEffect(TimeBasedEffect onhit){
        if(onhit.addList()){
            timedEffects.Add(onhit);
            allEffects.Add(onhit);
        }
    }


    public int ApplyOnHit(float d, float h, Enemy e, string except = null){
        if(e.Health<=0){return 0;}

        int amountOfOnHit=0;
        foreach (OnHitEffects oh in onHitEffects){
            if(oh.getText() == except){continue;}
            amountOfOnHit += oh.ApplyEffect(d,h,e);
        }
        return amountOfOnHit;
    }
    public int ApplyOnShoot(List<string> except = null){
        int res = 0;
        foreach (OnShootEffects oh in onShootEffects){
            if(except==null ||  !except.Contains(oh.getText())){
                res += oh.ApplyEffect();
            }
            
        }
        return res;
    }
    public void ApplyOnLand(UnityEngine.Vector2 pos){
       
        foreach (OnLandEffect oh in onLandEffects){oh.ApplyEffect(pos);}
    }
    public void ApplyOnHitted(Enemy e, int dmg){
        if(e.Health<=0){return;}
        foreach (OnHittedEffects oh in onHittedEffects){oh.ApplyEffect(e, dmg);}
    }

    public void ApplyOnKill(Vector2 pos){
        
        foreach (OnKillEffects oh in onKillEffects){oh.ApplyEffect(pos);}
    }

    public void ApplyTimed(){
        foreach (TimeBasedEffect oh in timedEffects){oh.ApplyEffect();}
    }
    public void ApplyTimedRound(){
        foreach (TimeBasedEffect oh in timedEffects){oh.ApplyRound();}
    }
    public GameObject SpawnObject(GameObject go){
        return Instantiate(go);
    }
    public static void DeSpawnObject(GameObject go){
        Destroy(go);
    }
    public void CallCoroutine(IEnumerator c){
        StartCoroutine(c);
    }
    public void callFunctionAfter(UnityAction a, float f){
       StartCoroutine(callFunctionAfterCoroutine(a,f));
    }
    public IEnumerator callFunctionAfterCoroutine(UnityAction a, float f){
        yield return new WaitForSeconds(f);
        a();
    }
   
   


    public List<SimpleStat> getBaseStats(){
        return new List<SimpleStat>
        {
            new SimpleStat("Enemies killed", TotalKills),
            new SimpleStat("Fireballs shot", TotalShots),
            new SimpleStat("Damage given", (int)TotalDamage),
            new SimpleStat("Damage taken", (int)TotalDamageTaken),
            new SimpleStat("Healed health", (int)TotalHealed)

        };
    }

    public void addEmbers(int n){
        if(MoneyMultipliers.Instance != null){
            n = (int)(n * MoneyMultipliers.Instance.mult);
        }
        Embers += n;
        if(Bullets.Instance != null){Bullets.Instance.EmbersInRound += n;}
        GameUI.Instance.SetEmberAmount(Embers);
        
        
    }
    public int removeEmbers(int n){
        int removed = Math.Min(Embers, n);
        Embers = Math.Max(0, Embers - n);
        GameUI.Instance.SetEmberAmount(Embers);
        return removed;
    }

    [HideInInspector] public float BegginingHP;
    [HideInInspector] public float BegginingAS;
    [HideInInspector] public float BegginingDMG;
    [HideInInspector] public float BegginingACC;
    void CheckBlackMarketItems(){
        
        Debug.Log("CHECKING ITEMS...");
        MaxHealth=500;
        Health=500;
        string[] HealthItems = new string[]{"Soggy Logs", "Dry Logs", "High-Quality Log Pack"};
        int[] HealthGiven = new int[]{250,250,500};
        int j = 0;
        foreach(string HealthItem in HealthItems){if(Item.has(HealthItem)){MaxHealth+=HealthGiven[j];Health+=HealthGiven[j];}j++;}

        atkSpeed = .75f;
        string[] AtkSpeedItems = new string[]{"Leaf Basket", "Twig Basket", "Pine Cone Basket", "Giant Pinecone"};
        float[] AtkSpedGiven = new float[]{.25f,0.25f,0.25f,0.5f};
        int c = 0;
        foreach(string AtkSpeedItem in AtkSpeedItems){if(Item.has(AtkSpeedItem)){atkSpeed += AtkSpedGiven[c];}c++;}

        Dmg = 30;
        string[] DmgItems = new string[]{"Old Stolen Fuel Bucket", "Old Petrol Tank", "5L Petrol Tank", "10L Premium Gas Tank"};
        int[] DmgValues = new int[]{10,10,20,30};
        for(int i = 0; i != DmgItems.Length; i++){if(Item.has(DmgItems[i])){Dmg+=DmgValues[i];}}


        string[] AccItems = new string[]{"Ash Bag", "Enchanted Ashes", "Ancient Ashes"};
        float[] AccValues = new float[]{15f, 15f, 20f};
        for(int i = 0; i != AccItems.Length; i++){if(Item.has(AccItems[i])){accuracy+=AccValues[i];}}

        BegginingHP = MaxHealth;
        BegginingAS = atkSpeed;
        BegginingDMG = Dmg;
        BegginingACC = accuracy;
    }

    
    
}
