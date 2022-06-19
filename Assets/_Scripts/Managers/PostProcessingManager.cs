using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PostProcessingManager : Singleton<PostProcessingManager> {

    /************************ FIELDS ************************/


    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    [SerializeField] private VolumeProfile globalVolumeProfile;
    
    /************************ INITIALIZE ************************/

    private void Start() {
        BlackHole.OnDeath += BlackHole_OnDeath;
        globalVolumeProfile.TryGet(out chromaticAberration);
        globalVolumeProfile.TryGet(out lensDistortion);
        chromaticAberration.intensity.value = 0f;
        lensDistortion.intensity.value = 0.2f;
    }

    /************************ LOOPING ************************/
    private void Update() {
        
    }

    /************************ METHODS ************************/
    private void BlackHole_OnDeath() {    
        StartCoroutine(Abberate(1f));
    }


    private void OnDestroy() {
        BlackHole.OnDeath -= BlackHole_OnDeath;
    }

    private IEnumerator Abberate(float multiplier) {

        do {
            if(lensDistortion.intensity.value < 0.35f) {
                lensDistortion.intensity.value += Time.deltaTime * multiplier/2;
            }
            chromaticAberration.intensity.value += Time.deltaTime * multiplier;
            yield return null;
        } while (chromaticAberration.intensity.value < 1f);

    }
        
}
