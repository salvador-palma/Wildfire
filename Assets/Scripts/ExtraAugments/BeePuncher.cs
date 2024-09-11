using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePuncher : Bee
{
    protected override void Attack()
    {
        if(target==null){return;}
        Flamey.Instance.ApplyOnHit(dmg, 0, target.GetComponent<Enemy>());
        Flamey.Instance.ApplyOnHit(dmg, 0, target.GetComponent<Enemy>());
        base.Attack();
    }
    protected override Enemy getTarget()
    {
        return Enemy.getClosestEnemy(transform.position, Random.Range(0,2));
    }
    
}
