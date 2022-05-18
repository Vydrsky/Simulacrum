using UnityEngine;

public class ParallaxEffect : MonoBehaviour {

    /************************ FIELDS ************************/

    private float length, startpos;
    private Camera mainCam;

    [Range(0f, 1f)]
    [SerializeField] private float parallaxEffect;
    
    /************************ INITIALIZE ************************/
    private void Awake() {
        mainCam = Camera.main;
        startpos = transform.position.x;
        length = GameObject.FindGameObjectWithTag("backgroundBase").GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Start() {
        
    }

    /************************ LOOPING ************************/
    private void Update() {
        float temp = mainCam.transform.position.x * (1 - parallaxEffect);
        float distance = mainCam.transform.position.x*parallaxEffect;

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += 2*length;
        else if (temp < startpos - length) startpos -= 2*length;
    }

    /************************ METHODS ************************/
    
    
}
