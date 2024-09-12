using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAgile : Bee
{
    public override void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = 2*s.speed;
        dmg = (int)(.5f * s.dmg);
        atkSpeed = 1.5f*s.atkSpeed;
    }
}
