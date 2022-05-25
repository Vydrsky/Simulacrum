using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

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
        private set {
            if (value == state) return;
            state = value;
            switch (state) {
                case PlayerState.Freefalling:
                    OnDeHooked?.Invoke();
                    hook.enabled = false;
                    hookLine.enabled = false;
                    rb.AddForce(new Vector2(0f, RightForceOnHook), ForceMode2D.Impulse);
                    break;
                case PlayerState.Hooked:
                    OnHooked?.Invoke();

                    //if its all good create a logic connection
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
    public static event Action OnHooked;
    public static event Action OnDeHooked;

    /************************ INITIALIZE ************************/
    private void Awake() {
        input = new PlayerInput();
        mainCam = Camera.main;
        hook = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        laserStartTransform = transform.Find("LaserStartPoint");
    }

    private void Start() {
        //temp
        State = PlayerState.Freefalling;
    }


    /************************ LOOPING ************************/
    private void Update() {
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
    public PlayerState ControlStates() {
        PlayerState tempState = PlayerState.Freefalling;
        Touch touch;

        if(State == PlayerState.Freefalling) {
            closestCollider = null;
            //this check doesnt not allow for changing the collider once its attached and the screen is held
        }
        if(input.isScreenBeingTouched()){
            if (closestCollider == null) {  //closest collider and input is only evaluated if the player is falling
                touch = input.GetTheFirstTouch();
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
            if (closestCollider.gameObject.tag.Contains("grapplingPoint"))
                tempState = PlayerState.Hooked;
            else
                tempState = PlayerState.Freefalling;
        }
        else if(!input.isScreenBeingTouched()) {
            tempState = PlayerState.Freefalling; 
        }
        return tempState;
    }


    public enum PlayerState {
        Standing,
        Freefalling,
        Hooked
    }
}
