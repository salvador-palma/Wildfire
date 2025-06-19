using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wukong : Boss
{

    public Predicate<Enemy> validThrow;

    protected override void Start()
    {
        base.Start();
        Phase = 0;
        NextPhase();
        validThrow = new Predicate<Enemy>(e =>
        !e.Attacking &&
        !(Vector2.Distance(e.AttackTarget.getPosition(), e.HitCenter.position) <= e.AttackRange) &&
        e.canTarget() &&
        e != this &&
        e.Name != "Sloth");

    }
    private Enemy GetThrowTarget()
    {
        return getPredicatedEnemy(
            (a, b) => UnityEngine.Random.Range(-1,2),
                new List<Enemy> { this }, validThrow
                );
    }
    public int MaxHitsToFlee = 10;
    int hitsToFlee = 0;
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if (Phase == 0 && onHit && !JumpingBack && Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) >= AttackRange && !Roaring && !Avoiding)
        {
            hitsToFlee++;
            if (hitsToFlee == MaxHitsToFlee)
            {

                StartCoroutine(Avoid());
            }
        }

        return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
    }
    float timeCounter;
    public float trailTime = 0.5f;
    public GameObject fader;
    public SpriteRenderer sp;
    public override void UpdateEnemy()
    {
        Move();
        if (Trailing)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= trailTime)
            {
                timeCounter = 0;
                GameObject f = Instantiate(fader, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity) as GameObject;
                SpriteRenderer faderSprite = f.GetComponent<SpriteRenderer>();
                faderSprite.sprite = sp.sprite;
                faderSprite.flipX = sp.flipX;
            }
        }
    }
    public bool Trailing;
    public bool Avoiding;
    public override void Move()
    {
        if (JumpingBack)
        {
            Vector2 vec = Vector2.MoveTowards(transform.position, transform.position.x > 0 ? Vector2.left : Vector2.right, -Speed * 3 * (1 - SlowFactor) * Time.deltaTime);
            if (Vector2.Distance(Vector2.zero, vec) <= 8f)
            {
                transform.position = vec;
            }
            return;
        }
        if (Stunned) { return; }
        if (Avoiding)
        {
            MoveSpiral(reverse: fleeDirection, Speed: Speed * 2);
        }
        else if (Jumping)
        {
            if (Phase != 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1 - SlowFactor) * Time.deltaTime);
            }
            else
            {
                if (throwTarget == null || !validThrow(throwTarget))
                {
                    throwTarget = GetThrowTarget();
                    if (throwTarget == null)
                    {
                        return;
                    }
                }
                transform.position = Vector2.MoveTowards(transform.position, throwTarget.HitCenter.position, Speed * 3 * (1 - SlowFactor) * Time.deltaTime);
            }
        }

        CheckFlip();



    }
    public override void CheckFlip()
    {

        bool flipped = transform.position.x > 0;
        sp.flipX = flipped;
        transform.Find("Cloud").GetComponent<SpriteRenderer>().flipX = flipped;

    }

    public void NextPhase()
    {
        Phase++;
        switch (Phase)
        {
            case 0:

                StartCoroutine(Phase0());
                break;

            case 1:

                StartCoroutine(Phase1());
                break;
        }
    }
    public bool Jumping;
    public bool JumpingBack;
    public void Jump(int n)
    {
        Jumping = n == 1;
    }
    public void JumpBack(int n)
    {
        JumpingBack = n == 1;
    }
    bool fleeDirection; // true = left, false = right
    public IEnumerator Avoid()
    {
        Avoiding = true;
        Trailing = true;
        fleeDirection = UnityEngine.Random.Range(0f, 1f) < 0.5f;
        GetComponent<Animator>().Play("CloudHop");
        yield return new WaitForSeconds(6f);
        hitsToFlee = 0;
        Avoiding = false;
        Trailing = false;
        GetComponent<Animator>().Play("CloudHopOff");
    }
    bool Roaring;
    public IEnumerator Phase0()
    {
        Jumping = false;
        GetComponent<Animator>().Play("Jump");
        while (Health > 2 * MaxHealth / 3)
        {
            Debug.Log("Phase 0");
            yield return new WaitForSeconds(8f);
            if (this == null) { break; }
            if (EnemySpawner.Instance.PresentEnemies.Count <= 1)
            {
                Roaring = true;
                GetComponent<Animator>().Play("Roar");
                yield return new WaitForSeconds(25f);
                if (this == null) { break; }
                Roaring = false;
            }
            JumpingBack = false;

            while (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) > AttackRange)
            {
                if (!Avoiding)
                {
                    GetComponent<Animator>().Play("Dash");
                }

                yield return new WaitUntil(() => Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange || Health > 2 * MaxHealth / 3);
                if (this == null || Health <= 2 * MaxHealth / 3) { break; }
            }
            if (this == null) { break; }
            if (!Avoiding)
            {
                Jumping = false;
                GetComponent<Animator>().Play("Attack");
            }

        }
        Debug.Log("Phase 0 complete");
        NextPhase();


    }
    int AvailableEnemies()
    {
        return EnemySpawner.Instance.PresentEnemies.Count(e => validThrow(e));
    }
    public IEnumerator Phase1()
    {

        GetComponent<Animator>().Play("Jump");
        yield return new WaitForSeconds(2f);
        if (this == null) { yield return null; }
        JumpingBack = false;

        while (Health > 0)
        {
            yield return new WaitForSeconds(2f);
            if (this == null) { yield return null; }

            while (throwTarget == null)
            {
                if (AvailableEnemies() <= 0)
                {
                    GetComponent<Animator>().Play("Roar");
                    yield return new WaitForSeconds(10f);
                    if (this == null) { yield return null; }
                }
                throwTarget = GetThrowTarget();
            }

            if (Vector2.Distance(throwTarget.HitCenter.position, HitCenter.position) < AttackRange)
            {
                GetComponent<Animator>().Play("Throw");
            }
            else
            {
                Trailing = true;
                GetComponent<Animator>().Play("CloudHop");
                while (throwTarget != null && Vector2.Distance(throwTarget.HitCenter.position, HitCenter.position) > AttackRange)
                {

                    yield return new WaitUntil(() => throwTarget == null || Vector2.Distance(throwTarget.HitCenter.position, HitCenter.position) < AttackRange || Health <= 0);
                    if (this == null || Health <= 0 ) { break; }

                    GetComponent<Animator>().Play("CloudHopOff");
                    Trailing = false;
                    Jumping = false;
                    yield return new WaitForSeconds(1f);
                    if (this == null) { yield return null; }
                    
                    if (throwTarget != null)
                    {
                        GetComponent<Animator>().Play("Throw");
                    }                    
                }
            }
            
            
        }
    }
    public void Roar()
    {
        Enemy[] available = EnemySpawner.Instance.PickedEnemies.Skip(6).Take(9).ToArray();
        int amount = 5;
        for (int i = 0; i < amount; i++)
        {
            Enemy spawn = available[UnityEngine.Random.Range(0, available.Length - 1)];
            Enemy en = Instantiate(spawn);

            en.transform.position = EnemySpawner.Instance.getPointAngle(360f / amount * i);
            en.CheckFlip();
        }
    }
    public Enemy throwTarget;
    public void Throw()
    {

        if (throwTarget == null)
        {
            return;
        }
        throwTarget.KnockBack(AttackTarget.getPosition(), power: 4f, retracting: true, time: 1f, stopOnOrigin: true, stopOnOriginMargin: throwTarget.AttackRange);
    }

}

