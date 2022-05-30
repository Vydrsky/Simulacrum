using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourceSystem : Singleton<ResourceSystem> {

    /************************ FIELDS ************************/

    [SerializeField] private List<LevelPart> levelPartsList = new List<LevelPart>();
    public Dictionary<string, LevelPart> levelPartsDictionary = new Dictionary<string, LevelPart>();
    public List<Sprite> heroSpritesList = new List<Sprite>();
    public List<Sprite> heroFragmentedSpritesList = new List<Sprite>();


    /************************ INITIALIZE ************************/

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    /************************ METHODS ************************/

    private void AssembleResources() {
        levelPartsList = Resources.LoadAll<LevelPart>("LevelParts").ToList();
        levelPartsDictionary = levelPartsList.ToDictionary(x => x.key, x => x);
        heroSpritesList = Resources.LoadAll<Sprite>("Hero_SpriteSheet").ToList();
        heroFragmentedSpritesList = Resources.LoadAll<Sprite>("Hero_Fragmented").ToList();
        Debug.Log(heroSpritesList.Count + " " + heroFragmentedSpritesList.Count);
    }
}
