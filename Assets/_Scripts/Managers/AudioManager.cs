using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {

    /************************ FIELDS ************************/

    private Dictionary<string, AudioSource> audioSourceDictionary = new Dictionary<string, AudioSource>();

    bool goingUp = true;

    /************************ METHODS ************************/

    protected override void Awake() {
        base.Awake();
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        GameManager.OnMenuLoaded += GameManager_OnMenuLoaded;
        DeathPlane.OnDeath += OnDeath;
        DeathLaser.OnDeath += OnDeath;
    }


    private void Start() {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources) {
            audioSourceDictionary.Add(source.clip.name, source);
        }

    }

    private void GameManager_OnGameStarted() {
        StopAllCoroutines();
        PlayAudioClip("menuFadeAudioClip",0.7f);
        StartCoroutine(LerpDownClip_Coroutine(audioSourceDictionary["menuAudioClip"], 1f));
        StartCoroutine(LerpUpClip_Coroutine(audioSourceDictionary["gameplayAudioClip"], 1f));
        
    }

    private void GameManager_OnMenuLoaded() {
        StopAllCoroutines();
        PlayAudioClip("menuAudioClip", 1f);
    }

    private void OnDeath() {
        Debug.Log("DEATH SOUND");
        StopAllCoroutines();
        StartCoroutine(LerpPitchDownClip_Coroutine(audioSourceDictionary["gameplayAudioClip"], 1f));
    }

    private void OnDestroy() {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
        GameManager.OnMenuLoaded -= GameManager_OnMenuLoaded;
        DeathPlane.OnDeath -= OnDeath;
        DeathLaser.OnDeath -= OnDeath;
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

}
