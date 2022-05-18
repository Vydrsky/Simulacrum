using UnityEngine;
using System.Collections.Generic;
public class AudioSystem : PersistantSingleton<AudioSystem> {

    /************************ FIELDS ************************/

    [SerializeField] private Dictionary<string, AudioSource> audioSourceDictionary = new Dictionary<string, AudioSource>();
    

    /************************ METHODS ************************/
    
    public void PlayAudioClip(string clipKey, Vector2 position, float volume = 1f) {
        audioSourceDictionary[clipKey].transform.position = position;
        audioSourceDictionary[clipKey].volume = volume;
        audioSourceDictionary[clipKey].Play();
    }

    public void PlayAudioClip(string clipKey, float volume = 1f) {
        audioSourceDictionary[clipKey].volume = volume;
        audioSourceDictionary[clipKey].Play();
    }

    public void StopAudioClip(string clipKey) {
        audioSourceDictionary[clipKey].Stop();
    }

}
