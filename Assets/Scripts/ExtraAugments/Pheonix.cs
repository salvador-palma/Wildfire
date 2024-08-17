using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheonix : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            collider.GetComponent<Enemy>().Die();
            Instantiate(EnemySpawner.Instance.ExplosionPrefab).transform.position = collider.GetComponent<Enemy>().HitCenter.position;
        }
    }
    public void EndPheonix(){
        Flamey.Instance.addHealth(Flamey.Instance.MaxHealth);
        Flamey.Instance.Unhittable = false;
    }
}
