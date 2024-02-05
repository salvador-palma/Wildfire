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
        GetComponent<Animator>().Play("MenuFadeout");
    }





    
   
}
