using UnityEngine;

public class BlackHole : MonoBehaviour {

    /************************ FIELDS ************************/
    [SerializeField] private int frameLength;

    private SpriteRenderer spriteRenderer;
    private Timer timer;
    private int iterator=0;
    /************************ INITIALIZE ************************/
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        timer = new Timer();
        timer.OnTimerCountingEnd += Timer_OnTimerCountingEnd;
    }


    private void Start() {
        timer.StartTimer(frameLength);
        spriteRenderer.sprite = ResourceSystem.Instance.blackHoleSpritesList[iterator];
    }

    /************************ LOOPING ************************/
    private void Update() {
        timer.Tick();
    }

    /************************ METHODS ************************/

    private void Timer_OnTimerCountingEnd() {
        timer.StopTimer();
        iterator++;
        if (iterator > ResourceSystem.Instance.blackHoleSpritesList.Count - 1) iterator = 0;
        spriteRenderer.sprite = ResourceSystem.Instance.blackHoleSpritesList[iterator];
        timer.StartTimer(frameLength);
    }
}
