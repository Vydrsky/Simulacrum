using System;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    /************************ FIELDS ************************/
    [SerializeField] private float frameLength;
    [SerializeField] private float gravityRange;
    [SerializeField] private float gravityForce;

    private SpriteRenderer spriteRenderer;
    private Timer timer;
    private int iterator=0;

    public static event Action OnDeath;
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

    private void FixedUpdate() {
        GravitatePlayer();
    }

    /************************ METHODS ************************/

    private void Timer_OnTimerCountingEnd() {
        timer.StopTimer();
        iterator++;
        if (iterator > ResourceSystem.Instance.blackHoleSpritesList.Count - 1) iterator = 0;
        spriteRenderer.sprite = ResourceSystem.Instance.blackHoleSpritesList[iterator];
        timer.StartTimer(frameLength);
    }

    private void GravitatePlayer() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,gravityRange);
        foreach(Collider2D collider in colliders) {
            if (collider.gameObject.tag.Contains("Player")) {
                Vector2 direction = (transform.position - collider.transform.position).normalized;
                collider.GetComponent<Rigidbody2D>().AddForce(direction * gravityForce * Time.fixedDeltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Contains("Player")) {
            OnDeath?.Invoke();
        }
    }
}
