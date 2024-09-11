using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    public void LoadGameScene(){

        SceneManager.LoadScene("MetaGame");
    }
    public void StartFadeOut(){
        if(PlayerPrefs.GetInt("V1.4",0)==0){
            PlayerPrefs.SetInt("V1.4",1);
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
            foreach (string filePath in filePaths){
                File.Delete(filePath);
            }
                
        }
        GetComponent<Animator>().Play("MenuFadeout");
    }





    
   
}
