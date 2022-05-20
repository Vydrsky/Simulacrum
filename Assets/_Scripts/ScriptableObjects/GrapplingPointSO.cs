using UnityEngine;

[CreateAssetMenu(fileName = "newGrapplingPoint", menuName = "ScriptableObjects/GrapplingPoint")]
public class GrapplingPointSO : ScriptableObject {
    public string key = "grapplingPoint";
    public GameObject prefab;
    public float weight;
    public bool isBeneficial;
}


