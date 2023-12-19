using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{

    [SerializeField] string SavedCode;
    [SerializeField] List<SkillTreeButton> AllSkills;
    public static SkillTreeManager Instance;
    [SerializeField] Color[] UpgradeColors;
 
    

    private void Awake() {
        Instance = this;
        
    }

    void Update(){
       if(Input.GetKeyDown(KeyCode.T)){
            Debug.Log("Deleting all PlayerPrefs");
            PlayerPrefs.DeleteAll();
       }
    }

    public Color getColor(int i, int max_upgrades){
        
        if(i >= UpgradeColors.Length){return Color.white;}
        if(max_upgrades == 5){
            return UpgradeColors[i];
        }else if(max_upgrades == 3){
            return UpgradeColors[i*2 - 1];
        }else{
            return UpgradeColors[i*5];
        }
    }
    public Color getInitialColor(){return UpgradeColors[0];}

    
    
}
