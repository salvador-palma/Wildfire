using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sloth : Enemy
{

    public Enemy throwTarget;
    public Predicate<Enemy> validThrow;
    void Start()
    {
        VirtualPreStart();
        if (!EnemySpawner.Instance.PresentEnemies.Contains(this))
        {
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f, Speed, 0.075f);
        if (EnemySpawner.Instance.current_round >= 60)
        {
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float)(Math.Pow(x - 20, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x - 20, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 20, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;
        validThrow = new Predicate<Enemy>(e =>
        !e.Attacking &&
        !(Vector2.Distance(e.AttackTarget.getPosition(), e.HitCenter.position) <= e.AttackRange) &&
        !(e.Name == "Sloth") &&
        e.canTarget());
    }
    public float cdtimer;
    public bool returnCheck;
    public bool throwing;
    public bool inReturnCheck;
    public override void UpdateEnemy()
    {
        if (inReturnCheck || throwing) { return; }
        if (throwTarget == null || !validThrow(throwTarget))
        {
            throwTarget = getPredicatedEnemy(
                (a, b) => Vector2.Distance(a.HitCenter.position, HitCenter.position) < Vector2.Distance(b.HitCenter.position, HitCenter.position) ? -1 : 1,
                new List<Enemy> { this }, validThrow
                );

            if (throwTarget == null && !returnCheck)
            {
                returnCheck = true;
                inReturnCheck = true;
                GetComponent<Animator>().Play("Wtv");

                return;
            }
        }

        
        Move();
        if (!returning && Vector2.Distance(throwTarget.transform.position, HitCenter.position) < AttackRange)
        {
            throwing = true;
            GetComponent<Animator>().Play(AttackAnimationName);

        }

    }
    public void Return()
    {
        GetComponent<Animator>().Play("Walk");
        inReturnCheck = false;
        returning = true;
    }
    public void Throw()
    {
        StartCoroutine(ThrowC());
    }
    public IEnumerator ThrowC()
    {
        if (throwTarget != null)
        {
            Debug.Log("Throwing: " + throwTarget.Name);
            throwTarget.KnockBack(AttackTarget.getPosition(), power: 4f, retracting: true, time: 1f, stopOnOrigin: true, stopOnOriginMargin: throwTarget.AttackRange);
            yield return new WaitForSeconds(3f);
            throwTarget = null;
            GetComponent<Animator>().Play("Walk");
            throwing = false;
        }
    }

    public bool returning;
    public override void Move()
    {
        if (Stunned) { return; }
        if (returning)
        {
            transform.position = Vector2.MoveTowards(transform.position, Flamey.Instance.getPosition(), -Speed * (1 - SlowFactor) * Time.deltaTime);
            if (Math.Abs(transform.position.x) > 10f || Math.Abs(transform.position.y) > 6f)
            {
                Destroy(gameObject);
            }

        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, throwTarget.transform.position, Speed * (1 - SlowFactor) * Time.deltaTime);
        }

        CheckFlip();
        prevX = transform.position.x;

    }
    float prevX;
    public override void CheckFlip(){
        
        GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

        transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }
}
