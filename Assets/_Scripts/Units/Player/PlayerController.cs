using UnityEngine;
using System;
using System.Collections.Generic;
public class PlayerController : Singleton<PlayerController> {

    /************************ FIELDS ************************/

    private Camera mainCam;
    private Collider2D[] colliders;
    private Collider2D closestCollider;
    private Vector3 touchInWorldSpace;
    private SpringJoint2D hook;
    private Rigidbody2D rb;
    private Transform laserStartTransform;
    private PlayerInput input;
    

    [SerializeField] private Vector2 jumpButtonForce;
    [Range(0f, 20f)]
    [SerializeField] private float ForceOnHook = 5f;
    [SerializeField] private LineRenderer hookLine;

    private PlayerState state;
    public PlayerState State {

        get { return state; }

        set {
            if (value == state) return;
            state = value;
            ControlStateEntry();
        }
    }

    /************************ INITIALIZE ************************/
    protected override void Awake() {
        
        base.Awake();
        input = new PlayerInput();
        mainCam = Camera.main;
        hook = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        laserStartTransform = transform.Find("LaserStartPoint");
    }

    private void Start() {
        State = PlayerState.Standing;
        rb.isKinematic = true;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        GameUI.OnJump += GameUI_OnJump;
    }



    /************************ LOOPING ************************/
    private void Update() {
        if (State == PlayerState.Standing || State == PlayerState.Death) return;
        State = ControlStates();
        switch (State) {
            case PlayerState.Freefalling:

                break;
            case PlayerState.Hooked:

                //update the line renderer
                hookLine.SetPosition(0, laserStartTransform.position);
                hookLine.SetPosition(1, closestCollider.transform.position);
                break;
        }
    }


    /************************ METHODS ************************/

    private void ControlStateEntry() {
        switch (state) {
            case PlayerState.Freefalling:
                rb.isKinematic = false;
                hook.enabled = false;
                hookLine.enabled = false;
                hook.frequency = 0.7f;
                hook.dampingRatio = 0.3f;
                if (rb.velocity.sqrMagnitude < ForceOnHook * ForceOnHook)
                    rb.AddForce(new Vector2(0f, ForceOnHook), ForceMode2D.Impulse);
                break;
            case PlayerState.Hooked:
                hook.connectedBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();
                hook.connectedAnchor = closestCollider.transform.position;
                hookLine.enabled = true;
                hook.enabled = true;
                switch (closestCollider.gameObject.tag) {
                    case "booster":
                        rb.velocity = Vector2.zero;
                        hook.distance = 1.5f;
                        hook.frequency = 0.65f;
                        hook.dampingRatio = 1f;
                        break;
                    case "grapplingPoint":
                        hook.distance = Vector3.Distance(transform.position, closestCollider.gameObject.transform.position);
                        if(rb.velocity.sqrMagnitude <ForceOnHook*ForceOnHook)
                            rb.AddForce(new Vector2(ForceOnHook, 0f), ForceMode2D.Impulse);
                        break;
                }
                break;
            case PlayerState.Death:
                HandleDeath();
                break;

        }
    }
    private PlayerState ControlStates() {
        PlayerState tempState = PlayerState.Freefalling;
        Touch touch;

        if(State == PlayerState.Freefalling) {
            closestCollider = null;
            //this check doesnt not allow for changing the collider once its attached and the screen is held
        }
        if(input.isScreenBeingTouched() &&!input.isUiBeingTouched()){
            if (closestCollider == null) {  //closest collider and input is only evaluated if the player is falling
                touch = input.GetTheFirstTouch();
                if (touch.phase != TouchPhase.Began) return PlayerState.Freefalling;
                touchInWorldSpace = mainCam.ScreenToWorldPoint(touch.position);
                touchInWorldSpace.z = 0f;
                colliders = Physics2D.OverlapCircleAll(touchInWorldSpace, 4f);

                //get the collider to attach to
                foreach (Collider2D collider in colliders) {
                    if (collider == colliders[0]) closestCollider = collider;
                    if (Vector3.Distance(collider.transform.position, touchInWorldSpace)
                        < Vector3.Distance(closestCollider.transform.position, touchInWorldSpace))
                        closestCollider = collider;
                }
            }
            //check if the collider is actually a grappling point
            if (closestCollider != null &&
                (closestCollider.gameObject.tag.Contains("grapplingPoint") || closestCollider.gameObject.tag.Contains("booster"))) {
                tempState = PlayerState.Hooked;
            }
            else
                tempState = PlayerState.Freefalling;
        }
        else if(!input.isScreenBeingTouched()) {
            tempState = PlayerState.Freefalling; 
        }
        return tempState;
    }

    private void HandleDeath() {
        hook.enabled = false;
        hookLine.enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

    }

    private void GameManager_OnGameStarted() {
        rb.AddForce(new Vector3(18f, 20f, 0f),ForceMode2D.Impulse);
    }

    private void GameUI_OnJump() {
        State = PlayerState.Freefalling;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(jumpButtonForce.x, jumpButtonForce.y), ForceMode2D.Impulse);
    }


    public enum PlayerState {
        Standing,
        Freefalling,
        Hooked,
        Death
    }

    private void OnDestroy() {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
        GameUI.OnJump -= GameUI_OnJump;
    }
}
