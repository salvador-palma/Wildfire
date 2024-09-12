
using System.Linq;
using UnityEngine;

public class Ghoul : MonoBehaviour
{
        
    public int remainingAttacks;
    [SerializeField] float speed;
    public float dmg;
    public float AtkInterval;
    public Enemy target;
    public bool isMega;
    public float radius;


    float AtkIntervalTimer;

    bool Spawning = true;
    SpriteRenderer sp;
    private void Start() {


        remainingAttacks = Necromancer.getAttackTimes();
        dmg = Flamey.Instance.Dmg * Necromancer.Instance.dmgPerc ;
        speed = 3.5f * ((Flamey.Instance.BulletSpeed -5)/15f) + 0.5f;
        AtkInterval = 2 - 1.75f*((1/Flamey.Instance.atkSpeed) - 1.333f)/(-1.25f);
        sp = GetComponentInChildren<SpriteRenderer>();
        sp.flipX = Random.Range(0f,1f) < 0.5f;
        if(isMega){Debug.Log("Mega Started");}
    }
    private void Update() {
        if(Spawning){return;}
        if(EnemySpawner.Instance.isOnAugments){Destroy(gameObject);}

        if(target == null){
            target = Enemy.getClosestEnemy(transform.position);
            if(target==null){return;}
        }


        if(AtkIntervalTimer > 0){
            AtkIntervalTimer -= Time.deltaTime;
        }else{
            if(remainingAttacks<=0){GetComponent<Animator>().Play("DespawnGhoul");return;}
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            checkFlip();
            if(Vector2.Distance(transform.position, target.transform.position) < 1.5f){
                GetComponent<Animator>().Play("GhoulAttack");
            }
        }
        
    }

    public void Die(){
        if(SkillTreeManager.Instance.getLevel("Necromancer")>=2){
            Flamey.Instance.ApplyOnKill(transform.position);
        }
        Destroy(gameObject);
    }
    
    private void Attack(){
        remainingAttacks--;
        if(target==null){return;}
        if(isMega){
            
            Enemy[] colcol = Physics2D.OverlapCircleAll(target.HitCenter.position, radius).Select(E => E.GetComponent<Enemy>()).ToArray();
            if(SkillTreeManager.Instance.getLevel("Necromancer")>=1 && remainingAttacks==0){
                
                foreach (Enemy enemy in colcol)
                {
                    enemy.Hitted((int)dmg, 8, ignoreArmor:true, onHit:false);
                }
                AtkIntervalTimer = AtkInterval;
                return;
            }
            
            foreach (Enemy enemy in colcol)
            {
                enemy.Hitted((int)dmg, 8, ignoreArmor:false, onHit:false);
            }
            AtkIntervalTimer = AtkInterval;
        }else{
            if(SkillTreeManager.Instance.getLevel("Necromancer")>=1 && remainingAttacks==0){
                target.Hitted((int)dmg, 13, ignoreArmor:true, onHit:false);
                AtkIntervalTimer = AtkInterval;
                return;
            }
            target.Hitted((int)dmg, 13, ignoreArmor:false, onHit:false);
            AtkIntervalTimer = AtkInterval;
        }
        
        
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
