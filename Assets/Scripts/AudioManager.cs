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
    private void Awake() {
        if (Instance != null){

            Debug.LogError("More than one Audio Manager found! Destroying...");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    private void Start() {
        InitializeAmbience(FMODEvents.Instance.Campsite);
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
        ambienceEventInstance.setParameterByName(param, value);
    }
}
