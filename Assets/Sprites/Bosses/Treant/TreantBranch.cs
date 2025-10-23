using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreantBranch : Enemy
{
    public Treant Tree;
    public override void Die(bool onKill = true)
    {
        try
        {
            
        Tree.DeleteBranch(this);
        }catch{}
        base.Die(onKill);
    }
    public override void KnockBack(Vector2 origin, bool retracting, float power, float time = 0.5F, bool stopOnOrigin = false, float angleMissStep = 0, float stopOnOriginMargin = 0.05F)
    {

    }

    void Start()
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
            Health = (int)(Health * (float)(Math.Pow(x - 50, 2) / 350) + 1f) < 0 ? int.MaxValue : (int)(Health * (float)(Math.Pow(x - 50, 2) / 350) + 1f);
            Armor = (int)(Armor * (float)(Math.Pow(x-500, 2) / 15f)) < 0 ? int.MaxValue : (int)(Armor * (float)(Math.Pow(x-500, 2) / 15f));
            Speed *= (float)(Math.Pow(x - 20, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 20, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;
        timer = UnityEngine.Random.Range(0f, 5f);
    }
    float timer;
    public override void UpdateEnemy()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {

            timer = UnityEngine.Random.Range(3f, 7f) + (EnemySpawner.Instance.RoundTotalSpentTime/30f);
            
            Enemy[] available = EnemySpawner.Instance.PickedEnemies.Take(6).ToArray();
            Enemy spawnable = available[UnityEngine.Random.Range(0, available.Length)];
            Enemy e = Instantiate(spawnable);
            e.transform.position = HitCenter.transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
            e.CheckFlip();
        }
    }
}
