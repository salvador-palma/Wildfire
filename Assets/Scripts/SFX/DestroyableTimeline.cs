using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class DestroyableTimeline : MonoBehaviour
{
    [field: SerializeField] public EventReference eventReference { get; private set; }
    private EventInstance eventInstance;
    private void Start() {
        eventInstance = AudioManager.CreateInstance(eventReference);
        eventInstance.start();
    }
    private void OnDestroy() {
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
}
