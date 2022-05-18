using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem> {

    /************************ FIELDS ************************/

    //<summary>
    //public Dictionaries of scriptable objects
    //</summary>


    /************************ INITIALIZE ************************/

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    /************************ METHODS ************************/

    private void AssembleResources() {
        //<summary>
        //Call Resources.LoadAll() to dictionary
        //</summary>
    }
}
