using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    static string version = "V1.5.1.1";
    public static string device = "Mobile"; //Mobile, PC, Web
    private void Awake() {
        if(PlayerPrefs.GetInt(version,0)==0){
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(version,1);
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
            foreach (string filePath in filePaths){
                try{
                    Debug.Log("Deleting: " + filePath);
                    File.Delete(filePath);
                }catch{
                    Debug.Log("Error Deleting File: "+filePath);
                }
                
            }
                
        }
    }
    public void LoadGameScene(){

        SceneManager.LoadScene("MetaGame");
    }
    
    public void StartFadeOut(){
        
        GetComponent<Animator>().Play("MenuFadeout");
    }





    
   
}
