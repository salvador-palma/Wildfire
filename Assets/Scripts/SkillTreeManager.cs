using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FMOD;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


[System.Serializable]
public class Skills{
    public string type;
    public int level;
    public bool ban;
    public bool pick;
    public int max_level_reached;
}

[System.Serializable]
public class SerializableList<T> {
    public long embers;
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
    public int QuestID;
    public NPC npc;
    public int DialogueID;

}
public class SkillTreeManager : MonoBehaviour
{
    
    [SerializeField] public SerializableList<Skills> PlayerData;
    [SerializeField] Ability[] Abilities;
    public static SkillTreeManager Instance;

    [Header("UI TMP")]
    [SerializeField] TextMeshProUGUI emberText;

    [Header("Info Panel")]
    [SerializeField] DynamicText titleText;
    [SerializeField] DynamicText typeText;
    [SerializeField] DynamicText purchaseButtonText;
    [SerializeField] DynamicText[] passivesText;
    
    [SerializeField] Animator anim;
    public event EventHandler treeReset;

    [Header("Color Pallete")]
    [SerializeField] ColorBlock UnlockedColor;
    [SerializeField] ColorBlock SilverColor;
    [SerializeField] ColorBlock GoldColor;
    [SerializeField] ColorBlock PrismaticColor;
    [SerializeField] ColorBlock ReachedColor;


    [Header("Black Market")]
    [SerializeField] private int BanLimit;
    [SerializeField] private int BanAmount;
    [SerializeField] private int PrePickLimit;
    [SerializeField] private int PrePickAmount;
    [SerializeField] DynamicText PreBanExplanation;

    [SerializeField] Button[] PickBanButtons;


