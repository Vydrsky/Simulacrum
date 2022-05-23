using UnityEngine;

[CreateAssetMenu(fileName = "levelPart", menuName = "ScriptableObjects/LevelPart")]
public class LevelPart : ScriptableObject {
    public string key;
    public GameObject prefab;
    public float weight;
    public bool isBeneficial;
}


