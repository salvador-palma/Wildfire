using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] TextMeshProUGUI[] UpgradeExtraTexts; //0-Title 1-Cost 2-Upgrade/Unlock
    [SerializeField] Color DisabledColor;
    [SerializeField] GameObject[] BeforeAndAfter;
    [SerializeField] RectTransform UpgradeInfoContentPanel;
    [SerializeField] TextMeshProUGUI emberAmountTxt;
    [SerializeField] TextMeshProUGUI emberLossTxt;
   

    public event EventHandler treeReset;
    private void Awake() {
        Instance = this;
        ReadData();
    }
    private void Start(){
        //changeUpgradeInfoTexts(displayedSkill);
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
    public void DisplaySkill(string skill, SkillTreeButton extra){
        if(skill==null){
            return;
        }
        GetComponentInParent<Animator>().SetTrigger("Info");
        Skills s = GetSkills(skill);
        if(s.max_value == 1){
            UpgradeExtraTexts[2].text = s.value == s.max_value ? "Unlocked" : "Unlock";
            
            Array.ForEach(BeforeAndAfter, x => x.SetActive(false));
            UpgradeInfoContentPanel.sizeDelta = new Vector2(168,44);
        }else{
            List<Augment> augments = DeckBuilder.Instance.GetAugmentsFromClasses(new List<string>{skill});

            augments.Sort((a,b)=> a.tier - b.tier );
            int b = 0;
            for(int i =0; i!= 3; i++){
                if(b >= augments.Count || ( b < augments.Count && (int)augments[b].tier != i)){
                    UpgradeInfoTexts[i].text = "";
                    UpgradeInfoTexts[i+3].text = "";
                    UpgradeInfoTexts[i].transform.parent.GetComponent<Image>().color = DisabledColor;
                    UpgradeInfoTexts[i+3].transform.parent.GetComponent<Image>().color = DisabledColor;
                    continue;
                }
                
                UpgradeInfoTexts[i].text = augments[b].getDescription();
                UpgradeInfoTexts[i+3].text = augments[b].getNextDescription();
                UpgradeInfoTexts[i].transform.parent.GetComponent<Image>().color = Color.white;
                UpgradeInfoTexts[i+3].transform.parent.GetComponent<Image>().color = Color.white;
                b++;
            }

            UpgradeExtraTexts[2].text = "Upgrade";
            
            UpgradeInfoContentPanel.sizeDelta = new Vector2(168,395);
            Array.ForEach(BeforeAndAfter, x => x.SetActive(true));
        }
        UpgradeExtraTexts[0].text = extra.DisplayTitle;
        UpgradeExtraTexts[1].text = s.value == s.max_value ? "" :  "Cost:" + DeckBuilder.Instance.getPrice(skill, getLevel(skill));
        
        

    }
    // public void changeUpgradeInfoTexts(string skill){
        
    //     Skills s = GetSkills(skill);
    //     if(s.max_value == 1){

    //     }else{
    //         List<Augment> augments = DeckBuilder.Instance.GetAugmentsFromClasses(new List<string>{skill});
    //         int i=0;
    //         foreach(Augment a in augments){
    //             UpgradeInfoTexts[i].text = a.getDescription();
    //             UpgradeInfoTexts[i+3].text = a.getNextDescription();
    //             i++;
    //         }
    //     }
        
    // }
    public void changeEmberAmountUI(){
        //9000 - 10000
        int difference = PlayerData.embers - int.Parse(emberAmountTxt.text);
        emberAmountTxt.text = PlayerData.embers.ToString();
        
        emberLossTxt.text = difference > 0 ? "+"+difference : ""+difference;
        GetComponentInParent<Animator>().Play("SkillTreeLoss");
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
    public void toggleSkillTree(GameObject SkillTreePanel){
        // bool new_state = !SkillTreePanel.activeInHierarchy;
        // SkillTreePanel.SetActive(new_state);
        // if(new_state){treeReset?.Invoke(this, new EventArgs());}
        GetComponentInParent<Animator>().SetTrigger("Rest");
        SkillTreePanel.transform.position = new Vector2(SkillTreePanel.transform.position.x > 2000 ? 0 : 2000, 0);

    }
    
}
