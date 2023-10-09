using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    public static EnemySpawner Instance {get; private set;}
    [SerializeField] GameObject[] Enemies;
    float height;
    float width;
    float TimerEnemySpawn;
    bool isOn = true;
    bool isOnAugments = false;

    

    public int current_round = 0;

    float FixedEnemyAmount = 5f;
    
    float FixedRoundDuration = 5f;
    float roundTimer = 10;
    [SerializeField] public GameObject ExplosionPrefab;
    
    public bool GameEnd;
    
    List<List<float>> ProbabiltyList = new List<List<float>>(){

        new List<float>(){1,0,0,0},
        new List<float>(){0.85f,.15f,0,0},
        new List<float>(){0.7f,0.3f,0f,0},
        new List<float>(){0.6f,0.4f,0f,0},
        new List<float>(){0.6f,0.4f,0f,0},

        new List<float>(){0.55f,0.3f,.15f,0},
        new List<float>(){0.2f,0.5f,.3f,0},
        new List<float>(){0.1f,0.6f,.3f,0},
        new List<float>(){0.05f,0.6f,.35f,0},
        new List<float>(){0,1f,0,0},
        new List<float>(){1f,0,0,0},
        new List<float>(){0,0.5f,.5f,0},
        new List<float>(){0,0.5f,.5f,0},
        new List<float>(){0,0.5f,.5f,0},
        new List<float>(){0,0.5f,.5f,0},
        new List<float>(){0,0.5f,.45f,0.05f},
        new List<float>(){0,0.5f,.45f,0.05f},
        new List<float>(){0,0.5f,.45f,0.05f},
        new List<float>(){0,0.4f,.5f,0.10f},
        new List<float>(){0,0.39f,.50f,0.10f,0.01f},
        new List<float>(){0,0.35f,.50f,0.10f,0.05f},
        new List<float>(){0,0f,.75f,0.20f,0.05f},
        new List<float>(){0,0f,.65f,0.20f,0.15f},
        new List<float>(){0,0f,0f,1f,0f},
        new List<float>(){0,0f,0f,0.75f,0.25f},
        new List<float>(){0,0f,0f,0.7f,0.25f, 0.05f},
        new List<float>(){0,0f,0f,0.7f,0.25f, 0.05f},
        new List<float>(){0,0f,0f,0.7f,0.2f, 0.10f},
        new List<float>(){0,0f,0f,0.6f,0.3f, 0.10f},
        new List<float>(){0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0f,0f,0.5f,0.35f, 0.15f},
        new List<float>(){0,0f,0f,0.33f,0.34f, 0.33f},
        new List<float>(){0,0f,0f,0.33f,0.34f, 0.33f},
        new List<float>(){0,0f,0f,0.33f,0.34f, 0.33f},
        new List<float>(){0,0f,0f,0.20f,0.40f, 0.40f},

    };

    
    private void Awake() {
        Instance = this;
    }
    public void Start(){

        roundTimer = getRoundTime(current_round);
        
        
        TimerEnemySpawn =(float)Distribuitons.RandomExponential(FixedEnemyAmount/FixedRoundDuration);
        updateSpawnLimits();
        GameUI.Instance.UpdateProgressBar(current_round);
        
        
        
    }
    private Vector2 getPoint(){
        double angle = Math.PI * UnityEngine.Random.Range(0,360)/180f;
        double x = 0.52f * width * Math.Cos(angle);
        double y = 0.52f * height * Math.Sin(angle);
        return new Vector2((float)x,(float)y);
    }

    private void Update() {
        if(GameEnd){return;}
        if(!isOn){
            if(GameObject.FindGameObjectWithTag("Enemy") == null && !isOnAugments){
                isOnAugments = true;
                Deck.Instance.StartAugments((current_round+1)%5 == 0);
                
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
    public void SpawnEnemy(GameObject enemy){
        GameObject g = Instantiate(enemy);
        g.transform.position = getPoint();
        CheckFlip(g);
    }
    private void updateSpawnLimits(){
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        
    }
    private void CheckFlip(GameObject g){
        if(g.transform.position.x < 0){
            g.GetComponent<SpriteRenderer>().flipX = true;
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
        return -1;
    }


    private float getRoundTime(int round){
        return Math.Min(5 + 1.2f * (float)Distribuitons.RandomUniform(round - 1, round + 1), 40);
    }
    private float getSpawnAmount(int round){
        return 5 + 3f * round;
    }
    




}
