using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAura : MonoBehaviour
{
    public GameObject aura;
    public int duration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ResetDuration()
    {
        StopAllCoroutines();
        //StartCoroutine(StartAura());
    }
}
