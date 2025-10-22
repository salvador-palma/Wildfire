using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boat : Enemy
{
    public GameObject[] enemiesToSpawn;
    public int spawnCount = 10;
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
            Damage = Math.Max(Damage, (int)(Damage * (float)(Math.Pow(x-50, 2) / 50f) + 1f) < 0 ? int.MaxValue : (int)(Damage * (float)(Math.Pow(x-50, 2) / 50f) + 1f));
        }
        MaxHealth = Health;

        enemiesToSpawn = new GameObject[spawnCount];
        string[] exceptions = new string[] { "Ant", "Mole", "Worm", "Owl", "Vulture", "Pelican" };
        GameObject[] available = EnemySpawner.Instance.PickedEnemies.Take(12).Where(e => !exceptions.Contains(e.Name)).Select(e => e.gameObject).ToArray();

        for (int i = 0; i < spawnCount; i++)
        {
            enemiesToSpawn[i] = available[UnityEngine.Random.Range(0, available.Length - 1)];
        }
    }
    public override void Attack()
    {

        base.Attack();
        Health = 0;
    }
    public override void UpdateEnemy()  {

        Move();
        
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange ){
            Attack();
        }


    }
    
    public override void Die(bool onKill = true)
    {
        if (onKill)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                try
                {
                    GameObject g = Instantiate(enemiesToSpawn[i]);
                    Enemy e = g.GetComponent<Enemy>();
                    g.transform.position = HitCenter.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0);
                    e.CheckFlip();
                    e.KnockBack(HitCenter.position, retracting: false, 0.5f, 1f);
                }
                catch
                {
                    Debug.Log("Error on Boat");
                }
                

            }
        }
        base.Die(onKill);
    }
    public override void CheckFlip()
    {
        transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = transform.position.x > 0;
        
    }
}
