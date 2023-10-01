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


    private int current_round = 0;

    float TimerEnemySpawn = 1f;
    float roundTimer = 10;
    


    List<List<float>> ProbabiltyList = new List<List<float>>(){
        new List<float>(){1,0,0,0},
        new List<float>(){0.85f,.15f,0,0},
        new List<float>(){0.7f,0.3f,0f,0},
        new List<float>(){0.55f,0.3f,.15f,0},
    };

    
    private void Awake() {
        Instance = this;
    }
    public void Start(){
        t = TimerEnemySpawn;
        updateSpawnLimits();
        
    }
    private Vector2 getPoint(){
        float angle = Mathf.Deg2Rad * UnityEngine.Random.Range(0,360);
        float x = width * MathF.Cos(angle);
        float y = height * MathF.Sin(angle);
        return new Vector2(x,y);
    }

    private void Update() {

        if(!isOn){
            if(GameObject.FindGameObjectWithTag("Enemy") == null && !isOnAugments){
                isOnAugments = true;
                Deck.Instance.StartAugments(current_round%5 == 0 && current_round != 0);
                
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
        height = 1f * Camera.main.orthographicSize;
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
        roundTimer = 10f;
    }


   

    private GameObject PickRandomEnemy(int round){
        if(round >= ProbabiltyList.Count){round =  ProbabiltyList.Count-1;}
        return Enemies[pickEnemyIndex(ProbabiltyList[round])];
    }

    private int pickEnemyIndex(List<float> prob){
        float val = UnityEngine.Random.Range(0f,1f);
        for(int i = 0 ; i< prob.Count; i++){
            if(prob[i] > val){
                //Debug.Log(i);
                return i;
            }else{
                val -= prob[i];
            }
        }
        return -1;
    }


    // private float getRoundTime(int phase, int round){
    //     if(round>=)
    //     10 * round + phase * 
    // }




}
