using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] private CinemachineVirtualCamera vCamMenu,vCamGame;
    
    /************************ INITIALIZE ************************/

    private void Start() {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }


    /************************ LOOPING ************************/
    private void Update() {
        
    }

    /************************ METHODS ************************/

    private void GameManager_OnGameStarted() {
        vCamGame.Priority += 10;
    }

    private void OnDestroy() {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }
}
