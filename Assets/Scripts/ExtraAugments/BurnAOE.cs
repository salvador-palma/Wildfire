using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurnAOE : MonoBehaviour
{
    public int Damage;
    public float Timer =1f;
    float t;
    public float LastingTime;
    float lt;
    List<Enemy> colliding;
    private void Start() {
        colliding = new List<Enemy>();
        lt = BurnOnLand.Instance.lasting;
        Damage = BurnOnLand.Instance.damage;
        Vector2 scale = transform.localScale * BurnOnLand.Instance.size;
        transform.localScale = scale;

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
                    if(item == null || !item.canTarget()){continue;}
                    item.Hitted(Damage,9, ignoreArmor:true, onHit: false);
                    
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
