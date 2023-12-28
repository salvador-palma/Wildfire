using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaMenuUI : MonoBehaviour
{
    //Deug
    [SerializeField] private GameObject SkillTreePanel;
    
    
    public void StartGame(){
        SceneManager.LoadScene("Game");
    }

    public void SkillTreeMenuToggle(){
        SkillTreePanel.SetActive(!SkillTreePanel.activeInHierarchy);
    }
}
