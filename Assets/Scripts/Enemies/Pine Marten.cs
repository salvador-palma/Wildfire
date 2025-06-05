using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineMarten : Squirrel
{
    public Projectile accorn;
    public Transform projectilePoint;
    public int ProjectileDmg;
    public override void Attack()
    {

        placedBomb = true;
        Projectile p = Instantiate(accorn);
        p.AttackTarget = AttackTarget;
        p.armPen = ArmorPen;
        p.Damage = ProjectileDmg;
        p.transform.position = projectilePoint.position;
        p.afterHit = new System.Action(() =>
        {
            if(this==null || Health<=0){ return; }
            BombPrefab = Instantiate(BombPrefab);
            Vector3 direction = (HitCenter.position - (Vector3)AttackTarget.getPosition()).normalized;
            BombPrefab.transform.position = (Vector3)AttackTarget.getPosition() + direction * deltaBomb;

        });


        TurnBack();

        Attacking = false;
    }
}
