using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
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

    public static double RandomGaussianDouble(float variance, float mean){
        float u1 = 1.0f- UnityEngine.Random.value; 
        float u2 = 1.0f- UnityEngine.Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); 
        double randNormal = mean + variance * randStdNormal; 
        
        return randNormal;
    }

    public static float RandomExponential(float lambda){
        float f = UnityEngine.Random.value;
        
        return -Mathf.Log(1f - f) / lambda;
    }

    public static double RandomUniform(double min, double max)
    {
        return UnityEngine.Random.value * (max - min) + min;
    }

     public static int RandomBinomial(int n, double p) {
        double[] probs = BinomialAux(n, p);
        double x = UnityEngine.Random.value;
        
        for(int i = 0; i != probs.Length; i++) {
            if(x < probs[i]) {
                return i;
            } else {
                x -= probs[i];
            }
        }
        return n;
    }

    public static double[] BinomialAux(int n, double p) {
        double[] probs = new double[n + 1];
        for(int i = 0; i != probs.Length; i++) {
            probs[i] = C(n,i) * Math.Pow(p,i) * Math.Pow(1-p, n-i);
        }
        return probs;
    }
    public static int C(int n, int x){
        return factorial(n)/(factorial(x) * factorial(n-x));
    }

    public static int factorial(int n) {
        if(n <= 1)
            return 1;
        return n * factorial(n-1);
    }

    public static bool[] sillyGoose(int len, int n) {
        bool[] boolArray = new bool[len];
        for (int i = 0; i < n; i++)
        {
            boolArray[i] = true;
        }
        for (int i = len - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            bool temp = boolArray[i];
            boolArray[i] = boolArray[j];
            boolArray[j] = temp;
        } 
        return boolArray;
    } 
 



    public static Dictionary<double, int> ValueFrequency(double[] values, double[] bins){
        Dictionary<double, int> binCounts = new Dictionary<double, int>();
        foreach (double bin in bins){binCounts[bin] = 0;}

        foreach(double v in values){
            bool valueAssigned = false;
            for (int i = 0; i < bins.Length - 1; i++)
            {
                if (v >= bins[i] && v < bins[i + 1])
                {
                    binCounts[bins[i]]++;
                    valueAssigned = true;
                    break;
                }
            }
            if (!valueAssigned && v >= bins[bins.Length - 1])
            {
                binCounts[bins[bins.Length - 1]]++;
            }

        }
        return binCounts;
    }

    public static double[] BinMaker(double from, double to, double step){
        

        int arrayLength = (int)Math.Ceiling((to - from) / step) + 1;

        double[] result = new double[arrayLength];

        for (int i = 0; i < arrayLength; i++)
        {
            result[i] = from + i * step;
        }
        return result;
    }
}
