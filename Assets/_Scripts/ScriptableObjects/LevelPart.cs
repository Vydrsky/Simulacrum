using UnityEngine;

[CreateAssetMenu(fileName = "levelPart", menuName = "ScriptableObjects/LevelPart")]
public class LevelPart : ScriptableObject {
    public string key;
    public GameObject prefab;
    public uint weight;
    public uint bank;
    public bool isBeneficial;
}


