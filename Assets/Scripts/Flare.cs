using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class Flare : MonoBehaviour
{
    private bool goingDown;
    private float speedAscend;
    private float speedDescend;
    private float YLimit = 10f;
    private Vector2 target;
    private GameObject FlareSpot;
    private float destY;
    [SerializeField] Color SpotColor;
    private void Start() {
        float val = transform.localScale.x * Flamey.Instance.BulletSize;
        transform.localScale = new Vector2(val,val);
        SpotColor.a = 0;
        speedAscend = Flamey.Instance.BulletSpeed;
        speedDescend = 1.5f * speedAscend;
        target = Flamey.Instance.current_homing.transform.position;
    }

    private void Update() {
        if(goingDown){
            transform.position = new Vector2(transform.position.x, transform.position.y - speedDescend * Time.deltaTime);
            FlareSpotUpdate();
            if(transform.position.y < destY){
                
                FlareSpotHit();
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

        float Accuracy = Flamey.Instance.Accuracy;
        Vector2 v = new Vector2(Distribuitons.RandomGaussian(Accuracy, target.x), Distribuitons.RandomGaussian(Accuracy, target.y - 0.4f));
        setPosition(v);
        SummonFlareSpot(v);
        goingDown = true;
        
    }
    private void setPosition(Vector2 dest){
        transform.position = new Vector2(dest.x, transform.position.y);
        destY = dest.y;
    }
    private void SummonFlareSpot(Vector2 vec){
        FlareSpot = Instantiate(Flamey.Instance.FlareSpotPrefab);
        
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

    
    
}
