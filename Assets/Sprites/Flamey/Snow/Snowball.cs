using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Snowball : MonoBehaviour
{

    Vector2 direction;
    public float LifeSpan = 10f;
    public float Speed = 5f;
    public float Scale = 1.1f;
    void Start()
    {
        getTarget();
    }
    void getTarget()
    {

        Enemy target = Flamey.Instance.current_homing;
        if (target == null)
        {
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        }
        else
        {
            direction = (Vector2)(target.HitCenter.position - Vector3.zero).normalized;
        }
        spinSpeed *= direction.x > 0 ? -1 : 1;

    }
    // Update is called once per frame
    public float spinSpeed;
    List<Enemy> enemiesHit = new List<Enemy>();

    float snowDropTick = 0.25f;
    void Update()
    {
        transform.position += (Vector3)direction * Speed * Time.deltaTime;
        Vector2 localScale = transform.localScale;
        localScale *= 1 + (Scale * Time.deltaTime);
        transform.localScale = localScale;


        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        LifeSpan -= Time.deltaTime;
        if (LifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3.07f * transform.localScale.x);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null && !enemiesHit.Contains(enemy) && enemy.canTarget())
                    {
                        enemiesHit.Add(enemy);
                        enemy.Stun(4f);
                        enemy.KnockBack(transform.position, false, 2f);
                    }
                }
            }
        }
        if (snowDropTick <= 0f)
        {
            snowDropTick = 0.25f;
            ObjectPooling.Spawn(IceOnLand.Instance.prefab, new float[]{transform.position.x, transform.position.y});
        }
        else
        {
            snowDropTick -= Time.deltaTime;
        }
        
    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy"))
    //     {
    //         Debug.Log("Snowball hit enemy: " + collision.gameObject.name);
    //         Enemy enemy = collision.GetComponent<Enemy>();
    //         enemy.Stun(2f);
    //         enemy.KnockBack(transform.position, false, 0.5f);
    //     }
    // }
}