    public static string CharacterPATH = "/charactersDEMO.json";
    public static string SkillsPATH = "/skillsDEMO.json";
    public static string BestiaryPATH = "/bestiaryDEMO.json";
    public static string VariablesPATH = "/variablesDEMO.json";
    public static string NPCPATH = "/npcsDEMO.json";
    
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
    public int getMaxLevelReached(string skill){
        Skills result = GetSkill(skill);
        result.max_level_reached = Math.Max(result.max_level_reached, result.level);
        return result==null ? -1 : result.max_level_reached;
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
            AudioManager.PlayOneShot(FMODEvents.Instance.MoneyDrop, transform.position);
            PlayerData.skillTreeEmbers+=price;
            PlayerData.embers-=price;
            changeEmberAmountUI();
            skill.level++;
            skill.max_level_reached = Math.Max(skill.max_level_reached, skill.level);

            if(skill.level==2){
                CheckForQuestDialogue(getAbility(skill_name));
            }

            CheckForConstelationUnlock();
            WritingData();

            return true;
        }
        return false;
    }

    private void CheckForQuestDialogue(Ability ability){
        if(ability.QuestID != -1 && !GameVariables.hasQuestAssigned(ability.QuestID)){
            NPC npc = ability.npc;
            if(npc != null){
                npc.StartDialogue(ability.DialogueID);
            }
        } 
    }

    public void BanSkill(){

        SkillTreeButton ability = SkillTreeButton.SelectedButton;
        Skills skill = GetSkill(ability.AbilityName);
        if(skill.ban){
            skill.ban = false;
            BanAmount--;
        }else{
            if(BanAmount<BanLimit){
                skill.ban = true;
                BanAmount++;
            }
        }
        ability.UpdatePickBan();
        DisplaySkill(ability.AbilityName, ability.level);
        WritingData();
    }
    public void PrePickSkill(){
        SkillTreeButton ability = SkillTreeButton.SelectedButton;
        Skills skill = GetSkill(ability.AbilityName);
        if(skill.pick){
            skill.pick = false;
            PrePickAmount--;
        }else{
            if(PrePickAmount<PrePickLimit){
                skill.pick = true;
                PrePickAmount++;
            }
        }
        ability.UpdatePickBan();
        DisplaySkill(ability.AbilityName, ability.level);
        WritingData();
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
        File.WriteAllText(Application.persistentDataPath + SkillTreeManager.SkillsPATH, json);
        Debug.Log("Finished Writing Skill Tree...");
    }
    
    public void ReadData(){
        
        if(File.Exists(Application.persistentDataPath +SkillTreeManager.SkillsPATH)){
            string json = File.ReadAllText(Application.persistentDataPath +SkillTreeManager.SkillsPATH);
            PlayerData = JsonUtility.FromJson<SerializableList<Skills>>(json);
            Debug.Log("Finished Reading Skill Tree...");
        }else{
            CreateFile();
            ReadData();
        }
    }
     public void CreateFile(){
        string str = "{\"embers\":2329,\"skillTreeEmbers\":0,\"skills\":[{\"type\":\"Assassin\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Immolate\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Critical Strike\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Pirate\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Snow Pool\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Necromancer\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Ember Generation\",\"level\":-1,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Explosion\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Vampire\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Multicaster\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Static Energy\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Gambling\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Ritual\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Thunder\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Regeneration\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Magical Shot\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Lava Pool\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Burst Shot\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Bee Summoner\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Thorns\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Freeze\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Orbits\",\"level\":2,\"ban\":false,\"pick\":false,\"max_level_reached\":2},{\"type\":\"Flower Field\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":1},{\"type\":\"Resonance\",\"level\":-2,\"ban\":false,\"pick\":false,\"max_level_reached\":2}]}";
        File.WriteAllText(Application.persistentDataPath +SkillTreeManager.SkillsPATH, str);
    }

    public static void AddEmbersToJSON(int n){
        Debug.Log("Adding " + n + " embers to JSON");
        if(File.Exists(Application.persistentDataPath +SkillTreeManager.SkillsPATH)){
            string jsonR = File.ReadAllText(Application.persistentDataPath +SkillTreeManager.SkillsPATH);
            SerializableList<Skills> p  = JsonUtility.FromJson<SerializableList<Skills>>(jsonR);
            p.embers += n;
            
            string jsonW = JsonUtility.ToJson(p);
            File.WriteAllText(Application.persistentDataPath + SkillTreeManager.SkillsPATH, jsonW);
        }
    }
    public void AddEmbers(long n){
        
        //if(n < 0){AudioManager.PlayOneShot(FMODEvents.Instance.MoneyDrop, transform.position);}
        PlayerData.embers = Math.Max(0, Math.Min(PlayerData.embers + n, 2147483647));
        changeEmberAmountUI(0);
        WritingData();
    }

    public void UpdateBlackMarketItems(){

        BanLimit = new string[]{"Bellow","Deluxe Bellow"}.Count(s=>Item.has(s));
        PrePickLimit = new string[]{"Marshmallow Bowl","Marshmallow Bag"}.Count(s=>Item.has(s));
        BanAmount = PlayerData.skills.Count(s => s.ban) ;
        PrePickAmount = PlayerData.skills.Count(s => s.pick);


    }
   

    //UI PARTITION
    public void DisplaySkill(string skill, int level){
        anim.SetBool("DisplayInfo", true);
        
        
        
        Ability ability = Abilities.ToList().FirstOrDefault(a => a.Name == skill);
        Skills skillSk = GetSkill(skill);
        string[] descriptions = new string[]{ability.AbilityDescription1,ability.AbilityDescription2,ability.AbilityDescription3};
        Color active = Color.white;
        Color disabled = new Color(1,1,1,0.3f);

        if(ability != null){
            
            for (int i = 0; i < 3; i++)
            {
                passivesText[i].setColor(level >= i ? active : disabled);
                string dp = skillSk.max_level_reached >= i - 1 ? descriptions[i] : "???";
                passivesText[i].SetText("<size=100%><style=\"Yellow\">- Level " + (i+1) + " -</style><br><size=80%>{0}", new string[]{dp});
            }  
            
            titleText.SetText(ability.Name);
            typeText.SetText(ability.Type);

            int price = DeckBuilder.Instance.getPrice(skill, level + 1);

            if(level >= -1){
                purchaseButtonText.SetText("Upgrade ({0})", new string[]{price == - 1 ? "Maxed Out" : price.ToString()});
                purchaseButtonText.transform.parent.gameObject.SetActive(price != - 1);
                Skills s = GetSkill(skill);

                PickBanButtons[0].gameObject.SetActive(!s.pick && (BanAmount != BanLimit || s.ban) && level > -1);
                PickBanButtons[0].transform.GetChild(0).GetChild(0).gameObject.SetActive(s.ban);
                PickBanButtons[1].gameObject.SetActive(!s.ban && (PrePickLimit != PrePickAmount || s.pick) && level > -1);
                PickBanButtons[1].transform.GetChild(0).GetChild(0).gameObject.SetActive(s.pick);


                PreBanExplanation.SetText(s.ban? "This skill will not appear during the night" : s.pick? "You will start the night with this skill" : "");
            }else{
                purchaseButtonText.transform.parent.gameObject.SetActive(false);
                PreBanExplanation.SetText("Upgrade previous skills to unlock this one");
                PickBanButtons[0].gameObject.SetActive(false);
                PickBanButtons[1].gameObject.SetActive(false);
            }
        
             
            
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
            case 10:
                return ReachedColor;
            default:
                return UnlockedColor;
        }
    }
    public void changeEmberAmountUI(int amount = 0){
        PlayerData.embers += amount;
        emberText.text = PlayerData.embers.ToString();
    }

    public void resetSkillTree(){
        
        anim.SetBool("DisplayInfo", false);
        
        
        
        List<string> exceptionsLayer1 = new List<string>(){"Bee Summoner", "Ritual", "Ember Generation", "Assassin", "Critical Strike", "Regeneration", "Orbits", "Multicaster", "Necromancer", "Gambling"};
        List<string> exceptionsLayer2 = new List<string>(){"Vampire", "Burst Shot", "Freeze", "Resonance", "Pirate"};
        foreach(Skills skill in PlayerData.skills){
            if(exceptionsLayer1.Contains(skill.type)){
                skill.level = Math.Min(skill.level, -1);
            }else if(exceptionsLayer2.Contains(skill.type)){
                skill.level = Math.Min(skill.level, -2);
            }else{
                skill.level = -3;
            }
            skill.ban = false;
            skill.pick = false;
        }
        BanAmount = 0;
        PrePickAmount = 0;
       

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

        // Vector2 newPos = new Vector2(SkillTreePanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);
        // AudioManager.Instance.SetAmbienceParameter("OST_Volume", newPos.x <= 0? 0 : 1);
        // SkillTreePanel.GetComponent<RectTransform>().anchoredPosition = newPos;
        MetaMenuUI.Instance.ToggleMenu(SkillTreePanel);
        Vector2 newPos = new Vector2(SkillTreePanel.GetComponent<RectTransform>().anchoredPosition.x, 0);
        if(newPos.x <= 0){
            UpdateBlackMarketItems();
        }

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
        if(result && save == -1){
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






