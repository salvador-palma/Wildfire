using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

using System.Dynamic;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set;}

    private EventInstance ambienceEventInstance;
    private bool initialized = false;
    private void Awake() {
        if (Instance != null){

           
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
        }
        
    }
   
    private void InitializeAmbience(EventReference eventReference){
        ambienceEventInstance = CreateInstance(eventReference);
        ambienceEventInstance.start();
    }
    private void PlayOneShotSelf(EventReference sound, Vector3 worldPos){
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public static void PlayOneShot(EventReference sound, Vector2 worldPos){
        Instance.PlayOneShotSelf(sound, worldPos);
    }

    
    public static EventInstance CreateInstance(EventReference eventReference){
        EventInstance eventInstance= RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void SetAmbienceParameter(string param, float value){
        if(!initialized){
            initialized=true;
            InitializeAmbience(FMODEvents.Instance.Campsite);
        }
        ambienceEventInstance.setParameterByName(param, value);
    }

    public static void PlayMusicTrack(OST level){
        PlayMusicTrack((float)level);
    }
    public static void PlayMusicTrack(float n){
        Instance.SetAmbienceParameter("Level", n);
        Instance.SetAmbienceParameter("OST_Intensity", 1);
        
    }
    public static void StopMusicTrack(){
        Instance.SetAmbienceParameter("OST_Intensity", 0);
    }

    private EventInstance HealthSoundInstance;
    private bool HealthSoundInstanceAffixed;
    float addHealingFreq = 0.05f;
    float HealingDecay = 0.1f;
    float lastTick;
    public void PlayHealingSound()
    {
        if(!HealthSoundInstanceAffixed){
            HealthSoundInstanceAffixed = true;
            HealthSoundInstance = CreateInstance(FMODEvents.Instance.Healing);
        }

        float cur;
        float target;

        float delta = lastTick == 0 ? lastTick : Time.time - lastTick;
        lastTick = Time.time;
        

        
        HealthSoundInstance.getParameterByName("HealingFreq", out cur, out target);
        float newVal = Math.Clamp( cur + addHealingFreq - delta * HealingDecay, 0, 1);
        HealthSoundInstance.setParameterByName("HealingFreq", newVal);

        HealthSoundInstance.start();
        
    }
}
public enum OST{
    CAMPSITE = 0,
    NIGHT = 1,
    BOSS = 2,
    CASINO = 3
}
