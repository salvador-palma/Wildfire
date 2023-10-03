using System.Collections;
using System.Collections.Generic;
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
}
