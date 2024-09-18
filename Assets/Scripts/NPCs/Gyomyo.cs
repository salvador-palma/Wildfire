using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gyomyo : NPC
{
    
    public void GoCasino(string minigame){
        SceneManager.LoadScene("Casino");
    }
}
