using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeePollinator : Bee
{
    public IPoolable FlowerPrefab;
    protected override void Attack()
    {
        
        atkTimer = 1/atkSpeed;
        if(target==null){return;}
        Vector2 pos = target.HitCenter.position;
        ObjectPooling.Spawn(FlowerPrefab, new float[]{pos.x, pos.y});

    }
    public override void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = s.speed;
        dmg = s.dmg;
        atkSpeed = .3f*s.atkSpeed;
    }
}
