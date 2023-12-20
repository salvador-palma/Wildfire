using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    const string UNLOCKED = "SkillTreeButtonUnlocked";
    const string LOCKED = "SkillTreeButtonLocked";
    const string UNLOCKING = "SkillTreeButtonUnlock";


    [SerializeField] bool UnlockableField;
    [SerializeField] string[] UnlockableClasses;

    [SerializeField] string AugmentClass;
   
    [SerializeField] List<SkillTreeButton> previousNode;
    [SerializeField] List<SkillTreeLine> nextPaths;
    public bool isBaseStat;
    void Start()
    {
        

        if(getLevel()>=0){GetComponent<Animator>().Play(UNLOCKED);NextPaths(false);}
        
        GetComponent<Button>().onClick.AddListener(Clicked);
        UpdateImage();
        
    }
    public void ping(){
        bool result =true;
        foreach (SkillTreeButton item in previousNode)
        {
            result = result && item.wasBought();
        }
        if(result){Unlock();SkillTreeManager.Instance.Upgrade(AugmentClass);UpdateImage();}
    }
    private void Unlock(){
        GetComponent<Animator>().Play(UNLOCKING);
    }
    private void Clicked(){
        //CHECK FOR PRICE
        

        Upgrade();
        UpdateImage();
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
