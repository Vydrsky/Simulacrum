using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] private GameObject menuUIObject;
    [SerializeField] private AudioMixer masterMixer;
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;
    /************************ INITIALIZE ************************/
    private void Awake() {
        masterSlider = transform.Find("MasterSlider").GetComponent<Slider>();
        musicSlider = transform.Find("MusicSlider").GetComponent<Slider>();
        sfxSlider = transform.Find("SFXSlider").GetComponent<Slider>();
    }

    private void Start() {
       if (!PlayerPrefs.HasKey("masterVolume")) {
            masterSlider.value = 1f;
            musicSlider.value = 1f;
            sfxSlider.value = 1f;
       }
       else {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
       }
    }

    /************************ LOOPING ************************/
    private void Update() {
        masterMixer.SetFloat("masterVolume", Mathf.Log(masterSlider.value) * 20);
        masterMixer.SetFloat("musicVolume", Mathf.Log(musicSlider.value) * 20);
        masterMixer.SetFloat("SFXVolume", Mathf.Log(sfxSlider.value) * 20);
    }

    /************************ METHODS ************************/

    public void EnableMenu() {
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        gameObject.SetActive(false);
        menuUIObject.SetActive(true);
    }
}
