using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAssassin : Bee
{
    protected override void Attack()
    {
        atkTimer = 1/atkSpeed;
        target.Hitted(dmg, 12, ignoreArmor:true, onHit:true);
    }
    public override void UpdateStats()
    {
        Summoner s = Summoner.Instance;
        speed = s.speed;
        dmg = s.dmg;
        atkSpeed = s.atkSpeed / 3;
    }
    protected override Enemy getTarget()
    {
        return Enemy.getPredicatedEnemy((e1,e2)=> e2.Health - e1.Health);
    }
}
