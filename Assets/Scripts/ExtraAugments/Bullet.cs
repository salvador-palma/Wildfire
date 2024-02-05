using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    

    float speed;
    float multiplier = 10f;
    int ttl = 1;
    int dmg;
    float maxDistance = 3.6f;
    Vector2 SpawnPos;
    Rigidbody2D rb;
    void Start(){
        SpawnPos = transform.position;
        speed = Flamey.Instance.BulletSpeed;
        rb = GetComponent<Rigidbody2D>();
        dmg = Bullets.Instance.dmg;
    }
    // Update is called once per frame
    void Update()
    {
        

        if(Vector2.Distance(transform.position, SpawnPos) > maxDistance || ttl <= 0){Destroy(gameObject);}


        rb.velocity = transform.up * speed * Time.deltaTime * multiplier;

    }

    public void PingHit(){
        ttl--;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy"){
            PingHit();
            Enemy e = other.GetComponent<Enemy>();
            e.Hitted(dmg, 10, ignoreArmor:false, onHit: true);
            Enemy.SpawnExplosion(other.transform.position);
            
        }
    }
}
