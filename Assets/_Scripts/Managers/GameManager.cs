using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    /************************ FIELDS ************************/


    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    
    public GameState State { get; private set; }
    

    /************************ METHODS ************************/
    private GameManager() {
        State = GameState.Starting;
        DeathPlane.OnDeath += DeathPlane_OnDeath;
    }


    public void ChangeState(GameState newState) {
        if (State == newState) return;

        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (State) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Running:
                HandleRunning();
                break;
            case GameState.Ending:
                HandleEnding();
                break;
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private void DeathPlane_OnDeath() {
        PlayerController.Instance.State = PlayerController.PlayerState.Death;
        ChangeState(GameState.Ending);
    }

    private void HandleStarting() {

    }

    private void HandleRunning() {

    }
    private void HandleEnding() {
        UIController.Instance.DisableAllUi();
        UIController.Instance.EnableEndingUi();
    }
    
    public void ReloadScene() {
        //needs change later
        SceneManager.LoadScene("GameScene");
    }

    public enum GameState {
        Starting,
        Running,
        Ending
    }

    private void OnDestroy() {
        DeathPlane.OnDeath -= DeathPlane_OnDeath;
    }
}


