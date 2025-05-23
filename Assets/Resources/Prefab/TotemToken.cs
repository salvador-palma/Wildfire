using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public interface Hittable
{
    public abstract void Hitted(int Dmg, float armPen, Enemy attacker, bool onhitted = true, bool isShake = true, int idHitTxt = 2);
    public abstract Vector2 getPosition();
    public abstract void Poison(int ticks);
    public bool Unhittable();
    public bool isEqual(GameObject other);
    public bool isOriginal();
}
public class TotemToken : IPoolable, Hittable
{
    public Sprite[] sprites;
    public float Health;
    public Transform HitPoint;
    public float radius;
    public int stacks;

    public override string getReference()
    {
        return "Totem";
    }
    public override void Pool()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        HitPoint = transform.GetChild(1).transform;
        Timer = 1f;
        unhitable = false;
    }

    public void Hitted(int Dmg, float armPen, Enemy attacker, bool onhitted = true, bool isShake = true, int idHitTxt = 2)
    {
        if (onhitted && SkillTreeManager.Instance.getLevel("Totem") >= 1) { Flamey.Instance.ApplyOnHitted(attacker, this, Dmg); }
        if(SkillTreeManager.Instance.getLevel("Totem") >= 2){ Flamey.Instance.addHealth(Dmg); }
        DamageUI.InstantiateTxtDmg(transform.position, "-" + Math.Abs(Dmg), 26);
        Health -= Dmg;
        if (Health <= 0)
        {
            UnPool();
        }

    }
    bool unhitable;
    public bool Unhittable()
    {
        return unhitable;
    }
    float Timer = 1f;
    public void Update()
    {   
        if (EnemySpawner.Instance.isOnAugments) { UnPool(); }
        if (Health > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Timer = 1f;

                Health -= 2;
                Taunt();
            }

        }else
        {
            UnPool();
        }

    }

    public override void UnPool()
    {
        unhitable = true;
        foreach (Enemy item in EnemySpawner.Instance.PresentEnemies)
        {
            if(item == null) { continue; }
           
            if (item.attack_target==null || item.attack_target.isEqual(gameObject))
            {

                item.AttackTarget = Flamey.Instance;
            }
        }
        stacks = 0;
        base.UnPool();
    }
    public void Taunt()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(HitPoint.position, radius);
        foreach (Collider2D target in targets)
        {
            if (target.GetComponent<Enemy>() != null)
            {
                target.GetComponent<Enemy>().Taunt(this);
            }
            if(target.GetComponent<TotemToken>() != null && target != gameObject)
            {
                target.GetComponent<TotemToken>().Stack(this);
            }
        }

    }

    public Vector2 getPosition()
    {
        return HitPoint.position;
    }

    public void Poison(int ticks)
    {
        Health = 0;
        UnPool();
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
        radius = args[2] * 2;

        Vector3 scale = new Vector3(args[2], args[2], args[2]);
        transform.localScale = scale;

        Health = args[3];
    }
    public void Stack(TotemToken totem)
    {
        if (Health <= 0 || totem == this) { return; }
        

        bool thisBetter = totem.stacks <= stacks;
        if (!thisBetter)
        {
            totem.Stack(this);
            return;
        }
        stacks++;
        Health += Totem.Instance.Health;

        float size = Totem.Instance.radius;
        float multFactor = 1f + Math.Clamp(stacks * 0.05f, 0f, 0.5f);
        radius = size * multFactor * 2f; 

        Vector2 scale = new Vector2(size*multFactor, size*multFactor);
        transform.localScale = scale;

        totem.UnPool();
    }

    public bool isEqual(GameObject other)
    {
        return other == gameObject;
    }

    public bool isOriginal()
    {
        return false;
    }
}
