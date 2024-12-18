using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class OrbitalHit : MonoBehaviour
{
    [field: SerializeField] public EventReference HitSound { get; private set; }
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){

            Enemy e = other.GetComponent<Enemy>();
            if(e.canTarget()){
                if(FlameCircle.Instance.PlanetType==7){
                    e.Stun(2f);
                }
                e.Hitted(FlameCircle.Instance.damage *(FlameCircle.Instance.PlanetType==6?2:1), 0, ignoreArmor:FlameCircle.Instance.PlanetType==1, onHit: true);
                Enemy.SpawnExplosion(other.transform.position);

                AudioManager.PlayOneShot(FMODEvents.Instance.OrbitalHit, transform.position);
            }
            
            
        }
    }
}
