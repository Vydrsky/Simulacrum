using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourceSystem : Singleton<ResourceSystem> {

    /************************ FIELDS ************************/

    [SerializeField][HideInInspector] 
    private List<LevelPart> levelPartsList = new List<LevelPart>();
    [HideInInspector]
    public Dictionary<string, LevelPart> levelPartsDictionary = new Dictionary<string, LevelPart>();
    [HideInInspector]
    public List<Sprite> heroSpritesList = new List<Sprite>();
    [HideInInspector]
    public List<Sprite> blackHoleSpritesList = new List<Sprite>();
    [HideInInspector]
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
        blackHoleSpritesList = Resources.LoadAll<Sprite>("black_hole").ToList();
    }
}
