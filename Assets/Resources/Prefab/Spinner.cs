using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed;
    public static float multiplier;

    public GameObject[] Spinners;
    
    public bool canSpin;
    protected virtual void Start() {
        
        speed = Flamey.Instance.BulletSpeed;
        if(!EnemySpawner.Instance.Paused){
            canSpin = true;
            if(FlameCircle.Instance.PlanetType==3){
                GetComponent<Animator>().enabled=true;
            }
        
        }
        
    }
    private void Update() {
        if(canSpin){
            transform.Rotate(0,0,speed*multiplier*Time.deltaTime);
        }
        
    }

    public GameObject SpawnCircle(int amount){
        return Instantiate(Spinners[amount]);
    }
    public void kill(){
        Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){

            Enemy e = other.GetComponent<Enemy>();
            if(e.canTarget()){
                if(FlameCircle.Instance.PlanetType==7){
                    e.Stun(2f);
                }
                e.Hitted(FlameCircle.Instance.damage *(FlameCircle.Instance.PlanetType==6?2:1), 0, ignoreArmor:FlameCircle.Instance.PlanetType==1, onHit: true);
                Enemy.SpawnExplosion(other.transform.position);
            }
            
            
        }
    }
}
