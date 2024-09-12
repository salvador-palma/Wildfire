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

    
    public int level;

    [SerializeField] List<SkillTreeButton> previousNode;
    [SerializeField] List<SkillTreeButton> followingNode;
    [SerializeField] List<Animator> nextPaths;
    
    void Start(){virtualStart(); SkillTreeManager.Instance.treeReset += virtualStart;}

    private void virtualStart(object sender, EventArgs e){virtualStart();}
    public void virtualStart(){
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        ReloadColor();
        ReloadFunctionality();

        nextPaths.ForEach(path => path.Play(level >= 0 ? UNLOCKED : LOCKED));

        if( level == -3 ){
            gameObject.SetActive(false);
        }
        
        GetComponent<Button>().onClick.AddListener(Clicked);
       
    }
    public void ping(){
        bool result =true;
        foreach (SkillTreeButton skills in previousNode)
        {
            result = result && skills.getLevel() >= 0;
        }
        if(result){
            if(Step()){
                
                if(level == -1){
                    followingNode.ForEach(skill =>{
                        if(skill.level == -3){
                            skill.gameObject.SetActive(true); skill.Step();
                        }
                    });
                }
            }
        }
    }
    public void Clicked(){
        SelectedButton = this;
        
        int lvl = getLevel();
        if(lvl <=-2){return;}

        SkillTreeManager.Instance.DisplaySkill(AbilityName, level);
        Vector2 LookUpPos = transform.localPosition;
        LookUpPos.x += 100f;
        LookUpPos.y += -20f;
        MetaMenuUI.Instance.moveSkillTree(transform);
    
    }
    public void ClickedUpgrade(){
        if(SkillTreeManager.Instance.Upgrade(AbilityName)){
            ReloadColor();
            ReloadFunctionality();
            if(level==0){StartCoroutine("NextPathCouroutine");}
        }
        SkillTreeManager.Instance.DisplaySkill(AbilityName, level);
    }
    public bool Step(){
        if( SkillTreeManager.Instance.Upgrade(AbilityName)){
            ReloadColor();
            ReloadFunctionality();
            return true;
        }
        return false;
    }
   
    private void ReloadColor(){
        level = getLevel();
        GetComponent<Button>().colors = SkillTreeManager.Instance.GetColors(level);
      
    }
    private void ReloadFunctionality(){
        Button self = GetComponent<Button>();
        if(level == -1){
            gameObject.SetActive(true);
            GetComponent<Animator>().Play("ButtonBuyable",-1,UnityEngine.Random.Range(0f,5f));

        }else{
            if(gameObject.activeInHierarchy){
                GetComponent<Animator>().Play("Static");
            }
            
        }
        if(level >= -1){
            self.interactable = true;
            self.transform.Find("Icon").GetComponent<Image>().enabled= true;
        }else{
            self.interactable = false;
            self.transform.Find("Icon").GetComponent<Image>().enabled= false;
        }

    }
    
    
    public IEnumerator NextPathCouroutine(){
        foreach(Animator item in nextPaths){item.Play(UNLOCKING);}
        yield return new WaitForSeconds(1);
        followingNode.ForEach(skill => skill.ping());
    }
    public int getLevel(){
        return SkillTreeManager.Instance.getLevel(AbilityName);
    }


    


     
}
