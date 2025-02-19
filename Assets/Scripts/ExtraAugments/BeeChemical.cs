using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeChemical : Bee
{
    

    public override void Start()
    {
        base.Start();
       
        
    }
    protected override void Attack()
    {
        
        atkTimer = 1/atkSpeed;
        if(target==null){return;}
        else{
            target.GetComponent<Enemy>().Poison(10);
            target.GetComponent<SpriteRenderer>().material.SetInt("_Poison", 1);
        }
        target=getTarget();
        
    }
    protected override Enemy getTarget(){
        return Flamey.Instance.getRandomHomingEnemy(true, new Predicate<Enemy>(x => x.poisonLeft <= 0));
        
         
    }
    
    
}
