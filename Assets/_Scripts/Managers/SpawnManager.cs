using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : Singleton<SpawnManager> {

    /************************ FIELDS ************************/

    [SerializeField] private uint goodCredits=150;
    [SerializeField] private uint badCredits=20;
    [SerializeField] private Transform LevelParts;
    [SerializeField] private Transform TutorialParts;
    [SerializeField] private RectTransform WorldSpaceCanvasTransform;
    [SerializeField] private float spawnSpacing;

    /************************ INITIALIZE ************************/
    private void Start() {
        ParallaxEnvironment.OnLevelPartsMoved += ParallaxEnvironment_OnLevelPartsMoved;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }




    /************************ LOOPING ************************/


    /************************ METHODS ************************/

    private void SpawnSingular(int amountToSpawn, Vector2 lowerBound, Vector3 upperBound, GameObject prefab) {
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

            Instantiate(prefab,
                        new Vector3(xrand, yrand, 0f),
                        Quaternion.identity,
                        GameObject.Find("LevelParts").transform);

        }
    }

    private void SpawnDeathLasers(int amountToSpawn, Vector2 lowerBound, Vector3 upperBound) {
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

            GameObject laserObject = Instantiate(ResourceSystem.Instance.levelPartsDictionary["deathLaser"].prefab,
                        new Vector3(xrand, yrand, 0f),
                        Quaternion.identity,
                        GameObject.Find("LevelParts").transform);
            float node1RandX = xrand;
            float node2RandY = yrand;
            tempSpacing = spawnSpacing;
            do {
                xrand = Random.Range(node1RandX - 5f, node1RandX + 5f);
                yrand = Random.Range(node2RandY - 5f, node2RandY + 5f);

                colliders = Physics2D.OverlapCircleAll(new Vector2(xrand, yrand), tempSpacing);
                tempSpacing -= 0.5f;
            } while (colliders.Length != 0);
            DeathLaser laser = laserObject.GetComponent<DeathLaser>();
            laser.Node2.position = new Vector2(xrand, yrand);
            laser.SetLaser();
        }
    }

    private void Despawn(Transform forwardTransform, float length) {
        foreach (Transform child in LevelParts) {
            if (child.position.x < forwardTransform.position.x - length * 5f) {
                Destroy(child.gameObject);
            }
        }
    }

    private void ParallaxEnvironment_OnLevelPartsMoved(Transform arg1, float arg2) {
        uint badCreditsCache;
        uint goodCreditsCache;
        if ((PlayerController.Instance.transform.position.x > 80f && PlayerPrefs.GetInt("tutorialFinished")==0) || PlayerPrefs.GetInt("tutorialFinished") == 1) {
            foreach (KeyValuePair<string, LevelPart> keyValuePair in ResourceSystem.Instance.levelPartsDictionary) {
                //set credits function
                if (keyValuePair.Value.isBeneficial) {
                    goodCreditsCache = goodCredits + keyValuePair.Value.bank;
                    if (keyValuePair.Value.weight <= goodCredits + keyValuePair.Value.bank) {
                        keyValuePair.Value.bank = 0;
                        do {
                            SpawnSingular(1, arg1.position, arg1.position + new Vector3(arg2, 30f - arg1.position.y, 0f),
                                          keyValuePair.Value.prefab);
                            goodCreditsCache -= keyValuePair.Value.weight;
                        } while (goodCreditsCache > keyValuePair.Value.weight);
                    }
                    keyValuePair.Value.bank += goodCreditsCache - keyValuePair.Value.bank;
                }
                else {
                    badCreditsCache = badCredits + keyValuePair.Value.bank;
                    if (keyValuePair.Value.weight <= badCredits + keyValuePair.Value.bank) {
                        keyValuePair.Value.bank = 0;
                        do {
                            switch (keyValuePair.Key) {
                                case "blackHole":
                                    SpawnSingular(1, arg1.position, arg1.position + new Vector3(arg2, 30f - arg1.position.y, 0f),
                                        ResourceSystem.Instance.levelPartsDictionary["blackHole"].prefab);
                                    break;
                                case "deathLaser":
                                    SpawnDeathLasers(1, arg1.position, arg1.position + new Vector3(arg2, 30f - arg1.position.y, 0f));
                                    break;
                            }
                            badCreditsCache -= keyValuePair.Value.weight;
                        } while (badCreditsCache >= keyValuePair.Value.weight);
                    }
                    keyValuePair.Value.bank += badCreditsCache - keyValuePair.Value.bank;
                }
            }
            Despawn(arg1, arg2);
        }
    }

    private void GameManager_OnGameStarted() {
        if (PlayerPrefs.GetInt("tutorialFinished") == 1) {
            TutorialParts.gameObject.SetActive(false);
            WorldSpaceCanvasTransform.gameObject.SetActive(false);
        }
    }

    private void OnDestroy() {
        ParallaxEnvironment.OnLevelPartsMoved -= ParallaxEnvironment_OnLevelPartsMoved;
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }
}
