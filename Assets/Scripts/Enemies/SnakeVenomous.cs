using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeVenomous : Snake
{
    public static new int DEATH_AMOUNT = 0;
    public override int getDeathAmount(){return DEATH_AMOUNT;}
    public override void incDeathAmount(){DEATH_AMOUNT++;}
    public override void ResetStatic(){DEATH_AMOUNT = 0;}
}
