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
    

    
    
}
