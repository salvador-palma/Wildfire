using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aligator : Enemy
{
    public float StunTime;
    // Start is called before the first frame update
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }

    // Update is called once per frame
    public override void Attack()
    {
        Flamey.Instance.Stun(StunTime);
        base.Attack();
    }

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
