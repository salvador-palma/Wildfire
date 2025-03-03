using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class OrbitalHit : MonoBehaviour
{
    [field: SerializeField] public EventReference HitSound { get; private set; }
    public static int multiplier = 1;
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){

            Enemy e = other.GetComponent<Enemy>();
            if(e.canTarget()){
                bool fullHealth = e.Health >= e.MaxHealth;
                
                switch(FlameCircle.Instance.PlanetType){
                    case 3: //MARS
                        DealDamage(e);
                        if(e.Health<=0){
                            multiplier*= Flamey.Instance.Health <= Flamey.Instance.MaxHealth/4f ? 3 : 2;
                        }else{
                            multiplier = 1;
                        }
                        break;
                    case 7: //NEPTUNE
                        e.Stun(2f);
                        DealDamage(e);
                        break;
                    default://MERCURY
                        DealDamage(e);
                        break;
                }

                if(fullHealth && e.Health <= 0){
                    GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(37,"Betsy",16); //VENUS UNLOCK
                }


                Enemy.SpawnExplosion(other.transform.position);

                AudioManager.PlayOneShot(FMODEvents.Instance.OrbitalHit, transform.position);
            }
            
            
        }
    }
    public void DealDamage(Enemy e){
        // Debug.Log("Damaged " + FlameCircle.Instance.damage);
        e.Hitted(FlameCircle.Instance.damage * multiplier, 0, ignoreArmor:FlameCircle.Instance.PlanetType==1, onHit: true);
    }
}
