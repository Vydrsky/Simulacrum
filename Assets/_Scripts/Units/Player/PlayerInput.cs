using UnityEngine;

public class PlayerInput {

    /************************ FIELDS ************************/
    


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

    
}
