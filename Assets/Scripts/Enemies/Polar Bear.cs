using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PolarBear : Enemy
{
    // Start is called before the first frame update
    public Transform projectilePoint;
    public Projectile iceCube;
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
            Health = (int)(Health * (float)(Math.Pow(x - 50, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x - 50, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 50, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;
    }

    public override void Attack()
    {
        Projectile p = Instantiate(iceCube);
        p.AttackTarget = AttackTarget;
        p.armPen = ArmorPen;
        p.Damage = Damage;
        p.transform.position = projectilePoint.position;

        //base.Attack();
    }

    public override void Stun(float f, string source = null) { }
    public override void Taunt(Hittable target){}
    public override void SlowDown(float seconds, float percentage, string SlowEffect)
    {
        
    }
    public override void KnockBack(Vector2 origin, bool retracting, float power, float time = 0.5F, bool stopOnOrigin = false, float angleMissStep = 0, float stopOnOriginMargin = 0.05F)
    {
        
    } 
}
