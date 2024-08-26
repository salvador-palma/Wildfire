using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSpinner : Spinner
{
    // Start is called before the first frame update
    protected override void Start()
    {
        speed = 1f;
        if(!EnemySpawner.Instance.Paused){
            canSpin = true;        
        }
        Deck.RoundStart += StartMoving;
        Deck.RoundOver += StopMoving;
    }

    private void StopMoving(object sender, EventArgs e)
    {
        canSpin = false;
    }

    private void StartMoving(object sender, EventArgs e)
    {
        canSpin = true;
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){
            Enemy e = other.GetComponent<Enemy>();
            e.Hitted(FlameCircle.Instance.damage * 4, 0, ignoreArmor:false, onHit: true);
            Flamey.Instance.ApplyOnLand(e.HitCenter.position);
            Enemy.SpawnExplosion(other.transform.position);
        }
    }
}
