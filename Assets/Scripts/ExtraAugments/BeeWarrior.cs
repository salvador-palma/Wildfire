using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeWarrior : Bee
{
    protected override Enemy getTarget()
    {
        if(Random.Range(0f,1f)<.5f){
            return Flamey.Instance.getHoming(1);
        }else{
            return Flamey.Instance.current_homing;
        }
        
    }

    public override void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = 0.5f*s.speed;
        dmg = 4 * s.dmg;
        atkSpeed = .75f*s.atkSpeed;
    }
}
