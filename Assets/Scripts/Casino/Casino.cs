using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Casino : MonoBehaviour
{
    public static string getMinigame(){
        DateTime d = DateTime.Today;
        if((int)d.DayOfWeek % 2 == 0){
                return "Drop the Acorn";//MINI GAME 1
            }else{
                return "Water the Flower";//MINI GAME 2
        }
    }
    public static int getMinigameID(){
        DateTime d = DateTime.Today;
        if((int)d.DayOfWeek % 2 == 0){
                return 0;//MINI GAME 1
            }else{
                return 1;//MINI GAME 2
        }
    }
    private void Awake() {
        
        transform.GetChild(getMinigameID()).gameObject.SetActive(true);
    }
    public void GoBack(){
        GetComponent<Animator>().Play("CurtainsOff");
    }
    public void LoadCampsite(){
        PlayerPrefs.SetInt("Origin", 1);
        SceneManager.LoadScene("MetaGame");
    }
}
