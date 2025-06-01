using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BabyOpossum : Enemy
{
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

    public override void Die(bool onKill = true)
    {
        if (this == null) { return; }
        try
        {
            flame.TotalKills++;

            if (onKill) { Flamey.Instance.ApplyOnKill(HitCenter.position); }

            AudioManager.PlayOneShot(DeathSound, transform.position);

            if (Character.Instance.isCharacter("Gravity") && Gravity.Instance != null)
            {
                Gravity.Instance.AddMass(MaxHealth, Vector2.Distance(Flamey.Instance.getPosition(), HitCenter.transform.position));
            }
        }
        catch
        {
            // Debug.Log("Error at: Enemy.Die()");
        }

        Destroy(gameObject);
    }

}
