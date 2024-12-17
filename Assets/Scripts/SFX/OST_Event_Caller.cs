using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OST_Event_Caller : MonoBehaviour
{
    public void StopMusicTrack(){
        AudioManager.StopMusicTrack();
    }
    public void StartMusicTrack(float n){
        AudioManager.PlayMusicTrack(n);
    }
    public void PlaySoundEffects(int ID){
        switch(ID){
            case 0: AudioManager.PlayOneShot(FMODEvents.Instance.BubblePop , transform.position); break;
            case 1: AudioManager.PlayOneShot(FMODEvents.Instance.MotorStop , transform.position); break;
            case 2: AudioManager.PlayOneShot(FMODEvents.Instance.MotorStart , transform.position); break;
            case 3: AudioManager.PlayOneShot(FMODEvents.Instance.Curtains , transform.position); break;
        }
    }

}
