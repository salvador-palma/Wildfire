using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : Enemy
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
    void Start()
    {


        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
        initialDistance = Vector2.Distance(flame.transform.position, HitCenter.position);


        initialPos = HitCenter.position;

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
        lineRenderer.SetPosition(1, HitCenter.position);
        lineRenderer.SetPosition(0, initialPos);
        
    }
    public override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime * (isUnderground ? undergroundSpeedMult : 1f));
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

    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null)
    {
        
        if(!isUnderground){ base.Hitted(Dmg, TextID, ignoreArmor, onHit, except);}
    }

   
}