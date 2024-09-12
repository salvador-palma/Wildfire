using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixedOptionMenu : MonoBehaviour
{
    [SerializeField] string AbilityName;
    [SerializeField] string[] Options;
    [SerializeField] TextMeshProUGUI UpdatableText;
    [SerializeField] int currentID;
    
    [SerializeField] Button Left;
    [SerializeField] Button Right;
    
    void Start(){
        Left.onClick.AddListener(() => Move(-1));
        Right.onClick.AddListener(() => Move(1));
        
        switch(AbilityName){
            case "Burst Shot":
                UpdatableText.text = Options[BurstShot.Instance.currentTargetingOption];
            break;
            case "Multicaster":
                UpdatableText.text = Options[SecondShot.Instance.currentTargetingOption];
            break;
        
        }
        
    }
  
    private void Move(int dir){
        currentID += dir;
        if(currentID >= Options.Length){
            currentID = 0;
        }else if(currentID < 0){
            currentID = Options.Length - 1;
        }
        UpdatableText.text = Options[currentID];
        ExtraBehaviour();
    }
    private void ExtraBehaviour(){
        switch(AbilityName){
            case "Burst Shot":
                BurstShot.Instance.currentTargetingOption = currentID;
                PlayerPrefs.GetInt("BurstShotTargetingOption", currentID);
            break;
            case "Multicaster":
                SecondShot.Instance.currentTargetingOption = currentID;
                PlayerPrefs.GetInt("MulticasterTargetingOption", currentID);
            break;
        
        }
    }
}
