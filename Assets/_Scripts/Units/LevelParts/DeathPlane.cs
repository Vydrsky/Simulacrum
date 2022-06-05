using UnityEngine;
using System;
public class DeathPlane : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] private Transform playerTransform;

    public static event Action OnDeath;

    /************************ INITIALIZE ************************/

    /************************ LOOPING ************************/
    private void FixedUpdate() {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
    }

    /************************ METHODS ************************/

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Contains("Player")) {
            GameManager.Instance.ChangeState(GameManager.GameState.Ending);
            OnDeath?.Invoke();
        }
    }
}
