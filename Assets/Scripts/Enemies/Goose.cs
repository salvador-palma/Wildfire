using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel.Design;

public class Goose : Enemy
{

    public bool active;
    // Start is called before the first frame update
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
    }


    public void Activate()
    {
        
        if (active) { return; }
        active = true;
        GetComponent<Animator>().Play("Run");
        Speed *= 3f;

    }
    public float radius;
    public override void Die(bool onKill = true)
    {
        Collider2D[] AnimalAround = Physics2D.OverlapCircleAll(HitCenter.position, radius, Flamey.EnemyMask);
        
        foreach (Collider2D col in AnimalAround)
        {
            Enemy e = col.GetComponent<Enemy>();
            if (e == null || e == this) { continue; }
            if (e is Goose)
            {
                ((Goose)e).Activate();
            }
        }
        base.Die(onKill);
    }

    
}
