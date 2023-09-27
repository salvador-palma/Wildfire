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
    private Enemy target;
    private GameObject FlareSpot;
    private float destY;
    [SerializeField] Color SpotColor;
    private void Start() {
        SpotColor.a = 0;
        speedAscend = Flamey.Instance.BulletSpeed;
        speedDescend = 1.5f * speedAscend;
        target = Flamey.Instance.current_homing;
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
        Vector2 vt = target.transform.position;
        float Accuracy = Flamey.Instance.Accuracy;
        Vector2 v = new Vector2(RandomGaussian(Accuracy, vt.x), RandomGaussian(Accuracy, vt.y) - 0.4f);
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
        FlareSpot.transform.position = vec;
    }
    private void FlareSpotHit(){
        FlareSpot.GetComponent<CircleCollider2D>().enabled = true;
        FlareSpot.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        FlareSpot.GetComponent<AutoDestroy>().StartCD(1f);
    }
    private void FlareSpotUpdate(){
        SpotColor.a = 0.6f - Vector2.Distance(transform.position, FlareSpot.transform.position)/6;
        FlareSpot.GetComponent<SpriteRenderer>().color = SpotColor;
    }

    
    private float RandomGaussian(float variance, float mean){
        float u1 = 1.0f- UnityEngine.Random.value; 
        float u2 = 1.0f- UnityEngine.Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); 
        double randNormal = mean + variance * randStdNormal; 
        
        return (float)randNormal;
    }
}
