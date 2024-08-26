using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeWarrior : Bee
{
    protected override Enemy getTarget()
    {
        return Flamey.Instance.current_homing;
    }

    public override void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = .5f*s.speed;
        dmg = 4 * s.dmg;
        atkSpeed = .25f*s.atkSpeed;
    }
}
