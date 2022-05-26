using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput {

    /************************ FIELDS ************************/

    public bool ScreenWasTouchedAlready { get; private set; }

    /************************ METHODS ************************/

    
    public bool isScreenBeingTouched() {
        if (Input.touchCount > 0)
            return true;
        else
            return false;
    }

    public Touch GetTheFirstTouch() {
        return Input.GetTouch(0);
    }

    public bool isUiBeingTouched() {
        if (EventSystem.current.IsPointerOverGameObject(0)) {
            return true;
        }
        return false;
    }
}
