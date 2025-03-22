using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Drum : MonoBehaviour
{
    public int Timing;
    public int Type;
    int[] limits = new int[]{30,55,70};
    float[] mult = new float[] { 1f, .33f, 0.16f };
    
    bool hitted;
    public int[] Score(int hitType, int timing){
        if(hitted){return new int[]{-1,-1};}

        int baseScore = Correct(hitType);
        
        int diff = Math.Abs(Timing - timing);
       
        
        for(int i = 0; i < limits.Length; i++){
            if(diff < limits[i]){
                hitted= true;
                return new int[]{(int)(baseScore * mult[i]), i};
            }
        }

        return new int[]{-1,-1};//Out of Range (Dont Count)
    }
    int baseDrum = 300;
    int finisherDrum = 1000;
    public int Correct(int hitType){
        switch(Type){
            case 0: if(hitType == 0||hitType==2) return baseDrum; break;
            case 1: if(hitType == 1||hitType==3) return baseDrum; break;
            case 2: if(hitType == 0){return baseDrum;}else if(hitType==2){return finisherDrum;} break;
            case 3: if(hitType == 1){return baseDrum;}else if(hitType==3){return finisherDrum;} break;
            default: return 0;
        }
        return 0;
    }
    public bool Missed(int timing){
        return Timing + 110 < timing;
    }
}
