using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeePollinator : Bee
{
    public GameObject FlowerPrefab;
    protected override void Attack()
    {
        atkTimer = 1/atkSpeed;
        GameObject go = Flamey.Instance.SpawnObject(FlowerPrefab);
        go.transform.position = target.HitCenter.position;
    }
    public override void UpdateStats(){
        Summoner s = Summoner.Instance;
        speed = s.speed;
        dmg = s.dmg;
        atkSpeed = .2f*s.atkSpeed;
    }
}
