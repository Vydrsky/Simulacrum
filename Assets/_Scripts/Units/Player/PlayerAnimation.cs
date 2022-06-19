using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerAnimation : Singleton<PlayerAnimation> {

    /************************ FIELDS ************************/

    private Rigidbody2D rb;
    private SpringJoint2D hook;
    private SpriteRenderer spriteRenderer;
    private Timer timer;
    private int spriteIterator = 0;
    private float dissolveAmount = 1;

    [SerializeField] private float yThresholdSpeed;
    [SerializeField] private float frameLength;
    [SerializeField] private GameObject fragmentPrefab;
    [SerializeField] private Material playerMaterial;

    /************************ INITIALIZE ************************/
    protected override void Awake() {
        rb = GetComponent<Rigidbody2D>();
        hook = GetComponent<SpringJoint2D>();
        spriteRenderer = transform.Find("Graphic").GetComponent<SpriteRenderer>();
        timer = new Timer();
        playerMaterial.SetFloat("_DissolveAmount", 1f);
    }

    private void Start() {
        DeathPlane.OnDeath += Death_OnDeath;
        DeathLaser.OnDeath += Death_OnDeath;
        BlackHole.OnDeath += BlackHoleDeath_OnDeath;
        timer.OnTimerCountingEnd += Timer_OnTimerCountingEnd;
    }


    /************************ LOOPING ************************/
    private void Update() {
        timer.Tick();

        switch (PlayerController.Instance.State) {
            case PlayerController.PlayerState.Freefalling:
                transform.rotation = Quaternion.Euler(0f, 0f, -5f);
                if (rb.velocity.y > 0) {
                    if (rb.velocity.y > yThresholdSpeed)
                        spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[3];  // going up fast sprite
                    else
                        spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[2];  // going up sprite
                }
                else if (rb.velocity.y < 0) {
                    if (rb.velocity.y < -yThresholdSpeed)
                        spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[5];  // going down fast sprite
                    else
                        spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[4];  // going down fast sprite
                }
                break;
            case PlayerController.PlayerState.Hooked:
                spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[6]; // hooked sprite
                Vector3 targetVector = (Vector3)hook.connectedAnchor - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, targetVector);
                break;
            case PlayerController.PlayerState.Standing:
                HandleStandingAnimations();
                break;
        }
    }

    /************************ METHODS ************************/

    private void Death_OnDeath() {
        spriteRenderer.enabled = false;
        foreach (var sprite in ResourceSystem.Instance.heroFragmentedSpritesList) {
            GameObject temp= Instantiate(fragmentPrefab,transform.position,Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = sprite;
            temp.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-180f, 180f));
            Rigidbody2D temprb = temp.GetComponent<Rigidbody2D>();
            Vector2 forceVector = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-180f, 180f)) * Vector2.up * 15f;
            temprb.AddForce(forceVector,ForceMode2D.Impulse);
            temprb.AddTorque(4f,ForceMode2D.Impulse);
        }
    }
    
    private void BlackHoleDeath_OnDeath() {
        StartCoroutine(DissolvePlayer());
    }

    private void Timer_OnTimerCountingEnd() {
        timer.StopTimer();
        spriteIterator++;
        //currently 2 sprites
        if(spriteIterator > 1) spriteIterator = 0;
        timer.StartTimer(frameLength);
    }

    private void HandleStandingAnimations() {
        spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[spriteIterator];
        timer.StartTimer(frameLength);
    }

    private void OnDestroy() {
        DeathPlane.OnDeath -= Death_OnDeath;
        DeathLaser.OnDeath -= Death_OnDeath;
        BlackHole.OnDeath -= BlackHoleDeath_OnDeath;
    }

    private IEnumerator DissolvePlayer() {
        do {
            dissolveAmount = dissolveAmount -= Time.deltaTime;
            playerMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        } while (dissolveAmount >= 0f);
    }
}
