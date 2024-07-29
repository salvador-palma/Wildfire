using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class Character : MonoBehaviour
{
    [Serializable]
    public class CharacterData{
        public string Name;
        public string AnimationBool;
        public string AbilityName;
        [TextArea] public string AbilityDescription;
        public Sprite BodyBack;
        public Sprite BodyFront;
        public Sprite Prop;
        public Sprite Face;
        public Color BodyBackColor;
        public Color BodyFrontColor;
        
    }
    public static Character Instance { get; private set; }

    public int active = -1;

    public CharacterData[] characterDatas;

    private void Awake() {
        Instance = this;
    }

    public void SetupCharacter(string abilty_name){
        SetupCharacter(characterDatas.Where(e => e.AbilityName == abilty_name).First());
    }
    public void SetupCharacter(CharacterData type){
        if(type == null){
            throw new ArgumentNullException("Type cannot be null");
        }
        if(active != -1){
            Debug.Log("Cannot play more than one character at a time");
            return;
        }
        
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
        if(active == -1){return false;}
        return characterDatas[active].AbilityName.Equals(ability_name);
    }
    public bool isCharacterUnlocked(string ability_name = null){
        if(ability_name == null){
            //active
            return false;
        }
        return false;
    }

    public void SetupActiveLooks(){
        SetupSkin(characterDatas[active]);
        SetupEnvironment(characterDatas[active]);
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
    public void TransformVesselToCharacter(GameObject Vessel, string ability_name = null){
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
        
        
    }

    
}
