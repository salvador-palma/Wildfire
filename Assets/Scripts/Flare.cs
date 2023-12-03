using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine;

public class Flare : MonoBehaviour
{
    private bool goingDown;
    private float speedAscend;
    private float speedDescend;
    private float YLimit = 10f;
    private Vector2 target;
    private GameObject FlareSpot;
    
    public int Damage;
    public bool isCrit;
    private float destY;
    [SerializeField] Color SpotColor;

    public int FlameType;
    public int DmgTextID;
    private void Start() {
        SetupTarget();
        SetupStats();
        //float val = transform.localScale.x * Flamey.Instance.BulletSize;
        //transform.localScale = new Vector2(val,val);
        
        

    }
    private void SetupTarget(){
        transform.position = new Vector2(UnityEngine.Random.Range(-0.4f,0.4f), transform.position.y);
        SpotColor.a = 0;
    }
    private void SetupStats(){
        speedAscend = Flamey.Instance.BulletSpeed;
        speedDescend = 1.5f * speedAscend;

        Damage = (int)GetDmgByType(FlameType);


    }
    private void Update() {
        if(goingDown){
            transform.position = new Vector2(transform.position.x, transform.position.y - speedDescend * Time.deltaTime);
            FlareSpotUpdate();
            if(transform.position.y < destY){
                
                FlareSpotHit();
                Flamey.Instance.ApplyOnLand(transform.position);
                Destroy(gameObject);
                
            }
            
        }else{
            transform.position = new Vector2(transform.position.x, transform.position.y + speedAscend * Time.deltaTime);
            if(transform.position.y > YLimit){
                goDown();
            }
        }
        
    }
   
    private void goDown(){
        if(target == Vector2.zero){
            Enemy e = Flamey.Instance.current_homing;
            if(e==null){Destroy(gameObject);return;}
            else{target = e.HitCenter.position;}
        }
        
        //try for 10 times
        float Accuracy = Flamey.Instance.Accuracy;
        Vector2 v = new Vector2(Distribuitons.RandomGaussian(Accuracy, target.x), Distribuitons.RandomGaussian(Accuracy, target.y ));
        transform.localRotation = new Quaternion(0f,0f,0f,0f);
        setPosition(v);
        SummonFlareSpot(v);
        goingDown = true;
        
    }
    private void setPosition(Vector2 dest){
        transform.position = new Vector2(dest.x, transform.position.y + dest.y);
        destY = dest.y;
    }
    private void SummonFlareSpot(Vector2 vec){
        FlareSpot = Instantiate(Flamey.Instance.FlareSpotPrefab);
        
        FlareSpot.GetComponent<FlareSpot>().Dmg = Damage;
        FlareSpot.GetComponent<FlareSpot>().DmgTextID = DmgTextID;
       // Debug.Log(Damage + " " + isCrit);
       // Debug.Log(FlareSpot.GetComponent<FlareSpot>().DmgCrit);
        FlareSpot.transform.localScale = new Vector2(FlareSpot.transform.localScale.x * Flamey.Instance.BulletSize,FlareSpot.transform.localScale.y * Flamey.Instance.BulletSize);
        FlareSpot.transform.position = vec;
    }
    private void FlareSpotHit(){
        FlareSpot.GetComponent<CircleCollider2D>().enabled = true;
        FlareSpot.GetComponent<SpriteRenderer>().enabled = false;
        FlareSpot.GetComponent<AutoDestroy>().StartCD(0.5f);

    }
    private void FlareSpotUpdate(){
        SpotColor.a = 0.6f - Vector2.Distance(transform.position, FlareSpot.transform.position)/6;
        FlareSpot.GetComponent<SpriteRenderer>().color = SpotColor;
    }

    public void setTarget(Vector2 v){
        target = v;
    }

    
    public static float GetDmgByType(int type){
        Flamey f = Flamey.Instance;
        switch(type){
            case 0: return f.Dmg;
            case 1: return f.Dmg * CritUnlock.Instance.mult;
            case 2: return f.Dmg + KrakenSlayer.Instance.extraDmg;
            case 3: return (f.Dmg + KrakenSlayer.Instance.extraDmg) * CritUnlock.Instance.mult;
            default: return f.Dmg;
        }
    }
    
}
