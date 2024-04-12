using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    
    
    private void Start() {
        flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.01f,Speed,0.03f);
     
        MaxHealth = Health;

    }
   
    

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}
