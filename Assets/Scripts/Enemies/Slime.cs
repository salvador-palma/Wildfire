using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    
    private bool check = false;
    [SerializeField]  private bool tiny;
    [SerializeField] private int DeathSpawnSwarm;
    [SerializeField] private GameObject TinySlime;
    [SerializeField] private Vector2[] spawnTinyPos;
    private float maxSpeed;
    private void Start() {
        base.flame = Flamey.Instance;
        maxSpeed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        Speed = 0.00001f;
        
        MaxHealth = Health;
        StartAnimations(3);
    }
    private void Update() {
        
        base.Move();
        if(Vector2.Distance(flame.transform.position, transform.position) < AttackRange && !check){
            check = true;
            Speed = 0.0001f;
            GetComponent<Animator>().SetTrigger("InRange");
            InvokeRepeating("Attack",0f, AttackDelay);
        }
    }
    public void SlideOn(){
        Speed = maxSpeed;
    }
    public void SlideOff(){
        Speed = 0.0001f;
    }

    override public void Die(){
        for(int i = 0; i != DeathSpawnSwarm; i++){
            GameObject go = Instantiate(TinySlime);
            go.transform.position =  (Vector2)transform.position + spawnTinyPos[i];
            EnemySpawner.Instance.CheckFlip(go);
        }
        
         
        base.Die();
    }
    
}

