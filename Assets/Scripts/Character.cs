using System;
using System.Linq;
using TMPro;

using UnityEngine;
using UnityEngine.Events;

using System.IO;
using Image = UnityEngine.UI.Image;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
[Serializable]
public enum OSTType
{
    DEFAULT, CYBER, EERIE, ADVENTURE, SPACE, CASINO
}
public class Character : MonoBehaviour
{
    [Serializable]
    public class CharacterData{
        public string Name;
        public string AnimationBool;

        [Header("Abilities")]
        public string AbilityName;
        [TextArea] public string AbilityDescription;

        [Header("Appearance")]
        public Sprite BodyBack;
        public Sprite BodyFront;
        public Sprite Prop;
        public Sprite Face;
        public Color BodyBackColor;
        public Color BodyFrontColor;
        [Header("Environment")]
        public GameObject Environment;
        [SerializeField] public OSTType EnvironmentMusic;
        public bool Unlocked;
        public string AbilityToSkillTree;
        [Header("Hierarchy")]
        public string Subtype;
        public string Supratype;
        
          
    }
    
    
    public static Character Instance { get; private set; }
    public bool InMenu;
    [Header("Runtime Data")]
    public int active;

    public CharacterData[] characterDatas;
    

    private void Awake() {
        if(Instance==null){
            Instance = this;
            
        }
        
        ReadData();
        SetupCharacterSelectOptions();
        active = PlayerPrefs.GetInt("Character", 0);
        if(active != 0 && InMenu){
            if(characterDatas[active].AnimationBool != "None"){
                MainFlameVessel.GetComponent<Animator>().SetBool(characterDatas[active].AnimationBool, true);
            }
        }
        SkillTreeManager.Instance.treeReset += resetCharacter;
        UpdateCharacterInfo(characterDatas[currentDisplayedCharacter]);
        
    }


    // *********************************************************************************************** //
    // *************************************** IN-GAME SECTION *************************************** //
    // *********************************************************************************************** //
    public void SetupCharacter(string abilty_name){
        SetupCharacter(characterDatas.Where(e => e.AbilityName == abilty_name).First());
    }
    
    public void SetupCharacter(CharacterData type){
        if(type == null){
            Debug.Log("ERRORE NULL");
            throw new ArgumentNullException("Type cannot be null");
        }
        if(active != 0){
            Debug.Log("Cannot play more than one character at a time");
            return;
        }

        Deck.Instance.gameState.Character = type.AbilityName;
        
        active = Array.FindIndex(characterDatas, e => e == type);
        GameUI.Instance.UpdateProfileCharacter();
        if(EnemySpawner.Instance.current_round <= 0){
            SetupActiveLooks();
        }else{
            EnemySpawner.Instance.Paused = true;
            GameUI.Instance.playCharacterTransition();
        }
        
    }
    public bool isCharacter(string ability_name){
        if(active == 0){return false;}
        return characterDatas[active].AbilityName.Equals(ability_name);
    }
    public bool isACharacter(){
        return active != 0;
    }

    public void SetupActiveLooks(){
        SetupSkin(characterDatas[active]);
        SetupEnvironment(characterDatas[active]);
        SetupBehaviour();
        SetupMusic(characterDatas[active].EnvironmentMusic);
    }

    private void SetupMusic(OSTType music)
    {
        AudioManager.Instance.SetAmbienceParameter("Arena", (int)music);
    }

