using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Flamey : MonoBehaviour
{
    public static Flamey Instance {get; private set;}

    [Header("Stats")]
    public int MaxHealth = 1000;
    public float Health;
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
    [SerializeField][Range(0.75f, 12f)] public float atkSpeed = 1;
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

    [Header("Prefabs")]
    [SerializeField] private GameObject[] FlarePrefabs;
    [SerializeField] public GameObject FlareSpotPrefab;

    [Header("References")]
    private Animator anim;
    [SerializeField]private Slider HealthSlider;
    

    public bool GameEnd;

    [Header("Status Effects")]
    private float stunTimeLeft;
    public int poisonsLeft;

    //FINAL STATS COUNTERS
    [HideInInspector] public int TotalKills;
    [HideInInspector] public int TotalShots;
    [HideInInspector] public ulong TotalDamage;
    [HideInInspector] public ulong TotalDamageTaken;
    [HideInInspector] public ulong TotalHealed;

  
    private float secondTimer = 0.25f;
    private float tick = 0.25f;
    private int tickNumber;
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
        target(getHoming());
        AtkSpeed = atkSpeed;
        timerASCounter = AtkSpeedToSeconds(atkSpeed);;
        timerAS = timerASCounter;
        
        UpdateHealthUI();
        
        FlareManager.EnemyMask = LayerMask.GetMask("Enemy");
           
    }

    // Update is called once per frame
    void Update()
    {
        if(GameEnd){return;}
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

        if(Health <= 0){EndGame();}

        secondTimer-=Time.deltaTime;
        if(secondTimer <= 0){
            secondTimer = tick;
            tickNumber++;
            if(poisonsLeft > 0  && tickNumber >= 4){tickNumber=0; ApplyPoison();}
            ApplyTimed();
        }

        
    }

    public void shoot(){
        if(EnemySpawner.Instance.isOnAugments || current_homing == null){return;}
        TotalShots++;
        anim.Play("FlameShoot");

        AudioManager.Instance.PlayFX(0,0,0.9f, 1.1f);
        int FlameType = ApplyOnShoot();

        FlareManager.InstantiateFlare(FlameType);
       // Instantiate(FlarePrefabs[FlameType]);
        
       
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
        GameObject go = FlareManager.InstantiateFlare(FlameType);
        return go.GetComponent<Flare>();
    }
    public static float AtkSpeedToSeconds(float asp){
        return 1/asp;
    }

    private Enemy getHoming(){
        
        List<Enemy> g =  new List<Enemy>();
        g.AddRange(GameObject.FindGameObjectsWithTag("Enemy").Select( item => item.GetComponent<Enemy>()).Where(x => x.canTarget()));
        
        return g.Count!=0 ? g.Min() : null;
    }
    public UnityEngine.Vector2 getRandomHomingPosition(){
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        try{
            GameObject g = go[UnityEngine.Random.Range(0, go.Length)];
            return g.GetComponent<Enemy>().HitCenter.position;
        }catch{
            Debug.Log("Covered Error! Flamey.getRandomHomingPosition()");
        }
        return UnityEngine.Vector2.zero;
    }
    public Enemy getRandomHomingEnemy(){
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        try{
            GameObject g = go[UnityEngine.Random.Range(0, go.Length)];
            return g.GetComponent<Enemy>();
        }catch{
            Debug.Log("Covered Error! Flamey.getRandomHomingPosition()");
        }
        return null;
    }
   
    private void updateTimerAS(float asp){
        timerASCounter = AtkSpeedToSeconds(asp);
    }
    private void updateAccuracy(float acc){
        Accuracy = PercentageToAccuracy(acc);
    }
    private float PercentageToAccuracy(float perc){
        return 0.8f - 0.008f * perc;
    }
    public void Hitted(int Dmg, float armPen, Enemy attacker, bool onhitted = true, bool isShake=true, int idHitTxt= 2){

        int dmgeff = (int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-armPen))) * Dmg);

        TotalDamageTaken+=(ulong)dmgeff;

        if(onhitted){ApplyOnHitted(attacker);}

        Health -= dmgeff;
        if(Health <= 0){EndGame();}
        UpdateHealthUI();
        DamageUI.InstantiateTxtDmg(transform.position, "-"+dmgeff, idHitTxt);
        if(isShake){CameraShake.Shake(0.5f,0.35f);} 
    }
    private void UpdateHealthUI(){
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;
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
        atkSpeed = Math.Min(atkSpeed + amount, 12f);
        if(atkSpeed == 12f){
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("AtkSpeed");
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
        }
    }
    public void multBulletSpeed(float amount){BulletSpeed = Math.Min(BulletSpeed * amount, 20f);}

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
        Health = (int)Math.Min(Health + MaxHealth * healperc,MaxHealth);
        TotalHealed+=(ulong)(MaxHealth * healperc);
        UpdateHealthUI();
        DamageUI.InstantiateTxtDmg(transform.position,""+ MaxHealth * healperc, 3);
    }
    public void addHealth(float HealAmount){
        
        TotalHealed+=(ulong)HealAmount;
        Health = Math.Min(Health + HealAmount, MaxHealth);
        UpdateHealthUI();
        DamageUI.InstantiateTxtDmg(transform.position, ""+ HealAmount, 3);
    }

    public void Stun(float t){
        if(stunTimeLeft > 0){return;}
        GetComponent<Animator>().Play("Stunned");
        stunTimeLeft = t;
    }
    public void Poison(int amount){
        poisonsLeft += amount;
    }
    public void ApplyPoison(){
        poisonsLeft--;
        Hitted((int)Math.Max(1,Health/25), 1, null, onhitted:false, isShake:false, idHitTxt:14);
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


    public void ApplyOnHit(float d, float h, Enemy e, string except = null){
        foreach (OnHitEffects oh in onHitEffects){
            if(oh.getText() == except){continue;}
            oh.ApplyEffect(d,h,e);
        }
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
    public void ApplyOnHitted(Enemy e){
        foreach (OnHittedEffects oh in onHittedEffects){oh.ApplyEffect(e);}
    }

    public void ApplyOnKill(Enemy e){
        foreach (OnKillEffects oh in onKillEffects){oh.ApplyEffect(e);}
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
        Embers += n;
        GameUI.Instance.SetEmberAmount(Embers);
    }
    public int removeEmbers(int n){
        int removed = Math.Min(Embers, n);
        Embers = Math.Max(0, Embers - n);
        GameUI.Instance.SetEmberAmount(Embers);
        return removed;
    }
    
}
