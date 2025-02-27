using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    const string UNLOCKED = "LineOn";
    const string LOCKED = "LineOff";
    const string DISCOVERED = "LineDiscovered";
    const string UNLOCKING = "LineSkill";

    public static SkillTreeButton SelectedButton;

    public string AbilityName;

    
    public int level;
    public int max_level_reached;

    [SerializeField] List<SkillTreeButton> previousNode;
    [SerializeField] List<SkillTreeButton> followingNode;
    [SerializeField] List<Animator> nextPaths;
    
    void Start(){virtualStart(); SkillTreeManager.Instance.treeReset += virtualStart;}

    private void virtualStart(object sender, EventArgs e){virtualStart();}
    public void virtualStart(){
        
       
        if(!gameObject.activeInHierarchy){return;}
        GetComponent<Button>().onClick.RemoveAllListeners();
        ReloadColor();
        ReloadFunctionality();

        nextPaths.ForEach(path => path.Play(level >= 0 ? UNLOCKED : max_level_reached >= 0 ? DISCOVERED : LOCKED));

        if( level == -3 && max_level_reached < -1){
            gameObject.SetActive(false);
        }
        
        
        
        GetComponent<Button>().onClick.AddListener(Clicked);
        
        UpdatePickBan();
       
    }
    public void ping(){
        if(hasOnePreviousSkills()){ //hasAllPreviousSkills

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
    public bool hasAllPreviousSkills(){
        bool result =true;
        foreach (SkillTreeButton skills in previousNode)
        {
            result = result && skills.getLevel() >= 0;
        }
        return result;
    }
    public bool hasOnePreviousSkills(){
        return previousNode.Any(s=>s.getLevel() >= 0);
    }
    public void Clicked(){
        
        SelectedButton = this;
        
        int lvl = getLevel();
        int max_level_reached = getLevelMaxReached();

        if(lvl <=-2 && max_level_reached < -1){return;}

        SkillTreeManager.Instance.DisplaySkill(AbilityName, level);
        MetaMenuUI.Instance.moveSkillTree(transform);
    
    }
    public void ClickedUpgrade(){
        if(!hasAllPreviousSkills()){return;}
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
        max_level_reached = getLevelMaxReached();
        if(level < -1 && max_level_reached >= -1){
            GetComponent<Button>().colors = SkillTreeManager.Instance.GetColors(10);
        }else{
            GetComponent<Button>().colors = SkillTreeManager.Instance.GetColors(level);
        }
        
      
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
        if(level >= -1 || max_level_reached >= -1){
            self.interactable = true;
            self.transform.Find("Icon").GetComponent<Image>().enabled= true;
        }else{
            self.interactable = false;
            self.transform.Find("Icon").GetComponent<Image>().enabled= false;
        }

    }
    
    public void UpdatePickBan(){
        Skills s = SkillTreeManager.Instance.GetSkill(AbilityName);
        transform.Find("Pick").gameObject.SetActive(s.pick);
        transform.Find("Ban").gameObject.SetActive(s.ban);        
    }
    
    public IEnumerator NextPathCouroutine(){
        foreach(Animator item in nextPaths){item.Play(UNLOCKING);}
        yield return new WaitForSeconds(1);
        followingNode.ForEach(skill => skill.ping());
    }
    public int getLevel(){
        return SkillTreeManager.Instance.getLevel(AbilityName);
    }
    public int getLevelMaxReached(){
        return SkillTreeManager.Instance.getMaxLevelReached(AbilityName);

    }


    


     
}
