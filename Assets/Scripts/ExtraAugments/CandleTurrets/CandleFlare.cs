using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandleFlare : Flare
{

    private void Start() {
        Damage = CandleTurrets.Instance.dmg;
        DmgTextID = 11;
        SpotColor.a = 0;
        speedAscend = Flamey.Instance.BulletSpeed * Gambling.getGambleMultiplier(1); 
        speedDescend = 1.5f * speedAscend;
    }
    public void setPosition(Vector2 pos){
        transform.position = new Vector2(pos.x + Random.Range(-0.2f,0.2f), pos.y);
    }
   
    public override void GetTarget()
    {
        if(target == Vector2.zero){
            Enemy e = Flamey.Instance.getRandomHomingEnemy();
            if(e==null){DestroyGameObject();return;}
            else{target = e.HitCenter.position;}
        }
    }

    protected override void SummonFlareSpot(Vector2 vec)
    {
        FlareSpot = Instantiate(FlareSpotPrefab);
        FlareSpot.transform.position = vec;
        FlareSpot.transform.localScale *= 0.35f;
    }

    override protected void HitGround(Vector2 vec){
        Flamey.Instance.ApplyOnLand(vec);

        Destroy(FlareSpot);
        

        Enemy[] colliders = Physics2D.OverlapCircleAll(vec, 0.5f, Flamey.EnemyMask).Select(e=>e.GetComponent<Enemy>()).ToArray();
        if(colliders.Length > 0){
            ObjectPooling.Spawn(EnemySpawner.Instance.ExplosionPrefab, new float[]{vec.x, vec.y});
            
        }
        
        foreach(Enemy e in colliders){
            if(e == null) continue;
            if(Character.Instance.isCharacter("Ritual") && e.Health < CandleTurrets.Instance.dmg){
                CandleTurrets.Instance.AddDamageTick();
            }
            e.Hitted(Damage, DmgTextID, ignoreArmor:false, onHit: true);
        }
    }


    public override string getReference()
    {
        return "CandleFlare";
    }

    public override void Pool()
    {
        transform.localRotation = new Quaternion(0f,0f,180f,0f);
        Reset();
        GetTarget();

        Damage = CandleTurrets.Instance.dmg;
        DmgTextID = 11;
        SpotColor.a = 0;
        speedAscend = Flamey.Instance.BulletSpeed * Gambling.getGambleMultiplier(1);
        speedDescend = 1.5f * speedAscend;
    }

    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
        // FlareType flareData = Flamey.Instance.FlareTypes[(int)args[0]];
        // GetComponent<SpriteRenderer>().color = flareData.FlareColor;
       
        // DmgTextID = flareData.DmgTextID;
        // SpotColor = flareData.FlareColor;
        // Damage = (int)GetDmgByType((int)args[0]);
        // ParticleSystem.MainModule main = GetComponentInChildren<ParticleSystem>().main;
        // main.startColor = new ParticleSystem.MinMaxGradient(flareData.ParticleColors[0], flareData.ParticleColors[1]);
    }

    

    
}
