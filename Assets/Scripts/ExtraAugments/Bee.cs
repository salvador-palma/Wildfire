using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public string Type;
    [TextArea] public string Description;
    public float speed;
    public float atkSpeed;
    public float atkTimer;
    public int dmg;

    public Enemy target;
    SpriteRenderer sp;
    SpriteRenderer propSp;
    EventInstance eventInstance;




    public virtual void Start()
    {
        sp = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        propSp = transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
        transform.position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

        eventInstance = AudioManager.CreateInstance(FMODEvents.Instance.BeeFlight);
        eventInstance.start();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (EnemySpawner.Instance.isOnAugments) {Move();return; }

        if (target == null)
        {
            target = getTarget();
            if (target == null) { Move(); return; }
        }
        if (Vector2.Distance(transform.position, target.HitCenter.position) >= 1f)
        {
            Move();
        }
       
        if (atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        else
        {
            if (Vector2.Distance(transform.position, target.HitCenter.position) < 1f)
            {
                GetComponent<Animator>().Play(sp.flipX ? "AttackReverse" : "Attack");
            }
        }
    }
    protected virtual void Move()
    {
        if (target != null && !EnemySpawner.Instance.isOnAugments)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.HitCenter.position, speed * Time.deltaTime);
        }
        else
        {
            float campfireRot = Time.frameCount * 0.1f * Time.timeScale % 360;
            int n = Summoner.Instance.bees.Count;
            float delta = 360 / n;
            float degrees = campfireRot + Summoner.Instance.bees.IndexOf(this) * delta;
            float radius = 2f;
            Vector2 v = new Vector2(radius, 0);
            float radians = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            Vector2 dest = new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
            transform.position = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);
        }
         checkFlip();

    }
    protected virtual Enemy getTarget()
    {
        return Flamey.Instance.getRandomHomingEnemy(true);


    }
    protected virtual void Attack()
    {

        atkTimer = 1 / atkSpeed;
        if (target == null) { return; }
        target.Hitted(dmg, 12, ignoreArmor: false, onHit: true);
    }
    float prevX;
    protected void checkFlip()
    {
        if (target != null && sp != null)
        {
            
                sp.flipX = target.transform.position.x > transform.position.x;
                propSp.flipX = target.transform.position.x > transform.position.x;
            
        }
        else
        {
            sp.flipX = prevX < transform.position.x;
            propSp.flipX = prevX < transform.position.x;
            prevX = transform.position.x;
        }
    }

    public virtual void UpdateStats()
    {
        Summoner s = Summoner.Instance;
        speed = s.speed;
        dmg = s.dmg;
        atkSpeed = s.atkSpeed;
    }

    private void OnDestroy()
    {
        eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
    public void Despawn()
    {
        Destroy(gameObject);
    }
}