    public string getDescription(string ability_name = null){
        if(ability_name == null){
            
            return characterDatas[active].AbilityDescription;
        }
        return characterDatas.Where(e => e.AbilityName == ability_name).First().AbilityDescription;
    }
    public string getName(string ability_name = null){
        if(ability_name == null){
            return characterDatas[active].Name;
        }
        return characterDatas.Where(e => e.AbilityName == ability_name).First().Name;
    }
    public void TransformVesselToCharacter(GameObject Vessel, string ability_name = null, bool locked = false){
        if(locked){
            Vessel.transform.Find("BodyBack").GetComponent<Image>().color = new Color(0.12f, 0.03f, 0.195f);
            Vessel.transform.Find("BodyFront").gameObject.SetActive(false);
            Vessel.transform.Find("Face").gameObject.SetActive(false);
            Vessel.transform.Find("Prop").gameObject.SetActive(false);
            return;
        }
        CharacterData data  = ability_name == null ? characterDatas[active] : characterDatas.Where(e => e.AbilityName == ability_name).First();
        if(data.BodyFront == null){
            Vessel.transform.Find("BodyFront").gameObject.SetActive(false);
        }else{
            Vessel.transform.Find("BodyFront").GetComponent<Image>().sprite = data.BodyFront;
            Vessel.transform.Find("BodyFront").GetComponent<Image>().color = data.BodyFrontColor;
        }
        if(data.BodyBack == null){
            Vessel.transform.Find("BodyBack").gameObject.SetActive(false);
        }else{
            Vessel.transform.Find("BodyBack").GetComponent<Image>().sprite = data.BodyBack;
            Vessel.transform.Find("BodyBack").GetComponent<Image>().color = data.BodyBackColor;
        }   
        if(data.Face == null){
            Vessel.transform.Find("Face").gameObject.SetActive(false);
        }else{
            Vessel.transform.Find("Face").GetComponent<Image>().sprite = data.Face;
        }
        if(data.Prop == null){
            Vessel.transform.Find("Prop").gameObject.SetActive(false);
        }else{
            Vessel.transform.Find("Prop").gameObject.SetActive(true);
            Vessel.transform.Find("Prop").GetComponent<Image>().sprite = data.Prop;
        }
    }
    
    private void SetupSkin(CharacterData type)
    {
        Flamey.Instance.GetComponent<Animator>().SetBool(type.AnimationBool, true);
    }

    private void SetupEnvironment(CharacterData type)
    {
        if(type.Environment != null){
            Instantiate(type.Environment);
            Destroy(GameObject.Find("Floor"));
            Debug.Log("Environment Change");
        }
    }
    
