using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Flare : MonoBehaviour
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

    
    public int DmgTextID;
    public void VirtualStart(){
        Reset();
        SetupTarget();
        SetupStats();  
    }
    
    private void SetupTarget(){
        transform.position = new Vector2(UnityEngine.Random.Range(-0.4f,0.4f), 0);
        SpotColor.a = 0;
    }
    private void SetupStats(){
        speedAscend = Flamey.Instance.BulletSpeed;
        speedDescend = 1.5f * speedAscend;
       
    }
    private void Update() {
        
        if(goingDownPhase == 1){

            transform.position = new Vector2(transform.position.x, transform.position.y - speedDescend * Time.deltaTime);
            FlareSpotUpdate();

            if(transform.position.y < destY){
                goingDownPhase++;
                HitGround(transform.position);   
                         
                FlareManager.DestroyFlare(gameObject);
            }
            
        }else if(goingDownPhase==0){
            transform.position = new Vector2(transform.position.x, transform.position.y + speedAscend * Time.deltaTime);
            if(transform.position.y > YLimit){
                goingDownPhase++;
                goDown();
            }
        }
        
    }
   
    private void goDown(){
        if(target == Vector2.zero){
            Enemy e = Flamey.Instance.current_homing;
            if(e==null){FlareManager.DestroyFlare(gameObject);return;}
            else{target = e.HitCenter.position;}
        }
        

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
    private void SummonFlareSpot(Vector2 vec){
        FlareSpot = Instantiate(Flamey.Instance.FlareSpotPrefab);
        FlareSpot.transform.position = vec;
    }

    private void HitGround(Vector2 position){
        Flamey.Instance.ApplyOnLand(position);
        Destroy(FlareSpot.gameObject);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, FlareManager.EnemyMask);
        if(colliders.Length > 0){
            GameObject g = Instantiate(EnemySpawner.Instance.ExplosionPrefab);
            g.transform.position = transform.position;
        }
        
        foreach(Collider2D col in colliders){
            col.GetComponent<Enemy>().Hitted(Damage, DmgTextID);
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
        FlareSpot = null;
        destY = 0;


    }
    
    
}
