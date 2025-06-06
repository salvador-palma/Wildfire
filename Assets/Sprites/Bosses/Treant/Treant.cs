using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class Treant : Boss
{
    public Enemy Branch;
    public List<TreantBranch> Branches;

    protected override void Start()
    {
        base.Start();
        NextPhase();

    }

    public override bool canTarget()
    {
        return Phase != 0;
    }
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if (Phase == 0 && (source ==null || source!="Branch")) { return 0; }
        return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
    }
    public override void UpdateEnemy()
    {
        Move();
    }
    public override void Move()
    {
        if (JumpingBack)
        {
            Vector2 vec = Vector2.MoveTowards(transform.position, Vector2.left, -Speed * 2 * (1 - SlowFactor) * Time.deltaTime);
            if (Vector2.Distance(Vector2.zero, vec) <= 8f)
            {
                transform.position = vec;
            }

        }
        if (Stunned) { return; }
        if (Jumping)
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1 - SlowFactor) * Time.deltaTime);
        }
        
        

    }
    public void DeleteBranch(TreantBranch branch)
    {
        Hitted(branch.MaxHealth, 0, true, false, source:"Branch");

        if (Branches.Count(b => b != null && b.Health > 0) <= 0)
        {
            GetComponent<Animator>().Play("Awaken");
        }
    }
    public void NextPhase()
    {
        Phase++;
        switch (Phase)
        {
            case 0:
                float angleMargin = 30f;
                int amount = 8;
                float angleGap = (360f - angleMargin * 2) / amount;

                for (int i = 0; i < amount; i++)
                {
                    Vector2 v = EnemySpawner.Instance.getPointAngle(angleMargin + i * angleGap, 0.4f);
                    v.y += 2.5f;
                    Enemy e = Instantiate(Branch);
                    ((TreantBranch)e).Tree = this;
                    e.transform.position = v;
                    Branches.Add((TreantBranch)e);

                }
                break;



            case 1:

                StartCoroutine(Phase1());
                break;
        }
    }
    public bool Jumping;
    public bool JumpingBack;
    public void Jump(int n)
    {
        Jumping = n == 1;
    }
    public void JumpBack(int n)
    {
        JumpingBack = n == 1;
    }
    public IEnumerator Phase1()
    {
        GetComponent<Animator>().Play("JumpBack");
        while (Health > 0)
        {


            yield return new WaitForSeconds(10f);
            if (this == null) { yield return null; }


            while (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) > AttackRange)
            {
                GetComponent<Animator>().Play("Jump");
                //yield return new WaitForSeconds(Random.Range(5f, 10f));

                float startTime = Time.time;
                yield return new WaitUntil(() => Time.time - startTime >= 10f || Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange);
                if (this == null) { yield return null; }
            }
            GetComponent<Animator>().Play("Hit");


            if (this == null) { yield return null; }


        }
    }
    public void Roar()
    {
        Enemy[] available = EnemySpawner.Instance.PickedEnemies.Take(6).ToArray();
        int amount = 15;
        for (int i = 0; i < amount; i++)
        {
            Enemy spawn = available[Random.Range(0, available.Length - 1)];
            Enemy en = Instantiate(spawn);

            en.transform.position = EnemySpawner.Instance.getPointAngle(360f / amount * i);
            en.CheckFlip();
        }
    }
}
