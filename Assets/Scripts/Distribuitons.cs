using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribuitons 
{
    public static float RandomGaussian(float variance, float mean){
        float u1 = 1.0f- UnityEngine.Random.value; 
        float u2 = 1.0f- UnityEngine.Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); 
        double randNormal = mean + variance * randStdNormal; 
        
        return (float)randNormal;
    }
}
