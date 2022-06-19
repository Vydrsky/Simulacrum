using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager> {

    /************************ FIELDS ************************/

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    public static int HighScore { get; private set; }

    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public static event Action OnMenuLoaded;
    public static event Action OnGameStarted;
    
    public GameState State { get; private set; }
    

    /************************ METHODS ************************/
    protected override void Awake() {
        base.Awake();
        State = GameState.Starting;
        DeathPlane.OnDeath += Death_OnDeath;
        DeathLaser.OnDeath += Death_OnDeath;
        BlackHole.OnDeath += Death_OnDeath;
    }


    private void Start() {
        OnMenuLoaded?.Invoke();
        HighScore = 0;
        HighScore = PlayerPrefs.GetInt("highscore");
        highscoreText.SetText("Highscore: " + HighScore);
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

    private void Death_OnDeath() {
        PlayerController.Instance.State = PlayerController.PlayerState.Death;
        if(HighScore < int.Parse(scoreText.text))
            PlayerPrefs.SetInt("highscore",int.Parse(scoreText.text));
        ChangeState(GameState.Ending);
        StartCoroutine(TimeEffectOnDeathCoroutine());
    }

    private void HandleStarting() {
        PlayerController.Instance.State = PlayerController.PlayerState.Standing;
    }

    private void HandleRunning() {
        PlayerController.Instance.State = PlayerController.PlayerState.Freefalling;
        OnGameStarted?.Invoke();
    }
    private void HandleEnding() {
        GameUI.Instance.DisableAllUi();
        GameUI.Instance.EnableEndingUi();
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
        DeathPlane.OnDeath -= Death_OnDeath;
        DeathLaser.OnDeath -= Death_OnDeath;
        BlackHole.OnDeath -= Death_OnDeath;
        Time.timeScale = 1f;
    }
}


