using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Hittable AttackTarget;
    public float speed;
    public float range;
    public bool LookAtTarget;
    public int Damage;
    public float armPen;
    public Action afterHit;

    void Update()
    {
        if (AttackTarget == null)
        {
            Destroy(gameObject);
        }
        if (LookAtTarget)
        {
            Vector2 direction = AttackTarget.getPosition() - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), speed * Time.deltaTime);

        if (Vector2.Distance(AttackTarget.getPosition(), transform.position) < range)
        {
            if(afterHit!=null){ afterHit(); }
            AttackTarget.Hitted(Damage, armPen, null, false);
            Destroy(gameObject);
        }
        
    }
}
