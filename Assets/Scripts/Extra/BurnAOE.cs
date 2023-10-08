using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurnAOE : MonoBehaviour
{
    public int Damage;
    public float Timer =1f;
    float t;
    List<Enemy> colliding;
    private void Start() {
        colliding = new List<Enemy>();
    }
    void Update()
    {
        t-=Time.deltaTime;
        if(t<=0){
            t= Timer;
            foreach (Enemy item in colliding)
            {
                if(item == null){continue;}
                item.Health -= Damage;
                if(item.Health <= 0){item.Die();}
                DamageUI.Instance.spawnTextDmg(item.transform.position, Damage+"", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            Debug.Log("HIT");
            colliding.Add(collider.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            colliding.Remove(collider.GetComponent<Enemy>());
        }
    }
}
