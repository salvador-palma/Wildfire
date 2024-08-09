using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    const string UNLOCKED = "LineOn";
    const string LOCKED = "LineOff";
    const string UNLOCKING = "LineSkill";

    public static SkillTreeButton SelectedButton;

    public string AbilityName;

    
   
    [SerializeField] List<SkillTreeButton> previousNode;
    [SerializeField] List<SkillTreeButton> followingNode;
    [SerializeField] List<Animator> nextPaths;
    
    void Start(){virtualStart(); SkillTreeManager.Instance.treeReset += virtualStart;}

    private void virtualStart(object sender, EventArgs e){virtualStart();/*ResetLines();*/}
    // private void ResetLines(){nextPaths.ForEach(l => l.PlayInit());}
    public void virtualStart(){
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        int lvl = getLevel();
  
        if(lvl>= 0){
            foreach (Animator path in nextPaths)
            {
                path.Play(UNLOCKED);
            }
        }

        if( lvl == -3 ){
            gameObject.SetActive(false);
        }
        
        GetComponent<Button>().onClick.AddListener(Clicked);
       
    }
    public void ping(){
        bool result =true;
        foreach (SkillTreeButton item in previousNode)
        {
            result = result && item.getLevel() >= 0;
        }
        /* && !wasUnlocked */ 
        if(result ){Unlock();SkillTreeManager.Instance.Upgrade(AbilityName);}
    }
    private void Unlock(){
       // wasUnlocked = true;
        // GetComponent<Animator>().Play(UNLOCKING);
    }
    public void Clicked(){
        Debug.Log("Clicked Skill...");
        SelectedButton = this;
        
        int lvl = getLevel();
        if(lvl <=-2){return;}

        SkillTreeManager.Instance.DisplaySkill(AbilityName);
        //MetaMenuUI.Instance.moveSkillTree(transform.localPosition * -1f);
    
    }
    public void ClickedUpgrade(){
        Upgrade();
        SkillTreeManager.Instance.DisplaySkill(AbilityName);
    }
   
    private void Upgrade(){
        SkillTreeManager.Instance.Upgrade(AbilityName);
        if(getLevel()==0){NextPaths(true);}
    }
    
    public void NextPaths(bool withPing){
        foreach (Animator item in nextPaths)
        {
            item.Play("");
        }
    }
    public int getLevel(){
        return SkillTreeManager.Instance.getLevel(AbilityName);
    }


    


     
}
