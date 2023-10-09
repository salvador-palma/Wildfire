using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatikShiv : MonoBehaviour
{
    public GameObject StatikPrefab;
    public Enemy currentTarget;
    public Vector2 locationOfEnemy;
    float LineTimeFixed;
    public float LineTime = 100;
    public float NextDelayFixed;
    public float NextDelay = 100;
    public int TTL;
    [SerializeField] List<Enemy> alreadyPassed;
    [SerializeField] Collider2D[] colcol;
    CircleCollider2D myCollider;
    LineRenderer lineRenderer;

    bool NextCheck;
    public bool Started;
    private void Start() {
        LineTimeFixed = LineTime;
        NextDelayFixed = NextDelay;
        myCollider = GetComponent<CircleCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update() {
        if(!Started){return;}
        LineTime-=Time.deltaTime;
        if(LineTime <= 0){
            Debug.Log("Statik Destroyed");
            //Destroy(gameObject);
        }

        NextDelay-=Time.deltaTime;
        if(NextDelay <= 0 && !NextCheck){
            NextCheck = true;
            
            SpawnNext();
        }
        
    }

    private Enemy Next(){
        if(currentTarget == null){Destroy(gameObject);}
        Vector2 overlapPosition = locationOfEnemy;
        
        colcol = Physics2D.OverlapCircleAll(overlapPosition, myCollider.radius);
        List<Enemy> collected = new List<Enemy>();
        
        foreach (Collider2D item in colcol)
        {
            if(item.tag != "Enemy"){continue;}
            Enemy e = item.GetComponent<Enemy>();
            if(!alreadyPassed.Contains(e)){
                collected.Add(e);
            }
        }
        if(collected.Count == 0){return null;}
        return collected[UnityEngine.Random.Range(0, collected.Count - 1)];
    }
    private void SpawnNext(){
        try{
            if(TTL <= 0){Destroy(gameObject);return;}
            if(alreadyPassed == null){alreadyPassed = new List<Enemy>();}
            alreadyPassed.Add(currentTarget);
            Enemy target = Next();
            if(target == null){Destroy(gameObject);return;}
            ActivateLine(target.HitCenter.position);
            DealDamage(target);
            GameObject go = Instantiate(StatikPrefab);
            go.transform.position = target.HitCenter.position;
            SetupNext(go.GetComponent<StatikShiv>(), target);
        }catch(Exception e){
            string str = e.StackTrace;
        }
        
    }
    private void ActivateLine(Vector3 pos){
        lineRenderer.SetPositions(new Vector3[]{currentTarget.HitCenter.position, pos});
        lineRenderer.enabled = true;

    }

    private void DealDamage(Enemy e){   
        e.HittedWithArmor(StatikOnHit.Instance.dmg, true, "Statik Energy");
    }

    private void SetupNext(StatikShiv statikShiv, Enemy t){
        statikShiv.alreadyPassed = alreadyPassed;
        statikShiv.TTL = TTL - 1;
        statikShiv.currentTarget = t;
        statikShiv.NextDelay = NextDelayFixed;
        statikShiv.LineTime = LineTimeFixed;
        statikShiv.locationOfEnemy = t.HitCenter.position;
        statikShiv.Started = true;
        
        Destroy(gameObject);
    }
}
