using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerAnimation : MonoBehaviour {

    /************************ FIELDS ************************/

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private SpringJoint2D hook;
    private bool isHooked = false;

    [SerializeField] private List<Sprite> spriteList;
    
    /************************ INITIALIZE ************************/
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hook = GetComponent<SpringJoint2D>();
    }

    private void Start() {
        PlayerController.OnHooked += PlayerController_OnHooked;
        PlayerController.OnDeHooked += PlayerController_OnDeHooked;
    }

    private void PlayerController_OnDeHooked() {
        isHooked = false;
    }

    private void PlayerController_OnHooked() {
        spriteRenderer.sprite = spriteList[4]; // hooked sprite
        isHooked = true;
    }

    /************************ LOOPING ************************/
    private void Update() {
        if (!isHooked) {
            if (rb.velocity.y > 0) {
                spriteRenderer.sprite = spriteList[2]; // going up sprite
                transform.rotation = Quaternion.Euler(0f, 0f, -10f);
            }
            else if (rb.velocity.y < 0) {
                spriteRenderer.sprite = spriteList[3]; // going down sprite
                transform.rotation = Quaternion.Euler(0f, 0f, 10f);
            }
        }
        else {
            Vector3 targetVector = (Vector3)hook.connectedAnchor - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, targetVector);
        }
    }

    /************************ METHODS ************************/
    
    
}
