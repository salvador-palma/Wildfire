using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flare : IPoolable
{

    public int goingDownPhase;
    
    public float speedAscend;
    public float speedDescend;
    private static float YLimit = 10f;
    public Vector2 target;
    public GameObject FlareSpot;
    
    public int Damage;
    public float destY;
    [SerializeField] public Color SpotColor;
    [SerializeField] public GameObject FlareSpotPrefab;

    
    public int DmgTextID;
    
    private void SetupTarget(){
        transform.position = new Vector2(UnityEngine.Random.Range(-0.4f,0.4f), 0);
        SpotColor.a = 0;
    }
    private void SetupStats(){
        speedAscend = Flamey.Instance.BulletSpeed * Gambling.getGambleMultiplier(1);
        speedDescend = 1.5f * speedAscend;
       
    }
    private void Update() {
        
        if(goingDownPhase == 1){

            transform.position = new Vector2(transform.position.x, transform.position.y - speedDescend * Time.deltaTime);
            
            FlareSpotUpdate();

            if(transform.position.y < destY){
                
                goingDownPhase++;
                HitGround(FlareSpot.transform.position);   
                DestroyGameObject(); 
            }
            
        }else if(goingDownPhase==0){
            transform.position = new Vector2(transform.position.x, transform.position.y + speedAscend * Time.deltaTime);
            if(transform.position.y > YLimit){
                goingDownPhase++;
                goDown();
            }
        }else{
           DestroyGameObject();

        }
        
    }
    virtual public void DestroyGameObject(){
        UnPool();
        if(FlareSpot!=null){FlareSpot.GetComponent<IPoolable>().UnPool();FlareSpot = null;}


    }
   
    virtual public void GetTarget(){
        if(target == Vector2.zero){
            Enemy e = Flamey.Instance.current_homing;
            if(e==null){DestroyGameObject();return;}
            else{target = e.HitCenter.position;}
        }
    }
    private void goDown(){

        GetTarget();
        if(target==null){return;}
        float Accuracy = Flamey.Instance.Accuracy;
        Vector2 v = new Vector2(Distribuitons.RandomGaussian(Accuracy, target.x), Distribuitons.RandomGaussian(Accuracy, target.y ));
        transform.localRotation = new Quaternion(0f,0f,0f,0f);
        setPosition(v);
        SummonFlareSpot(v);
        
        
    }
    private void setPosition(Vector2 dest){
        transform.position = new Vector2(dest.x, transform.position.y + dest.y);
        destY = dest.y;
    }
    virtual protected void SummonFlareSpot(Vector2 vec){
        FlareSpot = ObjectPooling.Spawn(FlareSpotPrefab.GetComponent<IPoolable>(), new float[]{vec.x, vec.y});
    }

    virtual protected void HitGround(Vector2 vec){
        Flamey.Instance.ApplyOnLand(vec);

        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(vec, 0.5f, Flamey.EnemyMask);
        if(colliders.Length > 0){

            ObjectPooling.Spawn(EnemySpawner.Instance.ExplosionPrefab, new float[]{vec.x, vec.y});
    
            if(SecondShot.Instance != null && SkillTreeManager.Instance.getLevel("Multicaster") >= 2 && UnityEngine.Random.Range(0f, 1f) < 0.1f){
           
                Flare f = Flamey.Instance.InstantiateShot(new List<string>(){"Multicaster"});
                f.transform.position = vec;
                f.setTarget(Flamey.Instance.getRandomHomingPosition());
                

            }

        }
        
        foreach(Collider2D col in colliders){
            if(col.tag=="Nature"){
                col.GetComponent<Nature>().Destroy();
            }else{
                col.GetComponent<Enemy>().Hitted(Damage, DmgTextID, ignoreArmor:false, onHit: true);
            }
            
        }
    }
    private void FlareSpotUpdate(){
        SpotColor.a = 0.6f - Vector2.Distance(transform.position, FlareSpot.transform.position)/6;
        
        FlareSpot.GetComponent<SpriteRenderer>().color = SpotColor;
    }

    public void setTarget(Vector2 v){
        target = v;
    }

    public void Reset(){
        goingDownPhase=0;
        target=Vector2.zero;
        
        
        destY = 0;
    }

    public override string getReference()
    {
        return "Flare";
    }

    public override void Pool()
    {
        transform.localRotation = new Quaternion(0f,0f,180f,0f);
        Reset();
        SetupTarget();
        SetupStats();
    }

    public override void Define(float[] args)
    {
        FlareType flareData = Flamey.Instance.FlareTypes[(int)args[0]];
        GetComponent<SpriteRenderer>().color = flareData.FlareColor;
       
        DmgTextID = flareData.DmgTextID;
        SpotColor = flareData.FlareColor;

        Damage = (int)GetDmgByType((int)args[0]);

        int[] critVals = new int[]{1,2,4,5,7,8};
        if(critVals.ToList().Contains((int)args[0])){
           if(Damage >= Flamey.Instance.Dmg * 5f){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(18, "Rowl", 17); //POWERED UP UNLOCK
            }
        }

        ParticleSystem.MainModule main = GetComponentInChildren<ParticleSystem>().main;
        main.startColor = new ParticleSystem.MinMaxGradient(flareData.ParticleColors[0], flareData.ParticleColors[1]);
    }

    public static float GetDmgByType(int type){
        Flamey f = Flamey.Instance;
        switch(type){
            case 0: return f.Dmg;
            case 1: return f.Dmg * CritUnlock.Instance.mult;
            case 2: return f.Dmg * CritUnlock.Instance.mult * CritUnlock.Instance.mult;
            case 3: return f.Dmg + KrakenSlayer.Instance.extraDmg;
            case 4: return (f.Dmg + KrakenSlayer.Instance.extraDmg) * CritUnlock.Instance.mult;
            case 5: return (f.Dmg + KrakenSlayer.Instance.extraDmg) * CritUnlock.Instance.mult * CritUnlock.Instance.mult;
            case 6: return (f.Dmg + KrakenSlayer.Instance.extraDmg) * 5;
            case 7: return (f.Dmg + KrakenSlayer.Instance.extraDmg) * 5 * CritUnlock.Instance.mult;
            case 8: return (f.Dmg + KrakenSlayer.Instance.extraDmg) * 5 * CritUnlock.Instance.mult * CritUnlock.Instance.mult;
            default: return f.Dmg;
        }
    }

}

[System.Serializable]
public class FlareType {
    public int DmgTextID;
    public Color FlareColor;
    public Color SpotColor;
    public Color[] ParticleColors;
}
