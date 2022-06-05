using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerAnimation : Singleton<PlayerAnimation> {

    /************************ FIELDS ************************/

    private Rigidbody2D rb;
    private SpringJoint2D hook;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float yThresholdSpeed;
    [SerializeField] private GameObject fragmentPrefab;

    /************************ INITIALIZE ************************/
    protected override void Awake() {
        rb = GetComponent<Rigidbody2D>();
        hook = GetComponent<SpringJoint2D>();
        spriteRenderer = transform.Find("Graphic").GetComponent<SpriteRenderer>();
    }

    private void Start() {
        DeathPlane.OnDeath += Death_OnDeath;
        DeathLaser.OnDeath += Death_OnDeath;
    }


    /************************ LOOPING ************************/
    private void Update() {
        if (PlayerController.Instance.State == PlayerController.PlayerState.Freefalling) {
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
        }
        else if(PlayerController.Instance.State == PlayerController.PlayerState.Hooked) {
            spriteRenderer.sprite = ResourceSystem.Instance.heroSpritesList[6]; // hooked sprite
            Vector3 targetVector = (Vector3)hook.connectedAnchor - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, targetVector);
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

    private void OnDestroy() {
        DeathPlane.OnDeath -= Death_OnDeath;
        DeathLaser.OnDeath -= Death_OnDeath;
    }

}
