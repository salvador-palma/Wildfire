using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class LaserBeam : MonoBehaviour
{

    public Enemy target;
    public float dmgIncrease = 1.05f;
    int stacks = 0;
    float Timer = 1f;
    float timerCounter = 0f;

    LineRenderer lineRenderer;
    GameObject particleBeam;
    Transform EnemyHitPoint;
    Transform StartingPoint;
    IPoolable Prefab;
    

    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        Prefab = Resources.Load<GameObject>("Prefab/LaserExplosion").GetComponent<IPoolable>();
        StartingPoint = transform.GetChild(0).GetChild(0).transform;
        particleBeam = transform.GetChild(0).GetChild(1).gameObject;
        EnemyHitPoint = transform.GetChild(0).GetChild(1).GetChild(2).transform;
    }

    Enemy getTarget()
    {
        Vector2 FlameyPos = Flamey.Instance.transform.position;
        List<Enemy> Current_enemies = Laser.Instance.CurrentTargets();
        switch (Laser.Instance.currentTargetingOption)
        {
            case 0:
                return Flamey.Instance.getRandomHomingEnemy();
            case 1:
                return Enemy.getPredicatedEnemy((a, b) => Vector2.Distance(a.HitCenter.position, FlameyPos) < Vector2.Distance(b.HitCenter.position, FlameyPos) ? -1 : 1, Current_enemies); 
            case 2:
                return Enemy.getPredicatedEnemy((e1, e2) => e2.MaxHealth - e1.MaxHealth, Current_enemies);
            case 3:
                return Enemy.getPredicatedEnemy((a, b) => Vector2.Distance(a.HitCenter.position, FlameyPos) < Vector2.Distance(b.HitCenter.position, FlameyPos) ? 1 : -1, Current_enemies);
            default:
                return Flamey.Instance.getRandomHomingEnemy();
        }
    }
    
    void Update()
    {
        if (EnemySpawner.Instance.isOnAugments) { target= null; }
        if (target == null)
        {
            lineRenderer.enabled = false;
            particleBeam.SetActive(false);
            target = getTarget();
            stacks = 0;
            if (target == null) return;
        }
        else
        {
            Vector2 direction = target.HitCenter.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle + 180f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50f * Time.deltaTime);



            float diff = transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z;
            bool active = Math.Abs(diff) <= 5f;
            lineRenderer.enabled = active;
            particleBeam.SetActive(active);



            lineRenderer.SetPosition(0, StartingPoint.position);
            lineRenderer.SetPosition(1, target.HitCenter.position);


            if (active)
            {
                EnemyHitPoint.position = target.HitCenter.position;
                timerCounter += Time.deltaTime;
                if (timerCounter >= Timer)
                {
                    target.Hitted((int)(Flamey.Instance.Dmg / Laser.Instance.amount * Math.Pow(dmgIncrease, stacks)), 2, ignoreArmor: false, onHit: false);
                    timerCounter = 0f;
                    stacks++;

                    if (Character.Instance.isCharacter("Laser Beam"))
                    {
                        RaycastHit2D[] hits = Physics2D.LinecastAll(StartingPoint.position, target.HitCenter.position, Flamey.EnemyMask);
                        foreach (RaycastHit2D hit in hits)
                        {
                            Enemy enemy = hit.collider.GetComponent<Enemy>();
                            if (enemy != null && enemy != target)
                            {
                                enemy.Hitted((int)(Flamey.Instance.Dmg / Laser.Instance.amount * Math.Pow(dmgIncrease, stacks)), 2, ignoreArmor: false, onHit: false);
                                
                            }
                        }
                    }
                    if (SkillTreeManager.Instance.getLevel("Laser Beam") >= 2)
                    {
                        if (stacks % 2 == 0)
                        {
                            Vector2 pos = target.HitCenter.position;
                            ObjectPooling.Spawn(Prefab, new float[] { pos.x, pos.y });
                            Flamey.Instance.ApplyOnKill(pos);
                        }
                    }

                }
            }



        }
    }
}
