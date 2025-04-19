using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public string Type;
    public float speed;
    public float atkSpeed;
    public float atkTimer;
    public int dmg;

    public Enemy target;
    SpriteRenderer sp;
    SpriteRenderer propSp;
    EventInstance eventInstance;


    public virtual void Start() {
        sp = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        propSp = transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
        transform.position = new Vector2(Random.Range(-5f,5f), Random.Range(-5f,5f));

        eventInstance = AudioManager.CreateInstance(FMODEvents.Instance.BeeFlight);
        eventInstance.start();
    }
    
    // Update is called once per frame
    public virtual void  Update()
    {
        if(EnemySpawner.Instance.isOnAugments){return;}

        if(target == null){
            target = getTarget();
            if(target==null){return;}
        }
        if(Vector2.Distance(transform.position, target.HitCenter.position) >= 1f){
            Move();
        }
        checkFlip();
        if(atkTimer > 0){
            atkTimer -= Time.deltaTime;
        }else{
            if(Vector2.Distance(transform.position, target.HitCenter.position) < 1f){
                GetComponent<Animator>().Play(sp.flipX ? "AttackReverse" : "Attack");
            }
        }
    }
    protected virtual void Move(){
        transform.position = Vector2.MoveTowards(transform.position, target.HitCenter.position, speed * Time.deltaTime);
    }
    protected virtual Enemy getTarget(){
        return Flamey.Instance.getRandomHomingEnemy(true);
        
         
    }
    protected virtual void Attack(){
        
        atkTimer = 1/atkSpeed;
        if(target==null){return;}
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

    private void OnDestroy() {
        eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
}
