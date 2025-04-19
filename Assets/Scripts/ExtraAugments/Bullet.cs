using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IPoolable
{
    

    float speed;
    float multiplier = 10f;
    int ttl = 1;
    int dmg;
    float maxDistance = 20f;
    Vector2 SpawnPos;
    Rigidbody2D rb;
   
    // Update is called once per frame
    void FixedUpdate()
    {
        

        if(Vector2.Distance(transform.position, SpawnPos) > maxDistance || ttl <= 0){UnPool();}


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
    public override void Pool()
    {
        if(Character.Instance.isCharacter("Pirate")){
            ttl = 3;
        }else{
            ttl=1;
        }
        
        speed = Flamey.Instance.BulletSpeed * Gambling.getGambleMultiplier(1);
        rb = GetComponent<Rigidbody2D>();
        dmg = Bullets.Instance.dmg;
    }

    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
        transform.rotation = Quaternion.Euler(0,0,args[2]);
        SpawnPos = transform.position;
    }

    public override string getReference()
    {
        return "Roundshot";
    }
}
