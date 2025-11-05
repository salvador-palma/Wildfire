using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Duck : Enemy
{
    public GameObject babyPrefab;
    public BabyDuck[] babies;
    public float[] babiesDist;
    public bool agressive;
    private void Start()
    {
        VirtualPreStart();
        flame = Flamey.Instance;
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f, Speed, 0.075f);
        if (EnemySpawner.Instance.current_round >= 60)
        {
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float)(Math.Pow(x, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;

        babies = new BabyDuck[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject g = Flamey.Instance.SpawnObject(babyPrefab);
            g.transform.position = HitCenter.position;
            babies[i] = g.GetComponent<BabyDuck>();
            babies[i].DuckParent = this;
            babies[i].index = i;
        }
    }

    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        
        int n = base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
        return n;
        
    }

    public override void UpdateEnemy()
    {
        base.UpdateEnemy();
    }

    public override void Die(bool onKill = true)
    {
        foreach (BabyDuck item in babies)
        {
            if(item==null){ continue; }
            item.Health = 0;
        }
        base.Die();
    }
    bool xcheck;
    public void checkBabies()
    {
        if (babies.Count(e => e.Health > 0 ) <= 0 && !xcheck)
        {
            xcheck = true;
            agressive = true;
            GetComponent<Animator>().Play("Run");
            Speed *= 3;
            Damage *= 2;
        }
    }
}
