using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otter : Enemy
{
    public GameObject Blocker;
    public Projectile fish;
    public Transform projectilePoint;
    private void Start()
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

    public override void UpdateEnemy()
    {
        if(sprouting){ return; }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        Move();
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !sprouting){
           Attacking = true;
           
           GetComponent<Animator>().SetTrigger("InRange");
           StartCoroutine(PlayAttackAnimation(AttackDelay));
        }
    }
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if (Flamey.Instance.current_homing == this && onHit && !sprouting && timer <= 0)
        {
            Sprout(true);
        }

        return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
    }
    bool sprouting;
    public float CDTimer = 5f;
    float timer;
    public void Sprout(bool on)
    {
        sprouting = on;
        Blocker.SetActive(on);
        GetComponent<Animator>().Play(on ? "Sprout" : "Walk");
        if (!on) { timer = CDTimer; }

    }
    public void Unsprout()
    {
        Sprout(false);
    }
    public override void Attack()
    {
        Projectile p = Instantiate(fish);
        p.AttackTarget = AttackTarget;
        p.armPen = ArmorPen;
        p.Damage = Damage;
        p.transform.position = projectilePoint.position;

        //base.Attack();
    }
}
