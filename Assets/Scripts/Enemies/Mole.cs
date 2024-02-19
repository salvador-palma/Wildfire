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

    public float initialDistance;
    void Start()
    {

        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
        initialDistance = Vector2.Distance(flame.transform.position, HitCenter.position);

    }

    // Update is called once per frame
    override public void UpdateEnemy() {
           
        if(!diggingUp){
            base.UpdateEnemy();
        }
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < initialDistance * DigUpDistance && isUnderground){
            StartCoroutine(DigUp());
        }
    }

    public override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime * (isUnderground ? undergroundSpeedMult : 1f));
    }

    IEnumerator DigUp(){
        isUnderground = false;
        diggingUp = true;
        GetComponent<Animator>().Play("DigOut");
        yield return new WaitForSeconds(diggingDelay);
        diggingUp= false;
    }

    protected override void OnMouseDown()
    {
        if(isUnderground){return;}
        base.OnMouseDown();
    }

    public override bool canTarget()
    {
        return !isUnderground;
    }

    public override void Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null)
    {
        if(!isUnderground){ base.Hitted(Dmg, TextID, ignoreArmor, onHit, except);}
    }
}
