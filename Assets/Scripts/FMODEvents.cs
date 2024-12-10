using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set;}
    
    
    [field:Header("NPC Voice")]
    [field: SerializeField] public EventReference Rowl { get; private set; }
    [field: SerializeField] public EventReference Cloris { get; private set; }
    [field: SerializeField] public EventReference Gyomyo { get; private set; }
    [field: SerializeField] public EventReference Jhat { get; private set; }
    [field: SerializeField] public EventReference Naal { get; private set; }

    [field:Header("Ambience SFX")]
    [field: SerializeField] public EventReference Campsite { get; private set; }

    [field:Header("Menu SFX")]
    [field: SerializeField] public EventReference BubblePop { get; private set; }
    [field: SerializeField] public EventReference PaperSlide { get; private set; }
    [field: SerializeField] public EventReference UnlockedEffect { get; private set; }
    
    private void Awake() {
        if (Instance != null){
            Debug.LogError("More than one FMOD Events found!");
        }
        Instance = this;
    }

    public static EventReference GetVoice(string name){
        switch(name){

            case "Rowl":
                return Instance.Rowl;
            case "Cloris":
                return Instance.Cloris;
            case "Gyomyo":
                return Instance.Gyomyo;
            case "Jhat":
                return Instance.Jhat;
            case "Naal":
                return Instance.Naal;
            default:
                return new EventReference();
        }
    }
}
