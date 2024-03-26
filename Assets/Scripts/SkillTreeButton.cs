using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    const string UNLOCKED = "SkillTreeButtonUnlocked";
    const string LOCKED = "SkillTreeButtonLocked";
    const string UNLOCKING = "SkillTreeButtonUnlock";

    public static SkillTreeButton SelectedButton;

    public string DisplayTitle;
    [SerializeField] bool UnlockableField;
    [SerializeField] string[] UnlockableClasses;

    [SerializeField] string AugmentClass;
   
    [SerializeField] List<SkillTreeButton> previousNode;
    [SerializeField] List<SkillTreeLine> nextPaths;
    private bool wasUnlocked;
    void Start(){virtualStart(); SkillTreeManager.Instance.treeReset += virtualStart;}

    private void virtualStart(object sender, EventArgs e){virtualStart();ResetLines();}
    private void ResetLines(){nextPaths.ForEach(l => l.PlayInit());}
    public void virtualStart(){
        wasUnlocked = false;
        GetComponent<Button>().onClick.RemoveAllListeners();
        int lvl = getLevel();
        if(lvl==-1){GetComponent<Animator>().Play(LOCKED);}
        else if(lvl>=0){GetComponent<Animator>().Play(UNLOCKED);}
        if(lvl>=1){NextPaths(false);}
        
        GetComponent<Button>().onClick.AddListener(Clicked);
        UpdateImage();
    }
    public void ping(){
        bool result =true;
        foreach (SkillTreeButton item in previousNode)
        {
            result = result && item.wasBought();
        }
        if(result && !wasUnlocked){Unlock();SkillTreeManager.Instance.Upgrade(AugmentClass);UpdateImage();}
    }
    private void Unlock(){
        wasUnlocked = true;
        GetComponent<Animator>().Play(UNLOCKING);
    }
    public void Clicked(){
        // Upgrade();
        // UpdateImage();
        SelectedButton = this;
        
        int lvl = getLevel();
        if(lvl ==-1){return;}
        SkillTreeManager.Instance.DisplaySkill(AugmentClass, this);
        MetaMenuUI.Instance.moveSkillTree(transform.localPosition * -1f);
    
    }
    public void ClickedUpgrade(){
        Upgrade();
        UpdateImage();
        SkillTreeManager.Instance.DisplaySkill(AugmentClass, this);
    }
   
    private void Upgrade(){

        SkillTreeManager.Instance.Upgrade(AugmentClass);
        if(getLevel()==1){NextPaths(true);}
    }
    
    public void NextPaths(bool withPing){
        foreach (SkillTreeLine item in nextPaths)
        {
            item.PlayUnlock(withPing);
        }
    }

    private void UpdateImage(){
        transform.Find("FillImage").GetComponent<Image>().color = SkillTreeManager.Instance.getColor(AugmentClass);
    }

    public bool wasBought(){
        return getLevel()>=1; 
    }

    private int getLevel(){
        return SkillTreeManager.Instance.getLevel(AugmentClass);
    }


    


     
}
