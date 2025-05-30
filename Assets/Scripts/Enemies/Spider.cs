using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Spider : Enemy
{
    


    public int direction = 1;
   
    
    // Start is called before the first frame update
    void Start()
    {
        VirtualPreStart(); 
        
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x, 2)/2500f) + 1f);
        }
        
        
        MaxHealth = Health;

        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        
    }

    // Update is called once per frame
    
    
    [Range(-90, 90)]
    public float angleStep = 30f;
    float prevX;
    public override void Move(){


        if(Stunned){return;}
        MoveSpiral(angleStep, reverse:direction == 1);
        
        CheckFlip();
        prevX = transform.position.x;
        
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }

    
}
