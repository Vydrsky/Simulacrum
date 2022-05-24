using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

    /************************ FIELDS ************************/

    private Touch currentTouch;
    private Camera mainCam;
    private Collider2D[] colliders;
    private Collider2D closestCollider;
    private Vector3 touchInWorldSpace;
    private SpringJoint2D hook;
    private Rigidbody2D rb;
    private Transform laserStartTransform;
    private bool forceCanBeAdded = true;
    private bool isTouched = false;

    [Range(0f,20f)]
    [SerializeField] private float RightForceOnHook = 5f;
    [SerializeField] private LineRenderer hookLine;

    public static event Action OnHooked;
    public static event Action OnDeHooked;

    /************************ INITIALIZE ************************/
    private void Awake() {
        mainCam = Camera.main;
        hook = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        laserStartTransform = transform.Find("LaserStartPoint");
    }

    private void Start() {

    }

    /************************ LOOPING ************************/
    private void Update() {
        hookLine.SetPosition(0, laserStartTransform.position);
        if (Input.touchCount == 0) {
            hook.enabled = false;
            hookLine.enabled = false;
            forceCanBeAdded = true;
            isTouched = false;
            OnDeHooked?.Invoke();
            return;
        }

        if (!isTouched) {
            currentTouch = Input.GetTouch(0);
            touchInWorldSpace = mainCam.ScreenToWorldPoint(currentTouch.position);
            touchInWorldSpace.z = 0f;
            colliders = Physics2D.OverlapCircleAll(touchInWorldSpace, 4f);
            isTouched = true;
            closestCollider = null;
            if (colliders.Length == 0) return;
            foreach(Collider2D collider in colliders) {
                if (collider == colliders[0]) closestCollider = collider;
                if (Vector3.Distance(collider.transform.position,touchInWorldSpace) 
                    < Vector3.Distance(closestCollider.transform.position, touchInWorldSpace))
                    closestCollider = collider;
            }
        }

        if (closestCollider == null) {
            return;
        }
        if (!closestCollider.gameObject.tag.Contains("grapplingPoint")) {
            return;
        }

        hookLine.enabled = true;
        hook.enabled = true;
        hook.connectedBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();
        hook.connectedAnchor = closestCollider.transform.position;
        hookLine.SetPosition(1, closestCollider.transform.position);
        OnHooked?.Invoke();
        if (!forceCanBeAdded || !isTouched) {
            return;
        }
        hook.distance = Vector3.Distance(transform.position, closestCollider.gameObject.transform.position) * 0.8f;
        rb.AddForce(new Vector2(RightForceOnHook, 0f), ForceMode2D.Impulse);
        forceCanBeAdded = false;
    }


    /************************ METHODS ************************/


}
