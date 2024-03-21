using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatterPillar : Enemy
{
    
    private void Start() {
        flame = Flamey.Instance;
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed, 0.075f);
        MaxHealth = Health;
        StartAnimations(1);
    }
    

    public static int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
    
}
