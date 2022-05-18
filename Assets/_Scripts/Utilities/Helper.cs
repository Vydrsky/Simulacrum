using UnityEngine;

//<summary>
//Static class containing static helping methods
//</summary>
public static class Helper  {

    /************************ METHODS ************************/
    
    public static void DestroyChildren(Transform parentTransform) {
        foreach (Transform transform in parentTransform) Object.Destroy(transform.gameObject);
    }
}
