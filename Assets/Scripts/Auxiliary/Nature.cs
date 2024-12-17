using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature : MonoBehaviour
{
    public bool isManager;

    void Start(){
        if(isManager){
           
            MetaMenuUI.Instance.NightFall += TransitionInGame;
        }
        
    }
    void TransitionInGame(object sender, EventArgs e){
        GetComponent<Animator>().Play("Nightfall");
    }
    public void Destroy(){
        Destroy(gameObject);
    }
}
