using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class DrainAOE : IPoolable
{
    [SerializeField] Sprite[] flowers;
    [SerializeField] Sprite carnivore;
    public float perc;
    public float Timer =1f;
    float t;
    public float LastingTime;
    float lt;
    List<Enemy> colliding;
    public bool isCarnivore;
    private void Start() {
        colliding = new List<Enemy>();
        lt = DrainOnLand.Instance==null? 1f : DrainOnLand.Instance.lasting;
        perc = DrainOnLand.Instance==null? 0.01f : DrainOnLand.Instance.perc;
        Vector2 scale = transform.localScale * (DrainOnLand.Instance==null?1f:DrainOnLand.Instance.size) * (isCarnivore? 1.5f : 1);
        transform.localScale = scale;
        if(isCarnivore){
            GetComponent<SpriteRenderer>().sprite = carnivore;
        }else{
            GetComponent<SpriteRenderer>().sprite = flowers[UnityEngine.Random.Range(0, flowers.Length-1)];
        }
         if(EnemySpawner.Instance.isOnAugments){Destroy(gameObject);}
        

    }
    void Update()
    {
        t-=Time.deltaTime;
        lt-=Time.deltaTime;
        
        if(t<=0){
            t= Timer;
            try{
                if(isCarnivore){
                    foreach (Enemy item in colliding)
                    {
                        if(item == null || !item.canTarget()){continue;}
                        item.Hitted((int)(item.MaxHealth * perc), 21, false, false);
                        Flamey.Instance.addHealth(item.MaxHealth * perc);

                    }
                }else{
                    foreach (Enemy item in colliding)
                    {
                        if(item == null || !item.canTarget()){continue;}
                        Flamey.Instance.addHealth(item.MaxHealth * perc);

                    }
                }
                
            }catch(InvalidOperatorException e){
                Debug.Log(e);
            }
            
        }
        if(lt<=0){
            lt = LastingTime;
            GetComponent<Animator>().Play("EndAOEBurn");
            if(SkillTreeManager.Instance.getLevel("Flower Field") >= 2){
                foreach (Enemy item in colliding)
                {
                    if(item == null || !item.canTarget()){continue;}
                    item.Stun(2f);
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Add(collider.GetComponent<Enemy>());
            if((colliding.Count > 5 && SkillTreeManager.Instance.getLevel("Flower Field") < 1) || colliding.Count > 15){
                lt=0;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Remove(collider.GetComponent<Enemy>());
        }
    }

    public override void Pool()
    {
        colliding = new List<Enemy>();
        lt = DrainOnLand.Instance==null? 1f : DrainOnLand.Instance.lasting;
        perc = DrainOnLand.Instance==null? 0.01f : DrainOnLand.Instance.perc;
        Vector2 scale = new Vector2(0.2232152f,0.2232152f) * (DrainOnLand.Instance==null?1f:DrainOnLand.Instance.size) * (isCarnivore? 1.5f : 1);
        transform.localScale = scale;
        if(isCarnivore){
            GetComponent<SpriteRenderer>().sprite = carnivore;
        }else{
            GetComponent<SpriteRenderer>().sprite = flowers[UnityEngine.Random.Range(0, flowers.Length-1)];
        }
         if(EnemySpawner.Instance.isOnAugments){UnPool();}

        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 1;
        GetComponent<SpriteRenderer>().color = c;
    }
    public override string getReference()
    {
        return "Flower";
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }
}
