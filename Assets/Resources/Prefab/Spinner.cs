using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed;
    public float multiplier = 1.2f;

    public GameObject[] Spinners;
    
    public bool canSpin = true;
    private void Start() {
        
        speed = Flamey.Instance.BulletSpeed;
        canSpin = true;
        
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
    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){
            Enemy e = other.GetComponent<Enemy>();
            e.Hitted(FlameCircle.Instance.damage, 0, ignoreArmor:false, onHit: true);
            Enemy.SpawnExplosion(other.transform.position);
        }
    }
}
