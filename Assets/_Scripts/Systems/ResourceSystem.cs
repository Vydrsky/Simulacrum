using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourceSystem : Singleton<ResourceSystem> {

    /************************ FIELDS ************************/

    [SerializeField] private List<GrapplingPointSO> levelPartsList = new List<GrapplingPointSO>();
    public Dictionary<string, GrapplingPointSO> levelPartsDictionary = new Dictionary<string, GrapplingPointSO>(); 
    


    /************************ INITIALIZE ************************/

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    /************************ METHODS ************************/

    private void AssembleResources() {
        levelPartsList = Resources.LoadAll<GrapplingPointSO>("LevelParts").ToList();
        levelPartsDictionary = levelPartsList.ToDictionary(x => x.key, x => x);
        Debug.Log(levelPartsDictionary.Values.Count);
    }
}
