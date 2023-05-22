using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startpos;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        //length = GetComponent<Image>().x;
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist =  cam.transform.position.x * parallaxEffect ;
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        //if(temp > startpos + length)

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
