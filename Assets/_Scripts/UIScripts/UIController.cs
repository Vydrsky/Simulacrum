using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {

    /************************ FIELDS ************************/

    private TextMeshProUGUI scoreText;
    private Camera mainCam;
    
    /************************ INITIALIZE ************************/
    private void Awake() {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        mainCam = Camera.main;
    }

    /************************ LOOPING ************************/
    private void Update() {
        //scoreText.text = mainCam.transform.position.x.ToString("0);
        //this should be controlled by game manager
    }

    /************************ METHODS ************************/
    
    
}
