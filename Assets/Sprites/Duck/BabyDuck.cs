using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDuck : Enemy
{
    public Duck DuckParent;
    public int index;
    private void Start()
    {
        VirtualPreStart();
        flame = Flamey.Instance;

        MaxHealth = Health;
    }

    

    public override void Move()
    {
        if (Stunned || DuckParent == null) { return; }

        if (Vector2.Distance(transform.position, DuckParent.HitCenter.position) > DuckParent.babiesDist[index])
        {
            transform.position = Vector2.MoveTowards(transform.position, DuckParent.HitCenter.position, Speed * (1 - SlowFactor) * Time.deltaTime);
        }


    }
    public override void UpdateEnemy()
    {
        //Debug.Log("Baby!!!!");
        Move();
        CheckFlip();
    }
    public override void Die(bool onKill = true)
    {
        if (this == null) { return; }
        try
        {
            flame.TotalKills++;
            DuckParent.checkBabies();

            if (onKill) { Flamey.Instance.ApplyOnKill(HitCenter.position); }

            AudioManager.PlayOneShot(DeathSound, transform.position);

            if (Character.Instance.isCharacter("Gravity") && Gravity.Instance != null)
            {
                Gravity.Instance.AddMass(MaxHealth, Vector2.Distance(Flamey.Instance.getPosition(), HitCenter.transform.position));
            }
        }
        catch
        {
            // Debug.Log("Error at: Enemy.Die()");
        }

        Destroy(gameObject);
    }

    public override void CheckFlip()
    {
        if(DuckParent==null){ return; }
        GetComponent<SpriteRenderer>().flipX = DuckParent.HitCenter.position.x < 0;
    }
}
