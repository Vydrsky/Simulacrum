using UnityEngine;
using System;

public class Prefs : Singleton<Prefs> {

    /************************ FIELDS ************************/

    bool endReached = false;
    
    /************************ INITIALIZE ************************/


    /************************ LOOPING ************************/
    private void Update() {
        if (endReached) return;
        if(PlayerController.Instance.transform.position.x >= 120f) {
            endReached = true;
            PlayerPrefs.SetInt("tutorialFinished", 1);
            Debug.Log("Saved To Prefs");
        }
    }

    /************************ METHODS ************************/
    
    
}
