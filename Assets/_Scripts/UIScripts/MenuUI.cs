using UnityEngine;

public class MenuUI : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] private GameObject gameUIObject;
    [SerializeField] private GameObject optionsUIObject;
    
    /************************ INITIALIZE ************************/
    private void Awake() {
        
    }

    private void Start() {
        
    }

    /************************ LOOPING ************************/
    private void Update() {
        
    }

    /************************ METHODS ************************/


    public void DisableMenuAndEnableGame() {
        gameObject.SetActive(false);
        gameUIObject.SetActive(true);
    }

    public void EnableOptions() {
        gameObject.SetActive(false);
        optionsUIObject.SetActive(true);
    }

    public void StartGame() {
        GameManager.Instance.ChangeState(GameManager.GameState.Running);
        DisableMenuAndEnableGame();
    }
}
