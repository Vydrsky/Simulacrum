using UnityEngine;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

    /************************ FIELDS ************************/
    [SerializeField] private List<ParticleSystem> deathParticleSystems;

    /************************ INITIALIZE ************************/
    private void Awake() {
        DeathLaser.OnDeath += Death_OnDeath;
        DeathPlane.OnDeath += Death_OnDeath;
    }


    private void Start() {
    }

    /************************ LOOPING ************************/
    private void Update() {
        
    }

    /************************ METHODS ************************/

    private void Death_OnDeath() {
        foreach(var particleSystem in deathParticleSystems) {
            particleSystem.Play();
        }
    }

    private void OnDestroy() {
        DeathLaser.OnDeath -= Death_OnDeath;
        DeathPlane.OnDeath -= Death_OnDeath;
    }
}
