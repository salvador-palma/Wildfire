using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ant : Enemy
{
    public Vector2 spawnpoint;
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


        Enemy e = EnemySpawner.Instance.PresentEnemies.FirstOrDefault(e => e.Name == "Ant" && e != this);
        if (e != null)
        {
            transform.position = ((Ant)e).spawnpoint;
            Speed = ((Ant)e).Speed * 0.9f;
        }
        spawnpoint = transform.position;
    }


}
