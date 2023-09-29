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
    float TimerEnemySpawn = 1f;
    float t;


    bool isOn = true;
    bool isOnAugments = false;

    float roundTimer = 10;

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
                Deck.Instance.StartAugments();
            } 
            return;}
        if(t > 0){
            t-= Time.deltaTime;
        }else{
            t = TimerEnemySpawn;
            SpawnEnemy(Enemies[0]);
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
        isOn = true;
        isOnAugments = false;
        roundTimer = 20f;
    }
}
