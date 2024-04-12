using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    
    [SerializeField]  protected bool withShell;
    [SerializeField] private float maxSpeed;
    private void Start() {
        base.flame = Flamey.Instance;
        maxSpeed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        Speed = maxSpeed;
        
        MaxHealth = Health;
    }
    bool Moving;
    public override void UpdateEnemy()
    {
        if(Moving){
            base.UpdateEnemy();
        }
    }

    public void SlideOn(){
        Moving = true;
    }
    public void SlideOff(){
        Moving = false;
    }
   

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}

