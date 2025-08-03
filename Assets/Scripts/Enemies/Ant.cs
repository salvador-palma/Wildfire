using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Ant : Enemy
{

    public static Vector2 spawnpoint;
    bool untargetable = true;
    private void Start()
    {



        VirtualPreStart();
        flame = Flamey.Instance;

        if (EnemySpawner.Instance.current_round >= 60)
        {
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float)(Math.Pow(x, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;

        if (nextAnts.Count(a => a != null  && a.untargetable) == 0)
        {
            nextAnts.Clear();
            spawnpoint = transform.position;
            Flamey.Instance.StartCoroutine(DelaySpawn(this));
        }
        nextAnts.Add(this);
        transform.position = new Vector2(100, 100);
  
        
        
    }
    static float SpawningDelay = 4.5f;
    public static List<Ant> nextAnts = new List<Ant>();
    static IEnumerator DelaySpawn(Ant ant)
    {
        yield return new WaitForSeconds(SpawningDelay);
        Spawn(ant);
    }
    public static void Spawn(Ant ant)
    {
        if (ant == null) return;
        ant.untargetable = false;
        ant.transform.position = spawnpoint;

        if (nextAnts.Count(a=>a!=null && a.untargetable) > 0)
        {
            
            Ant nextAnt = nextAnts.First(a=> a != null  && a.untargetable);
            nextAnts.Remove(nextAnt);
            Flamey.Instance.StartCoroutine(DelaySpawn(nextAnt));
        }
    }
    
    public override void Move()
    {
        if (untargetable) { return; }
        CheckFlip();
        base.Move();
    }
    public override bool canTarget()
    {
        return !untargetable;
    }
    public override void CheckFlip(){

           GetComponent<SpriteRenderer>().flipX = transform.position.x < 0;

        
    }


}
