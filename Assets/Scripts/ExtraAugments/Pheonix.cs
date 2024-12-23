using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Pheonix : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            collider.GetComponent<Enemy>().Die();
            Vector2 vec = collider.GetComponent<Enemy>().HitCenter.position;
            ObjectPooling.Spawn(EnemySpawner.Instance.ExplosionPrefab, new float[]{vec.x, vec.y});

        }
    }
    public void EndPheonix(){
        Flamey.Instance.addHealth(Flamey.Instance.MaxHealth);
        Flamey.Instance.Unhittable = false;
    }
}
