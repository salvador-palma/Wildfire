using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSpinner : Spinner
{
    // Start is called before the first frame update
    protected override void Start()
    {
        speed = 1f;
        if(!EnemySpawner.Instance.Paused){
            canSpin = true;        
        }
        Deck.RoundStart += StartMoving;
        Deck.RoundOver += StopMoving;
    }

    private void StopMoving(object sender, EventArgs e)
    {
        canSpin = false;
    }

    private void StartMoving(object sender, EventArgs e)
    {
        canSpin = true;
    }
    void OnDestroy()
    {
        Debug.Log("Destroying spinner");
        Deck.RoundOver -= StopMoving;
        Deck.RoundStart -= StartMoving;
    }
    
}
