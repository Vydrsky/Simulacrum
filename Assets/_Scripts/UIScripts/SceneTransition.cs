using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

    /************************ FIELDS ************************/

    [SerializeField] public Animator transition;
    
    /************************ INITIALIZE ************************/


    private void Start() {
        GameUI.OnRetryPressed += GameUI_OnRetryPressed;
    }


    /************************ LOOPING ************************/
    private void Update() {
        
    }

    /************************ METHODS ************************/

    private void GameUI_OnRetryPressed() {
        StartCoroutine(LoadLevel());
    }

    public void LoadNextLevel() {
        GameManager.Instance.ReloadScene();
    }

    private IEnumerator LoadLevel() {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        LoadNextLevel();
    }

    private void OnDestroy() {
        GameUI.OnRetryPressed -= GameUI_OnRetryPressed;
    }
}
