using UnityEngine;

public class SpawnManager : Singleton<SpawnManager> {

    /************************ FIELDS ************************/

    [SerializeField] private Transform LevelParts;
    [SerializeField] private float spawnSpacing;

    /************************ INITIALIZE ************************/
    private void Start() {
        ParallaxEnvironment.OnLevelPartsMoved += ParallaxEnvironment_OnLevelPartsMoved;
    }


    /************************ LOOPING ************************/

    private void Update() {
        
    }

    /************************ METHODS ************************/

    public void Spawn(int amountToSpawn,Vector2 lowerBound,Vector3 upperBound) {
        for (int i = 0; i < amountToSpawn; i++) {
            Collider2D[] colliders = { };
            float xrand, yrand;
            float tempSpacing = spawnSpacing;
            do {
                xrand = Random.Range(lowerBound.x, upperBound.x);
                yrand = Random.Range(lowerBound.y, upperBound.y);

                colliders = Physics2D.OverlapCircleAll(new Vector2(xrand, yrand), tempSpacing);
                tempSpacing -= 0.5f;
            } while (colliders.Length != 0);
            
            Instantiate(ResourceSystem.Instance.levelPartsDictionary["grapplingPoint"].prefab,
                        new Vector3(xrand, yrand, 0f),
                        Quaternion.identity,
                        GameObject.Find("LevelParts").transform);

        }
    }

    private void Despawn(Transform forwardTransform,float length) {
        foreach(Transform child in LevelParts) {
            if(child.position.x < forwardTransform.position.x - length*5f) {
                Destroy(child.gameObject);
            }
        }
    }

    private void ParallaxEnvironment_OnLevelPartsMoved(Transform arg1, float arg2) {
        Spawn(10, arg1.position, arg1.position + new Vector3(arg2,22f - arg1.position.y,0f));
        Despawn(arg1,arg2);
    }
}
