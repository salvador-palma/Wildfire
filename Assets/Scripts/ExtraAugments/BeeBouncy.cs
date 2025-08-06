using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BeeBouncy : Bee
{
    protected override Enemy getTarget()
    {
        return Flamey.Instance.getHoming(Summoner.Instance.bees.Where(e=>e.Type=="Bouncy").ToList().IndexOf(this)+1);
    }

    public override void UpdateStats()
    {
        Summoner s = Summoner.Instance;
        speed = 0.5f * s.speed;
        atkSpeed = .5f * s.atkSpeed;
    }

    protected override void Attack()
    {
        atkTimer = 1 / atkSpeed;
        if (target == null) { return; }
        target.KnockBack(Vector2.zero, retracting: false, 2f, 0.5f);
        target = null;
        
    }
}
