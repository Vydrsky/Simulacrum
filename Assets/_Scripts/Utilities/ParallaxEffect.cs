using UnityEngine;

public class ParallaxEffect : MonoBehaviour {

    /************************ FIELDS ************************/

    private float length, startpos;
    private Camera mainCam;
    private Transform[] childTransforms;
    private Transform furthestChild;
    private int immediateChildren = 0;


    [Range(0f, 1f)]
    [SerializeField] private float parallaxEffect;

    /************************ INITIALIZE ************************/
    private void Awake() {
        mainCam = Camera.main;
        startpos = transform.position.x;
    }

    private void Start() {
        length = GameObject.FindGameObjectWithTag("backgroundBase").GetComponent<SpriteRenderer>().bounds.size.x;
        childTransforms = GetComponentsInChildren<Transform>();
        furthestChild = childTransforms[1];
        foreach (Transform child in transform) {
            if (child == childTransforms[0]) continue;
            //if (child.parent != gameObject) continue;
            immediateChildren++;
        }
    }

    /************************ LOOPING ************************/
    private void Update() {
        float cameraRelativePosition = mainCam.transform.position.x * (1 - parallaxEffect);
        float distance = mainCam.transform.position.x * parallaxEffect;
        float padding = 0.9f;

        transform.position = new Vector3(distance, transform.position.y, transform.position.z);
        Debug.Log(immediateChildren);
        if(cameraRelativePosition > startpos + padding*length) {
            foreach(Transform child in transform) {
                //get all background children
                if(child.position.x < furthestChild.position.x) {
                    furthestChild = child;
                }
            }
            //move the most left children right by amount of immediate children tiles
            furthestChild.position += new Vector3(immediateChildren * length, 0f, 0f);
            //set new start position for the beggining of the next tile
            startpos += padding * length;
        }
        if (cameraRelativePosition < startpos - padding * length) {
            foreach (Transform child in transform) {
                if (child.position.x > furthestChild.position.x) {
                    furthestChild = child;
                }
            }
            furthestChild.position += new Vector3(immediateChildren * -length, 0f, 0f);
            startpos -= padding * length;
        }
    }


}
