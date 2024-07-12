using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Serializable]
    public class CharacterData{
        public string Name;
        public string AnimationBool;
        public string AbilityName;
        public string AbilityDescription;
        
    }
    public static Character Instance { get; private set; }

    public CharacterData active;

    public CharacterData[] characterDatas;

    private void Awake() {
        Instance = this;
    }

    public void SetupCharacter(string name){
        SetupCharacter(characterDatas.Where(e => e.Name == name).FirstOrDefault(null));
    }
    public void SetupCharacter(CharacterData type){
        if(type == null){
            throw new ArgumentNullException("Type cannot be null");
        }
        active = type;
        SetupSkin(type);
        SetupEnvironment(type);
    }
    public bool isCharacter(string name){
        if(active == null){return false;}
        return active.Name.Equals(name);
    }

    private void SetupSkin(CharacterData type)
    {
        Flamey.Instance.GetComponent<Animator>().SetBool(type.AnimationBool, true);
    }

    private void SetupEnvironment(CharacterData type)
    {
        throw new NotImplementedException();
    }

    
}
