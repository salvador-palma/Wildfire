using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamey : MonoBehaviour
{
    public static Flamey Instance {get; private set;}

    //STATS
    private int Health;
    public int Dmg;
    private float CritChance;
    private float CritMultiplier;
    [Range(5f, 20f)] public float BulletSpeed;
    [Range(0f, 0.45f)] public float Accuracy;
    
    [SerializeField][Range(0.75f, 7f)] float atkSpeed = 1;
    private float AtkSpeed;
    //===========

    //ATK SPEED TIMERS
    private float timerAS;
    private float timerASCounter;
    //===========

    [SerializeField] public Enemy current_homing;
    [SerializeField] private GameObject FlarePrefab;
    [SerializeField] public GameObject FlareSpotPrefab;

    private Animator anim;

    private void Awake() {
        Instance = this;
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        AtkSpeed = atkSpeed;
        timerASCounter = AtkSpeedToSeconds(atkSpeed);;
        timerAS = timerASCounter;
        current_homing = getHoming();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current_homing == null){
            current_homing = getHoming();
            if(current_homing == null){return;}
        }

        if(timerAS > 0){
            timerAS -= Time.deltaTime;
        }else{
            if(atkSpeed != AtkSpeed){
                AtkSpeed = atkSpeed;
                updateTimerAS(atkSpeed);
            }
            timerAS = timerASCounter;
            shoot();
        }

    }

    private void shoot(){
        anim.Play("FlameShoot");
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

    
}
