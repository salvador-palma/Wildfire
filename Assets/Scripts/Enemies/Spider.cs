using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spider : Enemy
{
    public Transform centerPoint;
    public float initialRadiusX = 9.39f;
    public float initialRadiusY = 5.07f;

    public Vector2 shrinkLimits;
    private float shrinkRate;

    private float radiusX;
    private float radiusY;
    private float angle = 0f;

    public int direction = 1;
    public bool rest;
    public float MaxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        radiusX = initialRadiusX;
        radiusY = initialRadiusY;
        centerPoint = Flamey.Instance.transform;
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x, 2)/5000f) + 1f);
        }
        MaxSpeed = Speed;
        
        MaxHealth = Health;

        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        Vector3 relativePosition = transform.position - centerPoint.position;
        angle = Mathf.Atan2(relativePosition.y, relativePosition.x);

        shrinkRate = Random.Range(shrinkLimits[0], shrinkLimits[1]);
    }

    // Update is called once per frame
    
    
    float prevX;
    public override void Move(){



        float x = centerPoint.position.x + Mathf.Cos(angle) * radiusX;
        float y = centerPoint.position.y + Mathf.Sin(angle) * radiusY;

        prevX = transform.position.x;

        transform.position = new Vector3(x, y, transform.position.z);


        angle += Speed * (1-SlowFactor) * direction * Time.deltaTime;
        radiusX -= (Speed/MaxSpeed) * shrinkRate * Time.deltaTime;
        radiusY -= (Speed/MaxSpeed) * shrinkRate * Time.deltaTime;
        
        CheckFlip();
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
