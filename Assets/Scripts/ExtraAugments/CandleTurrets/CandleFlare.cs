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
        speedAscend = Flamey.Instance.BulletSpeed;
        speedDescend = 1.5f * speedAscend;
    }
    public void setPosition(Vector2 pos){
        transform.position = new Vector2(pos.x + Random.Range(-0.2f,0.2f), pos.y);
    }
    override public void DestroyGameObject(){
        Destroy(gameObject);
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
        FlareSpot = Instantiate(Flamey.Instance.FlareSpotPrefab);
        FlareSpot.transform.position = vec;
        FlareSpot.transform.localScale *= 0.35f;
    }

    override protected void HitGround(Vector2 vec){
        Flamey.Instance.ApplyOnLand(vec);

        Destroy(FlareSpot);
        

        Enemy[] colliders = Physics2D.OverlapCircleAll(vec, 0.5f, FlareManager.EnemyMask).Select(e=>e.GetComponent<Enemy>()).ToArray();
        if(colliders.Length > 0){
            GameObject g = Instantiate(EnemySpawner.Instance.ExplosionPrefab);
            g.transform.position = vec;
        }
        
        foreach(Enemy e in colliders){
            if(Character.Instance.isCharacter("Ritual") && e.Health < CandleTurrets.Instance.dmg){
                CandleTurrets.Instance.AddDamageTick();
            }
            e.Hitted(Damage, DmgTextID, ignoreArmor:false, onHit: true);
        }
    }

    
}
