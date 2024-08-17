using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Worm : Enemy
{
    [Range(0f,1f)] public float DigUpDistance;
    public bool diggingUp;
    public bool isUnderground = true;
    public float diggingDelay = 2f;
    public float undergroundSpeedMult;
    public LineRenderer lineRenderer;
    public GameObject TrailPrefab;

    public float initialDistance;
    public  Vector3 initialPos;
    Transform pathEnd;
    void Start()
    {


        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x, 2)/5000f) + 1f);
        }
        MaxHealth = Health;

        pathEnd = transform.Find("PathEnd").transform;
        initialDistance = Vector2.Distance(flame.transform.position, HitCenter.position);


        initialPos = pathEnd.position;

        GameObject g = Instantiate(TrailPrefab);
        lineRenderer = g.GetComponent<LineRenderer>();
        lineRenderer.positionCount =2;
        lineRenderer.SetPositions(new Vector3[2]{initialPos, initialPos});
        if(transform.position.x < 0){
            lineRenderer.textureScale = new Vector2(lineRenderer.textureScale.x, -1);
        }
    }

    // Update is called once per frame
    override public void UpdateEnemy() {
           
        if(!diggingUp){
            base.UpdateEnemy();
        }
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < initialDistance * DigUpDistance && isUnderground){
            StartCoroutine(DigUp());
        }
        if(isUnderground){
            UpdateLineRenderer();
        }
    }

    public void UpdateLineRenderer(){
        lineRenderer.SetPosition(1, pathEnd.position);
        lineRenderer.SetPosition(0, initialPos);
        
    }
    public override void Move()
    {
        if(Stunned){return;}
        if(IceOnLand.Instance != null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 1){
            if(getSlowInfo("IceLand")[0] <= 0){
                transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * (1-SlowFactor) * Time.deltaTime * (isUnderground ? undergroundSpeedMult : 1f));
            }
        }else{
            transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * (1-SlowFactor) * Time.deltaTime * (isUnderground ? undergroundSpeedMult : 1f));
        }
        
    }

    IEnumerator DigUp(){
        isUnderground = false;
        diggingUp = true;
        GetComponent<Animator>().Play("DigOut");
        SpawnHole();
        yield return new WaitForSeconds(diggingDelay);
        diggingUp= false;
        lineRenderer.GetComponent<Animator>().Play("TrailOff");
        

        
    }

    private void SpawnHole(){
        Transform g = lineRenderer.transform.GetChild(0);
        g.position = transform.position;
        if(transform.position.x < 0){g.GetComponent<SpriteRenderer>().flipX = true;}
        g.gameObject.SetActive(true);

    }
    protected override void OnMouseDown()
    {
        if(isUnderground){return;}
        base.OnMouseDown();
    }

    public override bool canTarget()
    {
        return !isUnderground && !diggingUp;
    }

    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null)
    {
        if(!isUnderground)
        { base.Hitted(Dmg, TextID, ignoreArmor, onHit, except);}
        else if(isUnderground && SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 && source != null && source.Equals("Lava Pool")){
        base.Hitted(Dmg, TextID, ignoreArmor, onHit, except);
        }
    }

    public override void CheckFlip()
    {
        if(transform.position.x < 0){
            Transform tr = transform.Find("PathEnd");
            tr.localPosition = new Vector2(-tr.localPosition.x, tr.localPosition.y);
        }
        
        base.CheckFlip();
    }


    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
