using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingTurtle : Turtle
{

    public override void Attack()
    {
        Flamey.Instance.Stun(2f);
        base.Attack();
    }
    

}
