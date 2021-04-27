using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    //private float hoverHeight = (maxHeight + minHeight) / 2.0f;
    //private float hoverRange = maxHeight - minHeight;
    private float hovSpeed = 2;
    private float hovDist = 0.25f;
    private float startingHeight;

    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = new Vector3(transform.position.x, startingHeight + Mathf.Cos(Time.time * hovSpeed) * hovDist, transform.position.z);
       //Debug.Log(Mathf.Cos(Time.time));
       // transform.Translate(0, 1 * Mathf.Cos(Time.time), 0);
    }
}
