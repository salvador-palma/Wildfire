using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType{
        MASTER,
        MUSIC,
        SFX,
        MENU
    }
    [SerializeField] VolumeType volumeType;
    Slider slider;
    private void Start() {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener((a)=>OnSliderValueChanged(a));
        float res;
        switch (volumeType){
            case VolumeType.MASTER:  
                AudioManager.Instance.masterBus.getVolume(out res);
                slider.value = res;
            break;
            case VolumeType.MUSIC:
                AudioManager.Instance.musicBus.getVolume(out res);
                slider.value = res;
            break;
            case VolumeType.SFX:
                AudioManager.Instance.sfxBus.getVolume(out res);
                slider.value = res;
            break;
            case VolumeType.MENU:
                AudioManager.Instance.menuBus.getVolume(out res);
                slider.value = res;
            break;
            
        }

    }
    private void OnSliderValueChanged(float value){
        switch (volumeType){
            case VolumeType.MASTER:
                AudioManager.Instance.masterBus.setVolume(value);
            break;
            case VolumeType.MUSIC:
                AudioManager.Instance.musicBus.setVolume(value);
            break;
            case VolumeType.SFX:
                AudioManager.Instance.sfxBus.setVolume(value);
            break;
            case VolumeType.MENU:
                AudioManager.Instance.menuBus.setVolume(value);
            break;
            
        }
    }
}
