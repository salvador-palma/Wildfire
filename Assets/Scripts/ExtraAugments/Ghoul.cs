
using UnityEngine;

public class Ghoul : MonoBehaviour
{
        
    public int remainingAttacks;
    [SerializeField] float speed;
    public float dmg;
    public float AtkInterval;
    public Enemy target;


    float AtkIntervalTimer;

    bool Spawning = true;
    SpriteRenderer sp;
    private void Start() {


        remainingAttacks = Necromancer.AtkTimes;
        dmg = Flamey.Instance.Dmg * Necromancer.Instance.dmgPerc;
        speed = 3.5f*((Flamey.Instance.BulletSpeed -5)/15f) + 0.5f;
        AtkInterval = 2 - 1.75f*((1/Flamey.Instance.atkSpeed) - 1.333f)/(-1.25f);
        sp = GetComponentInChildren<SpriteRenderer>();
        sp.flipX = Random.Range(0f,1f) < 0.5f;
    }
    private void Update() {
        if(Spawning){return;}
        if(EnemySpawner.Instance.isOnAugments){Die();}

        if(target == null){
            target = Enemy.getClosestEnemy(transform.position);
            if(target==null){return;}
        }


        if(AtkIntervalTimer > 0){
            AtkIntervalTimer -= Time.deltaTime;
        }else{
            if(remainingAttacks<=0){Die();}
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            checkFlip();
            if(Vector2.Distance(transform.position, target.transform.position) < 1.5f){
                Attack();
            }
        }
        
    }

    private void Die(){
        Destroy(gameObject);
    }
    private void Attack(){
        remainingAttacks--;
        target.HittedWithArmor((int)dmg, true, 10);
        AtkIntervalTimer = AtkInterval;
        
    }
    private void checkFlip(){
        if(target!=null && sp != null){
            if((target.transform.position.x > transform.position.x && !sp.flipX) || (target.transform.position.x < transform.position.x && sp.flipX)){
                sp.flipX = !sp.flipX;
            }
        }
    }
    public void SetSpawn(){
        Spawning = false;
    }

}
