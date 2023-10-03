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
    float t;
    bool isOn = true;
    bool isOnAugments = false;

    [SerializeField] GameObject testObject;

    private int current_round = 0;

    float TimerEnemySpawn = 1f;
    float roundTimer = 10;
    [SerializeField] public GameObject ExplosionPrefab;
    

    
    List<List<float>> ProbabiltyList = new List<List<float>>(){
        new List<float>(){1,0,0,0},
        new List<float>(){0.85f,.15f,0,0},
        new List<float>(){0.7f,0.3f,0f,0},
        new List<float>(){0.55f,0.3f,.15f,0},
        new List<float>(){0.2f,0.5f,.3f,0},
    };

    
    private void Awake() {
        Instance = this;
    }
    public void Start(){
        roundTimer = getRoundTime(current_round);
        TimerEnemySpawn = roundTimer/getSpawnAmount(current_round);
       // t = TimerEnemySpawn;
        updateSpawnLimits();
        GameUI.Instance.UpdateProgressBar(current_round);
        Debug.Log(roundTimer);
        Debug.Log(TimerEnemySpawn);
        
        
    }
    private Vector2 getPoint(){
        double angle = Math.PI * UnityEngine.Random.Range(0,360)/180f;
        double x = 0.52f * width * Math.Cos(angle);
        double y = 0.52f * height * Math.Sin(angle);
        return new Vector2((float)x,(float)y);
    }

    private void Update() {

        if(!isOn){
            if(GameObject.FindGameObjectWithTag("Enemy") == null && !isOnAugments){
                isOnAugments = true;
                Deck.Instance.StartAugments((current_round+1)%5 == 0);
                
            } 
            return;}
        if(t > 0){
            t-= Time.deltaTime;
        }else{
            t = TimerEnemySpawn;
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
        TimerEnemySpawn = roundTimer/getSpawnAmount(current_round);
        GameUI.Instance.UpdateProgressBar(current_round);
        Debug.Log(roundTimer);
        Debug.Log(TimerEnemySpawn);
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
        return Math.Min(5 + 1.2f * round, 40);
    }
    private float getSpawnAmount(int round){
        return 5 + 3f * round;
    }
    




}
