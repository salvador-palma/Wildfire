using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : IPoolable
{
    public override string getReference()
    {
       return "Ghost";
    }

    public float Timer = 5f;
    public float Speed = 2f;

    private SpriteRenderer sp;
    private void Start() {
        sp = GetComponent<SpriteRenderer>();
    }
    private void Update() {
        
        if(Timer <= 0){
            UnPool();
        }else{
            Timer-=Time.deltaTime;
            transform.position = new Vector2(transform.position.x,transform.position.y + Speed * Time.deltaTime);

            float a = Math.Min(.6f, -.25f * (5f-Timer) + 1.25f );
            sp.color =new Color(1,1,1,a);
        }
    }


    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }
    public override void Pool()
    {
        try{
            Timer = 5f;
            Speed = Random.Range(1f, 2.5f);
            GetComponent<SpriteRenderer>().flipX = Random.Range(0f,1f)<0.5;
            gameObject.SetActive(true);
            int type = Random.Range(0, 4);
            sp.color =new Color(1,1,1,.6f);
            GetComponent<Animator>().Play("Ghost"+type);
        }catch{}
        
        
    }
    
}
