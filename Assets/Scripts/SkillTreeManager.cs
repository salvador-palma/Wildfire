using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;


[System.Serializable]
public class Skills{
    public string type;
    public int level;
}
[System.Serializable]
public class SerializableList<T> {
    public int embers;
    public int skillTreeEmbers;
    public List<T> skills;
}
[Serializable]
public class Ability{
    public string Name;
    [TextArea] public string AbilityDescription1;
    [TextArea] public string AbilityDescription2;
    [TextArea] public string AbilityDescription3;
}
public class SkillTreeManager : MonoBehaviour
{
    
    [SerializeField] SerializableList<Skills> PlayerData;
    [SerializeField] Ability[] Abilities;
    public static SkillTreeManager Instance;

    [Header("UI TMP")]
    [SerializeField] TextMeshProUGUI emberText;

    [Header("Info Panel")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI purchaseButtonText;
    [SerializeField] TextMeshProUGUI[] passivesText;
    [SerializeField] Animator anim;
    public event EventHandler treeReset;
    private void Awake() {
        Instance = this;
        anim = GetComponent<Animator>();
        ReadData();
    }
    private void Start(){
        //changeUpgradeInfoTexts(displayedSkill);
        changeEmberAmountUI();
    }
    

    
    

    public int getLevel(string skill){
        Skills result = GetSkill(skill);
        return result==null ? -1 : result.level;
    }

    public void Upgrade(string skill_name){
        Skills skill = GetSkill(skill_name);
        int price = DeckBuilder.Instance.getPrice(skill_name, skill.level);
        if(price < PlayerData.embers){
            PlayerData.skillTreeEmbers+=price;
            PlayerData.embers-=price;
            changeEmberAmountUI();
            skill.level++;
            WritingData();
        }
    }
    public Skills GetSkill(string skill){
        
        foreach (Skills item in PlayerData.skills)
        {
            if(item.type == skill){return item;}
        }
        Debug.LogWarning("Skill not found: " + skill);
        return null;
    }
    public void WritingData(){
        
        
        string json = JsonUtility.ToJson(PlayerData);
        File.WriteAllText(Application.persistentDataPath + "/skills.json", json);
        Debug.Log("Finished Writing...");
    }
    
    public void ReadData(){
        
        if(File.Exists(Application.persistentDataPath +"/skills.json")){
            string json = File.ReadAllText(Application.persistentDataPath +"/skills.json");
            PlayerData = JsonUtility.FromJson<SerializableList<Skills>>(json);
            Debug.Log("Finished Reading...");
        }else{
            CreateFile();
            ReadData();
        }
    }
     public void CreateFile(){
        string str = "{\"embers\":0,\"skillTreeEmbers\":0,\"skills\":[{\"type\":\"Assassin\",\"level\":-1},{\"type\":\"Immolate\",\"level\":-3},{\"type\":\"Critical Strike\",\"level\":-1},{\"type\":\"Pirate\",\"level\":-3},{\"type\":\"Snow Pool\",\"level\":-3},{\"type\":\"Necromancer\",\"level\":-3},{\"type\":\"Ember Generation\",\"level\":-3},{\"type\":\"Explosion\",\"level\":-3},{\"type\":\"Vampire\",\"level\":-3},{\"type\":\"Multicaster\",\"level\":-1},{\"type\":\"Static Energy\",\"level\":-3},{\"type\":\"Gambling\",\"level\":-3},{\"type\":\"Ritual\",\"level\":-3},{\"type\":\"Thunder\",\"level\":-3},{\"type\":\"Regeneration\",\"level\":-1},{\"type\":\"Magical Shot\",\"level\":-3},{\"type\":\"Lava Pool\",\"level\":-3},{\"type\":\"Burst Shot\",\"level\":-3},{\"type\":\"Bee Summoner\",\"level\":-3},{\"type\":\"Thorns\",\"level\":-3},{\"type\":\"Freeze\",\"level\":-3},{\"type\":\"Orbits\",\"level\":-1},{\"type\":\"Flower Field\",\"level\":-3},{\"type\":\"Resonance\",\"level\":-3}]}";
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
    public void DisplaySkill(string skill){
        Debug.Log("Displaying Skill...");
        anim.SetTrigger("DisplayInfo");
        
        
        Ability ability = Abilities.ToList().FirstOrDefault(a => a.Name == skill);
        if(ability != null){
            passivesText[0].text = ability.AbilityDescription1;
            passivesText[1].text = ability.AbilityDescription2;
            passivesText[2].text = ability.AbilityDescription3;

            titleText.text = ability.Name;
            typeText.text = "Not-Done Effect";
            purchaseButtonText.text = "Upgrade (1500)";

            
            
        }else{
            Debug.LogWarning("Skill Not Found: " + skill);
        }


    }
    
    public void changeEmberAmountUI(int amount = 0){
        PlayerData.embers += amount;
        //emberAmountTxt.text = PlayerData.embers.ToString();
    }

    public void resetSkillTree(){
        
        PlayerData.embers += PlayerData.skillTreeEmbers;
        PlayerData.skillTreeEmbers = 0;
        changeEmberAmountUI();
        WritingData();
        treeReset?.Invoke(this, new EventArgs());
        
    }
    public void InvokeUIReset(){
        treeReset?.Invoke(this, new EventArgs());
    }
    public void toggleSkillTree(GameObject SkillTreePanel){
        GetComponentInParent<Animator>().SetBool("InfoDisplay",false);
        SkillTreePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(SkillTreePanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);

    }


   
    
}
