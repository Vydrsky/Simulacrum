using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameUI : Singleton<GameUI> {

    /************************ FIELDS ************************/

    private TextMeshProUGUI scoreText;
    private Camera mainCam;
    private Image jumpButtonImage;
    [SerializeField] private float jumpCooldown = 5f;
    [SerializeField] private List<GameObject> endingUiList;

    public static event Action OnJump;
    public static event Action OnRetryPressed;
    public Timer cooldownTimer;

    /************************ INITIALIZE ************************/
    protected override void Awake() {
        base.Awake();
        scoreText = transform.Find("scoreText").GetComponent<TextMeshProUGUI>();
        cooldownTimer = new Timer();
        jumpButtonImage = transform.Find("jumpButton").GetComponent<Image>();
        jumpButtonImage.fillAmount = 1f;
    }

    private void Start() {
        mainCam = Camera.main;
        cooldownTimer.OnTimerCountingEnd += CooldownTimer_OnTimerCountingEnd;
    }

    /************************ LOOPING ************************/
    private void Update() {
        scoreText.text = Mathf.Clamp(mainCam.transform.position.x,0f,Mathf.Infinity).ToString("0");
        cooldownTimer.Tick();
        if (cooldownTimer.isRunning) {
            jumpButtonImage.fillAmount += Time.deltaTime/jumpCooldown;
        }
    }

    /************************ METHODS ************************/
    
    public void HandleJumpButton() {

        if (!cooldownTimer.isRunning && PlayerController.Instance.State == PlayerController.PlayerState.Freefalling) {
            jumpButtonImage.fillAmount = 0;
            OnJump?.Invoke();
            cooldownTimer.StartTimer(jumpCooldown);
        }
        
    }

    public void HandleTryAgainButton() {
        OnRetryPressed?.Invoke();
        GameManager.Instance.ReloadScene();
    }

    public void DisableAllUi() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }
    public void EnableEndingUi() {
        foreach(GameObject obj in endingUiList) {
            obj.SetActive(true);
        }
    }

    public static void EndGame() {
        Application.Quit();
        Debug.Log("Quitting the game...");
    }

    private void CooldownTimer_OnTimerCountingEnd() {
        cooldownTimer.StopTimer();
        jumpButtonImage.fillAmount = 1f;
    }

    private void OnDestroy() {
        cooldownTimer.OnTimerCountingEnd -= CooldownTimer_OnTimerCountingEnd;
    }
}
