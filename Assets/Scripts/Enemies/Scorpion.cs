using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpion : Enemy
{
    public bool isPoisonous;
    public int poisonTicks;
    // Start is called before the first frame update
    void Start()
    {
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }

    public override void Attack(){
        if(isPoisonous){
            flame.Poison(poisonTicks);
        }
        base.Attack();
        
    }
}
