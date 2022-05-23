using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourceSystem : Singleton<ResourceSystem> {

    /************************ FIELDS ************************/

    [SerializeField] private List<LevelPart> levelPartsList = new List<LevelPart>();
    public Dictionary<string, LevelPart> levelPartsDictionary = new Dictionary<string, LevelPart>(); 
    


    /************************ INITIALIZE ************************/

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    /************************ METHODS ************************/

    private void AssembleResources() {
        levelPartsList = Resources.LoadAll<LevelPart>("LevelParts").ToList();
        levelPartsDictionary = levelPartsList.ToDictionary(x => x.key, x => x);
        Debug.Log(levelPartsDictionary.Values.Count);
    }
}
