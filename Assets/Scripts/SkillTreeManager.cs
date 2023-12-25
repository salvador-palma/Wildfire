using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Skills{
    
    public string type;
    public int value;
    public int max_value;

}
[System.Serializable]
public class SerializableList<T> {
    public int embers;
    public int skillTreeEmbers;
    public List<T> skills;
}
public class SkillTreeManager : MonoBehaviour
{

    [SerializeField] SerializableList<Skills> PlayerData;
    public static SkillTreeManager Instance;
    [SerializeField] Color[] UpgradeColors;
 
    [SerializeField] TextMeshProUGUI[] UpgradeInfoTexts;
    [SerializeField] TextMeshProUGUI emberAmountTxt;
    private string _displayedSkill;
    public string displayedSkill {get{return _displayedSkill;}set{_displayedSkill=value;changeUpgradeInfoTexts(value);}}

    public event EventHandler treeReset;
    private void Awake() {
        Instance = this;
        ReadData();
    }
    private void Start(){
        changeUpgradeInfoTexts(displayedSkill);
        changeEmberAmountUI();
    }

    public Color getColor(string augmentClass){
        int maxlvl = getMaxLevel(augmentClass);
        int lvl = getLevel(augmentClass);
        if(lvl==-1 || lvl > maxlvl){return Color.white;}
        if(maxlvl == 4){
            return UpgradeColors[lvl+1];
        }else if(maxlvl == 2){
            return UpgradeColors[lvl*2 + 1];
        }else{
            return UpgradeColors[lvl*5];
        }
    }
    

    public int getLevel(string skill){
        Skills result = GetSkills(skill);
        return result==null ? 0 : result.value;
    }
    public int getMaxLevel(string skill){
        return GetSkills(skill).max_value;
    }
    public void Upgrade(string skill){
        Skills item = GetSkills(skill);
        if(item.value != -1){
            
            int price = DeckBuilder.Instance.getPrice(skill, getLevel(skill));
            if(price == -1 || price > PlayerData.embers){
                Debug.Log("Not enough cash. You have " + PlayerData.embers + " and need " + price);
                return;
            }
            PlayerData.skillTreeEmbers+=price;
            PlayerData.embers -= price;
            changeEmberAmountUI();
        }
        
        if(item.value < item.max_value){
            item.value++;
            
            WritingData();
        }   
        displayedSkill = skill;
    }
    public Skills GetSkills(string skill){
        foreach (Skills item in PlayerData.skills)
        {
            if(item.type == skill){return item;}
        }
        Debug.LogWarning("Skill not found: " + skill);
        return null;
    }
    public void WritingData(){
        //Debug.Log("Writing Data to " + Application.dataPath + "/skills.json");
        
        string json = JsonUtility.ToJson(PlayerData);
        File.WriteAllText(Application.dataPath + "/skills.json", json);
    }
    public void ReadData(){
        //Debug.Log("Reading Data...");
        if(File.Exists(Application.dataPath +"/skills.json")){
            string json = File.ReadAllText(Application.dataPath +"/skills.json");
            PlayerData = JsonUtility.FromJson<SerializableList<Skills>>(json);
        }
    }



    //UI PARTITION
    public void changeUpgradeInfoTexts(string skill){
        if(skill==null){
            foreach(TextMeshProUGUI txt in UpgradeInfoTexts){txt.text = "";}return;
        }
        List<Augment> augments = DeckBuilder.Instance.GetAugmentsFromClasses(new List<string>{skill});
        int i=0;
        foreach(Augment a in augments){
            UpgradeInfoTexts[i].text = a.getDescription();
            UpgradeInfoTexts[i+3].text = a.getNextDescription();
            i++;
        }
    }
    public void changeEmberAmountUI(){
        emberAmountTxt.text = PlayerData.embers.ToString();
    }

    public void resetSkillTree(){
        foreach (Skills item in PlayerData.skills)
        {
            item.value = item.max_value == 4 || item.max_value == 0 ? 0 : -1;
        }
        PlayerData.embers += PlayerData.skillTreeEmbers;
        PlayerData.skillTreeEmbers = 0;
        changeEmberAmountUI();
        WritingData();
        treeReset?.Invoke(this, new EventArgs());
        
    }
    
}
