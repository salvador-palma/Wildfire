using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineMarten : Squirrel
{
    public Projectile accorn;
    public Transform projectilePoint;
    public int ProjectileDmg;
    public bool hasThrow;
    public override void Attack()
    {
        if (!Attacking) { return; }
        hasThrow = true;
        Projectile p = Instantiate(accorn);
        p.AttackTarget = AttackTarget;
        p.armPen = ArmorPen;
        p.Damage = ProjectileDmg;
        p.transform.position = projectilePoint.position;
        p.afterHit = new System.Action(() =>
        {
            if (this == null || Health <= 0) { return; }
            BombPrefab = Instantiate(BombPrefab);
            Vector3 direction = (HitCenter.position - (Vector3)AttackTarget.getPosition()).normalized;
            BombPrefab.transform.position = (Vector3)AttackTarget.getPosition() + direction * deltaBomb;
            placedBomb = true;



        });


        TurnBack();

        Attacking = false;
    }
    
    public override void Move(){
        if(Stunned){return;}
        if (hasThrow)
        {
            transform.position = Vector2.MoveTowards(transform.position, Flamey.Instance.getPosition(), Speed* (1-SlowFactor)  * Time.deltaTime * -1f);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed* (1-SlowFactor)  * Time.deltaTime);
        }
        
    }
}
