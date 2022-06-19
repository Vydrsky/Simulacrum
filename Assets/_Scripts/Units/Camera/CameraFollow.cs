using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] private CinemachineVirtualCamera vCamMenu,vCamGame;
    
    /************************ INITIALIZE ************************/

    private void Start() {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        vCamMenu.m_Lens.OrthographicSize = 5 * Screen.width / Screen.height;
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
