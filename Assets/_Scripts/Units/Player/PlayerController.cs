using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

    /************************ FIELDS ************************/

    private Touch currentTouch;
    private Camera mainCam;
    private Collider2D coll;
    private Vector3 touchInWorldSpace;
    private SpringJoint2D hook;
    private Rigidbody2D rb;
    private bool forceCanBeAdded = true;
    private bool isTouched = false;

    [Range(0f,20f)]
    [SerializeField] private float RightForceOnHook = 5f;
    [SerializeField] private LineRenderer hookLine;

    /************************ INITIALIZE ************************/
    private void Awake() {
        mainCam = Camera.main;
        hook = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {

    }

    /************************ LOOPING ************************/
    private void Update() {
        hookLine.SetPosition(0, transform.position);
        if (Input.touchCount == 0) {
            hook.enabled = false;
            hookLine.enabled = false;
            forceCanBeAdded = true;
            isTouched = false;
            return;
        }

        if (!isTouched) {
            currentTouch = Input.GetTouch(0);
            touchInWorldSpace = mainCam.ScreenToWorldPoint(currentTouch.position);
            touchInWorldSpace.z = 0f;
            coll = Physics2D.OverlapCircle(touchInWorldSpace, 1f);
            isTouched = true;
        }

        if (coll == null) {
            return;
        }
        if (!coll.gameObject.tag.Contains("grapplingPoint")) {
            return;
        }

        hookLine.enabled = true;
        hook.enabled = true;
        hook.connectedBody = coll.gameObject.GetComponent<Rigidbody2D>();
        hook.connectedAnchor = coll.transform.position;
        hookLine.SetPosition(1, coll.transform.position);

        if (!forceCanBeAdded) {
            return;
        }
        hook.distance = Vector3.Distance(transform.position, coll.gameObject.transform.position) * 0.8f;
        rb.AddForce(new Vector2(RightForceOnHook, 0f), ForceMode2D.Impulse);
        forceCanBeAdded = false;
    }


    /************************ METHODS ************************/


}
