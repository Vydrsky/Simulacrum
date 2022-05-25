using UnityEngine;

public class DeathPlane : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] private Transform playerTransform;

    /************************ INITIALIZE ************************/

    /************************ LOOPING ************************/
    private void FixedUpdate() {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
    }

    /************************ METHODS ************************/

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Contains("Player")) {
            GameManager.Instance.ChangeState(GameManager.GameState.Ending);
            Debug.Log("DeathPlane Touched");
        }
    }
}
