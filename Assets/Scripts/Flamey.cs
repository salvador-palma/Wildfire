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
    public int Health;
    public int Dmg= 50;

    public int Armor = 0;
    public float ArmorPen = 0;
    public List<OnHitEffects> onHitEffects;
    public List<OnShootEffects> onShootEffects;
    public List<NotEspecificEffect> notEspecificEffects;
    public List<OnLandEffect> onLandEffects;
    public List<Effect> allEffects;
    [SerializeField][Range(0f,100f)]public float CritChance;
    [SerializeField][Range(1f,5f)]public float CritMultiplier;
    [Range(5f, 20f)] public float BulletSpeed;
    [Range(0f, 100f)] public float accuracy;
    [Range(0.5f,3f)] public float BulletSize;
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
    private void Awake() {
        
        Health = MaxHealth;
       
        Instance = this;
        anim = GetComponent<Animator>();
        onHitEffects = new List<OnHitEffects>();
        onShootEffects = new List<OnShootEffects>();
        notEspecificEffects = new List<NotEspecificEffect>();
        allEffects = new List<Effect>();
        onLandEffects = new List<OnLandEffect>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        target(getHoming());
        AtkSpeed = atkSpeed;
        timerASCounter = AtkSpeedToSeconds(atkSpeed);;
        timerAS = timerASCounter;
        
        UpdateHealthUI();
        
        addNotEspecificEffect(new FlameCircle(4,50000));
        addOnHitEffect(new IceOnHit(10000,1f));
        

        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameEnd){return;}
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameUI.Instance.TogglePausePanel();
        }
        
        if(current_homing == null){
            target(getHoming());
            if(current_homing == null){return;}
        }

        if(timerAS > 0){
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

    }

    public void shoot(){
        anim.Play("FlameShoot");
        AudioManager.Instance.PlayFX(0,0,0.9f, 1.1f);
        ApplyOnShoot();
        Tuple<int,bool> tp = getDmg();
        GameObject go = Instantiate(FlarePrefabs[tp.Item2 ? 1 : 0]);
        go.GetComponent<Flare>().Damage = tp.Item1;
        go.GetComponent<Flare>().isCrit = tp.Item2;
       
    }
    public void target(Enemy e){
        if(e ==null){return;}
        if(current_homing!=null){current_homing.untarget();}
        e.target();
        current_homing = e;
    }

    public Flare InstantiateShot(int extraDmg = 0, int flameindex = 0){
        Tuple<int,bool> tp = getDmg();
        if(flameindex == 0 && tp.Item2){flameindex = 1;}
        GameObject go = Instantiate(FlarePrefabs[flameindex]);
        go.GetComponent<Flare>().Damage = tp.Item1 + extraDmg;
        go.GetComponent<Flare>().isCrit = tp.Item2;
        return go.GetComponent<Flare>();

    }
    public static float AtkSpeedToSeconds(float asp){
        return 1/asp;
    }

    private Enemy getHoming(){
        
        List<Enemy> g =  new List<Enemy>();
        g.AddRange(GameObject.FindGameObjectsWithTag("Enemy").Select( item => item.GetComponent<Enemy>() ) );
        g.Sort();
        return g.Count!=0 ? g[0] : null;
    }
    public UnityEngine.Vector2 getRandomHomingPosition(){
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        return go[UnityEngine.Random.Range(0, go.Length)].GetComponent<Enemy>().HitCenter.position;
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

    public Tuple<int,bool> getDmg(){
        if(UnityEngine.Random.Range(0,100) < CritChance){
            return new Tuple<int, bool>((int)(Dmg * CritMultiplier),true);
        }else{
            return new Tuple<int, bool>(Dmg, false);
        }
        
    }

    public void Hitted(int Dmg, float armPen){
        
        Health -=(int)( MaxHealth/ (MaxHealth * (1 + Armor/100.0f * (1-armPen))) * Dmg);
        if(Health <= 0){EndGame();}
        UpdateHealthUI();
        DamageUI.Instance.spawnTextDmg(transform.position, "-"+Dmg, 2);
        CameraShake.Shake(0.5f,0.35f);
    }
    private void UpdateHealthUI(){
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;
    }
    private void EndGame(){
        GameEnd = true;
        EnemySpawner.Instance.GameEnd = true;
        GameUI.Instance.SpeedUp(1f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies){
            if(e != null){
                e.GetComponent<Enemy>().EndEnemy();
            }
        }
        GameUI.Instance.GameOverEffect();
    }

    public void addAccuracy(float amount){accuracy = Math.Min(accuracy + amount, 100f);}
    public void multAccuracy(float amount){accuracy = Math.Min(accuracy * amount, 100f);}
    public void addAttackSpeed(float amount){atkSpeed = Math.Min(atkSpeed + amount, 12f);}
    public void multAttackSpeed(float amount){atkSpeed = Math.Min(atkSpeed * amount, 12f);}

    public void addBulletSpeed(float amount){BulletSpeed = Math.Min(BulletSpeed + amount, 20f);}
    public void multBulletSpeed(float amount){BulletSpeed = Math.Min(BulletSpeed * amount, 20f);}

    public void addDmg(int amount){Dmg += amount;}
    public void multDmg(int amount){Dmg *= amount;}

    public void addCritChance(float amount){CritChance = Math.Min(CritChance + amount, 80f);}
    public void multCritChance(float amount){CritChance = Math.Min(CritChance * amount, 80f);}

    public void addCritDmg(float amount){CritMultiplier = Math.Min(CritMultiplier + amount, 5f);}
    public void multCritDmg(float amount){CritMultiplier = Math.Min(CritMultiplier * amount, 5f);}
    public void addDmg(float amount){Dmg += (int)amount;}
    public void multDmg(float amount){Dmg += (int)amount;}

    public void addArmor(int amount){Armor += (int)amount;}
    public void addArmorPen(float amount){ArmorPen = Mathf.Min(1.0f, ArmorPen + amount);}

    public void addHealth(int max_increase, float healperc){
        MaxHealth += max_increase;
        Health = (int)Math.Min(Health + MaxHealth * healperc,MaxHealth);
        UpdateHealthUI();
        DamageUI.Instance.spawnTextDmg(transform.position,""+ MaxHealth * healperc, 3);
    }
    public void addHealth(float HealAmount){
        Health = (int)Math.Min(Health + HealAmount, MaxHealth);
         UpdateHealthUI();
        DamageUI.Instance.spawnTextDmg(transform.position, ""+ HealAmount, 3);
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

    public void ApplyOnHit(float d, float h, Enemy e, string except = null){
        foreach (OnHitEffects oh in onHitEffects){
            if(oh.getText() == except){continue;}
            oh.ApplyEffect(d,h,e);
            }
    }
    public void ApplyOnShoot(){
        foreach (OnShootEffects oh in onShootEffects){oh.ApplyEffect();}
    }
    public void ApplyOnLand(UnityEngine.Vector2 pos){
        foreach (OnLandEffect oh in onLandEffects){oh.ApplyEffect(pos);}
    }


    public GameObject SpawnObject(GameObject go){
        return Instantiate(go);
    }
    
}
