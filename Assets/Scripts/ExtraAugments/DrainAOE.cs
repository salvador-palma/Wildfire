using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class DrainAOE : MonoBehaviour
{
    [SerializeField] Sprite[] flowers;
    public float perc;
    public float Timer =1f;
    float t;
    public float LastingTime;
    float lt;
    List<Enemy> colliding;
    private void Start() {
        colliding = new List<Enemy>();
        lt = DrainOnLand.Instance.lasting;
        perc = DrainOnLand.Instance.perc;
        Vector2 scale = transform.localScale * DrainOnLand.Instance.size;
        transform.localScale = scale;

        GetComponent<SpriteRenderer>().sprite = flowers[UnityEngine.Random.Range(0, flowers.Length-1)];

    }
    void Update()
    {
        t-=Time.deltaTime;
        lt-=Time.deltaTime;
        if(t<=0){
            t= Timer;
            try{
                foreach (Enemy item in colliding)
                {
                    if(item == null){continue;}
                    Flamey.Instance.addHealth(item.MaxHealth * perc);

                }
            }catch(InvalidOperatorException e){
                Debug.Log(e);
            }
            
        }
        if(lt<=0){
            lt = LastingTime;
            GetComponent<Animator>().Play("EndAOEBurn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Add(collider.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Remove(collider.GetComponent<Enemy>());
        }
    }
}
