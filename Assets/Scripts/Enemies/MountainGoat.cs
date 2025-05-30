using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainGoat : Enemy
{
    private void Start()
    {

        VirtualPreStart();

        base.flame = Flamey.Instance;

        Speed = Distribuitons.RandomTruncatedGaussian(0.02f, Speed, 0.075f);
        if (EnemySpawner.Instance.current_round >= 60)
        {
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float)(Math.Pow(x - 30, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x - 30, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 30, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;

    }
    public bool leaving;
    public Vector2 headingOutVec;
    public override void UpdateEnemy()
    {

        Move();

        if (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !leaving)
        {
            Attack();
            headingOutVec = AttackTarget.getPosition() - (Vector2)HitCenter.position;
            headingOutVec *= 100;
            leaving = true;
        }
        if ((Math.Abs(transform.position.x) > 10f || Math.Abs(transform.position.y) > 6f) && leaving)
        {
            Destroy(gameObject);
        }


    }
    public override void Move()
    {
        if(Stunned){return;}

        if (leaving)
        {
            transform.position = Vector2.MoveTowards(transform.position, headingOutVec, Speed * (1 - SlowFactor) * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1 - SlowFactor) * Time.deltaTime);
        }
    }
    

}
