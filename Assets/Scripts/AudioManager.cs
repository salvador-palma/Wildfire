using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioClip[] AudioClips;
    [SerializeField] AudioSource[] audioSources;
    private int playing;
    void Start()
    {
        Instance = this;
        
    }


    public void PlayFX(int SourceIndex, int SoundIndex, float rangeMin = -1f, float rangeMax = 0f){
        if (playing > 10) return;
        if(rangeMin != -1f){
            audioSources[SourceIndex].pitch = Random.Range(rangeMin, rangeMax);
        }
        StartCoroutine(Playclip(AudioClips[SoundIndex],audioSources[SourceIndex]));
        
    }

    IEnumerator Playclip(AudioClip clip, AudioSource source)
	{
		playing++;
		source.PlayOneShot(clip);
		yield return new WaitForSeconds(clip.length);
		playing--;
	}
}
