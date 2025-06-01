using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Owl : Enemy
{
    public Enemy carriage;
    public SpriteRenderer carriageRenderer;


    [Range(0f, 1f)] public float arenaLand;
    public float startingHeight;
    public bool flying = true;
    public float flyingSpeedRatio;
    public Vector2 LandDest;
    public float cos;


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


        carriage = pickCarriage();
        carriageRenderer.sprite = carriage.GetComponent<SpriteRenderer>().sprite;

        LandDest = Vector2.zero;
        LandDest.x += (transform.position.x - Vector2.zero.x) * arenaLand;
        LandDest.y += (transform.position.y - Vector2.zero.y) * arenaLand;
        transform.position = new Vector2(transform.position.x, transform.position.y + (float)(startingHeight * Math.Pow(cos, 3)));
    }

    private Enemy pickCarriage()
    {
        string[] exceptions = new string[] { "Mole", "Worm" };
        Enemy[] available = EnemySpawner.Instance.PickedEnemies.Take(6).Where(e => !exceptions.Contains(e.Name)).ToArray();

        return available[UnityEngine.Random.Range(0, available.Length - 1)];
        
    }

    public override void UpdateEnemy()
    {
        // LandDest =  Flamey.Instance.transform.position;
        // LandDest.x += (transform.position.x - Flamey.Instance.transform.position.x) * arenaLand;
        // LandDest.y += (transform.position.y - Flamey.Instance.transform.position.y) * arenaLand;
        Move();
        
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !flying){
           Attacking = true;
           
           GetComponent<Animator>().SetTrigger("InRange");
           StartCoroutine(PlayAttackAnimation(AttackDelay));
        }
        if (flying && Vector2.Distance(LandDest, transform.position) < 0.3f)
        {
            Land();
        }
    }

    public override void Move()
    {
        if (Stunned) { return; }
        if (!flying)
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1 - SlowFactor) * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, LandDest, Speed * (1 - SlowFactor) * flyingSpeedRatio * Time.deltaTime);
        }

    }
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if (!flying)
        {
            return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
        }
        return 0;
    }
    public override bool canTarget(){return !flying;}
    private void Land()
    {
        flying = false;
        GetComponent<Animator>().Play("Walk");
        carriageRenderer.sprite = null;

        Enemy e = Instantiate(carriage);
        e.transform.position = carriageRenderer.transform.position;
        e.CheckFlip();
    }


    
}
