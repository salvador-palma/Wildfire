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
    [field: SerializeField] public EventReference Betsy { get; private set; }

    [field:Header("Ambience SFX")]
    [field: SerializeField] public EventReference Campsite { get; private set; }
    

    [field:Header("Menu SFX")]
    [field: SerializeField] public EventReference BubblePop { get; private set; }
    [field: SerializeField] public EventReference PaperSlide { get; private set; }
    [field: SerializeField] public EventReference UnlockedEffect { get; private set; }
    [field: SerializeField] public EventReference MoneyDrop { get; private set; }
    [field: SerializeField] public EventReference ButtonClick { get; private set; }
    [field: SerializeField] public EventReference MoneyTick { get; private set; }

    [field:Header("Game SFX")]
    [field: SerializeField] public EventReference DefaultAugment { get; private set; }
    [field: SerializeField] public EventReference PrismaticAugment { get; private set; }
    [field: SerializeField] public EventReference Fireball { get; private set; }
    [field: SerializeField] public EventReference OrbitalHit { get; private set; }
    [field: SerializeField] public EventReference StatikHit { get; private set; }
    [field: SerializeField] public EventReference ResonanceEffect { get; private set; }
    [field: SerializeField] public EventReference RoundShot { get; private set; }
    [field: SerializeField] public EventReference Immolate { get; private set; }
    [field: SerializeField] public EventReference ThornsSlash { get; private set; }
    [field: SerializeField] public EventReference Explosion { get; private set; }
    [field: SerializeField] public EventReference Lava { get; private set; }
    [field: SerializeField] public EventReference Sizzle { get; private set; }
    
    [field: SerializeField] public EventReference Thunder { get; private set; }
    [field: SerializeField] public EventReference EagleCall { get; private set; }
    [field: SerializeField] public EventReference EagleHit { get; private set; }
    [field: SerializeField] public EventReference BeeFlight { get; private set; }
    [field: SerializeField] public EventReference MegaGhoulBash { get; private set; }
    [field: SerializeField] public EventReference GhoulBash { get; private set; }
    [field: SerializeField] public EventReference IceProc { get; private set; }
    [field: SerializeField] public EventReference EvilLaugh { get; private set; }
    [field: SerializeField] public EventReference Healing { get; private set; }
    [field: SerializeField] public EventReference PoisonPop { get; private set; }
    [field: SerializeField] public EventReference PoisonEffect { get; private set; }

    [field:Header("Specific SFX")]
    [field: SerializeField] public EventReference MotorStop { get; private set; }
    [field: SerializeField] public EventReference MotorStart { get; private set; }
    [field: SerializeField] public EventReference Curtains { get; private set; }

    [field:Header("Casino SFX")]
    [field: SerializeField] public EventReference AcornDrop { get; private set; }
    [field: SerializeField] public EventReference AcornSlot { get; private set; }
    [field: SerializeField] public EventReference FlowerProgress { get; private set; }
    [field: SerializeField] public EventReference FlowerWrong { get; private set; }

    private void Awake() {
        if (Instance != null){
            
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        
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
            case "Betsy":
                return Instance.Betsy;
            default:
                return new EventReference();
        }
    }
}
