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
        
        speed = Flamey.Instance.BulletSpeed * Gambling.getGambleMultiplier(1);
        if(!EnemySpawner.Instance.Paused){
            canSpin = true;
            if(FlameCircle.Instance.PlanetType==3){
                GetComponent<Animator>().enabled=true;
            }
        
        }
        Deck.RoundOver += SpinToggle;
        Deck.RoundStart += SpinTogglefalse;
       
        startingScale = transform.GetChild(0).localScale.x;

        int isJupiter = FlameCircle.Instance.PlanetType == 4 ? 2 : 1;

        float perc = 1.5f * Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        float percTrail = .55f * Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        foreach(Transform child in transform){
            child.localScale = new Vector3(startingScale*(1+perc)*isJupiter,startingScale*(1+perc)*isJupiter,startingScale*(1+perc)*isJupiter);
            child.GetComponent<TrailRenderer>().startWidth = 0.25f + percTrail;
        }
    }
    void OnDestroy()
    {
        Deck.RoundOver -= SpinToggle;
        Deck.RoundStart -= SpinTogglefalse;
    }
    private void SpinTogglefalse(object sender, EventArgs e)
    {

        int isJupiter = FlameCircle.Instance.PlanetType == 4 ? 2 : 1;

        float MaxHPFactor = Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002105f, 0, 1f);
        float perc = 1.5f * MaxHPFactor;
        float percTrail = .55f * MaxHPFactor;
        foreach(Transform child in transform){
            child.localScale = new Vector3(startingScale*(1+perc)*isJupiter,startingScale*(1+perc)*isJupiter,startingScale*(1+perc)*isJupiter);
            child.GetComponent<TrailRenderer>().startWidth = 0.25f + percTrail;
        }

        if(MaxHPFactor >= 1f){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(40,"Betsy",19); //JUPITER UNLOCK
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
