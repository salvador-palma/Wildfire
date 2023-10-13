using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    public void LoadGameScene(){
        SceneManager.LoadScene("Game");
    }
    public void StartFadeOut(){
        GetComponent<Animator>().Play("MenuFadeout");
    }





    double[] gaussianSamples = new double[10000];
    private void Update() {
        // if(Input.GetKeyDown(KeyCode.Space)){
        //     for (int i = 0; i < 10000; i++)
        //     {
        //         Debug.Log(i);
        //         gaussianSamples[i] = Distribuitons.RandomGaussianDouble(1,0);
        //     }

        //     Dictionary<double, int> frequency = Distribuitons.ValueFrequency(gaussianSamples, Distribuitons.BinMaker(-4.0,4.0,0.1));
        //     File.WriteAllLines("gaussian_samples.csv", frequency.Select(s => s.Key + " " + s.Value));
        //     Debug.Log("Done!");
        // }

        // if(Input.GetKeyDown(KeyCode.K)){
        //     float f = 0;
        //     for (int i = 0; i < 1000; i++)
        //     {
        //         float g = (float)Distribuitons.RandomExponential(8f/6.2f);
        //         f += g;
        //         Debug.Log(g);
        //     }
        //     Debug.Log("Results: " + f/1000);
        // }
        if(Input.GetKeyDown(KeyCode.Space)){
                foreach(double b in Distribuitons.BinomialAux(4,0.66)){
                    Debug.Log(b);
                }
                Debug.Log(Distribuitons.BinomialAux(4,0.66).Sum());
                Debug.Log("=========");
                
            
        }

    }
}
