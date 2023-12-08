using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    public static EnemySpawner Instance {get; private set;}
    [SerializeField] GameObject[] Enemies;
    float height;
    float width;
    float TimerEnemySpawn;
    bool isOn = true;
    [HideInInspector] public bool isOnAugments = false;

    

    public int current_round = 0;

    float FixedEnemyAmount = 5f;
    
    float FixedRoundDuration = 5f;
    float roundTimer = 10;
    [SerializeField] public GameObject ExplosionPrefab;
    
    public bool GameEnd = true;
    
    List<List<float>> ProbabiltyList = new List<List<float>>(){
        //0
        //Slug + TinySlime
        new List<float>(){1,0},
        new List<float>(){0.85f,.15f},
        new List<float>(){0.7f,0.3f},
        new List<float>(){0.6f,0.4f},
        new List<float>(){0.6f,0.4f},

        //5
        //CatterPie + Slime
        new List<float>(){0.4f,0.3f,.2f,0.1f},
        new List<float>(){0.05f,0.5f,.3f,0.15f},
        new List<float>(){0.05f,0.4f,.5f,0.05f},
        new List<float>(){0.05f,0.5f,.35f,0.1f}, 
        new List<float>(){0,0.8f,0,0.2f},

        //10
        //SlimeOrangeTiny
        new List<float>(){0,.3f,.15f,.45f,.1f}, 
        new List<float>(){0,.15f,.25f,0.5f, 0.1f}, 
        new List<float>(){0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0,.2f,.5f,.3f},

        //15 
        //Turtle
        new List<float>(){0,0,.3f,.15f,.45f,.1f},
        new List<float>(){0,0,.15f,.25f,0.5f, 0.1f},
        new List<float>(){0,0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0,0,.2f,.5f,.3f},

        //20
        //Slime Orange
        new List<float>(){0,0,0,.3f,.15f,.45f,.1f},
        new List<float>(){0,0,0,.15f,.25f,0.5f, 0.1f},
        new List<float>(){0,0,0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,0,0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0.2f,0,0.2f,.3f,0f,.3f},

        //25
        //Snail
        new List<float>(){0,0,0,0,.3f,.15f,.45f,.1f},
        new List<float>(){0,0,0,0,.15f,.25f,0.5f, 0.1f},
        new List<float>(){0,0,0,0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,0,0,0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0,0,0,0,.2f,.5f,.3f},
        
        //30
        //SlimeRedTiny + CatterillarRed
        new List<float>(){0,0,0,0,0,.3f,.15f,.45f,.1f},
        new List<float>(){0,0,0,0,0,.15f,.25f,0.5f, 0.1f},
        new List<float>(){0,0,0,0,0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,0,0,0,0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0,0,0,0,0,.2f,.5f,.3f},

        //35
        //SlimeRed
        new List<float>(){0,0,0,0,0,0,.3f,.15f,.45f,.1f},
        new List<float>(){0,0,0,0,0,0,.15f,.25f,0.5f, 0.1f},
        new List<float>(){0,0,0,0,0,0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,0,0,0,0,0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0.1f,0,0.1f,.15f,0f,.15f,0f,0.25f,0f, 0.25f},

        //40
        //Red Turtle
        new List<float>(){0,0,0,0,0,0,0,.3f,.15f,.45f,.1f},
        new List<float>(){0,0,0,0,0,0,0,.15f,.25f,0.5f, 0.1f},
        new List<float>(){0,0,0,0,0,0,0,.10f,.25f,0.5f,.15f},
        new List<float>(){0,0,0,0,0,0,0,.05f,.2f,.5f, 0.25f},
        new List<float>(){0,0,0,0,0,0,0,0,.2f,.5f,.3f},
        //45
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.55f,0.35f, 0.10f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.55f,0.35f, 0.10f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.55f,0.35f, 0.10f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.55f,0.35f, 0.10f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.55f,0.35f, 0.10f},
        //50
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.5f,0.35f, 0.15f},

        //55
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.33f,0.34f, 0.33f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.33f,0.34f, 0.33f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.33f,0.34f, 0.33f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.20f,0.40f, 0.40f},
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0.20f,0.40f, 0.40f},

        //60
        new List<float>(){0,0,0,0,0,0,0,0f,0f,0f,0f, 0f, 1f},


    };

    List<Enemy> PresentEnemies;
    private void Awake() {
        Instance = this;
        PresentEnemies = new List<Enemy>();
    }
    public void Start(){
        current_round = 500;
        GameEnd =true;
        Flamey.Instance.GameEnd = true;
        resetInstances();
        roundTimer = getRoundTime(current_round);
        
        
        TimerEnemySpawn =(float)Distribuitons.RandomExponential(FixedEnemyAmount/FixedRoundDuration);
        updateSpawnLimits();
        GameUI.Instance.UpdateProgressBar(current_round);
        GameUI.Instance.UpdateMenuInfo(current_round);
        
        
    }
    public void StartGame(){ 
        Flamey.Instance.GameEnd = false;       
        GameEnd = false;
    }
    private Vector2 getPoint(){
        double angle = Math.PI * (float)Distribuitons.RandomUniform(0,360)/180f;
        double x = 0.52f * width * Math.Cos(angle);
        double y = 0.52f * height * Math.Sin(angle);
        return new Vector2((float)x,(float)y);
    }

    private void Update() {
        if(GameEnd){return;}
        UpdateEnemies();
        if(!isOn){
            if(GameObject.FindGameObjectWithTag("Enemy") == null && !isOnAugments){
                if(current_round==59){GameUI.Instance.ShowLimitRoundPanel();}
                else{
                    isOnAugments = true;
                    Deck.Instance.StartAugments((current_round+1)%5 == 0);
                }
                
                
            } 
            return;}
        if(TimerEnemySpawn > 0){
            TimerEnemySpawn-= Time.deltaTime;
        }else{
            
            TimerEnemySpawn = (float)Distribuitons.RandomExponential(FixedEnemyAmount/FixedRoundDuration);
            
            SpawnEnemy(PickRandomEnemy(current_round));
            updateSpawnLimits();
        }
        if(roundTimer > 0){
            roundTimer-=Time.deltaTime ;
        }else{
            isOn = false;
        }
    }
    public void UpdateEnemies(){
        PresentEnemies.ForEach(e => {if(!e.Attacking){e.UpdateEnemy();}});
        List<Enemy> deadEnemies = PresentEnemies.Where(e => e.Health < 0).ToList();
        foreach(Enemy enemy in deadEnemies){
            PresentEnemies.Remove(enemy);
            enemy.Die();
        }


       
    }
    public void SpawnEnemy(GameObject enemy){
        GameObject g = Instantiate(enemy);

        PresentEnemies.Add(g.GetComponent<Enemy>());
        g.transform.position = getPoint();
        CheckFlip(g);
    }
    private void updateSpawnLimits(){
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        
    }
    public void CheckFlip(GameObject g){
        if(g.transform.position.x < 0){
            g.GetComponent<SpriteRenderer>().flipX = !g.GetComponent<SpriteRenderer>().flipX;
        }
    }

    public void newRound(){
        
        current_round++;
        
       
        isOn = true;
        isOnAugments = false;
        roundTimer = getRoundTime(current_round);
        FixedRoundDuration = roundTimer;
        FixedEnemyAmount = getSpawnAmount(current_round);
        float temp = FixedEnemyAmount/FixedRoundDuration;
        TimerEnemySpawn = (float)Distribuitons.RandomExponential(temp) ;
        GameUI.Instance.UpdateProgressBar(current_round);
        foreach (NotEspecificEffect item in Flamey.Instance.notEspecificEffects)
        {
            item.ApplyEffect();
        }
        
        GameUI.Instance.UpdateMenuInfo(current_round);
        
    }


   

    private GameObject PickRandomEnemy(int round){
        if(round >= ProbabiltyList.Count){round =  ProbabiltyList.Count-1;}
        return Enemies[pickEnemyIndex(ProbabiltyList[round])];
    }

    private int pickEnemyIndex(List<float> prob){
        float val = UnityEngine.Random.Range(0f,1f);
        for(int i = 0 ; i< prob.Count; i++){
            if(prob[i] > val){
                return i;
            }else{
                val -= prob[i];
            }
        }
        return prob.Count - 1;
    }


    private float getRoundTime(int round){
        //return Math.Min(5 + 1.2f * (float)Distribuitons.RandomUniform(round - 1, round + 1), 40);
        return Math.Min(5 + 1.2f * round, 40);
    }
    private float getSpawnAmount(int round){
        return 5 + 3f * round;
    }
    

    private void resetInstances(){
        // FlameCircle.Instance = null;

        // VampOnHit.Instance = null;
        // IceOnHit.Instance = null;
        // ShredOnHit.Instance = null;
        // ExecuteOnHit.Instance = null;
        // StatikOnHit.Instance = null;

        // BurnOnLand.Instance = null;

        // SecondShot.Instance = null;
        // BurstShot.Instance = null;
        // KrakenSlayer.Instance = null;
        // CritUnlock.Instance = null;

    }


}
