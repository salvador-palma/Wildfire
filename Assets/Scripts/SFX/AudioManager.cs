using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

using System.Dynamic;

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

    public EventInstance CreateInstance(EventReference eventReference){
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
}
public enum OST{
    CAMPSITE = 0,
    NIGHT = 1,
    BOSS = 2,
    CASINO = 3
}
