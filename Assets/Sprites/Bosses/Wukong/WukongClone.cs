using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class WukongClone : Enemy
{
    public bool Jumping;
    public SpriteRenderer sp;
    protected virtual void Start()
    {
        VirtualPreStart();
        if (!EnemySpawner.Instance.PresentEnemies.Contains(this))
        {
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;

        if (EnemySpawner.Instance.current_round >= 60)
        {
            int x = EnemySpawner.Instance.current_round;
            Speed *= (float)(Math.Pow(x - 30, 2) / 4000f) + 1f;
            Health = (int)(Health * (float)(Math.Pow(x-500, 2) / 350) + 1f) < 0 ? int.MaxValue : (int)(Health * (float)(Math.Pow(x-500, 2) / 350) + 1f);
            Armor = (int)(Armor * (float)(Math.Pow(x-500, 2) / 15f)) < 0 ? int.MaxValue : (int)(Armor * (float)(Math.Pow(x-500, 2) / 15f));
            //Speed *= (float)(Math.Pow(x, 2) / 4000f) + 1f;
            Damage = Math.Max(Damage, (int)(Damage * (float)(Math.Pow(x-500, 2) / 2500f) + 1f) < 0 ? int.MaxValue : (int)(Damage * (float)(Math.Pow(x-500, 2) / 2500f) + 1f));
        }
        MaxHealth = Health;

    }
    public void Jump(int n)
    {
        Jumping = n == 1;
    }
    public override void CheckFlip()
    {

        bool flipped = transform.position.x > 0;
        sp.flipX = flipped;
        transform.Find("Cloud").GetComponent<SpriteRenderer>().flipX = flipped;

    }
    public override void Move()
    {

        if (Jumping && !Attacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1 - SlowFactor) * Time.deltaTime);
        }
        if (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !Attacking)
        {
            Attacking = true;
            Jumping = false;
            
            GetComponent<Animator>().Play("AttackDisappear");
            
        }

        CheckFlip();

    }
  
    public void SpawnSmoke()
    {

        ObjectPooling.Spawn(ParticlesSmoke, new float[] { HitCenter.position.x, HitCenter.position.y });
    }
    public IPoolable ParticlesSmoke;
    public override void Die(bool onKill = true)
    {
        if (this == null)
        {
            return;
        }
        SpawnSmoke();
        base.Die(onKill);
    }
}
