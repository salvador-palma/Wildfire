using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Spider : Enemy
{
    public Transform centerPoint;


    public int direction = 1;
   
    
    // Start is called before the first frame update
    void Start()
    {
        VirtualPreStart(); 
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
        
        
        MaxHealth = Health;

        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        
    }

    // Update is called once per frame
    
    
    [Range(0, (float)Math.PI*2)]
    public float angleStep = 30f;
    float prevX;
    public override void Move(){


        if(Stunned){return;}

        Vector2 hk = flame.transform.position;
        Vector2 cv = HitCenter.position;

        double g = Math.Atan2(cv.y - hk.y, cv.x - hk.x);
        g = g < 0 ? g+2*Math.PI : g;
        g += angleStep * direction;
        int w = Math.PI/2 < g && g < 2*Math.PI - Math.PI/2 ? -1 : 1;

        double r = Math.Sqrt(  Math.Pow(cv.x - hk.x, 2)/4f  + Math.Pow(cv.y - hk.y, 2));
        double a = Math.Sqrt(4*r*r);
        double b = r;

        float nx = (float)(w*a*b/Math.Sqrt(b*b + a*a*Math.Pow(Math.Tan(g),2)));
        float ny = (float)(nx*Math.Tan(g));

        Vector2 dest = new Vector2(nx, ny);

        prevX = transform.position.x;
        transform.position = Vector2.MoveTowards(transform.position, dest, Speed * (1-SlowFactor) * Time.deltaTime);



        CheckFlip();
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }

    
}
