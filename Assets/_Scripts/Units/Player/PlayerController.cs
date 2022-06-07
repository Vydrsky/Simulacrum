using UnityEngine;
using System;

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


    [Range(0f, 20f)]
    [SerializeField] private float RightForceOnHook = 5f;
    [SerializeField] private LineRenderer hookLine;

    private PlayerState state;
    public PlayerState State {

        get { return state; }

        set {
            if (value == state) return;
            state = value;
            switch (state) {
                case PlayerState.Freefalling:
                    rb.isKinematic = false;
                    hook.enabled = false;
                    hookLine.enabled = false;
                    rb.AddForce(new Vector2(0f, RightForceOnHook), ForceMode2D.Impulse);
                    break;
                case PlayerState.Hooked:

                    hook.distance = Vector3.Distance(transform.position, closestCollider.gameObject.transform.position);
                    hook.connectedBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();
                    hook.connectedAnchor = closestCollider.transform.position;
                    hookLine.enabled = true;
                    hook.enabled = true;
                    rb.AddForce(new Vector2(RightForceOnHook, 0f), ForceMode2D.Impulse);
                    break;
                    
            }
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
    }



    /************************ LOOPING ************************/
    private void Update() {
        if (State == PlayerState.Standing) return;
        switch (State) {
            case PlayerState.Freefalling:

                break;
            case PlayerState.Hooked:

                //update the line renderer
                hookLine.SetPosition(0, laserStartTransform.position);
                hookLine.SetPosition(1, closestCollider.transform.position);
                break;

            case PlayerState.Death:
                HandleDeath();
                return;
        }
        State = ControlStates();
    }


    /************************ METHODS ************************/
    public PlayerState ControlStates() {
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
            if (closestCollider != null && closestCollider.gameObject.tag.Contains("grapplingPoint"))
                tempState = PlayerState.Hooked;
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
        rb.AddForce(new Vector3(10f, 15f, 0f),ForceMode2D.Impulse);
    }

    public enum PlayerState {
        Standing,
        Freefalling,
        Hooked,
        Death
    }

    private void OnDestroy() {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }
}
