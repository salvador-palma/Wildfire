using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public List<T> list;
}
public class SkillTreeManager : MonoBehaviour
{

    [SerializeField] SerializableList<Skills> AllSkills;
    public static SkillTreeManager Instance;
    [SerializeField] Color[] UpgradeColors;
 
    

    private void Awake() {
        Instance = this;
        ReadData();
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
        return GetSkills(skill).value;
    }
    public int getMaxLevel(string skill){
        return GetSkills(skill).max_value;
    }
    public void Upgrade(string skill){
        Skills item = GetSkills(skill);
        if(item.value < item.max_value){
            item.value++;
            WritingData();
        }   
    }
    public Skills GetSkills(string skill){
        foreach (Skills item in AllSkills.list)
        {
            if(item.type == skill){return item;}
        }
        return null;
    }
    public void WritingData(){
        Debug.Log("Writing Data to " + Application.dataPath + "/skills.json");
        
        string json = JsonUtility.ToJson(AllSkills);
        File.WriteAllText(Application.dataPath + "/skills.json", json);
    }
    public void ReadData(){
        Debug.Log("Reading Data...");
        if(File.Exists(Application.dataPath +"/skills.json")){
            string json = File.ReadAllText(Application.dataPath +"/skills.json");
            AllSkills = JsonUtility.FromJson<SerializableList<Skills>>(json);
        }
    }
    
}
