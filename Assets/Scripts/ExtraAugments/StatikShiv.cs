using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatikShiv : MonoBehaviour
{
    public GameObject StatikPrefab;
    public GameObject StatikEmpoweredPrefab;
    public Enemy currentTarget;
    public Vector2 locationOfEnemy;
    public bool isPowered;
    float LineTimeFixed;
    public float LineTime = 100;
    public float NextDelayFixed;
    public float NextDelay = 100;
    public int TTL;
    public int MAXTTL;
    public int Damage = -1;
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
        
        NextDelay-=Time.deltaTime;
        if(NextDelay <= 0 && !NextCheck){
            NextCheck = true;  
            SpawnNext();
        }
    }

    private Enemy Next(){
        
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
            if(target == null){Destroy(gameObject); if(isPowered){Debug.Log("Early Recall");} return;}
            ActivateLine(target.HitCenter.position);
            GameObject go = Instantiate(isPowered ? StatikEmpoweredPrefab : StatikPrefab);
            
            go.transform.position = target.HitCenter.position;
            SetupNext(go.GetComponent<StatikShiv>(), target);
            DealDamage(target);

        }catch(Exception e){
            string str = e.StackTrace;
        }
        Destroy(gameObject);
        
    }
    private void ActivateLine(Vector3 pos){
        lineRenderer.SetPositions(new Vector3[]{locationOfEnemy, pos});
        lineRenderer.enabled = true;

    }

    private void DealDamage(Enemy e){   
        if(isPowered){
            Debug.Log("Powered");
            e.Hitted(Damage, 14, ignoreArmor: true, onHit: true);
        }else{
            e.Hitted(Damage, 6, ignoreArmor: false, onHit: SkillTreeManager.Instance.getLevel("Static Energy") >= 1 , except:"Static Energy");
        }
        
    }

    private void SetupNext(StatikShiv statikShiv, Enemy t){
        
        if(isPowered){
            statikShiv.isPowered = true;
        }

        statikShiv.alreadyPassed = alreadyPassed;
        statikShiv.TTL =  TTL - 1;
        statikShiv.MAXTTL = isPowered ? 30 : MAXTTL;
        statikShiv.Damage = SkillTreeManager.Instance.getLevel("Static Energy") >= 2 ? Damage : (int)(Damage * 0.9f);
        statikShiv.currentTarget = t;
        statikShiv.NextDelay = NextDelayFixed;
        statikShiv.LineTime = LineTimeFixed;
        statikShiv.locationOfEnemy = t.HitCenter.position;
        statikShiv.Started = true;
    }
}
