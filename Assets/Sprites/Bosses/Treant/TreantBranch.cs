using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantBranch : Enemy
{
    public Treant Tree;
    public override void Die(bool onKill = true)
    {
        Tree.DeleteBranch(this);
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
            Health = (int)(Health * (float)(Math.Pow(x - 20, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x - 20, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 20, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;
    }
}
