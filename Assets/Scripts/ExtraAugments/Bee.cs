using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float speed;
    public float atkSpeed;
    public float atkTimer;
    public int dmg;

    Enemy target;
    SpriteRenderer sp;

    private void Start() {
        sp = GetComponentInChildren<SpriteRenderer>();
        transform.position = new Vector2(Random.Range(-5f,5f), Random.Range(-5f,5f));
    }
    
    // Update is called once per frame
    void Update()
    {
        if(EnemySpawner.Instance.isOnAugments){return;}

        if(target == null){
            target = Flamey.Instance.getRandomHomingEnemy();
            if(target==null){return;}
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        checkFlip();
        if(atkTimer > 0){
            atkTimer -= Time.deltaTime;
        }else{
            
            
            if(Vector2.Distance(transform.position, target.transform.position) < 1.5f){
                
                Attack();
            }
        }
    }
    private void Attack(){
        atkTimer = 1/atkSpeed;
        target.Hitted(dmg, 12, ignoreArmor:false, onHit:true);
    }
    private void checkFlip(){
        if(target!=null && sp != null){
            if((target.transform.position.x > transform.position.x && !sp.flipX) || (target.transform.position.x < transform.position.x && sp.flipX)){
                sp.flipX = !sp.flipX;
            }
        }
    }

    public void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = s.speed;
        dmg = s.dmg;
        atkSpeed = s.atkSpeed;
    }
}
