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
        File.WriteAllText(Application.persistentDataPath + "/skills.json", json);
    }
    public void ReadData(){
        //Debug.Log("Reading Data...");
        if(File.Exists(Application.persistentDataPath +"/skills.json")){
            string json = File.ReadAllText(Application.persistentDataPath +"/skills.json");
            PlayerData = JsonUtility.FromJson<SerializableList<Skills>>(json);
        }else{
            CreateFile();
            ReadData();
        }
    }
     public void CreateFile(){
        string str = "{\"embers\":1500,\"skillTreeEmbers\":0,\"skills\":[{\"type\":\"Dmg\",\"value\":0,\"max_value\":4},{\"type\":\"Acc\",\"value\":0,\"max_value\":4},{\"type\":\"AtkSpeed\",\"value\":0,\"max_value\":4},{\"type\":\"BltSpeed\",\"value\":0,\"max_value\":4},{\"type\":\"Armor\",\"value\":0,\"max_value\":4},{\"type\":\"Health\",\"value\":0,\"max_value\":4},{\"type\":\"GambleUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"GambleImprove\",\"value\":-1,\"max_value\":2},{\"type\":\"MulticasterUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"MulticasterProb\",\"value\":-1,\"max_value\":2},{\"type\":\"CritUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"CritChance\",\"value\":-1,\"max_value\":2},{\"type\":\"CritMult\",\"value\":-1,\"max_value\":2},{\"type\":\"VampUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"VampProb\",\"value\":-1,\"max_value\":2},{\"type\":\"VampPerc\",\"value\":-1,\"max_value\":2},{\"type\":\"BurstUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"BurstInterval\",\"value\":-1,\"max_value\":2},{\"type\":\"BurstAmount\",\"value\":-1,\"max_value\":2},{\"type\":\"IceUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"IceProb\",\"value\":-1,\"max_value\":2},{\"type\":\"IceDuration\",\"value\":-1,\"max_value\":2},{\"type\":\"Assassins\",\"value\":-1,\"max_value\":1},{\"type\":\"Execute\",\"value\":-1,\"max_value\":2},{\"type\":\"ArmorPen\",\"value\":-1,\"max_value\":2},{\"type\":\"OrbitalUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"OrbitalAmount\",\"value\":-1,\"max_value\":2},{\"type\":\"OrbitalDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"ShredUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"ShredProb\",\"value\":-1,\"max_value\":2},{\"type\":\"ShredPerc\",\"value\":-1,\"max_value\":2},{\"type\":\"RegenUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"IcePoolUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"IcePoolDuration\",\"value\":-1,\"max_value\":2},{\"type\":\"IcePoolProb\",\"value\":-1,\"max_value\":2},{\"type\":\"IcePoolSlow\",\"value\":-1,\"max_value\":2},{\"type\":\"IcePoolSize\",\"value\":-1,\"max_value\":2},{\"type\":\"ThornsUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"ThornsPerc\",\"value\":-1,\"max_value\":2},{\"type\":\"ThornsProb\",\"value\":-1,\"max_value\":2},{\"type\":\"MoneyUnlock\",\"value\":0,\"max_value\":1},{\"type\":\"MoneyProb\",\"value\":-1,\"max_value\":2},{\"type\":\"MoneyMult\",\"value\":-1,\"max_value\":2},{\"type\":\"VampDeathUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"VampDeathProb\",\"value\":-1,\"max_value\":2},{\"type\":\"VampDeathPerc\",\"value\":-1,\"max_value\":2},{\"type\":\"ExplodeUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"ExplodeProb\",\"value\":-1,\"max_value\":2},{\"type\":\"ExplodeDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"NecroUnlock\",\"value\":0,\"max_value\":1},{\"type\":\"NecroProb\",\"value\":-1,\"max_value\":2},{\"type\":\"NecroStats\",\"value\":-1,\"max_value\":2},{\"type\":\"BulletsUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"BulletsProb\",\"value\":-1,\"max_value\":2},{\"type\":\"BulletsDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"BulletsAmount\",\"value\":-1,\"max_value\":2},{\"type\":\"RegenUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"RegenPerSecond\",\"value\":-1,\"max_value\":2},{\"type\":\"RegenPerRound\",\"value\":-1,\"max_value\":2},{\"type\":\"ThunderUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"ThunderInterval\",\"value\":-1,\"max_value\":2},{\"type\":\"ThunderDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"ImmolateUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"ImmolateInterval\",\"value\":-1,\"max_value\":2},{\"type\":\"ImmolateDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"ImmolateRadius\",\"value\":-1,\"max_value\":2},{\"type\":\"CandleUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"CandleDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"CandleAtkSpeed\",\"value\":-1,\"max_value\":2},{\"type\":\"CandleAmount\",\"value\":-1,\"max_value\":2},{\"type\":\"SummonUnlock\",\"value\":0,\"max_value\":1},{\"type\":\"SummonAtkSpeed\",\"value\":-1,\"max_value\":2},{\"type\":\"SummonDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"SummonSpeed\",\"value\":-1,\"max_value\":2},{\"type\":\"SummonAmount\",\"value\":-1,\"max_value\":2},{\"type\":\"BlueFlameUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"BlueFlameInterval\",\"value\":-1,\"max_value\":2},{\"type\":\"BlueFlameDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"StatikUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"StatikProb\",\"value\":-1,\"max_value\":2},{\"type\":\"StatikDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"StatikTTL\",\"value\":-1,\"max_value\":2},{\"type\":\"LavaPoolUnlock\",\"value\":-1,\"max_value\":1},{\"type\":\"LavaPoolDmg\",\"value\":-1,\"max_value\":2},{\"type\":\"LavaPoolSize\",\"value\":-1,\"max_value\":2},{\"type\":\"LavaPoolProb\",\"value\":-1,\"max_value\":2},{\"type\":\"LavaPoolDuration\",\"value\":-1,\"max_value\":2}]}";
        File.WriteAllText(Application.persistentDataPath +"/skills.json", str);
    }

    public static void AddEmbersToJSON(int n){
        Debug.Log("Adding " + n + " embers to JSON");
        if(File.Exists(Application.persistentDataPath +"/skills.json")){
            string jsonR = File.ReadAllText(Application.persistentDataPath +"/skills.json");
            SerializableList<Skills> p  = JsonUtility.FromJson<SerializableList<Skills>>(jsonR);
            p.embers += n;
            string jsonW = JsonUtility.ToJson(p);
            File.WriteAllText(Application.persistentDataPath + "/skills.json", jsonW);
        }
    }


   

    //UI PARTITION
    public void DisplaySkill(string skill, SkillTreeButton extra){
        if(skill==null){
            return;
        }
        GetComponentInParent<Animator>().SetBool("InfoDisplay",true);
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
    
    public void changeEmberAmountUI(){
  
        //int difference = PlayerData.embers - int.Parse(emberAmountTxt.text);
        emberAmountTxt.text = PlayerData.embers.ToString();
        
        // emberLossTxt.text = difference > 0 ? "+"+difference : ""+difference;
        // GetComponentInParent<Animator>().Play("SkillTreeLoss");
    }

    public void resetSkillTree(){
        List<string> exceptions = new List<string>(){"SummonUnlock", "NecroUnlock", "MoneyUnlock"};
        

        foreach (Skills item in PlayerData.skills)
        {
            if(exceptions.Contains(item.type) && item.value > -1){
                item.value = 0;
            }
            else{
                item.value = item.max_value == 4 ? 0 : -1;
            }
            

        }

        GetComponentInParent<Animator>().SetBool("InfoDisplay",false);
        PlayerData.embers += PlayerData.skillTreeEmbers;
        PlayerData.skillTreeEmbers = 0;
        changeEmberAmountUI();
        WritingData();
        treeReset?.Invoke(this, new EventArgs());
        
    }
    public void toggleSkillTree(GameObject SkillTreePanel){
        GetComponentInParent<Animator>().SetBool("InfoDisplay",false);
        SkillTreePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(SkillTreePanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);

    }


   
    
}
