using UnityEngine;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

    /************************ FIELDS ************************/
    [SerializeField] private List<ParticleSystem> deathParticleSystems;
    [SerializeField] private List<ParticleSystem> jumpParticleSystems;

    /************************ INITIALIZE ************************/
    private void Awake() {
    }


    private void Start() {
        DeathLaser.OnDeath += Death_OnDeath;
        DeathPlane.OnDeath += Death_OnDeath;
        GameUI.OnJump += GameUI_OnJump;
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

    private void GameUI_OnJump() {
        foreach (var particleSystem in jumpParticleSystems) {
            particleSystem.Play();
        }
    }


    private void OnDestroy() {
        DeathLaser.OnDeath -= Death_OnDeath;
        DeathPlane.OnDeath -= Death_OnDeath;
        GameUI.OnJump -= GameUI_OnJump;
    }
}
