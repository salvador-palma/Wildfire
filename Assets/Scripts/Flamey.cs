using System;
using System.Collections;
using System.Collections.Generic;
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

    public List<OnHitEffects> onHitEffects;
    public List<OnShootEffects> onShootEffects;
    [SerializeField][Range(0f,100f)]public float CritChance;
    [SerializeField][Range(1f,5f)]public float CritMultiplier;
    [Range(5f, 20f)] public float BulletSpeed;
    [Range(0f, 100f)] public float accuracy;
    [Range(1f,5f)] public float BulletSize;
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
    [SerializeField] private GameObject FlarePrefab;
    [SerializeField] public GameObject FlareSpotPrefab;

    [Header("References")]
    private Animator anim;
    [SerializeField]private Slider HealthSlider;

    private void Awake() {
        Health = MaxHealth;
       
        Instance = this;
        anim = GetComponent<Animator>();
        onHitEffects = new List<OnHitEffects>();
        onShootEffects = new List<OnShootEffects>();
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        target(getHoming());
        AtkSpeed = atkSpeed;
        timerASCounter = AtkSpeedToSeconds(atkSpeed);;
        timerAS = timerASCounter;
        
        UpdateHealthUI();
        
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
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

        

    }

    public void shoot(){
        anim.Play("FlameShoot");
        ApplyOnShoot();
        Instantiate(FlarePrefab);
    }
    public void target(Enemy e){
        if(e ==null){return;}
        if(current_homing!=null){current_homing.untarget();}
        e.target();
        current_homing = e;
    }

    public void InstantiateShot(){
        Instantiate(FlarePrefab);
    }
    public static float AtkSpeedToSeconds(float asp){
        return 1/asp;
    }

    private Enemy getHoming(){
        GameObject g =  GameObject.FindGameObjectWithTag("Enemy");
        if(g == null){return null;}
        return g.GetComponent<Enemy>();
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

    public void Hitted(int Dmg){
        Health -= Dmg;
        UpdateHealthUI();
        DamageUI.Instance.spawnTextDmg(transform.position, "-"+Dmg, Color.red);
    }
    private void UpdateHealthUI(){
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;
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

    public void addHealth(int max_increase, float healperc){
        MaxHealth += max_increase;
        Health = (int)Math.Min(Health + MaxHealth * healperc,MaxHealth);
        UpdateHealthUI();
        DamageUI.Instance.spawnTextDmg(transform.position, "+"+MaxHealth * healperc, Color.green);
    }
    public void addHealth(float HealAmount){
        Health = (int)Math.Min(Health + HealAmount, MaxHealth);
         UpdateHealthUI();
        DamageUI.Instance.spawnTextDmg(transform.position, "+"+ HealAmount, Color.green);
    }
    
    public void addOnHitEffect(OnHitEffects onhit){
        if(onhit.addList()){
            onHitEffects.Add(onhit);
        } 
    }
    public void addOnShootEffect(OnShootEffects onhit){
        if(onhit.addList()){
            onShootEffects.Add(onhit);
        }
    }

    public void ApplyOnHit(float d, float h){
        foreach (OnHitEffects oh in onHitEffects){oh.ApplyEffect(d,h);}
    }
    public void ApplyOnShoot(){
        foreach (OnShootEffects oh in onShootEffects){oh.ApplyEffect();}
    }

    
}
