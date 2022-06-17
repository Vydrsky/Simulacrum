using System;
using UnityEngine;
using System.Collections.Generic;

public class DeathLaser : MonoBehaviour {

    /************************ FIELDS ************************/

    private Transform node1, node2;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    public static event Action OnDeath;
    public Transform Node2 { get { return node2; } set { node2 = value; } }
    
    /************************ INITIALIZE ************************/
    private void Awake() {
        node1 = transform.Find("Node1");
        node2 = transform.Find("Node2");
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Start() {
        SetLaser();
    }

    /************************ LOOPING ************************/
    private void Update() {
        
    }

    /************************ METHODS ************************/
    
    public void SetLaser() {
        List<Vector2> pointList = new List<Vector2>();
        pointList.Add(node1.position);
        pointList.Add(node2.position);
        lineRenderer.SetPosition(0, pointList[0]);
        lineRenderer.SetPosition(1, pointList[1]);
        pointList[0] = transform.InverseTransformPoint(pointList[0]);
        pointList[1] = transform.InverseTransformPoint(pointList[1]);
        edgeCollider.SetPoints(pointList);
        edgeCollider.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Contains("Player")) {
            GameManager.Instance.ChangeState(GameManager.GameState.Ending);
            OnDeath?.Invoke();
        }
    }
}
