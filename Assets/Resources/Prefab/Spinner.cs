using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed;
    public static float multiplier;

    public GameObject[] Spinners;
    
    public bool canSpin;
    public float startingScale;
    protected virtual void Start() {
        
        speed = Flamey.Instance.BulletSpeed;
        if(!EnemySpawner.Instance.Paused){
            canSpin = true;
            if(FlameCircle.Instance.PlanetType==3){
                GetComponent<Animator>().enabled=true;
            }
        
        }
        Deck.RoundOver += SpinToggle;
        Deck.RoundStart += SpinTogglefalse;
       
        startingScale = transform.GetChild(0).localScale.x;
        float perc = 1.5f * Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        float percTrail = .55f * Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        foreach(Transform child in transform){
            child.localScale = new Vector3(startingScale*(1+perc),startingScale*(1+perc),startingScale*(1+perc));
            child.GetComponent<TrailRenderer>().startWidth = 0.25f + percTrail;
        }
    }

    private void SpinTogglefalse(object sender, EventArgs e)
    {
        float perc = 1.5f * Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        float percTrail = .55f * Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        foreach(Transform child in transform){
            child.localScale = new Vector3(startingScale*(1+perc),startingScale*(1+perc),startingScale*(1+perc));
            child.GetComponent<TrailRenderer>().startWidth = 0.25f + percTrail;
        }
        canSpin = true;
        
    }

    private void SpinToggle(object sender, EventArgs e)
    {
        canSpin = false;
    }

    private void Update() {
        if(canSpin){
            transform.Rotate(0,0,speed*multiplier*Time.deltaTime);
        }
        
    }

    public GameObject SpawnCircle(int amount){
        return Instantiate(Spinners[amount]);
    }
    public void kill(){
        Destroy(gameObject);
    }
    
}
