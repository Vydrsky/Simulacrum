using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {

    /************************ FIELDS ************************/

    private Dictionary<string, AudioSource> audioSourceDictionary = new Dictionary<string, AudioSource>();


    /************************ METHODS ************************/

    protected override void Awake() {
        base.Awake();
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        GameManager.OnMenuLoaded += GameManager_OnMenuLoaded;
        DeathPlane.OnDeath += OnDeath;
        DeathLaser.OnDeath += OnDeath;
        BlackHole.OnDeath += BlackHole_OnDeath;
    }


    private void Start() {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources) {
            audioSourceDictionary.Add(source.clip.name, source);
        }

    }

    private void GameManager_OnGameStarted() {
        StartCoroutine(LerpDownClip_Coroutine(audioSourceDictionary["menuAudioClip"], 1f));
        StartCoroutine(LerpDownClip_Coroutine(audioSourceDictionary["menuIntroAudioClip"], 1f));
        StartCoroutine(LerpUpClip_Coroutine(audioSourceDictionary["gameplayAudioClip"], 0.2f));
        
    }

    private void GameManager_OnMenuLoaded() {
        PlayAudioClip("menuIntroAudioClip", 1f);
        StartCoroutine(WaitForClipToEndAndStartAnother_Coroutine(audioSourceDictionary["menuIntroAudioClip"], audioSourceDictionary["menuAudioClip"]));
    }

    private void OnDeath() {
        
        StartCoroutine(LerpPitchDownClip_Coroutine(audioSourceDictionary["gameplayAudioClip"], 1f));
    }

    private void BlackHole_OnDeath() {
        
        StartCoroutine(LerpPitchUpVolumeDownClip_Coroutine(audioSourceDictionary["gameplayAudioClip"], 1f));
    }

    private void OnDestroy() {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
        GameManager.OnMenuLoaded -= GameManager_OnMenuLoaded;
        DeathPlane.OnDeath -= OnDeath;
        DeathLaser.OnDeath -= OnDeath;
        BlackHole.OnDeath -= BlackHole_OnDeath;
    }

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

    private void StopAllAudio() {
        foreach(var item in audioSourceDictionary) {
            item.Value.Stop();
        }
    }

    private IEnumerator LerpUpClip_Coroutine(AudioSource source, float speedMultiplier) {
        source.volume = 0f;
        source.Play();
        while (source.volume <= 1f) {
            source.volume += Time.deltaTime * speedMultiplier;
            yield return null;
        }
        source.volume = 1f;
    }

    private IEnumerator LerpDownClip_Coroutine(AudioSource source, float speedMultiplier) {
        while (source.volume >=0f) {
            source.volume -= Time.deltaTime * speedMultiplier;
            yield return null;
        }
        source.Stop();
        source.volume = 0f;
    }
    private IEnumerator LerpPitchDownClip_Coroutine(AudioSource source, float speedMultiplier) {
        while (source.pitch >= 0f) {
            source.pitch -= Time.deltaTime * speedMultiplier;
            yield return null;
        }
        source.Stop();
        source.pitch = 0f;
    }

    private IEnumerator LerpPitchUpVolumeDownClip_Coroutine(AudioSource source, float speedMultiplier) {
        while (source.volume >=0f) {
            if (source.pitch <= 5f) {
                source.pitch += Time.deltaTime * speedMultiplier*2f;
            }
            source.volume -= Time.deltaTime * speedMultiplier;
            yield return null;
        }
        source.Stop();
        source.pitch = 0f;
    }

    private IEnumerator WaitForClipToEndAndStartAnother_Coroutine(AudioSource source1, AudioSource source2) {
        yield return new WaitForSeconds(source1.clip.length);
        source1.Stop();
        source2.Play();
    }
}
