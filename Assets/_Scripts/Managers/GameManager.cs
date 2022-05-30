using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    /************************ FIELDS ************************/


    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    
    public GameState State { get; private set; }
    

    /************************ METHODS ************************/
    protected override void Awake() {
        base.Awake();
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
        StartCoroutine(TimeEffectOnDeathCoroutine());
    }

    private void HandleStarting() {

    }

    private void HandleRunning() {

    }
    private void HandleEnding() {
        UIController.Instance.DisableAllUi();
        UIController.Instance.EnableEndingUi();
    }

    private IEnumerator TimeEffectOnDeathCoroutine() {
        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        yield return new WaitForSeconds(0.2f);
        while (true) {
            if (Time.timeScale >= 1f) {
                Time.timeScale = 1f;
                break;
            }
            Time.timeScale += Time.deltaTime*5f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            yield return null;
        }
    }
    
    public void ReloadScene() {
        SceneManager.LoadScene("GameScene");
    }

    public enum GameState {
        Starting,
        Running,
        Ending
    }

    private void OnDestroy() {
        DeathPlane.OnDeath -= DeathPlane_OnDeath;
        StopAllCoroutines();
    }
}


