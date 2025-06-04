using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Vulture : Enemy
{
    [Range(0f, 1f)] public float arenaLand;
    public float startingHeight;
    public bool flying = true;
    public Vector2 LandDest;
    public float cos;

    public Enemy EatingEnemy;

    public GameObject CorpsePrefab;
    public float PlaneSpeed;
    public float GoingSpeed;
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
        Speed = PlaneSpeed;
        transform.position = new Vector2(transform.position.x, transform.position.y + (float)(startingHeight * Math.Pow(cos, 3)));
        direction = UnityEngine.Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        if (!EnemySpawner.Instance.isOn && EnemySpawner.Instance.PresentEnemies.Count(e => e.Health > 0 && e.Name != "Vulture") == 0)
        {
            CallToLand();
        }
    }


    public override void UpdateEnemy()
    {
        Move();

        if (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !flying)
        {
            Attacking = true;

            GetComponent<Animator>().SetTrigger("InRange");
            StartCoroutine(PlayAttackAnimation(AttackDelay));
        }

        if (flying && Vector2.Distance(LandDest, transform.position) < 0.3f)
        {
            Land();
        }
    }

    int direction;
    public override void Move()
    {
        if (Stunned) { return; }
        prevX = transform.position.x;
        if (!flying)
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1 - SlowFactor) * Time.deltaTime);
        }
        else
        {
            if (!called)
            {
                MoveSpiral(0, direction == 1);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, LandDest, GoingSpeed * (1 - SlowFactor) * Time.deltaTime);
            }

        }
        CheckFlip();
        

    }
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if (!flying)
        {
            return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
        }
        return 0;
    }
    public override bool canTarget() { return !flying; }
    private void Land()
    {

        if (corpse == null)
        {
            FinishEating();
        }
        else
        {
             GetComponent<Animator>().Play("Eat");
        }
       
    }
    public void FinishEating()
    {
        flying = false;
        Destroy(corpse);
        GetComponent<Animator>().Play("Walk");
    }


    public static void CallVultures(Enemy en, int N)
    {
        List<Vulture> flyingVultures = EnemySpawner.Instance.PresentEnemies.Where(e => e.Name == "Vulture" && !((Vulture)e).called).Select(e => (Vulture)e).ToList();

        if (flyingVultures == null) { return; }

        Vulture[] picks;
        //SE N HOUVER MAIS INIMIGOS

        //SE N HOUVER VULTURES SUFICIENTES

        //SE HOUVER TUDO SUFICIENTE
        // if (EnemySpawner.Instance.PresentEnemies.Count(e => e != en && e.Health > 0 && e.Name != "Vulture") == 0)
        // {
        //     picks = flyingVultures.ToArray();
        // }
        // else
        // {
            picks = flyingVultures.OrderBy(_ => UnityEngine.Random.Range(0f, 1f)).Take(N).ToArray();
        // }

        Array.ForEach(picks, v => v.CallToBody(en));

    }
    public bool called;

    public void CallToLand()
    {
        if (called) { return; }
        GetComponent<Animator>().Play("Fly");
        called = true;
        LandDest = Vector2.zero;
        LandDest.x += (transform.position.x - Vector2.zero.x) * arenaLand;
        LandDest.y += (transform.position.y - Vector2.zero.y) * arenaLand;
    }
    GameObject corpse;
    public void CallToBody(Enemy en)
    {
        if (called) { return; }
        GetComponent<Animator>().Play("Fly");

        corpse = Instantiate(CorpsePrefab);
        corpse.transform.position = en.HitCenter.position;

        called = true;
        
        Speed = en.Speed;
        Damage += en.Damage;
        AttackDelay = en.AttackDelay;
        Armor += en.Armor;
        MaxHealth += en.MaxHealth;
        Health = MaxHealth;

        EatingEnemy = en;
        LandDest = en.HitCenter.position;
    }
    float prevX;
    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }
}
