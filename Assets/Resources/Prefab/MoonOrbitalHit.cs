using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonOrbitalHit : OrbitalHit
{
    protected override void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){
            Enemy e = other.GetComponent<Enemy>();
            e.Hitted(FlameCircle.Instance.damage * 4, 0, ignoreArmor:false, onHit: true);
            Flamey.Instance.ApplyOnLand(e.HitCenter.position);
            Enemy.SpawnExplosion(other.transform.position);
        }
    }
}
