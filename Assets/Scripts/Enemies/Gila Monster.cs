using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GilaMonster : Enemy
{
    // Start is called before the first frame update
    float timer;
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
            Health = (int)(Health * (float)(Math.Pow(x - 40, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x - 40, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 40, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;
        direction = UnityEngine.Random.Range(0f, 1f) < 0.5f ? -1 : 1;
    }
    int direction = 1;
    public override void UpdateEnemy()
    {
        base.UpdateEnemy();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = UnityEngine.Random.Range(1f, 3f);
            direction *= UnityEngine.Random.Range(0f, 1f) < 0.5f ? -1 : 1;

        }
    }
    public float angleStep;
    public float prevX;
    public override void Move()
    {

        if (Stunned) { return; }
        MoveSpiral(angleStep, direction == 1);
        CheckFlip();
        prevX = transform.position.x;
    }
    public int poisonInject;
    public override void Attack()
    {
        AttackTarget.Poison(poisonInject);
    }

    public override void CheckFlip()
    {

        GetComponent<SpriteRenderer>().flipX = HitCenter.position.x < AttackTarget.getPosition().x;
        transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = HitCenter.position.x < AttackTarget.getPosition().x;
    }
}
