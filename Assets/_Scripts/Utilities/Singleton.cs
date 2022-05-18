using UnityEngine;


//<summary>
//Singleton class template with scope only in the current scene
//</summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour{

    /************************ FIELDS ************************/

    public static T Instance { get; private set; }

    /************************ INITIALIZE ************************/
    protected virtual void Awake() {
        if (Instance != null) Destroy(gameObject);
        else Instance = this as T;
    }
    
    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

//<summary>
//Singleton class template with persistance of the holder object and instance between scenes
//</summary>
public abstract class PersistantSingleton<T> : Singleton<T> where T : MonoBehaviour {

    /************************ INITIALIZE ************************/
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

}
