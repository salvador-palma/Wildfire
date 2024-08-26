using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float speed;
    public float atkSpeed;
    public float atkTimer;
    public int dmg;

    public Enemy target;
    SpriteRenderer sp;
    SpriteRenderer propSp;

    public virtual void Start() {
        sp = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        propSp = transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
        transform.position = new Vector2(Random.Range(-5f,5f), Random.Range(-5f,5f));
    }
    
    // Update is called once per frame
    public virtual void  Update()
    {
        if(EnemySpawner.Instance.isOnAugments){return;}

        if(target == null){
            target = getTarget();
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
    protected virtual Enemy getTarget(){
        return Flamey.Instance.getRandomHomingEnemy(true);
        
         
    }
    protected virtual void Attack(){
        atkTimer = 1/atkSpeed;
        target.Hitted(dmg, 12, ignoreArmor:false, onHit:true);
    }
    protected void checkFlip(){
        if(target!=null && sp != null){
            if((target.transform.position.x > transform.position.x && !sp.flipX) || (target.transform.position.x < transform.position.x && sp.flipX)){
                sp.flipX = !sp.flipX;
                propSp.flipX = !propSp.flipX;
            }
        }
    }

    public virtual void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = s.speed;
        dmg = s.dmg;
        atkSpeed = s.atkSpeed;
    }
}
