using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Flamey flame;
    public float Speed;
    public int Health;
    private int Dmg;

    
    public void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
    }

    private void Hitted(int dmg){
        Health -= dmg;
        if(Health <= 0){Die();}
    }

    private void Die(){
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "FlareHit"){
            
            Hitted(Flamey.Instance.Dmg);
        }
    }
}
