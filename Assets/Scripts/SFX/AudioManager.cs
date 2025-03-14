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

    [Header("Volume Mixer")]
    [Range(0,1)]
    public float MasterVolume = 1;
    [Range(0,1)]
    public float MusicVolume = 1;
    [Range(0,1)]
    public float SFXVolume = 1;
    [Range(0,1)]
    public float MenuVolume = 1;

    public Bus masterBus;
    public Bus musicBus;
    public Bus sfxBus;
    public Bus menuBus;
    

    private EventInstance ambienceEventInstance;
    private bool initialized = false;
    private void Awake() {
        if (Instance != null){

           
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");
            menuBus = RuntimeManager.GetBus("bus:/Menu");

        }
    }
    // private void Update() {
    //     masterBus.setVolume(MasterVolume);
    //     musicBus.setVolume(MusicVolume);
    //     sfxBus.setVolume(SFXVolume);
    //     menuBus.setVolume(MenuVolume);
    // }
   
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
        Debug.Log("1");
        
    }
   
    
    public static void StopMusicTrack(float n){
        if(n!=-1){
                Instance.SetAmbienceParameter("Level", n);
        }
        
        Instance.SetAmbienceParameter("OST_Intensity", 0);
        Debug.Log("0");
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

    
    
    float addResonanceFreq = 0.25f;
    float ResonanceDecay = 0.1f;
    float lastResonanceTick;

    public void PlayResonanceSound()
    {
        
        float cur;
        float target;

        float delta = lastResonanceTick == 0 ? lastResonanceTick : Time.time - lastResonanceTick;
        lastResonanceTick = Time.time;
        
        ambienceEventInstance.getParameterByName("SoundBoost", out cur, out target);
        float newVal = Math.Clamp( cur + addResonanceFreq - delta * ResonanceDecay, 0, 1);
        ambienceEventInstance.setParameterByName("SoundBoost", newVal);

       
        
    }
}
public enum OST{
    CAMPSITE = 0,
    NIGHT = 1,
    BOSS = 2,
    CASINO = 3
}
