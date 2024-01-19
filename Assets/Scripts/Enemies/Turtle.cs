using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    
    
    private void Start() {
        flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.01f,Speed,0.03f);
     
        MaxHealth = Health;
        StartAnimations(4);
    }
   
    

    
    
}
