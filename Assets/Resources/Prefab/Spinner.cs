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
       
        
    }

    private void SpinTogglefalse(object sender, EventArgs e)
    {
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
