using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainLion : Enemy
{
    private void Start()
    {
        VirtualPreStart();
        if (!EnemySpawner.Instance.PresentEnemies.Contains(this))
        {
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        Speed = Distribuitons.RandomTruncatedGaussian(0.02f, Speed, 0.075f);
        if (EnemySpawner.Instance.current_round >= 60)
        {
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float)(Math.Pow(x - 40, 2) / 350) + 1f);
            Armor = (int)(Armor * (x - 45f) / 15f);
            Speed *= (float)(Math.Pow(x - 40, 2) / 4000f) + 1f;
            Damage = (int)(Damage * (float)(Math.Pow(x - 40, 2) / 2500f) + 1f);
        }
        MaxHealth = Health;
        Speed = RunningSpeed;

    }
    public float WalkingSpeed;
    public float RunningSpeed;
    public float circlingRadius;
    public float margin;
    public float surroundingTimeStart;
    float surroundingTime;
    public bool goingCircle = true;
    bool direction;
    float prevX;
    bool insideCircle;
    Vector2 moveBack;
    public override void UpdateEnemy()
    {
        if (goingCircle)
        {


            float distance = Vector2.Distance(AttackTarget.getPosition(), HitCenter.position);
            if (circlingRadius - margin < distance && circlingRadius + margin > distance || insideCircle)
            {
                //IN CIRCLE
                if (!insideCircle)
                {
                    insideCircle = true;
                    GetComponent<Animator>().Play("Walk");
                    surroundingTime = surroundingTimeStart;
                    Speed = WalkingSpeed;
                    direction = UnityEngine.Random.Range(0f, 1f) < .5f;
                }

                surroundingTime -= Time.deltaTime;
                if (surroundingTime <= 0)
                {
                    GetComponent<Animator>().Play("Run");
                    insideCircle = false;
                    goingCircle = false;
                    Speed = RunningSpeed;
                    moveBack = transform.position;
                    moveBack *= 100;
                }

                MoveSpiral(0, direction);
            }
            else if (circlingRadius - margin < distance && circlingRadius + margin < distance)
            {
                //OUTSIDE CIRCLE
                base.Move();
            }
            else if (circlingRadius - margin > distance && circlingRadius + margin > distance)
            {
                //INSIDE CIRCLE
                if (Stunned) { return; }
                transform.position = Vector2.MoveTowards(transform.position, moveBack, Speed * (1 - SlowFactor) * Time.deltaTime);
            }
        }
        else
        {
            Move();

            if (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange && !checker)
            {
                checker = true;
                GetComponent<Animator>().SetTrigger("InRange");
            }
        }
        CheckFlip();
        prevX = transform.position.x;

    }
    bool checker;
    public override void Attack()
    {
        goingCircle = true;
        GetComponent<Animator>().Play("Run");

        base.Attack();
        checker = false;
    }

    public override void CheckFlip()
    {
        GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
    }
    

   
}
