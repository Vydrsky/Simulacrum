using UnityEngine;
using System;

public class GameManager : Singleton<GameManager> {

    /************************ FIELDS ************************/
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    

    public GameState State { get; private set; }
    

    /************************ METHODS ************************/
    
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

    private void HandleStarting() {

    }

    private void HandleRunning() {

    }
    private void HandleEnding() {

    }

    public enum GameState {
        Starting,
        Running,
        Ending
    }
}


