using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField]  private bool tiny;
    [SerializeField] private int DeathSpawnSwarm;
    [SerializeField] private GameObject TinySlime;
    [SerializeField] private Vector2[] spawnTinyPos;
    [SerializeField] private float maxSpeed;
    private void Start() {
        VirtualPreStart(); 
        flame = Flamey.Instance;
        maxSpeed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        Speed = 0.00001f;
        
        MaxHealth = Health;
        StartAnimations(tiny ? 3 : 6);
    }

    public void SlideOn(){
        if(IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1){
            if(getSlowInfo("IceLand")[0] <= 0){
                Speed = maxSpeed;
            }
        }else{
            Speed = maxSpeed;
        }
    }
    public void SlideOff(){
        Speed = 0.0001f;
    }

    override public void Die(bool onkill = true){
        for(int i = 0; i != DeathSpawnSwarm; i++){
            GameObject go = Instantiate(TinySlime);
            go.transform.position =  (Vector2)transform.position + spawnTinyPos[i];
            EnemySpawner.Instance.addEnemy(go.GetComponent<Enemy>());
            go.GetComponent<Enemy>().CheckFlip();
        }
        
         
        base.Die(tiny);
    }

    

    
    
}

