using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


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
    public string Type;
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

    [Header("Color Pallete")]
    [SerializeField] ColorBlock UnlockedColor;
    [SerializeField] ColorBlock SilverColor;
    [SerializeField] ColorBlock GoldColor;
    [SerializeField] ColorBlock PrismaticColor;

    private void Awake() {
        Instance = this;
        anim = GetComponent<Animator>();

        

        ReadData();
    }
    private void Start(){
        changeEmberAmountUI();
    }
    
    public int getLevel(string skill){
        Skills result = GetSkill(skill);
        return result==null ? -1 : result.level;
    }

    
    public bool Upgrade(string skill_name, bool Unlock = false){
        Skills skill = GetSkill(skill_name);
        int price = DeckBuilder.Instance.getPrice(skill_name, skill.level + 1);
        if(price == -1){return false;}

        if(price == -2){
            changeEmberAmountUI();
            skill.level++;
            if(Unlock){skill.level=-1;}
            
            WritingData();
            return true;
        }
        if(price < PlayerData.embers){
            PlayerData.skillTreeEmbers+=price;
            PlayerData.embers-=price;
            changeEmberAmountUI();
            skill.level++;
            CheckForConstelationUnlock();
            WritingData();

            return true;
        }
        return false;
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
        string str = "{\"embers\":0,\"skillTreeEmbers\":0,\"skills\":[{\"type\":\"Assassin\",\"level\":-1},{\"type\":\"Immolate\",\"level\":-3},{\"type\":\"Critical Strike\",\"level\":-1},{\"type\":\"Pirate\",\"level\":-2},{\"type\":\"Snow Pool\",\"level\":-3},{\"type\":\"Necromancer\",\"level\":-2},{\"type\":\"Ember Generation\",\"level\":-1},{\"type\":\"Explosion\",\"level\":-3},{\"type\":\"Vampire\",\"level\":-2},{\"type\":\"Multicaster\",\"level\":-1},{\"type\":\"Static Energy\",\"level\":-3},{\"type\":\"Gambling\",\"level\":-2},{\"type\":\"Ritual\",\"level\":-1},{\"type\":\"Thunder\",\"level\":-3},{\"type\":\"Regeneration\",\"level\":-1},{\"type\":\"Magical Shot\",\"level\":-3},{\"type\":\"Lava Pool\",\"level\":-3},{\"type\":\"Burst Shot\",\"level\":-2},{\"type\":\"Bee Summoner\",\"level\":-3},{\"type\":\"Thorns\",\"level\":-3},{\"type\":\"Freeze\",\"level\":-2},{\"type\":\"Orbits\",\"level\":-1},{\"type\":\"Flower Field\",\"level\":-3},{\"type\":\"Resonance\",\"level\":-2}]}";
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
    public void DisplaySkill(string skill, int level){
        Debug.Log("Displaying Skill...");
        anim.SetBool("DisplayInfo", true);
        
        
        Ability ability = Abilities.ToList().FirstOrDefault(a => a.Name == skill);
        if(ability != null){

            passivesText[0].text = "<size=100%><color=#FFFF00>- Level 1 -</color><br><size=80%>" + ability.AbilityDescription1;
            passivesText[1].text = "<size=100%><color=#FFFF00>- Level 2 -</color><br><size=80%>" + ability.AbilityDescription2;
            passivesText[2].text = "<size=100%><color=#FFFF00>- Level 3 -</color><br><size=80%>" + ability.AbilityDescription3;
            passivesText[0].color = Color.white;
            passivesText[1].color = Color.white;
            passivesText[2].color = Color.white;

            switch (level+1)
            {
                
                case 0:
                    passivesText[0].color = new Color(1,1,1,0.3f);
                    passivesText[1].text = "<size=100%><color=#FFFF00>- Level 2 -</color><br><size=80%>???";
                    passivesText[1].color = new Color(1,1,1,0.3f);
                    passivesText[2].text = "<size=100%><color=#FFFF00>- Level 3 -</color><br><size=80%>???";
                    passivesText[2].color = new Color(1,1,1,0.3f);
                    break;
                case 1:
                    passivesText[1].color = new Color(1,1,1,0.3f);
                    passivesText[2].text = "<size=100%><color=#FFFF00>- Level 3 -</color><br><size=80%>???";
                    passivesText[2].color = new Color(1,1,1,0.3f);
                    break;
                case 2:
                    passivesText[2].color = new Color(1,1,1,0.3f);
                    break;
            }
           
            

            titleText.text = ability.Name;
            typeText.text = ability.Type;

            int price = DeckBuilder.Instance.getPrice(skill, level + 1);
            purchaseButtonText.text = string.Format("Upgrade ({0})",  price == - 1 ? "Maxed Out" : price);

            
            
        }else{
            Debug.LogWarning("Skill Not Found: " + skill);
        }


    }
    public Ability getAbility(string SkillTreeName){
        return Abilities.Where(a=> a.Name == SkillTreeName).FirstOrDefault();
    }
    
    public ColorBlock GetColors(int level){
        switch(level){
            case 0:
                return SilverColor;
            case 1:
                return GoldColor;
            case 2:
                return PrismaticColor;
            default:
                return UnlockedColor;
        }
    }
    public void changeEmberAmountUI(int amount = 0){
        PlayerData.embers += amount;
        emberText.text = PlayerData.embers.ToString();
    }

    public void resetSkillTree(){
        List<string> exceptionsLayer1 = new List<string>(){"Bee Summoner", "Ritual", "Ember Generation", "Assassin", "Critical Strike", "Regeneration", "Orbits", "Multicaster"};
        List<string> exceptionsLayer2 = new List<string>(){"Vampire", "Burst Shot", "Freeze", "Resonance", "Pirate", "Necromancer", "Gambling"};
        foreach(Skills skill in PlayerData.skills){
            if(exceptionsLayer1.Contains(skill.type)){
                skill.level = Math.Min(skill.level, -1);
            }else if(exceptionsLayer2.Contains(skill.type)){
                skill.level = Math.Min(skill.level, -2);
            }else{
                skill.level = -3;
            }
        }
        GetComponentInParent<Animator>().SetBool("InfoDisplay",false);

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
        
        SkillTreePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(SkillTreePanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);

    }

    public bool HasAtLeastOneSkill(){
        return PlayerData.skills.Any(a => a.level >= 0);
    }

    //CONSTELATION
    public void StartSkillTreeCutscene(bool on){
        
        GetComponentInParent<Animator>().SetBool("Constelation", on);
    }
    public void CheckForConstelationUnlock(){
        bool result = PlayerData.skills.All(e => e.level >= 0);
        int save = GameVariables.GetVariable("ConstelationCutScene");
        if(save == -1 && result){
            GameVariables.SetVariable("ConstelationCutScene", 1);
            StartSkillTreeCutscene(true);
        }

    }
    public void ResetSkillTreeDimensions(){
        anim.SetBool("DisplayInfo", false);
        GameObject SkillTree = MetaMenuUI.Instance.SkillTree;
        SkillTree.transform.localScale = new Vector2(0.2642097f,0.2642097f);
        SkillTree.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,15);
    }
    


   
    
}
