using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameUI : Singleton<GameUI> {

    /************************ FIELDS ************************/

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI deathText;
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
        deathText = transform.Find("deathText").GetComponent<TextMeshProUGUI>();
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
        scoreText.text = Mathf.Clamp(mainCam.transform.position.x - 13,0f,Mathf.Infinity).ToString("0");
        cooldownTimer.Tick();
        if (cooldownTimer.isRunning) {
            jumpButtonImage.fillAmount += Time.deltaTime/jumpCooldown;
        }
    }

    /************************ METHODS ************************/
    
    public void HandleJumpButton() {

        if (!cooldownTimer.isRunning && PlayerController.Instance.State != PlayerController.PlayerState.Death) {
            jumpButtonImage.fillAmount = 0;
            OnJump?.Invoke();
            cooldownTimer.StartTimer(jumpCooldown);
        }
        
    }

    public void HandleTryAgainButton() {
        OnRetryPressed?.Invoke();
    }

    public void DisableAllUi() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }
    public void EnableEndingUi() {
        RandomizeDeathText();
        StartCoroutine(WaitWithEndingMenu());
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

    private IEnumerator WaitWithEndingMenu() {
        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in endingUiList) {
            obj.SetActive(true);
        }
    }


    private void RandomizeDeathText() {
        UnityEngine.Random.InitState(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        String[] texts = { "Never Lucky", "Skill Issue", ":C", "Just Dodge", "Did you try not getting hit?", "Nice try", "Dont give up!" };
        int rand = UnityEngine.Random.Range(0, texts.Length-1);
        deathText.SetText(texts[rand]);
    }

}
