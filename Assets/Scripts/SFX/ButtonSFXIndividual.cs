using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFXIndividual : MonoBehaviour
{
    public void PlaySound(int n){
        AudioManager.Instance.PlayButtonSound(n);
    }
}
