using UnityEngine;

public class SpawnManager : Singleton<SpawnManager> {

    /************************ FIELDS ************************/
    
    
    
    /************************ INITIALIZE ************************/

    private void Start() {
        for (int i = 0; i < 1000; i++) {
            float xrand = Random.Range(0f, 1000f);
            float yrand = Random.Range(0f, 22f);

            Instantiate(ResourceSystem.Instance.levelPartsDictionary["grapplingPoint"].prefab,
                        new Vector3(xrand,yrand,0f),
                        Quaternion.identity,
                        GameObject.Find("LevelParts").transform);

        }
    }

    /************************ LOOPING ************************/
    private void Update() {

    }

    /************************ METHODS ************************/
    
    
}