    // *********************************************************************************************** //
    // ************************************** BEHAVIOUR SECTION ************************************** //
    // *********************************************************************************************** //
    public void SetupBehaviour(){
        switch(characterDatas[active].AbilityName){
            case "Default":break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Multicaster":
                if(SecondShot.Instance == null && SkillTreeManager.Instance.getLevel("Multicaster") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Multicaster").Activate();
                }
                
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Thorns":
                if(ThornsOnHitted.Instance == null && SkillTreeManager.Instance.getLevel("Thorns") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Thorns").Activate();
                }
                ThornsOnHitted.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "OrbitalMercury":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }
                FlameCircle.Instance.SpawnExtraAssets(0);
                 break;
            case "OrbitalVenus":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(1);
                 break;
            case "OrbitalEarth":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(2);
                 break;
            case "OrbitalMars":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(3);
                 break;
            case "OrbitalJupiter":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(4);
                 break;
            case "OrbitalSaturn":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(5);
                 break;
            case "OrbitalUranus":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(6);
                 break;
            case "OrbitalNeptune":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Orbits") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Orbital Flames").Activate();
                }FlameCircle.Instance.SpawnExtraAssets(7);
                 break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Ember Generation":
                if(FlameCircle.Instance == null && SkillTreeManager.Instance.getLevel("Ember Generation") >= 0){
                    Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, 1));
                }
                MoneyMultipliers.Instance.ReloadShinyStats();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Bee Summoner": 
                if(Summoner.Instance == null && SkillTreeManager.Instance.getLevel("Bee Summoner") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Beekeeper").Activate();
                }
                Summoner.Instance.SpawnExtraAssets(); 
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Vampire":
                if(VampOnHit.Instance == null && SkillTreeManager.Instance.getLevel("Vampire") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Vampire").Activate();
                }
                VampOnHit.Instance.cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/VampUnlock"));
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Freeze": 
                if(IceOnHit.Instance == null && SkillTreeManager.Instance.getLevel("Freeze") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Freeze!").Activate();
                }
                IceOnHit.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Shred":
                if(ShredOnHit.Instance == null && SkillTreeManager.Instance.getLevel("Resonance") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Resonance").Activate();
                }
                ShredOnHit.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Assassin":
                if(ExecuteOnHit.Instance == null && SkillTreeManager.Instance.getLevel("Assassin") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Assassin's Path").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Statik":
                if(StatikOnHit.Instance == null && SkillTreeManager.Instance.getLevel("Static Energy") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Static Energy").Activate();
                }
                StatikOnHit.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Explosion":
                if(Explosion.Instance == null && SkillTreeManager.Instance.getLevel("Explosion") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Explosion").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Necro":
                if(Necromancer.Instance == null && SkillTreeManager.Instance.getLevel("Necromancer") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Necromancer").Activate();
                }
                Necromancer.Instance.MegaGhoulProbability = 0.1f;
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Pirate":
                if(Bullets.Instance == null && SkillTreeManager.Instance.getLevel("Pirate") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Pirate").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Lava":
                if(BurnOnLand.Instance == null && SkillTreeManager.Instance.getLevel("Lava Pool") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Lava Pool").Activate();
                }
                BurnOnLand.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Snow Pool":
                if(IceOnLand.Instance == null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Snow Pool").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Flower Field":
                if(DrainOnLand.Instance == null && SkillTreeManager.Instance.getLevel("Flower Field") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Flower Field").Activate();
                }
               
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Burst":
                if(BurstShot.Instance == null && SkillTreeManager.Instance.getLevel("Burst Shot") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Burst Shot").Activate();
                }
                BurstShot.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Crit":
                if(CritUnlock.Instance == null && SkillTreeManager.Instance.getLevel("Critical Strike") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Critical Strike").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Ritual":
                if(CandleTurrets.Instance == null && SkillTreeManager.Instance.getLevel("Ritual") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Ritual").Activate();
                }
                CandleTurrets.Instance.SpawnExtraAssets();
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "ImmolateFire":
                if(Immolate.Instance == null && SkillTreeManager.Instance.getLevel("Immolate") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Immolate").Activate();
                }
                Immolate.Instance.SpawnExtraAssets(0);
                break;
            case "ImmolateWater":
                if(Immolate.Instance == null && SkillTreeManager.Instance.getLevel("Immolate") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Immolate").Activate();
                }
                Immolate.Instance.SpawnExtraAssets(1);
                break;
            case "ImmolateEarth":
                if(Immolate.Instance == null && SkillTreeManager.Instance.getLevel("Immolate") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Immolate").Activate();
                }
                Immolate.Instance.SpawnExtraAssets(2);
                break;
            case "ImmolateAir":
                if(Immolate.Instance == null && SkillTreeManager.Instance.getLevel("Immolate") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Immolate").Activate();
                }
                Immolate.Instance.SpawnExtraAssets(3);
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Thunder":
                if(LightningEffect.Instance == null && SkillTreeManager.Instance.getLevel("Thunder") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Thunder").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Regeneration":
                if(HealthRegen.Instance == null && SkillTreeManager.Instance.getLevel("Regeneration") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Regeneration").Activate();
                }
                HealthRegen.Instance.SpawnExtraAssets();
                break;
            case "Laser Beam":
                if(Laser.Instance == null && SkillTreeManager.Instance.getLevel("Laser Beam") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Laser Beam").Activate();
                }
                break;
            case "Gravity":
                if(Gravity.Instance == null && SkillTreeManager.Instance.getLevel("Gravity") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Gravity").Activate();
                }
                Gravity.Instance.SpawnExtraAssets();
                break;
            case "Whirlpool":
                if(Whirpool.Instance == null && SkillTreeManager.Instance.getLevel("Whirlpool") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Whirlpool").Activate();
                }
                break;
            case "Totem":
                if(Totem.Instance == null && SkillTreeManager.Instance.getLevel("Totem") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Totem").Activate();
                }
                break;
            case "Smog":
                if(Smog.Instance == null && SkillTreeManager.Instance.getLevel("Smog") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Smog").Activate();
                }
                break;
            case "Earthquake":
                if(Earthquake.Instance == null && SkillTreeManager.Instance.getLevel("Earthquake") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Earthquake").Activate();
                }
                break;
            /*--------------------------------------------------------------------------------------------------*/
            case "Magical Shot":
                if(KrakenSlayer.Instance == null && SkillTreeManager.Instance.getLevel("Magical Shot") >= 0){
                    DeckBuilder.Instance.getAugmentByName("Magical Shot").Activate();
                }
                KrakenSlayer.Instance.SpawnExtraAssets();
                break;
            case "Gambling":
                if(Gambling.Instance == null && SkillTreeManager.Instance.getLevel("Gambling") >= 0){
                    Flamey.Instance.addNotEspecificEffect(new Gambling());
                }
                Gambling.Instance.SpawnExtraAssets();
                break;
            default:
                Debug.Log("No Character Found: " + characterDatas[active].AbilityName);break;
            
        }
    }
    // *********************************************************************************************** //
    // ************************************ CHARACTER UI SECTION ************************************* //
    // *********************************************************************************************** //
    [Header("Character Select UI")]
    public GameObject CharacterSelectContainer;
    public GameObject CharacterSubTypeContainer;
    public DynamicText CharacterName;
    public DynamicText SkillDescription;
    [SerializeField] private int currentDisplayedCharacter = 0;
    [SerializeField] private GameObject MainFlameVessel;
    private void SetupCharacterSelectOptions(){
        
        foreach (Transform item in CharacterSelectContainer.transform)
        {
            if(item.gameObject.activeInHierarchy){Destroy(item.gameObject);}   
        }

        GameObject template = CharacterSelectContainer.transform.GetChild(0).gameObject;
        foreach (CharacterData character in characterDatas)
        {
            if(character.Subtype == ""){
                
                GameObject g  = Instantiate(template, CharacterSelectContainer.transform);
                if(character.Supratype == ""){
                    if(character.Unlocked){
                        TransformVesselToCharacter(g, character.AbilityName);
                    }else{
                        TransformVesselToCharacter(g, locked:true);
                    }
                }else{
                    if(characterDatas.Any(c => c.Subtype == character.Supratype && c.Unlocked)){
                        TransformVesselToCharacter(g, characterDatas.First(c => c.Subtype == character.Supratype && c.Unlocked).AbilityName);
                    }else{
                        TransformVesselToCharacter(g, locked:true);
                    }
                }
                g.SetActive(true);
            }
            
        }
    }
    private void SetupCharacterSupTypeOptions(string supraType){
        GameObject template = CharacterSubTypeContainer.transform.GetChild(0).gameObject;
        foreach (CharacterData character in characterDatas.Where(c => c.Subtype==supraType))
        {
            GameObject g  = Instantiate(template, CharacterSubTypeContainer.transform);
            if(character.Unlocked){
                TransformVesselToCharacter(g, character.AbilityName);
                g.GetComponent<Button>().onClick.AddListener(()=>SelectSubTypeOption(character.AbilityName));
            }else{
                TransformVesselToCharacter(g, locked:true);
                g.GetComponent<Button>().interactable = false;
            }
            g.SetActive(true);
        }
    }
    public void SelectSubTypeOption(string ability_name){
        int currentDisplayedCharacter = characterDatas.ToList().FindIndex(0, characterDatas.Length-1, c => c.AbilityName == ability_name);
        UpdateCharacterInfo(characterDatas[currentDisplayedCharacter]);
        int index = 0;
        for (int i = 0; i < characterDatas.Length; i++)
        {
            if(characterDatas[i].Supratype == characterDatas[currentDisplayedCharacter].Subtype){
                break;
            }
            if(characterDatas[i].Subtype == ""){
                index++;
            }
        }
        TransformVesselToCharacter(CharacterSelectContainer.transform.GetChild(index+1).gameObject,ability_name);
        

        if(!InMenu){return;}
        if(characterDatas[active].AnimationBool != "None"){
            MainFlameVessel.GetComponent<Animator>().SetBool(characterDatas[active].AnimationBool, false);
        }
        active = currentDisplayedCharacter;
        MainFlameVessel.GetComponent<Animator>().SetTrigger("Reset");
        if(characterDatas[active].AnimationBool != "None"){
            MainFlameVessel.GetComponent<Animator>().SetBool(characterDatas[active].AnimationBool, true);
        }
    }

    public void MoveCharacterSelectOption(int dir){
        int newIndex = currentDisplayedCharacter + dir;
        while (newIndex >= -1 && newIndex < characterDatas.Count() + 1)
        {
            if(newIndex < 0){newIndex = characterDatas.Count()-1;}
            if(newIndex > characterDatas.Count()-1){newIndex = 0;}
            if(characterDatas[currentDisplayedCharacter].Subtype != ""){
                if(characterDatas[newIndex].Supratype != characterDatas[currentDisplayedCharacter].Subtype && characterDatas[newIndex].Subtype == ""){
                    currentDisplayedCharacter = newIndex;
                    break;
                }
            }else if(characterDatas[newIndex].Subtype == ""){
                currentDisplayedCharacter = newIndex;
                break;
            }
            newIndex += dir;
            
        }
        int offset = currentDisplayedCharacter;
        for(int i =0; i<currentDisplayedCharacter; i++){
            if(characterDatas[i].Subtype != ""){offset--;}
        }
        CharacterSelectContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(344.5f + (-155.10608f) * offset, CharacterSelectContainer.GetComponent<RectTransform>().anchoredPosition.y); 
        
        if(characterDatas[currentDisplayedCharacter].Supratype != ""){
            if(characterDatas.Any(c => c.Subtype == characterDatas[currentDisplayedCharacter].Supratype && c.Unlocked)){
                if(SkillTreeManager.Instance.getLevel(characterDatas[currentDisplayedCharacter].AbilityToSkillTree)>=0){
                    SetupCharacterSupTypeOptions(characterDatas[currentDisplayedCharacter].Supratype);
                }
                currentDisplayedCharacter = characterDatas.ToList().FindIndex(0, characterDatas.Length-1, c => c.Subtype == characterDatas[currentDisplayedCharacter].Supratype && c.Unlocked);
                
            }
            UpdateCharacterInfo(characterDatas[currentDisplayedCharacter]);
        }else{
            UpdateCharacterInfo(characterDatas[currentDisplayedCharacter]);
            foreach (Transform t in CharacterSubTypeContainer.transform)
            {
                if(t.gameObject.activeInHierarchy){Destroy(t.gameObject);}
            }
        }
        if(!InMenu){return;}
        if(characterDatas[currentDisplayedCharacter].Unlocked && (characterDatas[currentDisplayedCharacter].AbilityToSkillTree == "" || SkillTreeManager.Instance.getLevel(characterDatas[currentDisplayedCharacter].AbilityToSkillTree)>=0)){
            if(characterDatas[active].AnimationBool != "None"){
            MainFlameVessel.GetComponent<Animator>().SetBool(characterDatas[active].AnimationBool, false);
            }
            active = currentDisplayedCharacter;
            MainFlameVessel.GetComponent<Animator>().SetTrigger("Reset");
            if(characterDatas[active].AnimationBool != "None"){
                MainFlameVessel.GetComponent<Animator>().SetBool(characterDatas[active].AnimationBool, true);
            }
        }

    }
    private void UpdateCharacterInfo(CharacterData data){
        
        if(data.Unlocked){
            CharacterName.SetText(data.Name);
            SkillDescription.SetText("<size=100%><style=\"Yellow\">- Ability -</style><size=80%><br>{0}", new string[]{data.AbilityDescription});
        }else{
            CharacterName.SetText("???");
            SkillDescription.SetText("<size=100%><style=\"Yellow\">- Ability -</style><size=80%><br>???");
        }
        
    }

    public void toggleCharacterPanel(GameObject CharacterSelectPanel){
        if(CharacterSelectPanel.GetComponent<RectTransform>().anchoredPosition.x > 2000){
            SyncSkillTreeManagerToCharacterSelect();
            foreach (Transform child in CharacterSubTypeContainer.transform)
            {
                if(child.gameObject.activeInHierarchy){Destroy(child.gameObject);}
            }
            int offset=0;
            currentDisplayedCharacter = active;
            if(characterDatas[active].Subtype==""){
                offset = currentDisplayedCharacter;
                for(int i =0; i<currentDisplayedCharacter; i++){if(characterDatas[i].Subtype != ""){offset--;}}
            }else{
                for (int i = 0; i < characterDatas.Length; i++)
                {
                    if(characterDatas[i].Supratype == characterDatas[currentDisplayedCharacter].Subtype){break;}
                    if(characterDatas[i].Subtype == ""){offset++;}
                }
            }

            if(characterDatas[currentDisplayedCharacter].Subtype!=""){
                SetupCharacterSupTypeOptions(characterDatas[currentDisplayedCharacter].Subtype);
            }
            TransformVesselToCharacter(CharacterSelectContainer.transform.GetChild(offset+1).gameObject, characterDatas[currentDisplayedCharacter].AbilityName);
            CharacterSelectContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(344.5f + (-155.10608f) * offset, CharacterSelectContainer.GetComponent<RectTransform>().anchoredPosition.y); 
            UpdateCharacterInfo(characterDatas[currentDisplayedCharacter]);
            //CharacterSelectPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);


        }else{
            PlayerPrefs.SetInt("Character", active);
            //CharacterSelectPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(4000, 0);
        }
        MetaMenuUI.Instance.ToggleMenu(CharacterSelectPanel);
    }
    public void WritingData(){
        SerialList<CharacterUnlockedData> unlockList = new SerialList<CharacterUnlockedData>(){list=new List<CharacterUnlockedData>()};
        foreach (CharacterData character in characterDatas)
        {
            unlockList.list.Add(new CharacterUnlockedData(character.Name, character.Unlocked));
        }
        string json = JsonUtility.ToJson(unlockList);
        File.WriteAllText(Application.persistentDataPath + "/characters.json", json);
    }
    public void ReadData(){
        
        if(File.Exists(Application.persistentDataPath +"/characters.json")){
            string json = File.ReadAllText(Application.persistentDataPath +"/characters.json");
            JsonUtility.FromJson<SerialList<CharacterUnlockedData>>(json).list.ForEach(c1 => characterDatas.First(c2 => c2.Name == c1.Name || (c2.Name=="Phoenix" && c1.Name=="Pheonix")).Unlocked = c1.Unlocked);
            WritingData();
        }else{
            CreateFile();
            ReadData();
        }
    }
    private void CreateFile(){
        string str = "{\"active\":0,\"list\":[{\"Name\":\"Flame\",\"Unlocked\":true},{\"Name\":\"Double Faced\",\"Unlocked\":false},{\"Name\":\"Rose Warrior\",\"Unlocked\":false},{\"Name\":\"Sir Flareington\",\"Unlocked\":false},{\"Name\":\"Fire Bee\",\"Unlocked\":false},{\"Name\":\"Von Van Pyre\",\"Unlocked\":false},{\"Name\":\"Ice Cube\",\"Unlocked\":false},{\"Name\":\"Echo\",\"Unlocked\":false},{\"Name\":\"Ash Pyre\",\"Unlocked\":false},{\"Name\":\"Tesla Coil\",\"Unlocked\":false},{\"Name\":\"Bomber\",\"Unlocked\":false},{\"Name\":\"King Ghoul\",\"Unlocked\":false},{\"Name\":\"Captain Ember Teach\",\"Unlocked\":false},{\"Name\":\"Mt. Vesuvius\",\"Unlocked\":false},{\"Name\":\"Mt. Everest\",\"Unlocked\":false},{\"Name\":\"Flora\",\"Unlocked\":false},{\"Name\":\"Blaze Brigade\",\"Unlocked\":false},{\"Name\":\"Powered Up\",\"Unlocked\":false},{\"Name\":\"Ankh-Ra\",\"Unlocked\":false},{\"Name\":\"Phoenix\",\"Unlocked\":false},{\"Name\":\"Monk\",\"Unlocked\":false},{\"Name\":\"Fire Monk\",\"Unlocked\":false},{\"Name\":\"Water Monk\",\"Unlocked\":false},{\"Name\":\"Air Monk\",\"Unlocked\":false},{\"Name\":\"Earth Monk\",\"Unlocked\":false},{\"Name\":\"Azureoth\",\"Unlocked\":false},{\"Name\":\"Orbital\",\"Unlocked\":false},{\"Name\":\"Mercury\",\"Unlocked\":false},{\"Name\":\"Venus\",\"Unlocked\":false},{\"Name\":\"Earth\",\"Unlocked\":false},{\"Name\":\"Mars\",\"Unlocked\":false},{\"Name\":\"Jupiter\",\"Unlocked\":false},{\"Name\":\"Saturn\",\"Unlocked\":false},{\"Name\":\"Uranus\",\"Unlocked\":false},{\"Name\":\"Neptune\",\"Unlocked\":false},{\"Name\":\"Zeus\",\"Unlocked\":false},{\"Name\":\"Clown\",\"Unlocked\":false}]}";
        File.WriteAllText(Application.persistentDataPath +"/characters.json", str);        
    }
    
    public bool isCharacterUnlocked(string character_name = null){

        return character_name==null ? characterDatas[active].Unlocked : characterDatas.First(c=>c.Name==character_name).Unlocked;
    }
    public void Unlock(string character_name){
        
        if(!characterDatas.Any(c=>c.Name==character_name)){Debug.LogError("Character " + character_name + " not found"); return;}
        
        CharacterUnlockPopUp(characterDatas.First(c=>c.Name==character_name));
        characterDatas.First(c=>c.Name==character_name).Unlocked = true;
        
        WritingData();

        //TEMPORARY CLORIS WARDROBE UNLOCK
        if(GameVariables.GetVariable("ClorisWardrobe")==-1){
            QuestBoard.Instance.Cloris.QueueDialogue(9);
            GameVariables.SetVariable("ClorisWardrobe",1);
        }

        SetupCharacterSelectOptions();

        
    }

    private void CharacterUnlockPopUp(CharacterData characterData)
    {
        UnityAction postAction = null;
        if(characterData.Name == "Saturn"){
            postAction = () => QuestBoard.PopUpPlanetsQuest();
            Debug.Log("Assigned");
        }
        
        
        
        MetaMenuUI.Instance.UnlockableScreen("NEW STYLE", characterData.Name, characterData.AbilityDescription, 4, postAction);
    }

    public void SyncSkillTreeManagerToCharacterSelect(){
        int i=0;
        foreach (CharacterData character in characterDatas)
        {
            if(character.Subtype!=""){continue;}
            if(character.AbilityToSkillTree==""){i++; continue;}
            if(SkillTreeManager.Instance.getLevel(character.AbilityToSkillTree)<0 && character.Unlocked){
                CharacterSelectContainer.transform.GetChild(i+1).Find("Warning").gameObject.SetActive(true);
                CharacterSelectContainer.transform.GetChild(i+1).Find("Warning").Find("Text").GetComponentInChildren<DynamicText>().SetText("Requires {0}", new string[]{character.AbilityToSkillTree});
            }else{
                CharacterSelectContainer.transform.GetChild(i+1).Find("Warning").gameObject.SetActive(false);
            }
            
            i++;
        }
    }
    public void resetCharacter(object sender, EventArgs e){
        MainFlameVessel.GetComponent<Animator>().SetBool(characterDatas[active].AnimationBool, false);
        active = 0;
        PlayerPrefs.SetInt("Character", 0);
        MainFlameVessel.GetComponent<Animator>().SetTrigger("Reset");
    }
    public bool HasAtLeastOneCharacter(){
        foreach(CharacterData character in characterDatas){
            if(character.Name != "Flame" && character.Unlocked){return true;}
        }
        return false;
    }
}
[Serializable]
public class SerialList<T>{
    public int active;
    public List<T> list;
}
[Serializable]
public class CharacterUnlockedData{
    public string Name;
    public bool Unlocked;
    public CharacterUnlockedData(string n, bool u){Name=n; Unlocked=u;}
}
